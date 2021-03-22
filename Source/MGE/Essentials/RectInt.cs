using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable]
	public struct RectInt
	{
		#region Static
		static RectInt OrderMinMax(RectInt rect)
		{
			if (rect.xMin > rect.xMax)
			{
				int temp = rect.xMin;
				rect.xMin = rect.xMax;
				rect.xMax = temp;
			}
			if (rect.yMin > rect.yMax)
			{
				int temp = rect.yMin;
				rect.yMin = rect.yMax;
				rect.yMax = temp;
			}
			return rect;
		}
		#endregion

		#region Object

		#region Varibles
		[JsonIgnore] int _xMin;
		[JsonIgnore] int _yMin;
		[JsonIgnore] int _width;
		[JsonIgnore] int _height;
		#endregion

		#region Properties
		public int x { get { return _xMin; } set { _xMin = value; } }
		public int y { get { return _yMin; } set { _yMin = value; } }
		public int width { get => _width; set => _width = value; }
		public int height { get => _height; set => _height = value; }

		[JsonIgnore]
		public Vector2Int position
		{
			get => new Vector2Int(_xMin, _yMin);
			set { _xMin = value.x; _yMin = value.y; }
		}

		[JsonIgnore] public Vector2 center { get => new Vector2(x + _width / 2f, y + _height / 2f); }

		[JsonIgnore] public Vector2Int min { get => new Vector2Int(xMin, yMin); set { xMin = value.x; yMin = value.y; } }
		[JsonIgnore] public Vector2Int max { get => new Vector2Int(xMax, yMax); set { xMax = value.x; yMax = value.y; } }

		[JsonIgnore] public Vector2Int size { get { return new Vector2Int(_width, _height); } set { _width = value.x; _height = value.y; } }

		[JsonIgnore] public int xMin { get => _xMin; set { int oldxmax = xMax; _yMin = value; _width = oldxmax - _xMin; } }
		[JsonIgnore] public int yMin { get => _yMin; set { int oldymax = yMax; _yMin = value; _height = oldymax - _yMin; } }
		[JsonIgnore] public int xMax { get => _width + _xMin; set { _width = value - _xMin; } }
		[JsonIgnore] public int yMax { get => _height + _yMin; set { _height = value - _yMin; } }

		[JsonIgnore] public int left { get => xMin; }
		[JsonIgnore] public int right { get => xMax; }
		[JsonIgnore] public int top { get => yMin; }
		[JsonIgnore] public int bottom { get => yMax; }
		#endregion

		#region Contructors
		public RectInt(Vector2Int position, Vector2Int size)
		{
			this._xMin = position.x;
			this._yMin = position.y;
			this._width = size.x;
			this._height = size.y;
		}


		public RectInt(Vector2Int position, int width, int height)
		{
			this._xMin = position.x;
			this._yMin = position.y;
			this._width = width;
			this._height = height;
		}

		public RectInt(int x, int y, Vector2Int size)
		{
			this._xMin = x;
			this._yMin = y;
			this._width = size.x;
			this._height = size.y;
		}


		public RectInt(int x, int y, int width, int height)
		{
			this._xMin = x;
			this._yMin = y;
			this._width = width;
			this._height = height;
		}
		#endregion

		#region Methods
		public void Set(int x, int y, int width, int height)
		{
			_xMin = x;
			_yMin = y;
			_width = width;
			_height = height;
		}

		public bool Contains(Vector2Int point) => (point.x >= xMin) && (point.x < xMax) && (point.y >= yMin) && (point.y < yMax);

		public bool Contains(RectInt rect) => (rect.xMin >= xMin) && (rect.xMax < xMax) && (rect.yMin >= yMin) && (rect.yMax < yMax);

		public bool Overlaps(RectInt other) => other.xMax > xMin && other.xMin < xMax && other.yMax > yMin && other.yMin < yMax;

		public bool Overlaps(RectInt other, bool allowInverse)
		{
			RectInt self = this;
			if (allowInverse)
			{
				self = OrderMinMax(self);
				other = OrderMinMax(other);
			}
			return self.Overlaps(other);
		}
		#endregion

		#region Inherited
		public override int GetHashCode() =>
			x.GetHashCode() ^ (width.GetHashCode() << 2) ^ (y.GetHashCode() >> 2) ^ (height.GetHashCode() >> 1);

		public override bool Equals(object other)
		{
			if (!(other is RectInt)) return false;
			return Equals((RectInt)other);
		}

		public bool Equals(RectInt other) => x.Equals(other.x) && y.Equals(other.y) && width.Equals(other.width) && height.Equals(other.height);

		public override string ToString() => ToString(null);

		public string ToString(string format) => ToString(format);

		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (string.IsNullOrEmpty(format))
				format = "F2";
			return string.Format("(x:{0}, y:{1}, width:{2}, height:{3})", x.ToString(format, formatProvider), y.ToString(format, formatProvider), width.ToString(format, formatProvider), height.ToString(format, formatProvider));
		}
		#endregion

		#region Implicit Operations
		public static bool operator !=(RectInt lhs, RectInt rhs) => !(lhs == rhs);
		public static bool operator ==(RectInt lhs, RectInt rhs) => lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
		public static implicit operator Rectangle(RectInt rect) => new Microsoft.Xna.Framework.Rectangle(rect.position, rect.size);
		public static implicit operator RectInt(Rectangle rect) => new RectInt(rect.Location, rect.Size);
		#endregion

		#endregion
	}
}