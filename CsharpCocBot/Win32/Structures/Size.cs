using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win32
{
	public struct Size
	{
		public int cx;

		public int cy;


		public Size(int cx, int cy)
		{
			this.cx = cx;
			this.cy = cy;
		}
	}
}
