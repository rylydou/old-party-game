using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.Graphics
{
	public class DrawBatch : EssentialVars, IDisposable
	{
		public DrawBatch()
		{
			sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, camera.Transformation());
		}

		public DrawBatch(SamplerState samplerState = null, Effect effect = null, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null)
		{
			sb.Begin(sortMode, blendState, samplerState, null, null, effect, camera.Transformation());
		}

		public DrawBatch(SamplerState samplerState = null, Effect effect = null, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, Matrix? transform = null)
		{
			sb.Begin(sortMode, blendState, samplerState, null, null, effect, transform);
		}

		public void Dispose()
		{
			sb.End();
		}
	}
}