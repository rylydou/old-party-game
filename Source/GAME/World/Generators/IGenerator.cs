namespace GAME.World.Generation
{
	public interface IGenerator
	{
		void Generate(ref int[,] world);
	}
}