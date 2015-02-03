using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoC.Bot.Tools.FastFind
{
    static public class FastFindTesting
    {
		const string fullSize = "FullSize";
        const string smallSize = "SmallSize";
        static public void Test()
        {
            FastFindWrapper.SetHWnd(BlueStackHelper.GetBlueStackWindowHandle(), true); // Bind FastFind with BlueStack window
            FastFindWrapper.SnapShot(0, 0, 860, 720, 0); // Take full window capture
            FastFindWrapper.SnapShot(200, 200, 600, 500, 1); // Take just a small part
						FastFindWrapper.SetDebugMode(FastFindWrapper.DEBUG_STREAM_SYSTEM_DETAIL); // Console and File - Detailed System Message
            IntPtr version = FastFindWrapper.FFVersion();
            string s = Marshal.PtrToStringAnsi(version);
            MessageBox.Show("FastFind Version " + s);            
						if (!FastFindWrapper.SaveJPG(0, fullSize, 100))
							MessageBox.Show("Failed to save full BS capture into " + fullSize + FastFindWrapper.GetLastFileSuffix().ToString() + ".JPG");
            else
							MessageBox.Show("Succeeded to save full BS capture into " + fullSize + FastFindWrapper.GetLastFileSuffix().ToString() + ".JPG");
						if (!FastFindWrapper.SaveJPG(1, smallSize, 100))
							MessageBox.Show("Failed to save partial BS capture into " + smallSize + FastFindWrapper.GetLastFileSuffix().ToString() + ".JPG");
						else
							MessageBox.Show("Succeeded to save partial BS capture into " + smallSize + FastFindWrapper.GetLastFileSuffix().ToString() + ".JPG");						
            FastFindHelper.GetPixel(10, 10);
        }
    }
}
