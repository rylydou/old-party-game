using System;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public struct Color
	{
		#region Static

		#region Constants
		static readonly Color _nullColor = new Color(1.0f, 0.0f, 1.0f);
		public static Color nullColor { get => _nullColor; }

		static readonly Color _white = new Color(1.0f);
		public static Color white { get => _white; }
		static readonly Color _gray = new Color(0.5f);
		public static Color gray { get => _gray; }
		static readonly Color _black = new Color(0.0f);
		public static Color black { get => _black; }
		static readonly Color _red = new Color(1.0f, 0.0f, 0.0f);
		public static Color red { get => _red; }
		static readonly Color _yellow = new Color(1.0f, 1.0f, 0.0f);
		public static Color yellow { get => _yellow; }
		static readonly Color _green = new Color(0.0f, 1.0f, 0.0f);
		public static Color cyan { get => _cyan; }
		static readonly Color _cyan = new Color(0.0f, 1.0f, 1.0f);
		public static Color green { get => _green; }
		static readonly Color _blue = new Color(0.0f, 0.0f, 1.0f);
		public static Color blue { get => _blue; }
		static readonly Color _magenta = new Color(1.0f, 0.0f, 1.0f);
		public static Color magenta { get => _magenta; }
		static readonly Color _violet = new Color(0.5f, 0.0f, 1.0f);
		public static Color violet { get => _violet; }
		static readonly Color _clear = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		public static Color clear { get => _clear; }

		public const float epsilon = 0.000010f;
		public const float epsilonNormalSqrt = 1e-150f;
		#endregion

		#region Methods
		public static Color FromHex(string hex) => new Color(hex);
		public static string ToHex(Color color, int length = 8) => color.ToHex(length);

		public static Color Lerp(Color a, Color b, float t)
		{
			t = (float)Math.Clamp01(t);
			return new Color(
				a.r + (b.r - a.r) * t,
				a.g + (b.g - a.g) * t,
				a.b + (b.b - a.b) * t,
				a.a + (b.a - a.a) * t
			);
		}

		public static Color LerpUnclamped(Color a, Color b, float t)
		{
			return new Color(
				a.r + (b.r - a.r) * t,
				a.g + (b.g - a.g) * t,
				a.b + (b.b - a.b) * t,
				a.a + (b.a - a.a) * t
			);
		}
		#endregion

		#endregion

		#region Object

		#region Variables
		public float r;
		public float g;
		public float b;
		public float a;

		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return r;
					case 1: return g;
					case 2: return b;
					case 3: return a;
					default: throw new IndexOutOfRangeException($"Invalid Color index of {index}!");
				}
			}

			set
			{
				switch (index)
				{
					case 0: r = value; break;
					case 1: g = value; break;
					case 2: b = value; break;
					case 3: a = value; break;
					default: throw new IndexOutOfRangeException($"Invalid Color index of {index}!");
				}
			}
		}
		#endregion

		#region Perams
		[JsonProperty] public string hex { get => ToHex(); set => this = FromHex(value); }
		public float grayscale { get => 0.299f * r + 0.587f * g + 0.114f * b; }
		public float max { get => (float)Math.Max(Math.Max(r, g), b); }
		public Color opaque { get => ChangeAlpha(1f); }
		public Color inverted { get => new Color(1 - r, 1 - g, 1 - b, a); }
		public Color readableColor { get => inverted.grayscale > 0.5f ? Color.white : Color.black; }
		#endregion

		#region Constructors
		public Color(string hex)
		{
			hex = hex.Replace("#", string.Empty);
			switch (hex.Length)
			{
				// Technically isn't a thing but it's nice to have
				case 1:
					this = new Color((float)Convert.ToUInt16(hex.Substring(0, 1), 16) / 15.0f);
					return;
				case 3:
					this.r = (float)Convert.ToUInt16(hex.Substring(0, 1), 16) / 15.0f;
					this.g = (float)Convert.ToUInt16(hex.Substring(1, 1), 16) / 15.0f;
					this.b = (float)Convert.ToUInt16(hex.Substring(2, 1), 16) / 15.0f;
					this.a = 1.0f;
					return;
				case 4:
					this.r = (float)Convert.ToUInt16(hex.Substring(0, 1), 16) / 15.0f;
					this.g = (float)Convert.ToUInt16(hex.Substring(1, 1), 16) / 15.0f;
					this.b = (float)Convert.ToUInt16(hex.Substring(2, 1), 16) / 15.0f;
					this.a = (float)Convert.ToUInt16(hex.Substring(3, 1), 16) / 15.0f;
					return;
				case 6:
					this.r = (float)Convert.ToUInt16(hex.Substring(0, 2), 16) / 255.0f;
					this.g = (float)Convert.ToUInt16(hex.Substring(2, 2), 16) / 255.0f;
					this.b = (float)Convert.ToUInt16(hex.Substring(4, 2), 16) / 255.0f;
					this.a = 1.0f;
					return;
				case 8:
					this.r = (float)Convert.ToUInt16(hex.Substring(0, 2), 16) / 255.0f;
					this.g = (float)Convert.ToUInt16(hex.Substring(2, 2), 16) / 255.0f;
					this.b = (float)Convert.ToUInt16(hex.Substring(4, 2), 16) / 255.0f;
					this.a = (float)Convert.ToUInt16(hex.Substring(6, 2), 16) / 255.0f;
					return;
			}

			this = Color.nullColor;
			Logger.LogError($"Color '{hex}' can not be parsed");
		}

		public Color(float value)
		{
			this.r = value;
			this.g = value;
			this.b = value;
			this.a = 1.0f;
		}

		public Color(float value, float alpha)
		{
			this.r = value;
			this.g = value;
			this.b = value;
			this.a = alpha;
		}

		public Color(float r, float g, float b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 1.0f;
		}

		public Color(float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
		#endregion

		#region Methods
		public string ToHex(int length = 8)
		{
			switch (length)
			{
				// Technically isn't a thing but it's nice to have
				case 1:
					if (r == g && g == b)
						return string.Format("#{0:X1}", (int)r * 15);
					break;
				case 3:
					return string.Format("#{0:X1}{1:X1}{2:X1}", (int)r * 15, (int)g * 15, (int)b * 15);
				case 4:
					return string.Format("#{0:X1}{1:X1}{2:X1}{3:X1}", (int)r * 15, (int)g * 15, (int)b * 15, (int)a * 15);
				case 6:
					return string.Format("#{0:X2}{1:X2}{2:X2}", (int)r * 255, (int)g * 255, (int)b * 255);
				case 8:
					return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", (int)r * 255, (int)g * 255, (int)b * 255, (int)a * 255);
			}

			Logger.LogError($"Color '{this.ToString()}' can not be convered to hex string a length of {length}!");
			return string.Empty;
		}

		public Color ChangeAlpha(float a) => new Color(this.r, this.g, this.b, a);
		#endregion

		#region Implicit
		public static Color operator +(Color left, Color right) => new Color(left.r + right.r, left.g + right.g, left.b + right.b, left.b + right.b);
		public static Color operator -(Color left, Color right) => new Color(left.r - right.r, left.g - right.g, left.b - right.b, left.b - right.b);
		public static Color operator *(Color left, Color right) => new Color(left.r * right.r, left.g * right.g, left.b * right.b, left.b * right.b);
		public static Color operator /(Color left, Color right) => new Color(left.r / right.r, left.g / right.g, left.b / right.b, left.b / right.b);

		public static Color operator -(Color color) => new Color(-color.r, -color.g, -color.b, -color.b);

		public static Color operator *(Color left, float right) => new Color(left.r * right, left.g * right, left.b * right, left.b * right);
		public static Color operator *(float left, Color right) => new Color(right.r * left, right.g * left, right.b * left, right.b * left);
		public static Color operator /(Color left, float right) => new Color(left.r / right, left.g / right, left.b / right, left.b / right);

		public static bool operator ==(Color left, Color right)
		{
			float diff_r = left.r - right.r;
			float diff_g = left.g - right.g;
			float diff_b = left.b - right.b;
			float diff_a = left.a - right.a;
			float sqrmag = diff_r * diff_r + diff_g * diff_g + diff_b * diff_b + diff_a * diff_a;
			return sqrmag < epsilon * epsilon;
		}

		public static bool operator !=(Color left, Color right) => !(left == right);

		public static implicit operator Microsoft.Xna.Framework.Color(Color color) => new Microsoft.Xna.Framework.Color(color.r, color.g, color.b, color.a);
		public static implicit operator Color(Microsoft.Xna.Framework.Color color) => new Color(color.R, color.G, color.B, color.A);
		public static implicit operator Microsoft.Xna.Framework.Vector3(Color color) => new Microsoft.Xna.Framework.Vector3(color.r, color.g, color.b);
		public static implicit operator Color(Microsoft.Xna.Framework.Vector3 vector) => new Microsoft.Xna.Framework.Vector3(vector.X, vector.Y, vector.Z);
		public static implicit operator Microsoft.Xna.Framework.Vector4(Color color) => new Microsoft.Xna.Framework.Vector4(color.r, color.g, color.b, color.a);
		public static implicit operator Color(Microsoft.Xna.Framework.Vector4 vector) => new Microsoft.Xna.Framework.Vector4(vector.X, vector.Y, vector.Z, vector.W);

		public static implicit operator System.Drawing.Color(Color color) =>
			System.Drawing.Color.FromArgb((int)Math.Clamp01(color.a) * 255, (int)Math.Clamp01(color.r) * 255, (int)Math.Clamp01(color.g) * 255, (int)Math.Clamp01(color.b) * 255);
		public static implicit operator Color(System.Drawing.Color color) =>
			new Color((float)color.R / 255, (float)color.G / 255, (float)color.B / 255, (float)color.A / 255);
		#endregion

		#region Inherited
		public override string ToString() => $"({r},{g},{b},{a})";

		public string ToString(string format)
		{
			if (format.StartsWith('#'))
				return string.Format(format, (int)r * 255, (int)g * 255, (int)b * 255, (int)a * 255);
			return string.Format(format, r, g, b, a);
		}

		public override bool Equals(object obj) => obj is Color color && r == color.r && g == color.g && b == color.b && a == color.a;

		public override int GetHashCode() => HashCode.Combine(r, g, b, a);
		#endregion

		#endregion
	}
}