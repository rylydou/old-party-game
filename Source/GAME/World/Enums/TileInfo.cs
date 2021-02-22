namespace GAME.World
{
	[System.Flags]
	public enum TileInfo
	{
		None,
		BlastProof,
		BadForEnvironment,
		NonCorruptible,
		Invincible = BlastProof | NonCorruptible
	}
}