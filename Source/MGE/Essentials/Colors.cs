namespace MGE
{
	public class Colors
	{
		static readonly Color _accent = new Color("#FF7504");
		public static Color accent { get => _accent; }

		static readonly Color _text = new Color("#EEE");
		public static Color text { get => _text; }

		static readonly Color _textDark = new Color("#888");
		public static Color textDark { get => _textDark; }

		static readonly Color _black = new Color("#0A0A0A");
		public static Color black { get => _black; }

		static readonly Color _darkGray = new Color("#111");
		public static Color darkGray { get => _darkGray; }

		static readonly Color _gray = new Color("#222");
		public static Color gray { get => _gray; }

		static readonly Color _lightGray = new Color("#333");
		public static Color lightGray { get => _lightGray; }

		static readonly Color _highlight = new Color(1.0f, 0.1f);
		public static Color highlight { get => _highlight; }

		static readonly Color _transBG = new Color(0.05f, 0.8f);
		public static Color transBG { get => _transBG; }

		static readonly Color _transBlack = new Color(0.0f, 0.9f);
		public static Color transBlack { get => _transBlack; }
	}
}