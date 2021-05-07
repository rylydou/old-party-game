using System.Linq;
using System.Text.RegularExpressions;

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
	}
}