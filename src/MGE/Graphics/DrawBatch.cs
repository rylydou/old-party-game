using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.Graphics
{
	public class DrawBatch : EssentialVars, IDisposable
	{
		public DrawBatch(SamplerState samplerState = null, Effect effect = null, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, Matrix? transformMatrix = null)
		{
			sb.Begin(sortMode, blendState, samplerState, null, null, effect, transformMatrix);
		}

		public void Dispose()
		{
			sb.End();
		}
	}
}