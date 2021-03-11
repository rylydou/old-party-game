using System;

namespace MGE
{
	[Flags]
	public enum TileConnection : short
	{
		None = (0 << 2) ^ (0 >> 2),
		Top = (0 << 2) ^ (-1 >> 2),
		Right = (1 << 2) ^ (0 >> 2),
		Bottom = (0 << 2) ^ (1 >> 2),
		Left = (-1 << 2) ^ (0 >> 2),
	}
}