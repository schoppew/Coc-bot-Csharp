namespace MouseAndKeyboard
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Threading;

	public static class MouseHelper
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

			
			var inputMouseUp = new Win32.INPUT();
            inputMouseUp.Type = 0; /// input type mouse
            inputMouseUp.Data.Mouse.Flags = 0x0004; /// left button up

			var inputs = new Win32.INPUT[] { inputMouseUp };
			Win32.Win32.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Win32.INPUT)));

            /// return mouse 
            //Cursor.Position = oldPos; // ref to System.Windows.Forms
        }

        private static void PostMessageSafe(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
			bool returnValue = Win32.Win32.PostMessage(hWnd, msg, wParam, lParam);            
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
						PostMessageSafe(wndHandle, Win32.Win32.WM_LBUTTONUP, (IntPtr)0x01, (IntPtr)((clientPoint.X) | ((clientPoint.Y) << 16)));                        
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

		/// <summary>
		/// FF: If this doesn't work (and I guess it won't), we may have to use SetCapture/ReleaseCapture Win32 api
		/// http://winapi.freetechsecrets.com/win32/WIN32SetCapture.htm
		/// </summary>
		/// <param name="wndHandle"></param>
		/// <param name="delay"></param>
		/// <returns></returns>
		public static Win32.POINT GetPointOnClick(IntPtr wndHandle, int delay = 20)
		{
			Win32.POINT clientPoint = new Win32.POINT();

			if (wndHandle != IntPtr.Zero)
			{
				Thread.Sleep(delay);
				while (true)
				{
					// Listen to Left Mouse Click event
					//      0	= Has not been pressed since the last call
					//      1	= Is not currently pressed, but has been pressed since the last call
					// -32767	= Is currently pressed
					var keyState = Win32.Win32.GetAsyncKeyState((ushort)KeyboardHelper.VirtualKeys.VK_LBUTTON);
					if (keyState == Int16.MinValue)
					{
						if (Win32.Win32.GetCursorPos(ref clientPoint))
						{
							// get screen coordinates
							Win32.Win32.ClientToScreen(wndHandle, ref clientPoint);							
						}
					}
				}
			}

			return clientPoint;
		}
    }
}
