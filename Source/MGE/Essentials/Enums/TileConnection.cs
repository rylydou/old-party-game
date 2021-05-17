using System;

namespace MGE
{
	[Flags]
	public enum TileConnection : byte
	{
		Top_Left = 0b_00000001,
		Top = 0b_00000010,
		Top_Right = 0b_00000100,
		Right = 0b_00001000,
		None = 0b_00000000,
		Left = 0b_00010000,
		Bottom_Left = 0b_00100000,
		Bottom = 0b_01000000,
		Bottom_Right = 0b_10000000,
	}
}