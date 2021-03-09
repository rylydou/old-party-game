using Microsoft.Xna.Framework.Graphics;
using MGE;
using MGE.ECS;
using MGE.InputSystem;
using MGE.Graphics;

namespace GAME.Components
{
	public class CPlayer : Component
	{
		CRigidbody rb;
		Texture2D body;

		public override void Init()
		{
			body = Assets.GetAsset<Texture2D>("Sprites/Player");

			rb = entity.GetComponent<CRigidbody>();

			rb.position = new Vector2(CStage.current.tileSize * 4);
		}

		public override void Update()
		{
			rb.velocity.x += ((Input.GetButton(Inputs.D) ? 1.0 : 0.0) - (Input.GetButton(Inputs.A) ? 1.0 : 0.0)) * Time.deltaTime;

			if (Input.GetButtonPress(Inputs.Space))
				rb.velocity.y = -4.0 * Time.deltaTime;
		}

		public override void Draw()
		{
			using (new DrawBatch())
			{
				GFX.Draw(body, entity.position, new Color(1, 0.5f));
			}
		}
	}
}