using System;
using System.Collections.Generic;
using MGE;
using GAME.World.Generation;
using Microsoft.Xna.Framework.Graphics;
using MGE.Graphics;
using MGE.UI;
using MGE.UI.Elements;
using MGE.InputSystem;
using Random = MGE.Random;
using MGE.ECS;
using GAME.Components;

namespace GAME.World
{
	public class Grid
	{
		public static Grid current;

		public static readonly List<Tile> tiles = new List<Tile>()
		{
			new Air(),
			new Stone(),
			new Dirt(),
			new Grass(),
			new Sand(),
			new Water(),
			new Salt(),
			new Sodium(),
			new Smoke(),
			new Sponge(),
			new Oil(),
			new Void(),
			new Acid(),
			new Vine(),
			new TNT(),
		};

		public static Tile IDToTile(int id) => tiles[id];
		public static int TileToID(Tile tile) => tiles.IndexOf(tile);

		public readonly Vector2Int gravity = new Vector2Int(0, 1);

		uint currentTick = 0;
		public bool debugEnabled = false;

		public Tile[,] world;

		public Vector2 position;
		public double scale;

		public readonly Vector2Int size;

		static Tile _air;

		public static Tile air
		{
			get
			{
				if (_air == null) _air = new Air();
				return _air;
			}
		}

		public Grid(Vector2Int size, IGenerator generator)
		{
			current = this;

			this.size = size;
			this.position = Window.gameSize / 2 - size / 2;

			this.world = new Tile[size.x, size.y];
			new GenTest().Generate(ref world, "cat".GetHashCode());
		}

		public Tile GetTile(Vector2Int position) => GetTile(position.x, position.y);
		public Tile GetTile(int x, int y)
		{
			if (x < 0 || x >= size.x || y < 0 || y >= size.y) return new Stone();
			return world[x, y];
		}

		public bool SetTile(Vector2Int position, Tile tile) => SetTile(position.x, position.y, tile);
		public bool SetTile(int x, int y, Tile tile)
		{
			if (x < 0 || x >= size.x || y < 0 || y >= size.y) return false;
			if (tile == null) tile = new Air();
			world[x, y] = tile;

			UpdateArea(x, y);

			return true;
		}

		public bool TileIsType(Vector2Int position, Type type) => TileIsType(position.x, position.y, type);
		public bool TileIsType(int x, int y, Type type) => GetTile(x, y).GetType() == type;

		public bool TileExists(Vector2Int position) => TileExists(position.x, position.y);
		public bool TileExists(int x, int y) => !TileIsType(x, y, typeof(Air));

		public bool TileExists(Vector2Int position, Type ignore) => TileExists(position, ignore);
		public bool TileExists(int x, int y, Type ignore) => !(TileIsType(x, y, typeof(Air)) || TileIsType(x, y, ignore));

		public bool SwapTile(Vector2Int from, Vector2Int to)
		{
			var fromTile = GetTile(from);
			var toTile = GetTile(to);

			var fromDensity = CalcDensity(fromTile);
			var toDensity = CalcDensity(toTile);

			if (toDensity >= fromDensity) return false;

			SetTile(from, toTile);
			SetTile(to, fromTile);

			return true;
		}

		public Vector2Int MoveTile(Vector2Int from, Vector2Int velocity)
		{
			var to = from + velocity;

			var endPos = from;
			Tile toTile = null;

			var fromTile = GetTile(from);
			var fromDensity = CalcDensity(fromTile);

			var points = Util.LineToPointsInGrid(from, to);

			foreach (var point in points)
			{
				if (point == from) continue;

				endPos = point;
				toTile = GetTile(point);

				if (CalcDensity(toTile) >= fromDensity) break;
			}

			SetTile(from, toTile);
			SetTile(endPos, fromTile);

			return endPos;
		}

		public int CalcDensity(Tile tile)
		{
			switch (tile.type)
			{
				case TileType.Solid:
					return tile.density + 100;
				case TileType.Liquid:
					return tile.density;
				case TileType.Gas:
					return tile.density - 100;
			}

			return tile.density;
		}

		public void UpdateArea(Vector2Int position) => UpdateArea(position.x, position.y);
		public void UpdateArea(int x, int y)
		{
			for (int yy = -1; yy <= 1; yy++)
				for (int xx = -1; xx <= 1; xx++)
					GetTile(x + xx, y + yy).isDirty = true;
		}

		public bool Erode(int x, int y)
		{
			var tile = GetTile(x, y);

			if (tile.type == TileType.Gas || tile.info.HasFlag(TileInfo.NonErodable)) return false;

			SetTile(x, y, null);

			return true;
		}

		public void Explode(Vector2Int position, Vector2Int size, bool recursive = true, bool ignoreLiquids = true)
		{
			for (int y = -size.y / 2; y <= size.y / 2; y++)
				for (int x = -size.x / 2; x <= size.x / 2; x++)
					if (recursive && GetTile(position.x + x, position.y + y) is IExplodable explodable)
						explodable.Explode(position + new Vector2Int(x, y), false);
					else if (Random.Bool(size.sqrMagnitude / new Vector2(x, y).sqrMagnitude / 32))
					{
						var tile = GetTile(position.x + x, position.y + y);

						if (!tile.info.HasFlag(TileInfo.BlastProof))
							if (!ignoreLiquids || tile.type == TileType.Solid)
								SetTile(position.x + x, position.y + y, null);
					}

			SceneManager.activeScene.layers[0].AddEntity(new Entity(new CFlash(position, (float)size.magnitude, 256)));

			GUI.gui.Image(new Rect((Vector2)position * Camera.main.scaleUpFactor, (Vector2)size * Camera.main.scaleUpFactor), Color.yellow);
		}

		public Vector2Int CamToTile(Vector2 mousePos)
		{
			var tilePos = Vector2Int.zero;

			mousePos -= position;

			tilePos.x = (int)mousePos.x;
			tilePos.y = (int)mousePos.y;

			tilePos.Clamp(0, 0, size.x - 1, size.y - 1);

			return tilePos;
		}

		public void Update()
		{
			currentTick++;

			for (int x = size.x - 1; x >= 0; x--)
				for (int y = size.y - 1; y >= 0; y--)
				{
					var tile = world[x, y];
					if (tile == null || tile.GetType() == typeof(Air)) continue;

					if (tile.isDirty)
					{
						tile.isDirty = false;
						tile.lastUpdated = currentTick;
						tile.Update(new Vector2Int(x, y));

						if (debugEnabled)
							GUI.gui.Image(new Rect(new Vector2(x, y) * Camera.main.scaleUpFactor, Camera.main.scaleUpFactor), new Color(0, 1, 0, 0.5f));
					}
				}
		}

		public void Draw()
		{
			using (new DrawBatch(SamplerState.PointClamp, null, SpriteSortMode.Deferred, BlendState.AlphaBlend))
			{
				for (int y = 0; y < size.y; y++)
				{
					for (int x = 0; x < size.x; x++)
					{
						var tile = world[x, y];
						if (tile.color != Color.clear)
							GFX.DrawPoint(position + new Vector2(x, y), tile.color);
					}
				}
			}
		}
	}
}