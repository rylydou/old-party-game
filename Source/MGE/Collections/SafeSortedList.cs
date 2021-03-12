using System;
using System.Collections;
using System.Collections.Generic;

namespace MGE
{
	public class SafeSortedList<T>
	{
		private Func<T, int> _sortingParameter;
		private List<T> _items, _outdatedItems;
		private bool _isOutdated = false;

		public SafeSortedList(Func<T, int> sortingParameter)
		{
			_sortingParameter = sortingParameter;
			_items = new List<T>();
			_outdatedItems = new List<T>();
		}

		public void Add(T item)
		{
			_isOutdated = true;

			var added = false;
			for (var i = 0; i < _items.Count; i += 1)
			{
				if (_sortingParameter(item) > _sortingParameter(_items[i]))
				{
					_items.Insert(i, item);
					added = true;
					break;
				}
			}
			if (!added)
			{
				_items.Add(item);
			}
		}

		public void Remove(T item)
		{
			_isOutdated = true;
			_items.Remove(item);
		}

		public bool Contains(T item) =>
			_items.Contains(item);

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