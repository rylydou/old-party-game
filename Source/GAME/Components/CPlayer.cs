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
		public double jumpForce = -2;
		public Vector2 gravity = new Vector2(0, 8);

		Vector2 velocity;
		Texture2D body;

		public override void Init()
		{
			body = Assets.GetAsset<Texture2D>("Sprites/Player");
		}

		public override void Update()
		{
			velocity += gravity * Time.deltaTime;

			velocity.x = (System.Convert.ToUInt16(Input.GetButton(Inputs.D)) - System.Convert.ToUInt16(Input.GetButton(Inputs.A))) * speed * Time.deltaTime;

			if (Input.GetButtonPress(Inputs.Space))
				velocity.y = jumpForce;

			if ((entity.position + velocity).y + body.Height > Window.gameSize.y)
			{
				velocity.y = 0;
			}

			entity.position += velocity;
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				GFX.Draw(body, entity.position, Color.white);
			}
		}
	}
}