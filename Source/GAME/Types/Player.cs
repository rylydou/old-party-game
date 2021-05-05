using GAME.Components;
using MGE;

namespace GAME.Types
{
	public class Player
	{
		public static float timeToRespawn = 3.0f;

		public static Color[] colors = new Color[]
		{
			new Color("#F73"),
			new Color("#0AE"),
			new Color("#7b0"),
			new Color("#fb0"),
			new Color("#025"),
		};

		public int index;
		string _skin;
		public string skin { get => _skin; set { _skin = value; _icon = null; } }
		public Color color { get => colors[Math.Clamp(Main.current.players.FindIndex(x => x == this), colors.Length)]; }
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

		public PlayerControls controls;
		public CPlayer player;

		public float timeRespawing;

		public Player(int index, string skin = "_Default")
		{
			this.index = index;
			this.skin = skin;
			this.controls = new PlayerControls(index);
		}
	}
}