using GAME.Components;
using MGE;

namespace GAME.Types
{
	public class Player
	{
		public static float timeToRespawn = 5.0f;

		public static Color[] colors = new Color[]
		{
			new Color("#f6511d"),
			new Color("#00a6ed"),
			new Color("#7fb800"),
			new Color("#ffb400"),
			new Color("#0d2c54"),
		};

		public int index;
		string _skin;
		public string skin { get => _skin; set { _skin = value; _icon = null; } }
		public Color color { get => colors[Math.Clamp(index, 4)]; }
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

		public int points = 0;
		public byte wins = 0;
		public ushort kills = 0;
		public ushort deaths = 0;

		public PlayerControls controls;
		public CPlayer player;

		public float timeRespawing;

		public Player(int index, string skin)
		{
			this.index = index;
			this.skin = skin;
			this.controls = new PlayerControls(index);
		}
	}
}