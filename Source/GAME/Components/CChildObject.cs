using System.Linq;
using System.Text.RegularExpressions;
using MGE;

namespace GAME.Components
{
	public abstract class CChildObject : CObject
	{
		protected override string basePath => $"Items/{basePathOfParent}";
		protected override string relitivePath
		{
			get => Regex.Replace(GetType().ToString().Split('.').Last().Remove(0, 1), @"\p{Lu}", c => " " + c.Value.ToUpperInvariant()).Remove(0, 1);
		}

		public string basePathOfParent = string.Empty;

		protected CChildObject(string basePathOfParent)
		{
			this.basePathOfParent = basePathOfParent;
		}

		public override T GetAsset<T>(string path) where T : class
		{
			var asset = Assets.GetAsset<T>($"{basePath}/{relitivePath}/{path}");
			if (asset is null)
				asset = Assets.GetAsset<T>($"Items/_Default/{path}");

			if (asset is null) LogWarning("No asset found at " + $"{basePath}/{relitivePath}/{path}" + " | " + $"{basePath}/_Default/{path}");

			return asset;
		}
	}
}