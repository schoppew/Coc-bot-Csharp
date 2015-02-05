using System;
using System.Runtime.InteropServices;

namespace Win32
{
	[StructLayout(LayoutKind.Sequential)]
	public struct APPBARDATA
	{
		public UInt32 cbSize;
		public IntPtr hWnd;
		public UInt32 uCallbackMessage;
		public UInt32 uEdge;
		public RECT rc;
		public Int32 lParam;
	}
}
