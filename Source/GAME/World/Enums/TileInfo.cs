namespace GAME.World
{
	[System.Flags]
	public enum TileInfo
	{
		None = 0,

		Airtight,

		BadForEnvironment,

		NonErodable,
		BlastProof,
		NonCorruptible,
		Invincible = NonErodable | BlastProof | NonCorruptible
	}
}