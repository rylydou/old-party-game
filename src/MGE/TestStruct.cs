using System.Collections.Generic;

namespace MGE
{
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
		public Dictionary<string, double> someDoubleDictionary { get; set; }
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
			this.someDoubleDictionary = new Dictionary<string, double>()
			{
				{"health", 69.1},
				{"mana", 1337.5},
				{"exp", 888.12}
			};
			this.someTestSubstructDictionary = new Dictionary<string, TestSubstruct>()
			{
				{"player 1", new TestSubstruct('B', "Bob", false, 55.3f, 756.52, 4714.41678m, new double[]{0.5, 1.5, 2.4}, new List<double>() {0.4, 0.72, 1.85}, new Dictionary<string, double>()
				{
					{"item 1", 0.45},
					{"item 2", 1.15},
					{"item 3", 0.99}
				})}
			};
		}

		public struct TestSubstruct
		{
			public char someChar { get; set; }
			public string someString { get; set; }
			public bool someBool { get; set; }
			public float someFloat { get; set; }
			public double someDouble { get; set; }
			public decimal someDecimal { get; set; }
			public double[] someDoubleArray { get; set; }
			public List<double> someDoubleList { get; set; }
			public Dictionary<string, double> someDoubleDictionary { get; set; }

			public TestSubstruct(char someChar, string someString, bool someBool, float someFloat, double someDouble, decimal someDecimal, double[] someDoubleArray, List<double> someDoubleList, Dictionary<string, double> someDoubleDictionary)
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