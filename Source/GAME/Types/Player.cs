using GAME.Components;
using MGE;

namespace GAME
{
	public class Player
	{
		public static float timeToRespawn = 0.5f;

		public static Color[] colors = new Color[]
		{
			new Color("#FF595E"),
			new Color("#269BE3"),
			new Color("#8AC926"),
			new Color("#FFCA3A"),
			new Color("#7E5DAC"),
		};

		public sbyte index;
		string _skin;
		public string skin { get => _skin; set { _skin = value; _icon = null; _iconDead = null; } }
		Color? _color;
		public Color color
		{
			get
			{
				if (!_color.HasValue)
					_color = colors[Math.Clamp(GameSettings.current.players.FindIndex(x => x == this), colors.Length - 1)];
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

		public PlayerControls controls { get => GameSettings.current.controllers[(EController)index]; }
		public CPlayer player = null;

		public float timeRespawing = 0.0f;

		public Player(sbyte index, string skin = "_Default")
		{
			this.index = index;
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