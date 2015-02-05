using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtBitmap;
using Point = Win32.POINT;
using Rect = Win32.RECT;
namespace ExtBitmap
{
	public partial class ExtBitmap
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


		public Rectangle CaptureClientArea { get; private set; }

		// Full client area variant of BackgroundSnapShot
		public bool SnapShot(bool backgroundMode = true)
		{
			IntPtr hWnd = GetHWnd();
			return SnapShot(GetBSArea(hWnd), backgroundMode);
		}

		public bool SnapShot(Rectangle rect, bool backgroundMode = true)
		{
			IntPtr hWnd = GetHWnd();
			CaptureClientArea = rect;
			ClientToWindow(hWnd, ref rect);
			return SnapShot(rect.Left, rect.Top, rect.Width, rect.Height, backgroundMode);
		}

		private bool ClientToWindow(IntPtr hWnd, ref Rectangle rect)
		{
			Rect rWindow = new Rect();
			Rect rClient = new Rect();
			Win32.Win32.GetWindowRect(hWnd, out rWindow);
			Win32.Win32.GetClientRect(hWnd, out rClient);
			Point topleft = new Point(rClient.Left, rClient.Top);

			Win32.Win32.ClientToScreen(hWnd, ref topleft);
			rect.Offset(topleft.X - rWindow.Left, topleft.Y - rWindow.Top);
			return true;
		}

		private Rectangle GetBSArea(IntPtr hWnd)
		{
			Rect win32rect;
			if (!Win32.Win32.GetClientRect(hWnd, out win32rect))
				return Rectangle.Empty;
			return Rectangle.FromLTRB(win32rect.Left, win32rect.Top, win32rect.Right, win32rect.Bottom);
		}

		/// <summary>
		/// This capture algorithm should behave as on the background mode of Autoit Coc Bot (as at end of january 2015)
		/// Coordinates are relative to client area from BlueStack window
		/// </summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public bool SnapShot(int left, int top, int width, int height, bool backgroundMode = true)
		{
			IntPtr hWnd = GetHWnd();
			
			Stopwatch sw = Stopwatch.StartNew();
			FreeCurrentImage();
			IntPtr hCaptureDC = Win32.Win32.GetWindowDC(hWnd);
			IntPtr hMemDC = Win32.Win32.CreateCompatibleDC(hCaptureDC);
			IntPtr hBitmap = Win32.Win32.CreateCompatibleBitmap(hCaptureDC, width, height);
			IntPtr hObjOld = Win32.Win32.SelectObject(hMemDC, hBitmap);

			bool result = true;

			if (backgroundMode)
			{
				if (result)
					result = Win32.Win32.PrintWindow(hWnd, hMemDC, 0);
				if (result)
					Win32.Win32.SelectObject(hMemDC, hBitmap);
			}
			if (result)
				result = Win32.Win32.BitBlt(hMemDC, 0, 0, width, height, hCaptureDC, left, top, Win32.TernaryRasterOperations.SRCCOPY);
			if (result)
				BitMap = Bitmap.FromHbitmap(hBitmap);
			Win32.Win32.DeleteDC(hMemDC);
			Win32.Win32.SelectObject(hMemDC, hObjOld);
			Win32.Win32.ReleaseDC(hWnd, hCaptureDC);
			Win32.Win32.DeleteObject(hBitmap);
			Debug.Assert(result);
			sw.Stop();
			Debug.WriteLine("SnapShot {0}x{1} => {2} (background:{3})", width, height, sw.Elapsed.ToString(), backgroundMode.ToString());
			if (!FillDataFromBitmap()) return false;
			return true;
		}

		/// <summary>
		/// Simple capture implementation using C# high level functions. 
		/// It should only work when BlueStack is fully visible (NOT background mode). 
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public bool DotNetSnapShot(Rectangle rect)
		{
			IntPtr hWnd = GetHWnd();
			
			FreeCurrentImage();
			Point topleft = new Point(rect.Left, rect.Top);
			Point bottomright = new Point(rect.Right, rect.Bottom);
			if (!Win32.Win32.ClientToScreen(hWnd, ref topleft)) return false;
			
			//Create a new bitmap.
			BitMap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);

			// Create a graphics object from the bitmap.
			using (var gfxScreenshot = Graphics.FromImage(BitMap))
			{
				// Take the screenshot from the upper left corner to the right bottom corner.
				gfxScreenshot.CopyFromScreen(topleft.X, topleft.Y, 0, 0, new Size(rect.Width, rect.Height), CopyPixelOperation.SourceCopy);
			}
			if (!FillDataFromBitmap()) return false;
			return true;
		}

	}
}
