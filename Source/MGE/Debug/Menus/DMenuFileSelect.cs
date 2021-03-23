using System.Text;
using System.IO;
using MGE.UI.Layouts;
using System;

namespace MGE.Debug.Menus
{
	public class DMenuFileSelect : DebugMenu
	{
		public override string name => "File Select";

		public StringBuilder pathBuilder;
		public Action<string> onSelect;
		public string pattern;
		public FileSelectType type;

		public DMenuFileSelect(string title, string path, Action<string> onSelect, string pattern = "*", FileSelectType type = FileSelectType.File)
		{
			this.title = title;
			this.pathBuilder = new StringBuilder(path);
			this.onSelect = onSelect;
			this.pattern = pattern;
			this.type = type;
		}

		public override void UpdateBG()
		{
			base.UpdateBG();

			var folders = Directory.GetDirectories(pathBuilder.ToString());
			var files = Directory.GetFiles(pathBuilder.ToString(), pattern);

			using (var layout = new StackLayout(offset, allSize))
			{
				if (gui.ButtonClicked(pathBuilder.ToString(), layout.newElement.y, layout.currentSize, null, TextAlignment.Left))
				{
					pathBuilder = new StringBuilder(new DirectoryInfo(pathBuilder.ToString()).Parent.FullName.Replace('\\', '/') + "/");
				}

				foreach (var folder in folders)
				{
					var name = new DirectoryInfo(folder).Name + "/";

					if (gui.ButtonClicked(name, layout.newElement.y, layout.currentSize, null, TextAlignment.Left))
					{
						pathBuilder.Append(name);

						if (type == FileSelectType.Folder)
						{
							onSelect?.Invoke(pathBuilder.ToString());
							Close();
						}
					}
				}

				if (type == FileSelectType.File)
				{
					foreach (var file in files)
					{
						var name = new FileInfo(file).Name;

						if (gui.ButtonClicked(name, layout.newElement.y, layout.currentSize, null, TextAlignment.Left))
						{
							pathBuilder.Append(name);
							onSelect?.Invoke(pathBuilder.ToString());
							Close();
						}
					}
				}
			}
		}
	}
}