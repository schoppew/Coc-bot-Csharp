using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using CoC.Bot.Tools;
using FastFind;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class PixelStuffTests
	{

		[TestMethod]
		public void LaunchBlueStacksAndWaitText()
		{
			using (Chrono chrono = new Chrono("LaunchBlueStacksAndWait"))
				chrono.Success = BlueStacksHelper.StartClashOfClanAndWait();
		}

		[TestMethod]
		public void FullScreenCaptureTest()
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
		public void ReadAndSaveBitmapsTest()
		{
			ExtBitmap.ExtBitmap.SetHWndProvider(null);
			ExtBitmap.ExtBitmap2.SetHWndProvider(null);
			ExtBitmap.ExtBitmap3.SetHWndProvider(null);
			FastFind.FastFindHelper.SetHWndProvider(null);
			ExtBitmap.ExtBitmap eb1 = new ExtBitmap.ExtBitmap();
			ExtBitmap.ExtBitmap2 eb2 = new ExtBitmap.ExtBitmap2();
			ExtBitmap.ExtBitmap3 eb3 = new ExtBitmap.ExtBitmap3();
			
			using (Chrono chrono = new Chrono("Screen capture with ExtBitmap in Background mode"))
			{
				chrono.Success = eb1.SnapShot(false);
				Assert.IsTrue(chrono.Success == true, "ExtBitmap screen capture");
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", eb1.Width, eb1.Height);
			}
			using (Chrono chrono = new Chrono("Screen capture with ExtBitmap2 in Background mode"))
			{
				chrono.Success = eb2.SnapShot(false);
				Assert.IsTrue(chrono.Success == true, "ExtBitmap2 screen capture");
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", eb2.Width, eb2.Height);
			}
			using (Chrono chrono = new Chrono("Screen capture with ExtBitmap3 in Background mode"))
			{
				chrono.Success = eb3.SnapShot(false);
				Assert.IsTrue(chrono.Success == true, "ExtBitmap3 screen capture");
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", eb3.Width, eb3.Height);
			}
			using (Chrono chrono = new Chrono("Screen capture with FastFind"))
			{
				chrono.Success = FastFindHelper.TakeFullScreenCapture(true);
				Assert.IsTrue(chrono.Success == true, "FastFind screen capture");
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", eb3.Width, eb3.Height);
			}
			eb1.Save("eb1.png");
			eb2.Save("eb2.png");
			eb3.Save("eb3.png");
			FastFindWrapper.SaveBMP(FastFindHelper.DEFAULT_SNAP, "ff");
			//FastFindWrapper.SaveJPG(FastFindHelper.DEFAULT_SNAP, "ff", 100);
			FastFindWrapper.LoadFromFile(FastFindHelper.DEFAULT_SNAP, FastFindWrapper.LastSavedFileName);
			eb1.LoadFromFile(FastFindWrapper.LastSavedFileName);
			eb2.LoadFromFile(FastFindWrapper.LastSavedFileName);
			eb3.LoadFromFile(FastFindWrapper.LastSavedFileName);

			// Makes sure that all 4 bitmaps returns the same pixel color for an arbitrary pixel			
			Assert.AreEqual(eb1.NativeGetPixel(150, 150).ToArgb() & 0x00FFFFFF, eb1.GetPixel(150, 150) & 0x00FFFFFF);
			Assert.AreEqual(eb2.NativeGetPixel(150, 150).ToArgb() & 0x00FFFFFF, eb2.GetPixel(150, 150) & 0x00FFFFFF);
			Assert.AreEqual(eb3.NativeGetPixel(150, 150).ToArgb() & 0x00FFFFFF, eb3.GetPixel(150, 150) & 0x00FFFFFF);
			Assert.AreEqual(eb1.GetPixel(150, 150), eb2.GetPixel(150, 150));
			Assert.AreEqual(eb2.GetPixel(150, 150), eb3.GetPixel(150, 150));
			Assert.AreEqual(FastFindWrapper.GetPixel(150, 150, FastFindHelper.DEFAULT_SNAP), eb1.GetPixel(150, 150));
			int clr = FastFindWrapper.GetPixel(150, 150, FastFindHelper.DEFAULT_SNAP);

			// Makes sure that all 4 bitmaps have the same colour count with shade variation
			Assert.AreEqual(FastFindWrapper.ColorCount(clr, FastFindHelper.DEFAULT_SNAP, 20), eb1.CountPixels(clr, 20, true));
			Assert.AreEqual(eb1.CountPixels(clr, 20, true), eb2.CountPixels(clr, 20, true));
			Assert.AreEqual(eb2.CountPixels(clr, 20, true), eb3.CountPixels(clr, 20, true));			
		}

		[TestMethod]
		public void BSScreenCaptureTest()
		{
			int shadeVariation = 30;
			int nbLoop = 1000;
			ExtBitmap.ExtBitmap.SetHWndProvider(BlueStacksHelper.GetBlueStacksWindowHandle);
			ExtBitmap.ExtBitmap2.SetHWndProvider(BlueStacksHelper.GetBlueStacksWindowHandle);
			ExtBitmap.ExtBitmap3.SetHWndProvider(BlueStacksHelper.GetBlueStacksWindowHandle);
			FastFind.FastFindHelper.SetHWndProvider(BlueStacksHelper.GetBlueStacksWindowHandle);
			ExtBitmap.ExtBitmap ebBkgrd = new ExtBitmap.ExtBitmap();
			using (Chrono chrono = new Chrono("BlueStacks capture with ExtBitmap in Background mode"))
			{
				chrono.Success = ebBkgrd.SnapShot(true);
				Assert.IsTrue(chrono.Success == true, "ExtBitmap (Background mode) BS capture");
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", ebBkgrd.Width, ebBkgrd.Height);
			}
			Thread.Sleep(100);
			
			ExtBitmap.ExtBitmap ebNoBkgrd = new ExtBitmap.ExtBitmap();
			using (Chrono chrono = new Chrono("BlueStacks capture with ExtBitmap NOT in Background mode"))
			{
				chrono.Success = ebNoBkgrd.SnapShot(false);
				Assert.IsTrue(chrono.Success == true, "ExtBitmap (NOT in Background mode) BS capture");
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", ebNoBkgrd.Width, ebNoBkgrd.Height);
			}
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("BlueStacks capture with FastFind"))
			{
				chrono.Success = FastFindHelper.TakeFullScreenCapture(true);
				Assert.IsTrue(chrono.Success == true, "FastFind BS capture");
			}
			ExtBitmap.ExtBitmap ebDotNet = new ExtBitmap.ExtBitmap();
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("BlueStacks capture with ExtBitmap.DotNetSnapShot"))
			{
				chrono.Success = ebDotNet.DotNetSnapShot(Rectangle.Empty);
				Assert.IsTrue(chrono.Success == true, "ExtBitmap DotNetSnapShot capture");
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", ebDotNet.Width, ebDotNet.Height);
			}
			ExtBitmap.ExtBitmap2 ebNoBkgrd2 = new ExtBitmap.ExtBitmap2();
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("BlueStacks capture with ExtBitmap2 NOT in Background mode"))
			{
				chrono.Success = ebNoBkgrd2.SnapShot(false);
				Assert.IsTrue(chrono.Success == true, "ExtBitmap2 NOT background capture");
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", ebNoBkgrd2.Width, ebNoBkgrd2.Height);
			}
			ExtBitmap.ExtBitmap3 ebNoBkgrd3 = new ExtBitmap.ExtBitmap3();
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("BlueStacks capture with ExtBitmap3 NOT in Background mode"))
			{
				chrono.Success = ebNoBkgrd3.SnapShot(false);
				Assert.IsTrue(chrono.Success == true, "ExtBitmap3 NOT background capture");
				if (chrono.Success == true)
					chrono.Comment = string.Format("Size {0}x{1}", ebNoBkgrd3.Width, ebNoBkgrd3.Height);
			}
			Thread.Sleep(100);
			
			int ff = FastFindHelper.GetPixel(150, 150);
			int eb1Native = ebBkgrd.NativeGetPixel(150, 150).ToArgb() & 0x00FFFFFF;
			int eb2Native = ebNoBkgrd.NativeGetPixel(150, 150).ToArgb() & 0x00FFFFFF;
			int eb1fast = ebBkgrd.GetPixel(150, 150 );
			int eb2fast = ebNoBkgrd.GetPixel(150, 150 );
			int eb2fast2 = ebNoBkgrd2.GetPixel(150, 150);
			int eb3Native = ebNoBkgrd3.NativeGetPixel(150, 150).ToArgb() & 0x00FFFFFF;
			int eb3fast3 = ebNoBkgrd3.GetPixel(150, 150);

			Assert.AreEqual(ff, eb1Native, "ExtBitmap and FastFind failed to return same color for arbitrary pixel");
			Assert.AreEqual(eb1Native, eb2Native, "ExtBitmap2 failed to return proper NativeGetPixel value");
			Assert.AreEqual(eb2Native, eb1fast, "ExtBitmap (bckground) failed to return proper GetPixel value");
			Assert.AreEqual(eb1fast, eb2fast, "ExtBitmap failed to return proper GetPixel value");
			Assert.AreEqual(eb2fast2 & 0x00FFFFFF, eb1Native, "ExtBitmap2 failed to return proper GetPixel value");
			Assert.AreEqual(eb1fast & 0x00FFFFFF, eb3fast3 & 0x00FFFFFF, "ExtBitmap3 failed to return proper GetPixel value");
			
			int c1 = 0, c2 = 0, c3 = 0, c4 = 0, c5 = 0, c6 = 0, c7 = 0, c8 = 0;

			c1 = FastFindWrapper.ColorCount(eb1Native, 0, 0);
			c2 = ebNoBkgrd.CountPixels(eb1Native, 0, false);
			c3 = ebNoBkgrd2.CountPixels(eb1Native, 0, false);
			c7 = ebNoBkgrd3.CountPixels(eb1Native, 0, false);
			Assert.AreEqual(c1, c2, "ebBkgrd and eb1fast count for a given color");
			Assert.AreEqual(c1, c3, "ExtBitmap.CountPixels and ExtBitmap2.CountPixels");
			Assert.AreEqual(c1, c7, "ExtBitmap.CountPixels and ExtBitmap3.CountPixels");
			c4 = FastFindWrapper.ColorCount(eb1Native, 0, shadeVariation);
			c5 = ebNoBkgrd.CountPixels(eb1Native, shadeVariation, false);
			c6 = ebNoBkgrd2.CountPixels(eb1Native, shadeVariation, false);
			c8 = ebNoBkgrd3.CountPixels(eb1Native, shadeVariation, false);
			Assert.AreEqual(c4, c5);
			Assert.AreEqual(c5, c6);
			Assert.AreEqual(c5, c8);
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("\r\n**SEQUENTIAL PROCESSING**\r\nCount with no Shade Variation (Full BS screen x" + nbLoop.ToString()+ ")\r\n\t\tFastFind:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c1 = FastFindWrapper.ColorCount(eb1Native, 0, 0);
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("\t\tExtBitmap:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c2 = ebNoBkgrd.CountPixels(eb1Native, 0, false);
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("\t\tExtBitmap2:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c3 = ebNoBkgrd2.CountPixels(eb1Native, 0, false);
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("\t\tExtBitmap3:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c7 = ebNoBkgrd3.CountPixels(eb1Native, 0, false);
			Thread.Sleep(100);
			
			using (Chrono chrono = new Chrono("Count with Shade Variation (Full BS screen x" + nbLoop.ToString() + ")\r\n\t\tFastFind:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c4 = FastFindWrapper.ColorCount(eb1Native, 0, shadeVariation);
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("\t\tExtBitmap:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c5 = ebNoBkgrd.CountPixels(eb1Native, shadeVariation, false);
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("\t\tExtBitmap2:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c6 = ebNoBkgrd2.CountPixels(eb1Native, shadeVariation, false);
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("\t\tExtBitmap3:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c8 = ebNoBkgrd3.CountPixels(eb1Native, shadeVariation, false);
			Thread.Sleep(100);
			

			c2 = ebNoBkgrd.CountPixels(eb1Native, 0, true);
			Assert.AreEqual(c1, c2);
			c3 = ebNoBkgrd2.CountPixels(eb1Native, 0, true);
			Assert.AreEqual(c1, c3);
			c7 = ebNoBkgrd3.CountPixels(eb1Native, 0, true);
			Assert.AreEqual(c2, c7);
			c5 = ebNoBkgrd.CountPixels(eb1Native, shadeVariation, true);
			c6 = ebNoBkgrd2.CountPixels(eb1Native, shadeVariation, true);
			Assert.AreEqual(c4, c5);
			Assert.AreEqual(c5, c6);
			c8 = ebNoBkgrd3.CountPixels(eb1Native, shadeVariation, true);
			Assert.AreEqual(c4, c8);

			using (Chrono chrono = new Chrono("\r\n**PARALLEL PROCESSING**\r\nCount with no Shade Variation (Full BS screen x" + nbLoop.ToString() + ")\r\n\t\tExtBitmap:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c2 = ebNoBkgrd.CountPixels(eb1Native, 0, true);
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("\t\tExtBitmap2:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c3 = ebNoBkgrd2.CountPixels(eb1Native, 0, true);
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("\t\tExtBitmap3:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c7 = ebNoBkgrd3.CountPixels(eb1Native, 0, true);
			Thread.Sleep(100);
			
			using (Chrono chrono = new Chrono("Count with Shade Variation (Full BS screen x" + nbLoop.ToString() + ")\r\n\t\tExtBitmap:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c5 = ebNoBkgrd.CountPixels(eb1Native, shadeVariation, true);
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("\t\tExtBitmap2:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c6 = ebNoBkgrd2.CountPixels(eb1Native, shadeVariation, true);
			Thread.Sleep(100);
			using (Chrono chrono = new Chrono("\t\tExtBitmap3:", nbLoop))
				for (int i = 0; i < nbLoop; i++)
					c8 = ebNoBkgrd2.CountPixels(eb1Native, shadeVariation, true);
			Thread.Sleep(100);
			
		}
	}
}
