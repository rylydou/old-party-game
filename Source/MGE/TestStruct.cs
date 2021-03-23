using System.Collections.Generic;
using BinarySerialization;
using MGE.FileIO;

namespace MGE
{
	[System.Serializable]
	public struct TestStruct
	{
		[FieldOrder(0)] public char someChar;
		[FieldOrder(1)] public string someString;
		[FieldOrder(2)] public bool someBool;
		[FieldOrder(3)] public float someFloat;
		[FieldOrder(4)] public double someDouble;
		[FieldOrder(5)] public int someInt;
		[FieldOrder(6)] public double[] someDoubleArray;
		[FieldOrder(7)] public List<double> someDoubleList;
		// [Ignore] public Dictionary<string, float> someDoubleDictionary;
		// [Ignore] public Dictionary<string, TestSubstruct> someTestSubstructDictionary;

		public TestStruct(bool dontWorryAboutThis)
		{
			this.someChar = 'A';
			this.someString = "The best save file ever!!! \"Warning: Very epic!!!\" ";
			this.someBool = true;
			this.someFloat = 7.4f;
			this.someDouble = 11.25;
			this.someInt = 255;
			this.someDoubleArray = new double[] { 0.9, 1.25, 4.33, 65.42 };
			this.someDoubleList = new List<double>() { 0.9, 1.25, 4.33, 65.42 };
			// this.someDoubleDictionary = new Dictionary<string, float>()
			// {
			// 	{"health", 69.1f},
			// 	{"mana", 1337.5f},
			// 	{"exp", 888.12f}
			// };
			// this.someTestSubstructDictionary = new Dictionary<string, TestSubstruct>()
			// {
			// 	{"player 1", new TestSubstruct('B', "Bob", false, 55.3f, 756.52f, 4714.41678m, new float[]{0.5f, 1.5f, 2.4f}, new List<float>() {0.4f, 0.72f, 1.85f}, new Dictionary<string, float>()
			// 	{
			// 		{"item 1", 0.45f},
			// 		{"item 2", 1.15f},
			// 		{"item 3", 0.99f}
			// 	})},
			// 	{"player 2", new TestSubstruct('B', "Bob", false, 55.3f, 756.52f, 4714.41678m, new float[]{0.5f, 1.5f, 2.4f}, new List<float>() {0.4f, 0.72f, 1.85f}, new Dictionary<string, float>()
			// 	{
			// 		{"item 1", 0.69f},
			// 		{"item 2", 4.20f},
			// 		{"item 3", 13.37f}
			// 	})}
			// };
		}

		[System.Serializable]
		public struct TestSubstruct
		{
			[FieldOrder(0)] public char someChar;
			[FieldOrder(1)] public string someString;
			[FieldOrder(2)] public bool someBool;
			[FieldOrder(3)] public float someFloat;
			[FieldOrder(4)] public float someDouble;
			[FieldOrder(5)] public decimal someDecimal;
			[FieldOrder(6)] public float[] someDoubleArray;
			[FieldOrder(7)] public List<float> someDoubleList;
			// [Ignore] public Dictionary<string, float> someDoubleDictionary;

			public TestSubstruct(char someChar, string someString, bool someBool, float someFloat, float someDouble, decimal someDecimal, float[] someDoubleArray, List<float> someDoubleList, Dictionary<string, float> someDoubleDictionary)
			{
				this.someChar = someChar;
				this.someString = someString;
				this.someBool = someBool;
				this.someFloat = someFloat;
				this.someDouble = someDouble;
				this.someDecimal = someDecimal;
				this.someDoubleArray = someDoubleArray;
				this.someDoubleList = someDoubleList;
				// this.someDoubleDictionary = someDoubleDictionary;
			}
		}
	}
}