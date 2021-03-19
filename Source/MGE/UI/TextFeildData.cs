using System.Text;

namespace MGE.UI
{
	public struct TextFeildData
	{
		public int cursorIndex;
		public bool isSelecting;
		public int selectionStart;
		public int selectionEnd;

		public float lastTimeTyped;

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
	}
}