using System.Text;

namespace MGE.UI
{
	[System.Serializable]
	public struct TextFeildData
	{
		[System.NonSerialized] public bool isActive;

		[System.NonSerialized] public int cursorIndex;
		[System.NonSerialized] public bool isSelecting;
		[System.NonSerialized] public int selectionStart;
		[System.NonSerialized] public int selectionEnd;

		[System.NonSerialized] public float lastTimeTyped;
		[System.NonSerialized] public float blinkOffset;

		StringBuilder _textBuilder;
		public StringBuilder textBuilder
		{
			get
			{
				if (_textBuilder == null)
					_textBuilder = new StringBuilder();
				return _textBuilder;
			}
			set => _textBuilder = value;
		}
		public string text { get => textBuilder.ToString(); }

		public TextFeildData(string text)
		{
			isActive = false;
			cursorIndex = text.Length;
			isSelecting = false;
			selectionStart = 0;
			selectionEnd = 0;
			lastTimeTyped = 0;
			blinkOffset = 0;

			_textBuilder = new StringBuilder(text);
		}

		public void Typed()
		{
			lastTimeTyped = Time.unscaledTime;
			blinkOffset = Math.Round(Time.unscaledTime) - Time.unscaledTime;
		}
	}
}