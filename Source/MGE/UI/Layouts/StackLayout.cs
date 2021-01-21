using System;
using Microsoft.Xna.Framework.Graphics;
using MGE;

namespace MGE.UI.Layouts
{
	public class StackLayout : EssentialVars, IDisposable
	{
		readonly Vector2Int _position;
		readonly int _sizePerElement;
		readonly bool _isHorizontal;

		Vector2 _offset;
		bool _isFirstElement = true;

		public StackLayout(Vector2Int position, int sizePerElement, bool isHorizontal)
		{
			_position = position;
			_offset = position;
			_sizePerElement = sizePerElement;
			_isHorizontal = isHorizontal;

			sb.Begin(blendState: BlendState.NonPremultiplied, samplerState: SamplerState.LinearClamp);
		}

		public Vector2Int AddElement(int elementSize = -1)
		{
			if (elementSize < 0) elementSize = _sizePerElement;

			if (!_isFirstElement)
			{
				switch (_isHorizontal)
				{
					case true:
						_offset.x += elementSize;
						break;
					case false:
						_offset.y += elementSize;
						break;
				}
			}
			_isFirstElement = false;

			return _offset;
		}

		public void Dispose()
		{
			sb.End();
		}
	}
}