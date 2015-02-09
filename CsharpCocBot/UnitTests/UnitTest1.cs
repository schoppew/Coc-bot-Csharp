using System;
using CoC.Bot.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class PixelStuffTests
	{
		[TestMethod]
		public void LaunchBlueStacksAndWait()
		{
			
		}

		[TestMethod]
		public void CheckScreenCapture()
		{
			ExtBitmap.ExtBitmap ebBkgrd = new ExtBitmap.ExtBitmap();
			ebBkgrd.SnapShot();
			ExtBitmap.ExtBitmap ebNoBkgrd = new ExtBitmap.ExtBitmap();
			ebNoBkgrd.SnapShot();
			ExtBitmap.ExtBitmap ebDotNet = new ExtBitmap.ExtBitmap();
			ebDotNet.SnapShot();
			FastFind.FastFindHelper.TakeFullScreenCapture(true);
		}
	}
}
