using GAME.Components;
using MGE;

namespace GAME
{
	public struct DamageInfo
	{
		public CPlayer doneBy;
		public Vector2 origin;

		public DamageInfo(CPlayer doneBy)
		{
			this.doneBy = doneBy;
			this.origin = doneBy.entity.position;
		}
	}
}