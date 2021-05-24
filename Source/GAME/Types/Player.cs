using GAME.Components;
using MGE;

namespace GAME
{
	public class Player
	{
		public PlayerControls controls;
		string _skin;
		public string skin { get => _skin; set { _skin = value; _icon = null; _iconDead = null; } }
		public Color? _color;
		public Color color
		{
			get
			{
				if (!_color.HasValue)
					_color = Setup.playerColors[Math.Clamp(GameSettings.players.FindIndex(x => x == this), Setup.playerColors.Length - 1)];
				return _color.Value;
			}
		}
		Texture _icon;
		public Texture icon
		{
			get
			{
				if (_icon is null)
					_icon = Assets.GetAsset<Texture>($"Players/{skin}/Icon");
				return _icon;
			}
		}
		Texture _iconDead;
		public Texture iconDead
		{
			get
			{
				if (_iconDead is null)
					_iconDead = Assets.GetAsset<Texture>($"Players/{skin}/Icon Dead");
				return _iconDead;
			}
		}

		public bool READY = false;

		public int points = 0;
		public byte wins = 0;
		public ushort kills = 0;
		public ushort deaths = 0;

		public CPlayer player = null;

		public Player(EController controlsIndex, string skin = null)
		{
			if (string.IsNullOrEmpty(skin))
				skin = Setup.skins.Random();

			controls = GameSettings.GetControls(controlsIndex);
			this.skin = skin;
		}

		public void Reset()
		{
			READY = false;
			points = 0;
			kills = 0;
			deaths = 0;
		}
	}
}