using System;
using MGE.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace MGE.UI.Layouts
{
	public class StackLayout : IDisposable
	{
		public readonly Vector2 position;
		float _sizePerElement;
		bool _isHorizontal;

		Vector2 _offset;
		bool _isFirstElement = true;

		public float currentSize;
		public Vector2 newElement { get => AddElement(); }
		public Vector2 currentElement { get => _offset; }

		public StackLayout(Vector2 position, float sizePerElement, bool isHorizontal = false)
		{
			this.position = position;
			_offset = position;
			_sizePerElement = sizePerElement;
			_isHorizontal = isHorizontal;
			currentSize = sizePerElement;
		}

		public Vector2Int AddElement(float elementSize = -1)
		{
			if (elementSize < 0) elementSize = _sizePerElement;
			currentSize = elementSize;

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

		public void ChangeDirection(bool isHorizontal)
		{
			_isHorizontal = isHorizontal;
		}

		public void SetDefualtSize(float size)
		{
			_sizePerElement = size;
		}

		public void Dispose()
		{
		}
	}
}