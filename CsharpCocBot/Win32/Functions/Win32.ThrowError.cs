using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Win32
{
	static public partial class Win32
	{
		public static void ThrowWin32ErrorIfNeeded(string origin=null)
		{
			int lastError = Marshal.GetLastWin32Error();
			if (lastError != 0)
			{
				Win32Exception ex = new Win32Exception(lastError);
				if (origin != null) ex.Source = origin;
			}
		}
	}
}
