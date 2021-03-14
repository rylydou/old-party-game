using MGE;
using MGE.ECS;
using MGE.Graphics;
using MGE.InputSystem;
using Microsoft.Xna.Framework.Graphics;
using Texture = MGE.Texture;

namespace GAME.Components.UI
{
	public class CUIHUD : Component
	{
		Vector2 heartOffset = new Vector2(8);
		float heartSize = 48;

		Vector2 hotbarOffset = new Vector2(12);
		float hotbarItemSize = 64;
		float hotbarItemSpacing = 12;

		Vector2 buttonSize = new Vector2(128, 32);
		// float buttonOffset = 16;

		Font font;
		SpriteSheet hearts;
		Texture panel;

		public override void Init()
		{
			font = Assets.GetAsset<Font>("Fonts/Basic");
			hearts = Assets.GetAsset<SpriteSheet>("Sprites/UI/Hearts");
			panel = Assets.GetAsset<Texture>("Sprites/UI/Panel");
		}

		public override void Draw()
		{
			using (new DrawBatch(transform: null))
			{
				for (int i = 0; i < Main.current.maxHealth + Main.current.extraHealth; i++)
				{
					hearts.Draw("Shadow", new Rect(heartOffset.x + i * heartSize, heartOffset.y, heartSize, heartSize), Color.white);
				}

				for (int i = 0; i < Main.current.maxHealth; i++)
				{
					if (i < Main.current.health)
						hearts.Draw("Normal", new Rect(heartOffset.x + i * heartSize, heartOffset.y, heartSize, heartSize), Color.white);
					else
						hearts.Draw("Empty", new Rect(heartOffset.x + i * heartSize, heartOffset.y, heartSize, heartSize), Color.white);
				}

				for (int i = 0; i < Main.current.extraHealth; i++)
				{
					hearts.Draw("Extra", new Rect(heartOffset.x + (Main.current.maxHealth + i) * heartSize, heartOffset.y, heartSize, heartSize), Color.white);
				}

				for (int i = 0; i < Main.current.inv.Length; i++)
				{
					GFX.Draw(panel, new Rect(hotbarOffset.x + (hotbarItemSize + hotbarItemSpacing) * i, Window.windowedSize.y - hotbarItemSize - hotbarOffset.y, hotbarItemSize, hotbarItemSize), new Color(0, 0.5f));
				}

				// var buttonRect = new Rect(Window.windowedSize.x - buttonSize.x - buttonOffset, buttonOffset, buttonSize.x, buttonSize.y);

				// if (buttonRect.Contains(Input.windowMousePosition))
				// {
				// 	GFX.DrawBox(buttonRect, Colors.gray);
				// 	if (Input.GetButtonPress(Inputs.MouseLeft))
				// 		CChunk.current.Save();
				// }
				// else
				// 	GFX.DrawBox(buttonRect, Colors.black);

				// font.DrawText("Save", buttonRect.position, Colors.text);

				// buttonRect = new Rect(Window.windowedSize.x - buttonSize.x - buttonOffset, buttonOffset * 2 + buttonSize.y, buttonSize.x, buttonSize.y);

				// if (buttonRect.Contains(Input.windowMousePosition))
				// {
				// 	GFX.DrawBox(buttonRect, Colors.gray);
				// 	if (Input.GetButtonPress(Inputs.MouseLeft))
				// 		CChunk.current.Load();
				// }
				// else
				// 	GFX.DrawBox(buttonRect, Colors.black);

				// font.DrawText("Load", buttonRect.position, Colors.text);
			}
		}
	}
}