using System;
using System.Linq;
using System.Collections.Generic;
using MGE.ECS;

namespace MGE
{
	public class Layer
	{
		public readonly string name;
		public readonly bool isUI;

		public bool visible = true;
		public bool enabled = true;

		public int prioity = 0;

		List<Entity> _entities = new List<Entity>();

		public Scene scene;

		public int entityCount { get => _entities.Count; }
		public int componentCount { get => _entities.Sum((x) => x.componentCount); }

		public Layer(List<Entity> entities = null)
		{
			if (entities != null)
			{
				foreach (var entity in entities)
					AddEntity(entity);
			}
		}

		#region Entity Management
		public void AddEntity(Entity entity)
		{
			if (entity.layer != null)
				throw new Exception("Entity aready has an owner!");

			_entities.Add(entity);
		}

		public void RemoveEntity(Entity entity)
		{
			if (entity.layer != this)
				throw new Exception("Layer does not own entity");

			_entities.Remove(entity);
		}

		public bool ContainsEntity<T>() where T : Entity
		{
			for (var i = 0; i < _entities.Count; i++)
			{
				if (_entities[i] is T)
					return true;
			}
			return false;
		}

		public T FindEntity<T>() where T : Entity
		{
			for (var i = 0; i < _entities.Count; i++)
			{
				if (_entities[i] is T)
					return (T)_entities[i];
			}
			return null;
		}

		public Entity FindEntityByComponent<T>() where T : Component
		{
			for (var i = 0; i < _entities.Count; i += 1)
			{
				if (_entities[i].HasComponent<T>())
				{
					return _entities[i];
				}
			}
			return null;
		}

		internal void CleanupEntityList()
		{
			foreach (var entity in _entities)
			{
				if (entity.destroyed)
					_entities.Remove(entity);
			}
		}

		public void ReorderEntity(Entity entity, int index)
		{
			if (entity.layer != this) throw new Exception("Cannot reorder entity - it doesn't belong to this layer.");

			_entities.Remove(entity);
			_entities.Insert(index, entity);
		}

		public void ReorderEntityToTop(Entity entity) => ReorderEntity(entity, 0);

		public void ReorderEntityToBottom(Entity entity)
		{
			if (entity.layer != this) throw new Exception("Cannot reorder entity - it doesn't belong to this layer.");

			_entities.Remove(entity);
			_entities.Add(entity);
		}
		#endregion

		#region Updates
		public void FixedUpdate()
		{
			foreach (var entity in _entities)
			{
				if (entity.enabled && !entity.destroyed)
				{
					entity.FixedUpdate();
				}
			}
		}

		public void Update()
		{
			foreach (var entity in _entities)
			{
				if (entity.enabled && !entity.destroyed)
				{
					entity.Update();
				}
			}
		}

		public void Draw()
		{
			foreach (var entity in _entities)
			{
				if (entity.visible && !entity.destroyed)
				{
					entity.Draw();
				}
			}
		}

		public void DrawUI()
		{
			foreach (var entity in _entities)
			{
				if (entity.visible && !entity.destroyed)
				{
					entity.Draw();
				}
			}
		}
		#endregion
	}
}