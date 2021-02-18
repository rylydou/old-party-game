using System;
using Microsoft.Xna.Framework.Graphics;
using MGE;
using MGE.ECS;
using MGE.InputSystem;
using MGE.Graphics;

namespace GAME.Components
{
	public class CPlayer : Component
	{
		public double speed = 64;

		Vector2 velocity;
		Texture2D body;

		public override void Init()
		{
			body = Assets.GetAsset<Texture2D>("Sprites/Icon");
		}

		public override void Update()
		{
			entity.position += velocity;

			var input = new Vector2();

			input.y = System.Convert.ToUInt16(Input.GetButton(Inputs.S)) - System.Convert.ToUInt16(Input.GetButton(Inputs.W));
			input.x = System.Convert.ToUInt16(Input.GetButton(Inputs.D)) - System.Convert.ToUInt16(Input.GetButton(Inputs.A));

			velocity = input.normalized * speed * Time.deltaTime;
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				GFX.Draw(body, new Rect(entity.position, new Vector2(16)), Color.white);
			}
		}
	}
}