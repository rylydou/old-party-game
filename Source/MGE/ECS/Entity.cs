using System.Collections.Generic;
using System;

namespace MGE.ECS
{
	[System.Serializable]
	public class Entity : IComparable<Entity>
	{
		public bool enabled = true;
		public bool visible = true;

		public bool destroyed { get; protected set; } = false;

		public Dictionary<Type, Component> components = new Dictionary<Type, Component>();

		public Vector2 position;
		public float roation;
		public Vector2 scale;

		int priority = 0;

		internal Layer _layer;
		public Layer layer
		{
			get => _layer;
			set
			{
				if (_layer != null)
					_layer.RemoveEntity(this);
				_layer = value;
				_layer.AddEntity(this);
			}
		}

		public Entity() { }

		public Entity(params Component[] components)
		{
			if (components != null)
				foreach (var component in components)
					if (component == null)
						throw new Exception("Component is null");
					else
					{
						if (component.entity != null)
							Logger.LogError($"Component {component} already is atached to an entity!");

						this.components.Add(component.GetType(), component);
						component.entity = this;
					}
		}

		public virtual T AddComponent<T>(T component) where T : Component
		{
			if (component.entity != null)
				Logger.LogError($"Component {component} already is atached to an entity!");

			components.Add(component.GetType(), component);
			component.entity = this;
			component.Init();
			component.inited = true;

			return component;
		}

		public Component RemoveComponent(Type type)
		{
			if (components.TryGetValue(type, out Component component))
			{
				component.Destroy();
				components.Remove(type);
				component.entity = null;
				return component;
			}
			return null;
		}

		public T GetComponent<T>() where T : Component
		{
			if (components.ContainsKey(typeof(T)))
				return (T)components[typeof(T)];
			return null;
		}

		public bool HasComponent<T>() where T : Component => components.ContainsKey(typeof(T));

		public Component[] GetAllComponents()
		{
			var components = new Component[this.components.Count];
			var index = 0;

			foreach (var component in this.components)
			{
				components[index] = component.Value;
				index++;
			}

			return components;
		}

		#region Updates
		internal virtual void Init()
		{
			foreach (var component in components.Values)
			{
				if (!component.enabled) continue;

				component.Init();
				component.inited = true;
			}
		}

		public virtual void FixedUpdate()
		{
			foreach (var component in components.Values)
			{
				if (!component.enabled) continue;

				component.FixedUpdate();
			}
		}

		public virtual void Update()
		{
			foreach (var component in components.Values)
			{
				if (!component.enabled) continue;

				component.Update();
			}
		}

		public virtual void Draw()
		{
			foreach (var component in components.Values)
			{
				if (!component.visible) continue;

				component.Draw();
			}
		}

		public void Destroy()
		{
			if (!destroyed)
			{
				destroyed = true;

				if (enabled)
					OnDestroy();
			}
		}

		protected virtual void OnDestroy()
		{
			destroyed = true;

			if (enabled)
			{
				foreach (var component in components.Values)
					component.Destroy();
			}
		}
		#endregion

		public int CompareTo(Entity other)
		{
			if (other == null)
				return 1;
			else
				return priority.CompareTo(other.priority);
		}
	}
}