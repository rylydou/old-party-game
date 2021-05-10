using GAME.Components;
using MGE;

namespace GAME
{
	public class Player
	{
		public static float timeToRespawn = 0.5f;

		public static Color[] colors = new Color[]
		{
			new Color("#FF6B12"),
			new Color("#30ADF2"),
			new Color("#34CD34"),
			new Color("#FCBD03"),
			new Color("#003D71"),
		};

		public sbyte index;
		string _skin;
		public string skin { get => _skin; set { _skin = value; _icon = null; } }
		Color? _color;
		public Color color
		{
			get
			{
				if (!_color.HasValue)
					_color = colors[Math.Clamp(GameSettings.current.players.FindIndex(x => x == this), colors.Length)];
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

		public int points = 0;
		public byte wins = 0;
		public ushort kills = 0;
		public ushort deaths = 0;

		public PlayerControls controls { get => GameSettings.current.controllers[(EController)index]; }
		public CPlayer player;

		public float timeRespawing;

		public Player(sbyte index, string skin = "_Default")
		{
			this.index = index;
			this.skin = skin;
		}
	}
}