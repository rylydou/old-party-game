using MGE;
using MGE.Graphics;

namespace GAME.States
{
	public class StateTileEditor : GameState
	{
		Tileset tileset;

		public override void Init()
		{
			base.Init();

			tileset = Assets.GetAsset<Tileset>("Tilesets/Basic");
		}

		public override void Draw()
		{
			base.Draw();

			foreach (var tile in tileset.tiles)
			{
				GFX.Draw(tileset.texture, new Rect(tile.Value, tileset.tileSize), tile.Value / 16);
			}

			foreach (var tile in tileset.tiles)
			{
				for (int y = -1; y <= 1; y++)
				{
					for (int x = -1; x <= 1; x++)
					{
						var hit = false;

						switch ($"{x},{y}")
						{
							case "0,-1":
								if (tile.Key.HasFlag(TileConnection.Top))
									hit = true;
								break;
							case "1,0":
								if (tile.Key.HasFlag(TileConnection.Right))
									hit = true;
								break;
							case "0,1":
								if (tile.Key.HasFlag(TileConnection.Bottom))
									hit = true;
								break;
							case "-1,0":
								if (tile.Key.HasFlag(TileConnection.Left))
									hit = true;
								break;
							case "-1,-1":
								if (tile.Key.HasFlag(TileConnection.Top_Left))
									hit = true;
								break;
							case "1,-1":
								if (tile.Key.HasFlag(TileConnection.Top_Right))
									hit = true;
								break;
							case "-1,1":
								if (tile.Key.HasFlag(TileConnection.Bottom_Left))
									hit = true;
								break;
							case "1,1":
								if (tile.Key.HasFlag(TileConnection.Bottom_Right))
									hit = true;
								break;
						}

						if (hit)
							GFX.DrawLine((Vector2)tile.Value / 16 + 0.5f, (Vector2)tile.Value / 16 + 0.5f + new Vector2(x, y) / 2, Color.red.ChangeAlpha(0.5f), 2);
					}
				}
			}
		}
	}
}