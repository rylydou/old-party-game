using System.Collections.Generic;
using MGE;

namespace GAME.Components.Items
{
	public class CBoomerang : CItem
	{
		public enum State
		{
			None,
			GoingOut,
			Returning
		}

		public int damage = 20;
		public float distance = 4.0f;
		public float speed = 4.0f;
		public float returnSpeed = 8.0f;

		State rangState = State.None;
		Vector2 flyTo;

		List<CObject> thingsHit;
		CPlayer owner;

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			switch (rangState)
			{
				case State.GoingOut:
					entity.position = Vector2.MoveTowards(entity.position, flyTo, speed * Time.fixedDeltaTime);
					if (entity.position == flyTo)
						rangState = State.Returning;
					break;
				case State.Returning:
					entity.position = Vector2.MoveTowards(entity.position, owner.entity.position, returnSpeed * Time.fixedDeltaTime);
					if (entity.position == player.entity.position)
					{
						rangState = State.None;
						if (owner.heldItem is null)
							owner.Pickup(this);
					}
					break;
			}
		}

		public override void Use()
		{
			rangState = State.GoingOut;
			flyTo = player.entity.position + new Vector2(entity.scale.x * distance, 0);

			Drop();
		}

		public override void Pickup(CPlayer player)
		{
			base.Pickup(player);

			owner = player;
		}
	}
}