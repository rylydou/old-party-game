namespace GAME.States
{
	public abstract class GameState
	{
		public virtual void Init() { }

		public virtual void Tick() { }
		public virtual void Update() { }
		public virtual void Draw() { }
		public virtual void DrawUI() { }

		public virtual void Exit() { }
	}
}