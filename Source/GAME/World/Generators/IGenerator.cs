namespace GAME.World.Generation
{
	public interface IGenerator
	{
		void Generate(ref Tile[,] world, int seed);
	}
}