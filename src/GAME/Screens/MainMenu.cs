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

		Vector2 playerVel;
		Vector2 playerPosition;

		protected override void OnInit()
		{
			texBG = Assets.GetAsset<Texture2D>("Sprites/Background.png");
			texIcon = Assets.GetAsset<Texture2D>("Sprites/Icon.png");
		}

		protected override void OnUpdate()
		{
			playerVel.y += 1f * Time.deltaTime;

			playerPosition += playerVel;
		}

		protected override void OnDraw()
		{
			sb.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.Transformation());
			sb.Draw(texBG, new Rect(0, 0, Window.renderSize.x, Window.renderSize.y), Color.white);
			sb.Draw(texIcon, playerPosition, Color.white);
			sb.End();
		}

		protected override void OnDrawUI()
		{
			sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp);
			Graphics.DrawBox(new Rect(0, 0, 64, 64), new Color(1, 0, 0, 0.5f));
			Graphics.DrawBox(new Rect(32, 0, 64, 64), new Color(0, 1, 0, 0.5f));
			Graphics.DrawBox(new Rect(64, 0, 64, 64), new Color(0, 0, 1, 0.5f));
			Graphics.DrawBox(new Rect(0, 0, 64 * 2, 16), new Color(1, 1, 1, 0.5f));
			Graphics.DrawBox(new Rect(0, 16 * 3, 64 * 2, 16), new Color(0, 0, 0, 0.5f));
			sb.End();
		}
	}
}