using MGE;
using MGE.FileIO;

namespace GAME
{
	[System.Serializable]
	public class Chunk
	{
		public Vector2Int position;
		public Grid<ushort> tiles;

		public Vector2Int size { get => tiles.size; }

		public Chunk(Vector2Int chunkPosition, Grid<ushort> tiles)
		{
			this.position = chunkPosition;
			this.tiles = tiles;
		}

		public void Save(Folder folder)
		{
			IO.Save(folder.GetFullPath($"/Chunks/{position.x} {position.y}.chunk"), this);
		}

		public static Chunk Load(Folder folder, int x, int y)
		{
			return IO.Load<Chunk>(folder.GetFullPath($"/Chunks/{x} {y}.chunk"));
		}
	}
}