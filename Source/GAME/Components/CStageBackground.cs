using MGE;
using MGE.ECS;
using MGE.Graphics;

namespace GAME.Components
{
	public class CStageBackground : Component
	{
		const int tileSize = 16;

		Stage stage;

		Texture tileset;

		public override void Init()
		{
			base.Init();

			stage = GameSettings.current.stage;

			tileset = Assets.GetAsset<Texture>("Tilesets/_Background");
		}

		public override void Draw()
		{
			base.Draw();

			stage.tilesBackground.For((x, y, pos) =>
				GFX.Draw(tileset, new RectInt(new Vector2Int(pos.Item1, pos.Item2) * tileSize, tileSize), new Vector2(x, y), Color.white)
			);
		}
	}
}