using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace MGE.Graphics
{
	public class Render : EssentialVars, IDisposable
	{
		public RenderTarget2D render;
		public bool drawOnDispose;

		public Render(Vector2Int size, bool drawOnDispose = false)
		{
			this.render = new RenderTarget2D(graphicsDevice, size.x, size.y);
			this.drawOnDispose = drawOnDispose;

			graphicsDevice.SetRenderTarget(render);
		}

		public Render(Rect size, bool drawOnDispose = false)
		{
			this.render = new RenderTarget2D(graphicsDevice, (int)size.width, (int)size.height);
			this.drawOnDispose = drawOnDispose;

			graphicsDevice.SetRenderTarget(render);
		}

		public ref RenderTarget2D Done()
		{
			graphicsDevice.SetRenderTarget(null);
			return ref render;
		}

		public void Dispose()
		{
			Done();

			sb.Begin(samplerState: SamplerState.PointClamp);
			sb.Draw(render, new Rect(0, 0, Window.renderSize.x, Window.renderSize.y), Color.white);
			sb.End();

			render.Dispose();
		}
	}
}