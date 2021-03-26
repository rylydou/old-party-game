using System.Collections.Generic;
using System.Text;
using MGE.Graphics;
using Newtonsoft.Json;

namespace MGE
{
	[System.Serializable, JsonObject(MemberSerialization.OptIn)]
	public class Font
	{
		public Texture texture;

		[JsonProperty] public int offset;
		[JsonProperty] public char defualtChar;
		[JsonProperty] public char spIndicator;
		[JsonProperty] public Vector2Int charSize;
		[JsonProperty] public Vector2 charPaddingSize;
		[JsonProperty] public List<string> spChars;

		public Font() { }

		public void DrawText(string text, Vector2 position, Color color, float scale = 1.0f)
		{
			DrawText(text, new Rect(position, Measure(text, scale)), color, scale);
		}

		public void DrawText(string text, Rect rect, Color color, float scale = 1.0f, TextAlignment alignment = TextAlignment.Left)
		{
			if (text == null) text = string.Empty;

			var drawIndex = 0;
			var isSp = false;
			var spName = new StringBuilder();
			var textSize = Measure(text, scale);
			var startingPos = Vector2.zero;

			switch (alignment)
			{
				case TextAlignment.Left:
					startingPos.x = rect.position.x;
					break;
				case TextAlignment.Center:
					startingPos.x = rect.position.x / 2 + ((rect.position.x + rect.width) / 2 - (textSize.x / 2));
					break;
				case TextAlignment.Right:
					startingPos.x = rect.position.x + rect.width - textSize.x;
					break;
			}
			startingPos.y = rect.position.y + (rect.height / 2 - (textSize.y / 2));

			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == spIndicator)
				{
					if (isSp == true)
					{
						isSp = false;

						var spCharName = spName.ToString();
						var spChar = spChars.FindIndex((x) => x == spCharName);
						spChar = Math.Max(spChar, 0);

						DrawChar(spChar, true, startingPos, ref drawIndex, scale, color);

						spName.Clear();
					}
					else
						isSp = true;

					continue;
				}

				if (isSp)
					spName.Append(text[i]);
				else
				{
					DrawChar(text[i] + offset, false, startingPos, ref drawIndex, scale, color);
				}
			}
		}

		public void DrawChar(int charPos, bool isSp, Vector2 startingPos, ref int drawIndex, float scale, Color color)
		{
			GFX.Draw(
				texture,
				new Rect(charPos * charSize.x, isSp ? charSize.y : 0, charSize.x, charSize.y),
				new Rect(startingPos.x + charPaddingSize.x * drawIndex * scale, startingPos.y, charSize.x * scale, charSize.y * scale),
				color
			);
			drawIndex++;
		}

		public Vector2 Measure(string text, float scale = 1)
		{
			if (text == null) text = string.Empty;
			return new Vector2(text.Length * charPaddingSize.x * scale, charPaddingSize.y * scale);
		}
	}
}