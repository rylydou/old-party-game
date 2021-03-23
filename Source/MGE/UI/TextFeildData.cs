using System;
using System.Runtime.Serialization;
using System.Text;

namespace MGE.UI
{
	[System.Serializable]
	public struct TextFeildData
	{
		[NonSerialized] public bool isActive;

		[NonSerialized] public int cursorIndex;
		[NonSerialized] public bool isSelecting;
		[NonSerialized] public int selectionStart;
		[NonSerialized] public int selectionEnd;

		[NonSerialized] public float lastTimeTyped;
		[NonSerialized] public float blinkOffset;

		[NonSerialized] StringBuilder _textBuilder;
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
		string _text;
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

			_text = string.Empty;
		}

		public void Typed()
		{
			lastTimeTyped = Time.unscaledTime;
			blinkOffset = Math.Round(Time.unscaledTime) - Time.unscaledTime;
		}

		[OnSerializing]
		public void OnSerializing()
		{
			_text = text;
		}

		[OnDeserialized]
		public void OnDeserialized()
		{
			textBuilder = new StringBuilder(_text);
		}
	}
}