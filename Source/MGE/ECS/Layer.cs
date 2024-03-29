using System;
using System.Collections.Generic;
using MGE.Graphics;

namespace MGE.ECS
{
	[System.Serializable]
	public class Layer
	{
		public readonly string name;
		public readonly bool isUI;

		public bool visible = true;
		public bool enabled = true;

		public int prioity = 0;

		public List<Entity> entities = new List<Entity>();

		public ICanRaycast raycaster;

		public Scene scene;

		public List<(Vector2, Vector2)> debugPhysics = new List<(Vector2, Vector2)>();

		public Layer() { }

		public Layer(bool isUI = false)
		{
			this.isUI = isUI;
		}

		public Layer(string name, params Entity[] entities)
		{
			this.name = name;
			this.isUI = name.StartsWith("UI");

			if (entities != null)
			{
				foreach (var entity in entities)
					AddEntityNoInit(entity);
			}
		}

		#region Entity Management
		internal void AddEntityNoInit(Entity entity)
		{
			if (entity.layer != null)
				throw new Exception("Entity aready has an owner!");

			entity._layer = this;

			entities.Add(entity);
		}

		public void AddEntity(Entity entity)
		{
			if (entity.layer != null)
				throw new Exception("Entity aready has an owner!");

			entity._layer = this;
			entity.Init();

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

		public T GetEntity<T>() where T : Entity
		{
			for (var i = 0; i < entities.Count; i++)
			{
				if (entities[i] is T)
					return (T)entities[i];
			}
			return null;
		}

		public Entity GetEntityWithComponent<T>() where T : Component
		{
			for (var i = 0; i < entities.Count; i += 1)
			{
				if (entities[i].HasComponent<T>())
					return entities[i];
			}
			return null;
		}

		public Entity[] GetEntitiesWithTag(string tag)
		{
			var entitiesWithTag = new List<Entity>();

			foreach (var entity in entities)
			{
				if (!entity.enabled || entity.destroyed) continue;

				if (entity.HasTag(tag))
					entitiesWithTag.Add(entity);
			}

			return entitiesWithTag.ToArray();
		}

		internal void CleanupEntityList()
		{
			foreach (var entity in entities.ToArray())
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

		public Entity[] GetEntities()
		{
			var foundEntities = new List<Entity>();

			foreach (var entity in entities)
			{
				if (!entity.enabled || entity.destroyed) continue;

				foundEntities.Add(entity);
			}

			return foundEntities.ToArray();
		}
		#endregion

		#region Updates
		internal virtual void Init()
		{
			foreach (var entity in entities)
			{
				if (!entity.enabled) continue;

				entity.Init();
			}
		}

		public void Tick()
		{
			if (Physics.Physics.DEBUG)
				debugPhysics.Clear();

			foreach (var entity in entities.ToArray())
			{
				if (!entity.enabled | entity.destroyed) continue;

				entity.Tick();
			}
		}

		public void Update()
		{
			foreach (var entity in entities.ToArray())
			{
				if (!entity.enabled) continue;
				if (entity.destroyed)
					entities.Remove(entity);

				entity.Update();
			}
		}

		public void Draw()
		{
			using (new DrawBatch(false))
			{
				foreach (var entity in entities.ToArray())
				{
					if (!entity.visible | entity.destroyed) continue;

					entity.Draw();
				}

				if (Physics.Physics.DEBUG)
				{
					foreach (var call in debugPhysics)
					{
						if (Math.Approximately(call.Item2.y, 0))
							GFX.DrawCircle(call.Item1 + 0.5f, call.Item2.x / 2, Color.red);
						else
							GFX.DrawRect(new Rect(call.Item1, call.Item2), Color.red);
					}
				}
			}
		}

		public void DrawUI()
		{
			using (new DrawBatch(true))
			{
				foreach (var entity in entities.ToArray())
				{
					if (!entity.visible | entity.destroyed) continue;

					entity.Draw();
				}
			}
		}

		public void DestroyAllEntites()
		{
			foreach (var entity in entities)
				entity.Destroy();
		}
		#endregion

		#region Physics
		public Entity GetNearestEntity(Vector2 position, string tag = null)
		{
			var nearestDistSqr = float.PositiveInfinity;
			Entity nearestEntity = null;

			var entitiesToSearch = string.IsNullOrEmpty(tag) ? GetEntities() : GetEntitiesWithTag(tag);

			foreach (var entity in entitiesToSearch)
			{
				var distSqr = Vector2.DistanceSqr(position, entity.position);

				if (distSqr < nearestDistSqr)
				{
					nearestDistSqr = distSqr;
					nearestEntity = entity;
				}
			}

			if (Physics.Physics.DEBUG)
				debugPhysics.Add((position, new Vector2(0.1f, 0.1f)));

			return nearestEntity;
		}

		public Entity GetNearestEntity(Vector2 position, float radius, string tag = null)
		{
			radius = radius * radius;

			Entity nearestEntity = null;

			var entitiesToSearch = string.IsNullOrEmpty(tag) ? GetEntities() : GetEntitiesWithTag(tag);

			foreach (var entity in entitiesToSearch)
			{
				var distSqr = Vector2.DistanceSqr(position, entity.position);

				if (distSqr < radius)
				{
					radius = distSqr;
					nearestEntity = entity;
				}
			}

			if (Physics.Physics.DEBUG)
				debugPhysics.Add((position, new Vector2(radius, 0)));

			return nearestEntity;
		}

		public Entity[] GetEntities(Rect rect, string tag = null)
		{
			var foundEntities = new List<Entity>();
			var entitiesToSearch = string.IsNullOrEmpty(tag) ? GetEntities() : GetEntitiesWithTag(tag);

			foreach (var entity in entitiesToSearch)
			{
				if (rect.Contains(entity.position))
					foundEntities.Add(entity);
			}

			if (Physics.Physics.DEBUG)
				debugPhysics.Add((rect.position, rect.size));

			return foundEntities.ToArray();
		}

		public Entity[] GetEntities(Vector2 position, float radius, string tag = null)
		{
			radius = radius * radius;

			var foundEntities = new List<Entity>();
			var entitiesToSearch = string.IsNullOrEmpty(tag) ? GetEntities() : GetEntitiesWithTag(tag);

			foreach (var entity in entitiesToSearch)
			{
				var distSqr = Vector2.DistanceSqr(position, entity.position);

				if (distSqr < radius)
					foundEntities.Add(entity);
			}

			if (Physics.Physics.DEBUG)
				debugPhysics.Add((position, new Vector2(radius, 0)));

			return foundEntities.ToArray();
		}
		#endregion
	}
}