using System;
using System.Runtime.InteropServices;

namespace Win32
{
	// RECT structure required by WINDOWPLACEMENT structure
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public RECT(System.Drawing.Rectangle rect)
		{
			Left = rect.Left;
			Top = rect.Top;
			Right = rect.Right;
			Bottom = rect.Bottom;
		}
	}
}
