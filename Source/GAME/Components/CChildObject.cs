namespace GAME.Components
{
	public abstract class CChildObject : CObject
	{
		public override string basePath => $"Items/{basePathOfParent}";
		public override string relitivePath => "Projectile";

		public string basePathOfParent = string.Empty;

		protected CChildObject(string basePathOfParent)
		{
			this.basePathOfParent = basePathOfParent;
		}
	}
}