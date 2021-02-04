using System;
using System.Collections.Generic;

namespace MGE
{
	public class ObjectCollection
	{
		public Dictionary<Guid, Object> objects = new Dictionary<Guid, Object>();

		public void AddNew(in Object obj)
		{
			var guid = Guid.NewGuid();

			obj.guid = guid;

			objects.Add(guid, obj);
		}
	}
}