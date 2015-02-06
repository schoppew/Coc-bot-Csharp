using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
//using System.Windows.Forms; // this is WPF, we don't need WinForms here

namespace MouseAndKeyboard
{
    static public class MouseHelper
    {
		#region Window handle provider
		public delegate IntPtr HandleProvider();
		static HandleProvider CustomProvider = null;

		/// <summary>
		/// Use this method to provide a the proper Window Handle to bind with the right window.
		/// If you don't set this, then FastFind will work on FullScreen. 
		/// </summary>
		/// <param name="provider"></param>
		static public void SetHWndProvider(HandleProvider provider)
		{
			CustomProvider = provider;
		}

		static IntPtr GetHWnd()
		{
			if (CustomProvider != null) return CustomProvider();
			return IntPtr.Zero;
		}
		#endregion Window handle provider

        // Mephobia HF reported that this function fails to send mouse clicks to hidden windows 
        public static void ClickOnPoint(IntPtr wndHandle, Win32.POINT clientPoint)
        {
            //var oldPos = Cursor.Position; // ref to System.Windows.Forms

            /// get screen coordinates
            Win32.Win32.ClientToScreen(wndHandle, ref clientPoint);

            /// set cursor on coords, and press mouse
            //Cursor.Position = new Point(clientPoint.x, clientPoint.y); // ref to System.Windows.Forms

			var inputMouseDown = new Win32.INPUT();
            inputMouseDown.Type = 0; /// input type mouse
            inputMouseDown.Data.Mouse.Flags = 0x0002; /// left button down

			var inputMouseUp = new Win32.INPUT();
            inputMouseUp.Type = 0; /// input type mouse
            inputMouseUp.Data.Mouse.Flags = 0x0004; /// left button up

			var inputs = new Win32.INPUT[] { inputMouseDown, inputMouseUp };
			Win32.Win32.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Win32.INPUT)));

            /// return mouse 
            //Cursor.Position = oldPos; // ref to System.Windows.Forms
        }


        static void PostMessageSafe(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
			bool returnValue = Win32.Win32.PostMessage(hWnd, msg, wParam, lParam);
            if (!returnValue)
            {
                // An error occured
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        // SendMessage and PostMessage should work on hidden forms, use them with the WM_MOUSEXXXX codes and provide the mouse location in the wp or lp parameter, I forget which.
		public static bool ClickOnPoint2(IntPtr wndHandle, Win32.POINT clientPoint, int times = 1, int delay = 20)
        {
            //BlueStacksHelper.ActivateBlueStacks();
            try
            {
                /// set cursor on coords, and press mouse
                if (wndHandle != IntPtr.Zero)
                {
                    for (int x = 0; x < times; x++)
                    {
						PostMessageSafe(wndHandle, Win32.Win32.WM_LBUTTONDOWN, (IntPtr)0x01, (IntPtr)((clientPoint.X) | ((clientPoint.Y) << 16)));
						PostMessageSafe(wndHandle, Win32.Win32.WM_LBUTTONUP, (IntPtr)0x01, (IntPtr)((clientPoint.X) | ((clientPoint.Y) << 16)));
                        Thread.Sleep(delay);
                    }
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Debug.Assert(false, ex.Message);
                return false;
            }
            return true;
        }

    }
}
