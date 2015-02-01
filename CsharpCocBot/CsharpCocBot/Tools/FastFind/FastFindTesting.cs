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

            IntPtr version = FastFindWrapper.FFVersion();
            string s = Marshal.PtrToStringAnsi(version);
            MessageBox.Show("FastFind Version " + s);
            FastFindWrapper.SaveBMP(0, fullSize);

            FastFindWrapper.SaveJPG(0, fullSize, 100);
            FastFindWrapper.SaveJPG(1, smallSize, 100);
            FastFindHelper.GetPixel(10, 10);
        }
    }
}
