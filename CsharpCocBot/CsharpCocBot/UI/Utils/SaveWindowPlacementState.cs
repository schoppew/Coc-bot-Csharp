/*
 * Brief Description:
 * Save the size, location, and state of a Window to ensure that the Window opens in the same position it was closed.
 * Furthermore, making it multi-monitor aware.
 * Also, it handles all of the crazy edge cases that can occur (multiple monitors, save the normal size of the window if it is closed while maximized, etc.) so that you don't have to.
 * In other words, we will make the OS do the work.
 * 
 * Detailed Explanation:
 * 
 * A common user interface tweak is to save the size and location of a window and restore that state when the program starts up again.
 * That way, the users don’t need to re-do their work of resizing and moving the windows to where they want them.
 * However I’ve found that there isn’t really a convenient way to do with in C#.
 * Many solutions focus on recording the window width, height, x and y coordinates and current state (maximized, restored, etc), then applying all those values again on startup.
 * But then you have to deal with edge cases:
 *  What if the user has changed resolutions since your program last ran?
 *  What if it was present on a monitor that’s now disconnected?
 *  
 * Fortunately, there are a couple of native Windows functions that can do all this work for us: SetWindowPlacement and GetWindowPlacement.
 * 
 * Sources:
 * https://msdn.microsoft.com/en-us/library/aa972163.aspx
 * http://blogs.msdn.com/b/davidrickard/archive/2010/03/09/saving-window-size-and-location-in-wpf-and-winforms.aspx
 * http://stackoverflow.com/questions/847752/net-wpf-remember-window-size-between-sessions
 */

namespace CoC.Bot.UI.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Interop;
    using System.Xml;
    using System.Xml.Serialization;

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
    }

    // POINT structure required by WINDOWPLACEMENT structure
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    // WINDOWPLACEMENT stores the position, size, and state of a window
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public int showCmd;
        public POINT minPosition;
        public POINT maxPosition;
        public RECT normalPosition;
    }

    public static class WindowPlacementState
    {
        private static Encoding encoding = new UTF8Encoding();
        private static XmlSerializer serializer = new XmlSerializer(typeof(WINDOWPLACEMENT));

        [DllImport("user32.dll")]
        private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;

        public static void SetPlacement(IntPtr windowHandle, string placementXml)
        {
            if (string.IsNullOrEmpty(placementXml))
            {
                return;
            }

            WINDOWPLACEMENT placement;
            byte[] xmlBytes = encoding.GetBytes(placementXml);

            try
            {
                using (var memoryStream = new MemoryStream(xmlBytes))
                {
                    placement = (WINDOWPLACEMENT)serializer.Deserialize(memoryStream);
                }

                placement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
                placement.flags = 0;
                placement.showCmd = (placement.showCmd == SW_SHOWMINIMIZED ? SW_SHOWNORMAL : placement.showCmd);
                SetWindowPlacement(windowHandle, ref placement);
            }
            catch (InvalidOperationException)
            {
                // Parsing placement XML failed. Fail silently.
            }
        }

        public static void SetPlacement(this Window window, string placementXml)
        {
            WindowPlacementState.SetPlacement(new WindowInteropHelper(window).Handle, placementXml);
        }

        public static string GetPlacement(IntPtr windowHandle)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            GetWindowPlacement(windowHandle, out placement);

            using (var memoryStream = new MemoryStream())
            {
                using (var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                {
                    serializer.Serialize(xmlTextWriter, placement);
                    byte[] xmlBytes = memoryStream.ToArray();
                    return encoding.GetString(xmlBytes);
                }
            }
        }

        public static string GetPlacement(this Window window)
        {
            return WindowPlacementState.GetPlacement(new WindowInteropHelper(window).Handle);
        }
    }
}