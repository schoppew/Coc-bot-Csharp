using System;
using System.Runtime.InteropServices;

namespace Win32
{
	[StructLayout(LayoutKind.Explicit)]
	public struct MOUSEKEYBDHARDWAREINPUT
	{
		[FieldOffset(0)]
		public MOUSEINPUT Mouse;
	}
}
