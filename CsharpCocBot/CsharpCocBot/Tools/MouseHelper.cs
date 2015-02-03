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

namespace CoC.Bot.Tools
{
    public class MouseHelper
    {
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Win32.Point lpPoint);

        [DllImport("user32.dll")]
        public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

#pragma warning disable 649
        public struct INPUT
        {
            public UInt32 Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        public struct MOUSEINPUT
        {
            public Int32 X;
            public Int32 Y;
            public UInt32 MouseData;
            public UInt32 Flags;
            public UInt32 Time;
            public IntPtr ExtraInfo;
        }

#pragma warning restore 649

        // Mephobia HF reported that this function fails to send mouse clicks to hidden windows 
        public static void ClickOnPoint(IntPtr wndHandle, Win32.Point clientPoint)
        {
            //var oldPos = Cursor.Position; // ref to System.Windows.Forms

            /// get screen coordinates
            ClientToScreen(wndHandle, ref clientPoint);

            /// set cursor on coords, and press mouse
            //Cursor.Position = new Point(clientPoint.x, clientPoint.y); // ref to System.Windows.Forms

            var inputMouseDown = new INPUT();
            inputMouseDown.Type = 0; /// input type mouse
            inputMouseDown.Data.Mouse.Flags = 0x0002; /// left button down

            var inputMouseUp = new INPUT();
            inputMouseUp.Type = 0; /// input type mouse
            inputMouseUp.Data.Mouse.Flags = 0x0004; /// left button up

            var inputs = new INPUT[] { inputMouseDown, inputMouseUp };
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));

            /// return mouse 
            //Cursor.Position = oldPos; // ref to System.Windows.Forms
        }


        static void PostMessageSafe(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            bool returnValue = Win32.PostMessage(hWnd, msg, wParam, lParam);
            if (!returnValue)
            {
                // An error occured
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        // SendMessage and PostMessage should work on hidden forms, use them with the WM_MOUSEXXXX codes and provide the mouse location in the wp or lp parameter, I forget which.
        public static bool ClickOnPoint2(IntPtr wndHandle, Point clientPoint, int times = 1, int delay = 0)
        {
            BlueStackHelper.ActivateBlueStack();
            try
            {
                /// set cursor on coords, and press mouse
                if (wndHandle != IntPtr.Zero)
                {
                    for (int x = 0; x < times; x++)
                    {
                        PostMessageSafe(wndHandle, Win32.WM_LBUTTONDOWN, (IntPtr)0x01, (IntPtr)((clientPoint.X) | ((clientPoint.Y) << 16)));
                        PostMessageSafe(wndHandle, Win32.WM_LBUTTONUP, (IntPtr)0x01, (IntPtr)((clientPoint.X) | ((clientPoint.Y) << 16)));
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
