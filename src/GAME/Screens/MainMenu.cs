using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MGE;
using MGE.Graphics;

namespace GAME.Screens
{
	public class MenuScreen : Screen
	{
		Texture2D texBG;
		Texture2D texIcon;
		Texture2D texTile;

		Vector2 playerVel;
		Vector2 playerPosition;
		Vector2 mousePos;

		protected override void OnInit()
		{
			texBG = Assets.GetAsset<Texture2D>("Sprites/Background.png");
			texIcon = Assets.GetAsset<Texture2D>("Sprites/Icon.png");
			texTile = Assets.GetAsset<Texture2D>("Sprites/Tile.png");
		}

		protected override void OnUpdate()
		{
			playerVel.y += 1f * Time.deltaTime;

			playerPosition += playerVel;

			mousePos = Camera.main.MouseToScreenPos(Mouse.GetState().Position);

			Logger.Log(mousePos);
		}

		protected override void OnDraw()
		{
			sb.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.Transformation());
			sb.Draw(texBG, new Rect(0, 0, Window.gameSize.x, Window.gameSize.y), Color.white);
			sb.Draw(texIcon, mousePos, Color.white);
			// Graphics.DrawCircle(mousePos, 3, Color.white, 0);
			sb.End();
		}

		protected override void OnDrawUI()
		{
			sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp);
			Graphics.DrawBox(new Rect(0, 0, 32, 32), new Color(1, 0, 0, 1));
			Graphics.DrawBox(new Rect(32, 0, 32, 32), new Color(0, 1, 0, 1));
			Graphics.DrawBox(new Rect(64, 0, 32, 32), new Color(0, 0, 1, 1));

			Graphics.DrawBox(new Rect(0, 0, 64, 8), new Color(1, 1, 1, 0.2f));
			Graphics.DrawBox(new Rect(0, 32 - 8, 64, 8), new Color(0, 0, 0, 0.1f));
			sb.End();
		}
	}
}