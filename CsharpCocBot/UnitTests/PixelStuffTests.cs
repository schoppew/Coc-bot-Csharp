using System;
using CoC.Bot.Tools;
using FastFind;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class PixelStuffTests
	{

		[TestMethod]
		public void LaunchBlueStacksAndWait()
		{
			using (Chrono chrono = new Chrono("LaunchBlueStacksAndWait"))
				chrono.Success = BlueStacksHelper.StartClashOfClanAndWait();
		}

		[TestMethod]
		public void CheckFullScreenCapture()
		{
			
			//ExtBitmap.ExtBitmap ebBkgrd = new ExtBitmap.ExtBitmap();
			//using (Chrono chrono = new Chrono("Full screen capture with ExtBitmap in Background mode"))
			//{
			//	chrono.Success = ebBkgrd.SnapShot(true);
			//	Assert.IsTrue(chrono.Success==true, "Full Screen capture failed with ExtBitmap in background mode (normal: PrintScrint doesn't work in full screen)");
			//	if (chrono.Success==true)
			//		chrono.Comment = string.Format("Size {0}x{1}", ebBkgrd.Width, ebBkgrd.Height);
			//}
			ExtBitmap.ExtBitmap ebNoBkgrd = new ExtBitmap.ExtBitmap();
			using (Chrono chrono = new Chrono("Full screen capture with ExtBitmap NOT in Background mode"))
			{
				chrono.Success = ebNoBkgrd.SnapShot(false);
				Assert.IsTrue(chrono.Success == true);
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", ebNoBkgrd.Width, ebNoBkgrd.Height);
			}
			ExtBitmap.ExtBitmap ebDotNet = new ExtBitmap.ExtBitmap();
			using (Chrono chrono = new Chrono("Full screen capture with FastFind"))
			{
				FastFindWrapper.SetHWnd(IntPtr.Zero, false);
				chrono.Success = FastFindWrapper.SnapShot(0, 0, 0, 0, 2) != 0;
				Assert.IsTrue(chrono.Success == true);				
			}
		}

		[TestMethod]
		public void CheckBSScreenCapture()
		{
			ExtBitmap.ExtBitmap.SetHWndProvider(BlueStacksHelper.GetBlueStacksWindowHandle);
			FastFind.FastFindHelper.SetHWndProvider(BlueStacksHelper.GetBlueStacksWindowHandle);
			ExtBitmap.ExtBitmap ebBkgrd = new ExtBitmap.ExtBitmap();
			using (Chrono chrono = new Chrono("BlueStacks capture with ExtBitmap in Background mode"))
			{
				chrono.Success = ebBkgrd.SnapShot(true);
				Assert.IsTrue(chrono.Success == true, "ExtBitmap (Background mode) BS capture");
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", ebBkgrd.Width, ebBkgrd.Height);
			}
			ExtBitmap.ExtBitmap ebNoBkgrd = new ExtBitmap.ExtBitmap();
			using (Chrono chrono = new Chrono("BlueStacks capture with ExtBitmap NOT in Background mode"))
			{
				chrono.Success = ebNoBkgrd.SnapShot(false);
				Assert.IsTrue(chrono.Success == true, "ExtBitmap (NOT in Background mode) BS capture");
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", ebNoBkgrd.Width, ebNoBkgrd.Height);
			}
			ExtBitmap.ExtBitmap ebDotNet = new ExtBitmap.ExtBitmap();
			using (Chrono chrono = new Chrono("BlueStacks capture with FastFind"))
			{
				chrono.Success = FastFindHelper.TakeFullScreenCapture(true);
				Assert.IsTrue(chrono.Success == true, "FastFind BS capture");
			}
			int ff = FastFindHelper.GetPixel(150, 150);
			int eb1Native = ebBkgrd.NativeGetPixel(150, 150).ToArgb() & 0x00FFFFFF;
			int eb2Native = ebNoBkgrd.NativeGetPixel(150, 150).ToArgb() & 0x00FFFFFF;
			int eb1fast = ebBkgrd.GetPixel(150, 150 );
			int eb2fast = ebNoBkgrd.GetPixel(150, 150 );
			
			int c1=0, c2=0;
			c1 = ebNoBkgrd.CountPixels(eb1Native, 0);
			c2 = FastFindWrapper.ColorCount(eb1Native, 0, 0);
			Assert.IsTrue(c1 == c2, "ebBkgrd and eb1fast count for a given color");

			using (Chrono chrono = new Chrono("ExtBitmap Count with no Shade Variation (Full BS screen x50)"))
				for (int i = 0; i < 50; i++)
					c1 = ebNoBkgrd.CountPixels(eb1Native, 0);
			using (Chrono chrono = new Chrono("FastFind Count with no Shade Variation (Full BS screen x50)"))
				for (int i = 0; i < 50; i++)
					c2 = FastFindWrapper.ColorCount(eb1Native, 0, 0);
			Assert.IsTrue(c1 == c2, "ebBkgrd and eb1fast count for a given color (No SV)");

			using (Chrono chrono = new Chrono("ExtBitmap Count with Shade Variation (Full BS screen x50)"))
				for (int i = 0; i < 50; i++)
					c1 = ebNoBkgrd.CountPixels(eb1Native, 1);
			using (Chrono chrono = new Chrono("FastFind Count with Shade Variation (Full BS screen x50)"))
				for (int i = 0; i < 50; i++)
					c2 = FastFindWrapper.ColorCount(eb1Native, 0, 1);
			Assert.IsTrue(c1 == c2, "ebBkgrd and eb1fast count for a given color (SV=30)");			

			Assert.AreEqual(ff, eb1Native, "eb1Native color count in ebNoBkgrd");
			Assert.AreEqual(eb1Native, eb2Native, "ebBkgrd.NativeGetPixel vs ebNoBkgrd.NativeGetPixel");
			Assert.AreEqual(eb2Native, eb1fast, "ebNoBkgrd.NativeGetPixel vs ebBkgrd.GetPixel");
			Assert.AreEqual(eb1fast, eb2fast, "ebBkgrd.GetPixel vs ebNoBkgrd.GetPixel");
		}


	}
}
