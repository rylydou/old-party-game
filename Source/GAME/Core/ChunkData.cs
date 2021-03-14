using MGE;

namespace GAME
{
	[System.Serializable]
	public struct ChunkData
	{
		public Vector2Int position;
		public bool[,] tiles;

		public Vector2Int size { get => new Vector2Int(tiles.GetLength(0), tiles.GetLength(1)); }

		public ChunkData(Vector2Int chunkPosition, bool[,] tiles)
		{
			this.position = chunkPosition;
			this.tiles = tiles;
		}
	}
}