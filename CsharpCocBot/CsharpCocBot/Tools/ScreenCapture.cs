using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace CoC.Bot.Tools
{

    public class ScreenCapture : IDisposable
    {
        public Bitmap BitMap { get; private set; }
        private IntPtr hBitmap = IntPtr.Zero;
		public Rectangle CurrentArea { get; private set; }

        #region IDisposable interface
        public void Dispose()
        {
            FreeCurrentImage();
        }
        #endregion IDisposable interface

        private void FreeCurrentImage()
        {
            if (BitMap != null)
            {
                BitMap.Dispose();
                Win32.DeleteObject(hBitmap);
                BitMap = null;
                hBitmap = IntPtr.Zero;
            }
        }

        // Full client area variant of BackgroundSnapShot
        public Bitmap SnapShot(bool backgroundMode = true)
        {
            return SnapShot(GetBSArea(), backgroundMode);
        }

        public Bitmap SnapShot(Rectangle rect, bool backgroundMode = true)
        {
			CurrentArea = rect;
			ClientToWindow(ref rect);
            return SnapShot(rect.Left, rect.Top, rect.Width, rect.Height, backgroundMode);
        }


        /// <summary>
        /// This capture algorithm should behave as on the background mode of Autoit Coc Bot (as at end of january 2015)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Bitmap SnapShot(int left, int top, int width, int height, bool backgroundMode = true)
        {
			Stopwatch sw = Stopwatch.StartNew();
            FreeCurrentImage();
            IntPtr hWnd = BlueStackHelper.GetBlueStackWindowHandle();
            if (hWnd == IntPtr.Zero)
                return null;
            IntPtr hCaptureDC = Win32.GetWindowDC(hWnd);
            IntPtr hMemDC = Win32.CreateCompatibleDC(hCaptureDC);
            hBitmap = Win32.CreateCompatibleBitmap(hCaptureDC, width, height);
            IntPtr hObjOld = Win32.SelectObject(hMemDC, hBitmap);

            bool result = true;

            if (backgroundMode)
            {
                if (result)
                    result = Win32.PrintWindow(hWnd, hMemDC, 0);
                if (result)
                    Win32.SelectObject(hMemDC, hBitmap);
            }
            if (result)
                result = Win32.BitBlt(hMemDC, 0, 0, width, height, hCaptureDC, left, top, Win32.TernaryRasterOperations.SRCCOPY);
            if (result)
                BitMap = Bitmap.FromHbitmap(hBitmap);
            Win32.DeleteDC(hMemDC);
            Win32.SelectObject(hMemDC, hObjOld);
            Win32.ReleaseDC(hWnd, hCaptureDC);
            Win32.DeleteObject(hBitmap);
            Debug.Assert(result);
			sw.Stop();
			Debug.WriteLine("SnapShot {0}x{1} => {2} (background:{3})", width, height, sw.Elapsed.ToString(), backgroundMode.ToString());
            return BitMap;
        }

		private bool ClientToWindow(ref Rectangle rect)
		{
			Win32.RECT rWindow= new Win32.RECT();
		  Win32.RECT rClient= new Win32.RECT();
			Win32.GetWindowRect(BlueStackHelper.GetBlueStackWindowHandle(), out rWindow);
			Win32.GetClientRect(BlueStackHelper.GetBlueStackWindowHandle(), out rClient);
			Point topleft = new Point(rClient.Left, rClient.Top);
			
			Win32.ClientToScreen(BlueStackHelper.GetBlueStackWindowHandle(), ref topleft);			
			rect.Offset(topleft.X - rWindow.Left, topleft.Y - rWindow.Top);
			return true;
		}


        private Rectangle GetBSArea()
        {
            if (!BlueStackHelper.IsBlueStackRunning) return Rectangle.Empty;
            Win32.RECT win32rect;
            if (!Win32.GetClientRect(BlueStackHelper.GetBlueStackWindowHandle(), out win32rect))
                return Rectangle.Empty;
            return Rectangle.FromLTRB(win32rect.Left, win32rect.Top, win32rect.Right, win32rect.Bottom);
        }


        public bool SaveCurrentSnapShotToFile(string fileName)
        {
            if (BitMap == null) return false;
            try
            {
                BitMap.Save(fileName);
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Simple capture implementation using C# high level functions. 
        /// It should only work when BlueStack is fully visible (NOT background mode). 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Bitmap DotNetSnapShot(Rectangle rect)
        {
            FreeCurrentImage();
			//Win32.RECT wrect = new Win32.RECT(rect);
			Win32.Point topleft = new Win32.Point(rect.Left, rect.Top);
			Win32.Point bottomright = new Win32.Point(rect.Right, rect.Bottom);
			if (!Win32.ClientToScreen(BlueStackHelper.GetBlueStackWindowHandle(), ref topleft)) return null;
			rect = new Rectangle(topleft.ToNative(), new Size(rect.Width, rect.Height));

            //Create a new bitmap.
            BitMap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            using (var gfxScreenshot = Graphics.FromImage(BitMap))
            {
                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            }
            return BitMap;
        }

        public Bitmap DotNetSnapShot(int left, int top, int width, int height)
        {
            return DotNetSnapShot(new Rectangle(left, top, width, height));
        }
    }
}
