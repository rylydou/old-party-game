using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MGE
{
	public struct Rect
	{
		#region Static
		static Rect OrderMinMax(Rect rect)
		{
			if (rect.xMin > rect.xMax)
			{
				double temp = rect.xMin;
				rect.xMin = rect.xMax;
				rect.xMax = temp;
			}
			if (rect.yMin > rect.yMax)
			{
				double temp = rect.yMin;
				rect.yMin = rect.yMax;
				rect.yMax = temp;
			}
			return rect;
		}
		#endregion

		#region Object

		#region Varibles
		[JsonIgnore] double _xMin;
		[JsonIgnore] double _yMin;
		[JsonIgnore] double _width;
		[JsonIgnore] double _height;
		#endregion

		#region Properties
		public double x { get { return _xMin; } set { _xMin = value; } }
		public double y { get { return _yMin; } set { _yMin = value; } }
		public double width { get => _width; set => _width = value; }
		public double height { get => _height; set => _height = value; }

		[JsonIgnore]
		public Vector2 position
		{
			get => new Vector2(_xMin, _yMin);
			set { _xMin = value.x; _yMin = value.y; }
		}

		[JsonIgnore]
		public Vector2 center
		{
			get => new Vector2(x + _width / 2f, y + _height / 2f);
			set { _xMin = value.x - _width / 2f; _yMin = value.y - _height / 2f; }
		}

		[JsonIgnore] public Vector2 min { get => new Vector2(xMin, yMin); set { xMin = value.x; yMin = value.y; } }
		[JsonIgnore] public Vector2 max { get => new Vector2(xMax, yMax); set { xMax = value.x; yMax = value.y; } }

		[JsonIgnore] public Vector2 size { get { return new Vector2(_width, _height); } set { _width = value.x; _height = value.y; } }

		[JsonIgnore] public double xMin { get => _xMin; set { double oldxmax = xMax; _yMin = value; _width = oldxmax - _xMin; } }
		[JsonIgnore] public double yMin { get => _yMin; set { double oldymax = yMax; _yMin = value; _height = oldymax - _yMin; } }
		[JsonIgnore] public double xMax { get => _width + _xMin; set { _width = value - _xMin; } }
		[JsonIgnore] public double yMax { get => _height + _yMin; set { _height = value - _yMin; } }

		[JsonIgnore] public double left { get => xMin; }
		[JsonIgnore] public double right { get => xMax; }
		[JsonIgnore] public double top { get => yMin; }
		[JsonIgnore] public double bottom { get => yMax; }
		#endregion

		#region Contructors
		public Rect(Vector2 position, Vector2 size)
		{
			this._xMin = position.x;
			this._yMin = position.y;
			this._width = size.x;
			this._height = size.y;
		}

		public Rect(double x, double y, double width, double height)
		{
			this._xMin = x;
			this._yMin = y;
			this._width = width;
			this._height = height;
		}
		#endregion

		#region Methods
		public void Set(double x, double y, double width, double height)
		{
			_xMin = x;
			_yMin = y;
			_width = width;
			_height = height;
		}

		public bool Contains(Vector2 point) => (point.x >= xMin) && (point.x < xMax) && (point.y >= yMin) && (point.y < yMax);

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
		public override int GetHashCode() => x.GetHashCode() ^ (width.GetHashCode() << 2) ^ (y.GetHashCode() >> 2) ^ (height.GetHashCode() >> 1);

		public override bool Equals(object other)
		{
			if (!(other is Rect)) return false;

			return Equals((Rect)other);
		}

		public bool Equals(Rect other) => x.Equals(other.x) && y.Equals(other.y) && width.Equals(other.width) && height.Equals(other.height);

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
		public static bool operator !=(Rect lhs, Rect rhs) => !(lhs == rhs);
		public static bool operator ==(Rect lhs, Rect rhs) => lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
		public static implicit operator Rectangle(Rect rect) => new Microsoft.Xna.Framework.Rectangle(rect.position, rect.size);
		public static implicit operator Rect(Rectangle rect) => new Rect(rect.Location, rect.Size);
		#endregion

		#endregion
	}
}