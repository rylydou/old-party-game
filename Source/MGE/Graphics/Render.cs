using Microsoft.Xna.Framework.Graphics;
using System;

namespace MGE.Graphics
{
	// TODO: Make this not suck
	public class Render : IDisposable
	{
		public readonly Color color;

		public RenderTarget2D render;

		public Render(Vector2Int size, Color color)
		{
			this.color = color;
			this.render = new RenderTarget2D(Engine.game.GraphicsDevice, size.x, size.y);

			Engine.game.GraphicsDevice.SetRenderTarget(render);
		}

		public ref RenderTarget2D Done()
		{
			Engine.game.GraphicsDevice.SetRenderTarget(null);
			return ref render;
		}

		public void Done(Rect rect)
		{
			Engine.game.GraphicsDevice.SetRenderTarget(null);

			using (new DrawBatch(transform: null))
				GFX.sb.Draw(render, rect, Color.white);
		}

		public void Dispose()
		{
			Done();

			using (new DrawBatch(transform: null))
				GFX.sb.Draw(render, new Rect(0, 0, Window.renderSize.x, Window.renderSize.y), Color.white);

			render.Dispose();
		}
	}
}