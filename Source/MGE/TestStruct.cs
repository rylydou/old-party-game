using System.Collections.Generic;
using MGE.FileIO;

namespace MGE
{
	[System.Serializable]
	public struct TestStruct
	{
		public char someChar { get; set; }
		public string someString { get; set; }
		public bool someBool { get; set; }
		public float someFloat { get; set; }
		public double someDouble { get; set; }
		public int someInt { get; set; }
		public double[] someDoubleArray { get; set; }
		public List<double> someDoubleList { get; set; }
		public Dictionary<string, float> someDoubleDictionary { get; set; }
		public Dictionary<string, TestSubstruct> someTestSubstructDictionary { get; set; }

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
			this.someDoubleDictionary = new Dictionary<string, float>()
			{
				{"health", 69.1f},
				{"mana", 1337.5f},
				{"exp", 888.12f}
			};
			this.someTestSubstructDictionary = new Dictionary<string, TestSubstruct>()
			{
				{"player 1", new TestSubstruct('B', "Bob", false, 55.3f, 756.52f, 4714.41678m, new float[]{0.5f, 1.5f, 2.4f}, new List<float>() {0.4f, 0.72f, 1.85f}, new Dictionary<string, float>()
				{
					{"item 1", 0.45f},
					{"item 2", 1.15f},
					{"item 3", 0.99f}
				})},
				{"player 2", new TestSubstruct('B', "Bob", false, 55.3f, 756.52f, 4714.41678m, new float[]{0.5f, 1.5f, 2.4f}, new List<float>() {0.4f, 0.72f, 1.85f}, new Dictionary<string, float>()
				{
					{"item 1", 0.69f},
					{"item 2", 4.20f},
					{"item 3", 13.37f}
				})}
			};
		}

		[System.Serializable]
		public struct TestSubstruct
		{
			public char someChar { get; set; }
			public string someString { get; set; }
			public bool someBool { get; set; }
			public float someFloat { get; set; }
			public float someDouble { get; set; }
			public decimal someDecimal { get; set; }
			public float[] someDoubleArray { get; set; }
			public List<float> someDoubleList { get; set; }
			public Dictionary<string, float> someDoubleDictionary { get; set; }

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
				this.someDoubleDictionary = someDoubleDictionary;
			}
		}
	}
}