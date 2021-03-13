using System.Collections.Generic;

namespace MGE
{
	public class SafeList<T>
	{
		private List<T> _items, _outdatedItems;
		private bool _isOutdated = false;

		public SafeList()
		{
			_items = new List<T>();
			_outdatedItems = new List<T>();
		}

		public SafeList(SafeList<T> list)
		{
			_items = new List<T>(list._items);
			_outdatedItems = new List<T>(list._items);
		}

		public SafeList(List<T> list)
		{
			_items = new List<T>(list);
			_outdatedItems = new List<T>(list);
		}

		public void Add(T item)
		{
			_isOutdated = true;
			_items.Add(item);
		}

		public void Remove(T item)
		{
			_isOutdated = true;
			_items.Remove(item);
		}

		public bool Contains(T item) =>
			_items.Contains(item);

		public void Insert(int index, T item) =>
			_items.Insert(index, item);

		public void AddRange(SafeList<T> items) =>
			_items.AddRange(items._items);

		public int Count =>
			_items.Count;

		public void Clear()
		{
			_isOutdated = true;
			_items.Clear();
		}

		public T this[int index]
		{
			get => _items[index];
			set
			{
				_isOutdated = true;
				_items[index] = value;
			}
		}

		public List<T> ToList() =>
			new List<T>(_items);

		public T[] ToArray() =>
			_items.ToArray();

		public void Sort(IComparer<T> comparer)
		{
			_isOutdated = true;
			_items.Sort(comparer);
		}

		private void Update()
		{
			if (_isOutdated)
			{
				_outdatedItems.Clear();
				_outdatedItems.AddRange(_items);

				_isOutdated = false;
			}
		}

		public List<T>.Enumerator GetEnumerator()
		{
			Update();
			return _outdatedItems.GetEnumerator();
		}
	}
}