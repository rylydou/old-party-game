using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.Graphics
{
	public class DrawBatch : IDisposable
	{
		public DrawBatch()
		{
			GFX.sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Camera.main.Transformation());
		}

		public DrawBatch(SamplerState samplerState = null, Effect effect = null, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null)
		{
			if (samplerState == null) samplerState = SamplerState.PointClamp;
			if (blendState == null) blendState = BlendState.NonPremultiplied;

			GFX.sb.Begin(sortMode, blendState, samplerState, null, null, effect, Camera.main.Transformation());
		}

		public DrawBatch(SamplerState samplerState = null, Effect effect = null, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, Matrix? transform = null)
		{
			if (samplerState == null) samplerState = SamplerState.PointClamp;
			if (blendState == null) blendState = BlendState.NonPremultiplied;

			GFX.sb.Begin(sortMode, blendState, samplerState, null, null, effect, transform);
		}

		public void Dispose()
		{
			GFX.sb.End();
		}
	}
}