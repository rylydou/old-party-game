using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.Graphics
{
	public class DrawBatch : IDisposable
	{
		public static void Start()
		{
			GFX.sb.Begin();
		}

		public static void End()
		{
			GFX.sb.End();
		}

		public DrawBatch()
		{
			GFX.sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Camera.Transformation());
		}

		public DrawBatch(SamplerState sampler = null, Effect effect = null, SpriteSortMode sort = SpriteSortMode.Deferred, BlendState blend = null, Matrix? transform = null)
		{
			if (sampler == null) sampler = SamplerState.PointClamp;
			if (blend == null) blend = BlendState.NonPremultiplied;

			GFX.sb.Begin(sort, blend, sampler, DepthStencilState.None, RasterizerState.CullNone, effect, transform);
		}

		public void Dispose()
		{
			GFX.sb.End();
		}
	}
}