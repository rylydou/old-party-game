using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public struct Rect
	{
		#region Static

		#region Constants
		static Rect _zero = new Rect(0, 0, 0, 0);
		public static Rect zero { get => _zero; }
		#endregion

		#region Methods
		static Rect OrderMinMax(Rect rect)
		{
			if (rect.xMin > rect.xMax)
			{
				float temp = rect.xMin;
				rect.xMin = rect.xMax;
				rect.xMax = temp;
			}
			if (rect.yMin > rect.yMax)
			{
				float temp = rect.yMin;
				rect.yMin = rect.yMax;
				rect.yMax = temp;
			}
			return rect;
		}

		#endregion

		#endregion

		#region Object

		#region Varibles
		float _xMin;
		float _yMin;
		float _width;
		float _height;
		#endregion

		#region Properties
		[JsonProperty] public float x { get => _xMin; set => _xMin = value; }
		[JsonProperty] public float y { get => _yMin; set => _yMin = value; }
		[JsonProperty] public float width { get => _width; set => _width = value; }
		[JsonProperty] public float height { get => _height; set => _height = value; }

		public Vector2 position
		{
			get => new Vector2(_xMin, _yMin);
			set { _xMin = value.x; _yMin = value.y; }
		}
		public Vector2 size { get => new Vector2(_width, _height); set { _width = value.x; _height = value.y; } }

		public Vector2 center
		{
			get => new Vector2(x + _width / 2f, y + _height / 2f);
			set { _xMin = value.x - _width / 2f; _yMin = value.y - _height / 2f; }
		}

		public Vector2 min { get => new Vector2(xMin, yMin); set { xMin = value.x; yMin = value.y; } }
		public Vector2 max { get => new Vector2(xMax, yMax); set { xMax = value.x; yMax = value.y; } }

		public float xMin { get => _xMin; set { var oldxmax = xMax; _yMin = value; _width = oldxmax - _xMin; } }
		public float left { get => xMin; set => xMin = left; }
		public float yMin { get => _yMin; set { var oldymax = yMax; _yMin = value; _height = oldymax - _yMin; } }
		public float top { get => yMin; set => yMin = top; }
		public float xMax { get => _width + _xMin; set => _width = value - _xMin; }
		public float right { get => xMax; set => xMax = right; }
		public float yMax { get => _height + _yMin; set => _height = value - _yMin; }
		public float bottom { get => yMax; set => yMax = bottom; }

		public Vector2 topLeft { get => new Vector2(xMin, yMin); set { xMin = value.x; yMin = value.y; } }
		public Vector2 topRight { get => new Vector2(xMax, yMin); set { xMax = value.x; yMin = value.y; } }
		public Vector2 bottomLeft { get => new Vector2(xMin, yMax); set { xMin = value.x; yMax = value.y; } }
		public Vector2 bottomRight { get => new Vector2(xMax, yMax); set { xMax = value.x; yMax = value.y; } }
		#endregion

		#region Contructors
		public Rect(float x, float y, float width, float height)
		{
			this._xMin = x;
			this._yMin = y;
			this._width = width;
			this._height = height;
		}

		public Rect(float x, float y, float size)
		{
			this._xMin = x;
			this._yMin = y;
			this._width = size;
			this._height = size;
		}

		public Rect(Vector2 position, float size)
		{
			this._xMin = position.x;
			this._yMin = position.y;
			this._width = size;
			this._height = size;
		}

		public Rect(Vector2 position, float width, float height)
		{
			this._xMin = position.x;
			this._yMin = position.y;
			this._width = width;
			this._height = height;
		}

		public Rect(float x, float y, Vector2 size)
		{
			this._xMin = x;
			this._yMin = y;
			this._width = size.x;
			this._height = size.y;
		}

		public Rect(Vector2 position, Vector2 size)
		{
			this._xMin = position.x;
			this._yMin = position.y;
			this._width = size.x;
			this._height = size.y;
		}
		#endregion

		#region Methods
		public bool Contains(Vector2 point) => (point.x >= xMin) && (point.x < xMax) && (point.y >= yMin) && (point.y < yMax);
		public bool Contains(Rect rect) => (rect.xMin >= xMin) && (rect.xMax < xMax) && (rect.yMin >= yMin) && (rect.yMax < yMax);

		public bool Overlaps(Rect other) => other.xMax > xMin && other.xMin < xMax && other.yMax > yMin && other.yMin < yMax;
		public bool Overlaps(Rect other, bool allowInverse)
		{
			Rect self = this;
			if (allowInverse)
			{
				self = OrderMinMax(self);
				other = OrderMinMax(other);
			}
			return self.Overlaps(other);
		}

		public static Vector2 NormalizedToPoint(Rect rectangle, Vector2 normalizedRectCoordinates)
		{
			return new Vector2(
				Math.Lerp(rectangle.x, rectangle.xMax, normalizedRectCoordinates.x),
				Math.Lerp(rectangle.y, rectangle.yMax, normalizedRectCoordinates.y)
			);
		}

		public static Vector2 PointToNormalized(Rect rectangle, Vector2 point)
		{
			return new Vector2(
				Math.InverseLerp(rectangle.x, rectangle.xMax, point.x),
				Math.InverseLerp(rectangle.y, rectangle.yMax, point.y)
			);
		}

		#endregion

		#region Inherited
		public override int GetHashCode() =>
			x.GetHashCode() ^ (width.GetHashCode() << 2) ^ (y.GetHashCode() >> 2) ^ (height.GetHashCode() >> 1);

		public override bool Equals(object other)
		{
			if (!(other is Rect)) return false;
			return Equals((Rect)other);
		}

		public bool Equals(Rect other) => x.Equals(other.x) && y.Equals(other.y) && width.Equals(other.width) && height.Equals(other.height);

		public override string ToString() => ToString(null);

		public string ToString(string format) => ToString(format, null);

		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (string.IsNullOrEmpty(format))
				format = "F2";
			return string.Format("(x:{0}, y:{1}, width:{2}, height:{3})", x.ToString(format, formatProvider), y.ToString(format, formatProvider), width.ToString(format, formatProvider), height.ToString(format, formatProvider));
		}
		#endregion

		#region Implicit Operations
		public static bool operator !=(Rect lhs, Rect rhs) => !(lhs == rhs);
		public static bool operator ==(Rect lhs, Rect rhs) => lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
		public static implicit operator Rect(RectInt rect) => new Rect(rect.x, rect.y, rect.width, rect.height);
		public static implicit operator RectInt(Rect rect) => new RectInt((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
		public static implicit operator Rectangle(Rect rect) => new Microsoft.Xna.Framework.Rectangle(rect.position, rect.size);
		public static implicit operator Rect(Rectangle rect) => new Rect(rect.Location, rect.Size);
		#endregion

		#endregion
	}
}