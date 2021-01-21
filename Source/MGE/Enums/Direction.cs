using System;

namespace MGE
{
	[Flags]
	public enum Direction
	{
		None = 0b_0000,
		Up = 0b_1000,
		Down = 0b_0100,
		Right = 0b_0010,
		Left = 0b_0001,
		UpRight = Up | Right,
		DownRight = Down | Right,
		DownLeft = Down | Left,
		UpLeft = Up | Left,
	}
}