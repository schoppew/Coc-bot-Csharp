using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.Bot.Tools
{
    public static class BlueStackHelper
    {
        static IntPtr bshandle = IntPtr.Zero;

        public static bool IsBlueStacksFound { get { return bshandle != IntPtr.Zero; } }

        public static IntPtr GetBlueStackWindowHandle(bool force = false)
        {
            if (bshandle == IntPtr.Zero || force)
              bshandle = Win32.FindWindow("WindowsForms10.Window.8.app.0.33c0d9d", "BlueStacks App Player"); // First try
            if (bshandle == IntPtr.Zero)
                bshandle = Win32.FindWindow(null, "BlueStacks App Player"); // Maybe the class name has changes
            if (bshandle == IntPtr.Zero)
            {
                Process[] proc = Process.GetProcessesByName("BlueStacks App Player"); // If failed, then try with .NET functions
                if (proc == null || proc.Count() == 0)
                    return IntPtr.Zero;
                bshandle = proc[0].MainWindowHandle;
            }
            return bshandle;
        }

        public static bool Click(int x, int y)
        {
            return Click(new Win32.Point(x, y));
        }

        public static bool Click(Win32.Point point)
        {
            if (bshandle == IntPtr.Zero)
                bshandle = GetBlueStackWindowHandle();
            if (bshandle == IntPtr.Zero)
                return false;
            MouseHelper.ClickOnPoint(bshandle, point);
            return true;
        }

        public static bool IsBlueStackRunning
        {
            get
            {
                bshandle = IntPtr.Zero;
                return GetBlueStackWindowHandle() != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Activates and displays the window. If the window is 
        /// minimized or maximized, the system restores it to its original size 
        /// and position. An application should use this when restoring 
        /// a minimized window.
        /// </summary>
        /// <returns></returns>
        public static bool RestoreBlueStack()
        {
            if (!IsBlueStackRunning) return false;
            return Win32.ShowWindow(bshandle, Win32.WindowShowStyle.Restore);
        }

        /// <summary>
        /// Activates the window and displays it in its current size 
        /// and position.
        /// </summary>
        /// <returns></returns>
        public static bool ActivateBlueStack()
        {
            if (!IsBlueStackRunning) return false;
            return Win32.ShowWindow(bshandle, Win32.WindowShowStyle.Show);
        }
    }
}
