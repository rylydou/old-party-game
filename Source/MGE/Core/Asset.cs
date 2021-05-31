namespace MGE
{
	[System.Serializable]
	public abstract class Asset
	{
		public abstract string extension { get; }

		public string fullPath { get; private set; }
		public string localPath { get; private set; }

		public virtual void Load(string fullPath, string localPath = null)
		{
			this.fullPath = fullPath;
			this.localPath = localPath;
		}

		public virtual void Save(string fullPath) { }
	}
}