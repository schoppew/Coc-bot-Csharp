using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CoC.Bot
{
    public static class GlobalVariables
    {
        public static Bitmap hBitmap;
        public static Bitmap hHBitmap;

        public static IntPtr HWnD;

        public static string logPath;
        public static int hLogFileHandle;
        public static bool restart = false;
        public static bool runState = false;
        public static bool backgroundMode = true;

        public static Tools.ScreenCapture screenCap = null;
    }
}
