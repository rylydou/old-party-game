using System.Collections.Generic;
using System;

namespace MGE.ECS
{
	public class Entity : IComparable<Entity>
	{
		public bool enabled = true;
		public bool visible = true;

		public bool destroyed { get; protected set; } = false;

		protected Dictionary<Type, Component> _components = new Dictionary<Type, Component>();

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

		public int componentCount { get => _components.Count; }

		public Entity(List<Component> components = null)
		{
			if (components != null)
				foreach (var component in components)
					if (component == null)
						throw new Exception("Component is null");
					else
						AddComponent(component);
		}

		public virtual T AddComponent<T>(T component) where T : Component
		{
			if (component.entity != null)
				Logger.LogError($"Component {component} already is atached to an entity!");

			_components.Add(component.GetType(), component);
			component.entity = this;
			component.Init();
			component.inited = true;

			return component;
		}

		public Component RemoveComponent(Type type)
		{
			if (_components.TryGetValue(type, out Component component))
			{
				component.Destroy();
				_components.Remove(type);
				component.entity = null;
				return component;
			}
			return null;
		}

		public T GetComponent<T>() where T : Component
		{
			if (_components.ContainsKey(typeof(T)))
				return (T)_components[typeof(T)];
			return null;
		}

		public bool HasComponent<T>() where T : Component => _components.ContainsKey(typeof(T));

		public Component[] GetAllComponents()
		{
			var components = new Component[_components.Count];
			var index = 0;

			foreach (var component in _components)
			{
				components[index] = component.Value;
				index++;
			}

			return components;
		}

		#region Updates
		public virtual void FixedUpdate()
		{
			foreach (var component in _components.Values)
			{
				if (component.enabled)
				{
					component.FixedUpdate();
				}
			}
		}

		public virtual void Update()
		{
			foreach (var component in _components.Values)
			{
				if (component.enabled)
				{
					component.Update();
				}
			}
		}

		public virtual void Draw()
		{
			foreach (var component in _components.Values)
			{
				if (component.enabled)
				{
					component.Draw();
				}
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
			if (!destroyed)
			{
				destroyed = true;
				if (enabled)
				{
					foreach (var component in _components.Values)
					{
						component.Destroy();
					}
				}
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