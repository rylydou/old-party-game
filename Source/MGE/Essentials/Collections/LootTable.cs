using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MGE
{
	[Serializable, JsonObject(MemberSerialization.OptIn)]
	public class LootTable<T>
	{
		[JsonProperty] public Dictionary<T, int> lootTable;
		[NonSerialized] public T[] calcedLootTable;

		public void Add(T item, int weight)
		{
			if (lootTable.ContainsKey(item))
				lootTable[item] = weight;
			else
				lootTable.Add(item, weight);
		}

		public void CalcLootTable()
		{
			var calcedLootTable = new List<T>();

			foreach (var item in lootTable)
			{
				for (int i = 0; i < item.Value; i++)
				{
					calcedLootTable.Add(item.Key);
				}
			}

			this.calcedLootTable = calcedLootTable.ToArray();
		}

		public T GetRandomItem()
		{
			if (calcedLootTable is null)
				throw new System.Exception("No Calced Loot Table has been generated, be sure to call `CalcLootTable()`");
			return calcedLootTable.Random();
		}

		[OnDeserialized]
		public void OnDeserialized(StreamingContext context)
		{
			CalcLootTable();
		}
	}
}