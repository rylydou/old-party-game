using MGE;
using MGE.Graphics;

namespace GAME.Components
{
	public class CBobber : CProjectile
	{
		public CBobber(DamageInfo info, string basePath) : base(info, basePath) { }

		public override void Draw()
		{
			base.Draw();

			GFX.DrawLine(entity.position + 0.5f, info.doneBy.entity.position + 0.5f, new Color(0.8f), 1f / 16);
		}
	}
}