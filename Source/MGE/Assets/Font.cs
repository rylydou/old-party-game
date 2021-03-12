using System.Collections.Generic;
using System.Text;
using MGE.Graphics;
using Newtonsoft.Json;

namespace MGE
{
	public class Font
	{
		[JsonIgnore] public Texture texture;

		public int offset;
		public char defualtChar;
		public char spIndicator;
		public Vector2Int charSize;
		public Vector2 charPaddingSize;
		public List<string> spChars;

		public Font() { }

		public void DrawText(string text, Vector2 position, Color color, float scale = 1.0f)
		{
			var drawIndex = 0;
			var isSp = false;
			var spName = new StringBuilder();

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

						GFX.Draw(
							texture,
							new Rect(spChar * charSize.x, charSize.y, charSize.x, charSize.y),
							new Rect(position.x + charPaddingSize.x * drawIndex * scale, position.y, charSize.x * scale, charSize.y * scale),
							color
						);
						drawIndex++;

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
					GFX.Draw(
						texture,
						new Rect((text[i] + offset) * charSize.x, 0, charSize.x, charSize.y),
						new Rect(position.x + charPaddingSize.x * drawIndex * scale, position.y, charSize.x * scale, charSize.y * scale), color
					);
					drawIndex++;
				}
			}
		}
	}
}