using MGE.Physics;

namespace MGE
{
	public interface ICanRaycast
	{
		RaycastHit Raycast(Vector2 origin, Vector2 direction, int maxIterations = -1);
	}
}