using Microsoft.Xna.Framework.Graphics;
using System;

namespace MGE.Graphics
{
	// TODO: Make this not suck
	public class Render : IDisposable
	{
		public RenderTarget2D render;
		public bool drawOnDispose;

		public Render(Vector2Int size, bool drawOnDispose = true)
		{
			this.render = new RenderTarget2D(Engine.game.GraphicsDevice, size.x, size.y);
			this.drawOnDispose = drawOnDispose;

			Engine.game.GraphicsDevice.SetRenderTarget(render);
		}

		public Render(Rect size, bool drawOnDispose = true)
		{
			this.render = new RenderTarget2D(Engine.game.GraphicsDevice, (int)size.width, (int)size.height);
			this.drawOnDispose = drawOnDispose;

			Engine.game.GraphicsDevice.SetRenderTarget(render);
		}

		public ref RenderTarget2D Done()
		{
			Engine.game.GraphicsDevice.SetRenderTarget(null);
			return ref render;
		}

		public void Dispose()
		{
			Done();

			GFX.sb.Begin(samplerState: SamplerState.PointClamp);
			GFX.sb.Draw(render, new Rect(0, 0, Window.renderSize.x, Window.renderSize.y), Color.white);
			GFX.sb.End();

			render.Dispose();
		}
	}
}