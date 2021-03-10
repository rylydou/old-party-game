// using System;
// using System.Collections;
// using System.Collections.Generic;

// namespace MGE
// {
// 	public class List<T> : IEnumerable<T>, IEnumerable
// 	{
// 		System.Collections.Generic.List<T> list;

// 		public T this[int index] { get => list[index]; set => list[index] = value; }

// 		public int count => list.Count;
// 		public int length => list.Count;
// 		public int Count => list.Count;

// 		public void Add(T item) => list.Add(item);

// 		public void Clear() => list.Clear();

// 		public bool Contains(T item) => list.Contains(item);

// 		public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

// 		public int IndexOf(T item) => list.IndexOf(item);

// 		public void Insert(int index, T item) => list.Insert(index, item);

// 		public bool Remove(T item) => list.Remove(item);

// 		public void RemoveAt(int index) => list.RemoveAt(index);

// 		public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
// 		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => list.GetEnumerator();
// 	}
// }