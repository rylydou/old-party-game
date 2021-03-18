using System;
using System.Collections;

namespace MGE
{
	[Serializable]
	public class Grid<T> : ICollection, IEnumerable
	{
		public T[,] array;
		public T outOfBoundsElementValue;

		public int width { get => array.GetLength(0); }
		public int height { get => array.GetLength(1); }

		public Vector2Int size { get => new Vector2Int(width, height); }

		public int Count => width * height;
		public bool IsSynchronized => array.IsSynchronized;
		public object SyncRoot => array.SyncRoot;

		public Grid(Vector2Int size, T defualtElementValue = default)
		{
			this.array = new T[size.x, size.y];
			this.outOfBoundsElementValue = defualtElementValue;
		}

		public Grid(int width, int height, T defualtElementValue = default)
		{
			this.array = new T[width, height];
			this.outOfBoundsElementValue = defualtElementValue;
		}

		public Grid(T[,] array, T defualtElementValue = default)
		{
			this.array = array;
			this.outOfBoundsElementValue = defualtElementValue;
		}

		/// <summary> Does not do safety checks use Get() or Set() instead! </summary>
		public T this[int x, int y]
		{
			get => array[x, y];
			set => array[x, y] = value;
		}

		/// <summary> Does not do safety checks use Get() or Set() instead! </summary>
		public T this[Vector2Int position]
		{
			get => this[position.x, position.y];
			set => this[position.x, position.y] = value;
		}

		public T Get(Vector2Int position) => Get(position.x, position.y);
		public T Get(int x, int y)
		{
			if (IsInBounds(x, y))
				return array[x, y];
			return outOfBoundsElementValue;
		}

		public bool Set(Vector2Int position, T value) => Set(position.x, position.y, value);
		public bool Set(int x, int y, T value)
		{
			if (IsInBounds(x, y))
			{
				array[x, y] = value;
				return true;
			}
			return false;
		}

		public void For(Action<int, int> action)
		{
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					action.Invoke(x, y);
				}
			}
		}

		public void For(Action<int, int, T> action)
		{
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					action.Invoke(x, y, array[x, y]);
				}
			}
		}

		public void For(Func<int, int, T> function)
		{
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					this[x, y] = function.Invoke(x, y);
				}
			}
		}

		public void ForEach(Action<T> action)
		{
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					action.Invoke(this[x, y]);
				}
			}
		}

		public void ForEach(Func<T, T> function)
		{
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					this[x, y] = function.Invoke(this[x, y]);
				}
			}
		}

		public bool IsInBounds(int x, int y) =>
			x >= 0 && x < width && y >= 0 && y < height;

		public void CopyTo(Array array, int index) =>
			array.CopyTo(array, index);

		public IEnumerator GetEnumerator() =>
			array.GetEnumerator();

		public override string ToString() =>
			$"{GetType()} {size}";
	}
}