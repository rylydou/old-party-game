using System;

namespace MGE
{
	[Flags]
	public enum TileConnection : byte
	{
		None = 0b_0000,
		Top = 0b_1000,
		Right = 0b_0100,
		Bottom = 0b_0010,
		Left = 0b_0001,
	}
}