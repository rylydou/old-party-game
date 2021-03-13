using MGE;

namespace GAME
{
	[System.Serializable]
	public struct ChunkData
	{
		public Vector2Int chunkPosition;
		public bool[,] tiles;

		public Vector2Int size { get => new Vector2Int(tiles.GetLength(0), tiles.GetLength(1)); }
	}
}