using MGE.Graphics;
using MGE.InputSystem;
using MGE.UI;

namespace MGE.Debug
{
	public abstract class DebugMenu : ILogic
	{
		public const int allSize = 32;

		public const int barSize = allSize;

		public abstract string name { get; }
		public string title = "Window";
		public Vector2 position = Vector2.zero;
		public Vector2 size = new Vector2(allSize * 12, allSize * 12);

		protected Rect barRect;

		protected Color barColor = Colors.transBlack;
		protected Color bgColor = Colors.transBG;

		protected bool isDragging = false;
		protected Vector2 dragStartMousePos = Vector2.zero;
		protected Vector2 dragStartOg = Vector2.zero;
		protected bool isResizing = false;

		protected internal GUI gui;

		public bool active
		{
			get => Menuing.menus.Count > 0 ? Menuing.menus[0] == this : false;
		}

		public virtual void Init()
		{
			title = name;
		}

		public virtual void Update()
		{
			if (GUI.gui.MouseInside(barRect))
			{
				if (Input.GetButtonPress(Inputs.MouseLeft))
				{
					dragStartMousePos = Input.windowMousePosition;
					dragStartOg = position;

					isDragging = true;
				}
				else if (Input.GetButtonPress(Inputs.MouseRight))
				{
					if (size == (Vector2)Window.windowedSize)
					{
						position = new Vector2(allSize, allSize * 2);
						size = new Vector2(allSize * 12, allSize * 12);
					}
					else
					{
						position = new Vector2(0, allSize);
						size = new Vector2(Window.windowedSize.x, Window.windowedSize.y - allSize);
					}
				}
				else if (Input.GetButtonPress(Inputs.MouseMiddle))
					Close();
			}
			else if (GUI.gui.MouseClick(new Rect(position.x + size.x - allSize / 2, position.y + size.y - allSize / 2, barSize, allSize)))
			{
				dragStartMousePos = Input.windowMousePosition;
				dragStartOg = size;

				isResizing = true;
			}

			if (isDragging)
			{
				if (Input.GetButton(Inputs.MouseLeft))
				{
					position = dragStartOg + Input.windowMousePosition - dragStartMousePos;
					position.x = Math.Clamp(position.x, -size.x + barSize, Window.windowedSize.x - barSize);
					position.y = Math.Clamp(position.y, barSize, Window.windowedSize.y);
				}
				else
				{
					position = new Vector2(Math.Round(position.x), Math.Round(position.y));

					isDragging = false;
				}
			}
			else if (isResizing)
			{
				if (Input.GetButton(Inputs.MouseLeft))
				{
					size = dragStartOg + Input.windowMousePosition - dragStartMousePos;

					size.x = Math.Clamp(size.x, 256, Window.windowedSize.x);
					size.y = Math.Clamp(size.y, 256, Window.windowedSize.y);
				}
				else
				{
					size = new Vector2(Math.Round(size.x), Math.Round(size.y));

					if (size.x % 2 != 0)
						size.x++;

					if (size.y % 2 != 0)
						size.y++;

					isResizing = false;
				}
			}

			barRect = new Rect(position.x, position.y - barSize, size.x, barSize);

			gui.Rect(new Rect(0, -barSize, size.x, size.y + barSize), Colors.accent, 1);
		}

		public virtual void UpdateBG()
		{
			gui = new GUI(new Rect(position, size), active);

			gui.Image(new Rect(new Vector2(0, -barSize), new Vector2(size.x, barSize)), barColor);
			gui.Image(new Rect(Vector2.zero, size), bgColor);

			var text = $"[{title}]";
			var textSize = (Vector2)Config.defualtFont.MeasureString(text);
			gui.Text(text, new Vector2(size.x / 2 - textSize.x / 2, barSize / 2 - textSize.y / 2 - barSize), Colors.text);

			gui.Rect(new Rect(0, -barSize, size.x, size.y + barSize), Colors.black, 1);
		}

		public virtual void Draw()
		{
			gui?.Draw();
		}

		public virtual void Close()
		{
			Menuing.CloseMenu(this);
		}
	}
}