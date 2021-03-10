using System;

namespace MGE.ECS
{
	public class Layer
	{
		public readonly string name;
		public readonly bool isUI;

		public bool visible = true;
		public bool enabled = true;

		public int prioity = 0;

		public SafeList<Entity> entities = new SafeList<Entity>();

		public Scene scene;

		public Layer(bool isUI = false)
		{
			this.isUI = isUI;
		}

		public Layer(bool isUI, params Entity[] entities)
		{
			if (entities != null)
			{
				foreach (var entity in entities)
					AddEntity(entity);
			}

			this.isUI = isUI;
		}

		#region Entity Management
		public void AddEntity(Entity entity)
		{
			if (entity.layer != null)
				throw new Exception("Entity aready has an owner!");

			entity._layer = this;

			entities.Add(entity);
		}

		public void RemoveEntity(Entity entity)
		{
			if (entity.layer != this)
				throw new Exception("Layer does not own entity");

			entity.Destroy();
			entity._layer = null;

			entities.Remove(entity);
		}

		public bool ContainsEntity<T>() where T : Entity
		{
			for (var i = 0; i < entities.Count; i++)
			{
				if (entities[i] is T)
					return true;
			}
			return false;
		}

		public T FindEntity<T>() where T : Entity
		{
			for (var i = 0; i < entities.Count; i++)
			{
				if (entities[i] is T)
					return (T)entities[i];
			}
			return null;
		}

		public Entity FindEntityByComponent<T>() where T : Component
		{
			for (var i = 0; i < entities.Count; i += 1)
			{
				if (entities[i].HasComponent<T>())
				{
					return entities[i];
				}
			}
			return null;
		}

		internal void CleanupEntityList()
		{
			foreach (var entity in entities)
			{
				if (entity.destroyed)
					entities.Remove(entity);
			}
		}

		public void ReorderEntity(Entity entity, int index)
		{
			if (entity.layer != this) throw new Exception("Cannot reorder entity - it doesn't belong to this layer.");

			entities.Remove(entity);
			entities.Insert(index, entity);
		}

		public void ReorderEntityToTop(Entity entity) => ReorderEntity(entity, 0);

		public void ReorderEntityToBottom(Entity entity)
		{
			if (entity.layer != this) throw new Exception("Cannot reorder entity - it doesn't belong to this layer.");

			entities.Remove(entity);
			entities.Add(entity);
		}
		#endregion

		#region Updates
		public void FixedUpdate()
		{
			foreach (var entity in entities)
			{
				if (!entity.enabled) continue;
				if (entity.destroyed)
				{
					entities.Remove(entity);
					continue;
				}

				entity.FixedUpdate();
			}
		}

		public void Update()
		{
			foreach (var entity in entities)
			{
				if (!entity.enabled) continue;
				if (entity.destroyed)
				{
					entities.Remove(entity);
					continue;
				}

				entity.Update();
			}
		}

		public void Draw()
		{
			foreach (var entity in entities)
			{
				if (!entity.visible) continue;
				if (entity.destroyed)
				{
					entities.Remove(entity);
					continue;
				}

				entity.Draw();
			}
		}

		public void DrawUI()
		{
			foreach (var entity in entities)
			{
				if (!entity.visible) continue;
				if (entity.destroyed)
				{
					entities.Remove(entity);
					continue;
				}

				entity.Draw();
			}
		}

		public void DestroyAllEntites()
		{
			foreach (var entity in entities)
				if (!entity.destroyed)
					entity.Destroy();
		}
		#endregion
	}
}