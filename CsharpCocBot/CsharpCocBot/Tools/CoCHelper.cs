using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Data;
using FastFind;

namespace CoC.Bot.Tools
{
	static public class CoCHelper
	{
		static public bool Click(ClickablePoint point, int nbClick = 1, int delay = 0)
		{
			return BlueStacksHelper.Click(point, nbClick, delay);
		}
		
		[Obsolete("Add a ClickablePoint variable in Data.ScreenData.ScreenData for that. All screen coordinates should be centralized and named. No hard-coded coordinates anywhere else!")]
		static public bool ClickBad(Point point, int nbClick = 1, int delay = 0)
		{
			return Click((ClickablePoint)point, nbClick, delay);
		}

		static public ClickablePoint SearchPixelInRect(DetectableArea point)
		{
			return (ClickablePoint)FastFindHelper.PixelSearch(point.Left, point.Top, point.Right, point.Bottom, point.Color, point.ShadeVariation);
		}

		[Obsolete("Add a DetectableArea variable in Data.ScreenData.ScreenData for that. All screen coordinates should be centralized and named. No hard-coded coordinates anywhere else!")]
		static public ClickablePoint SearchPixelInRect(int left, int top, int right, int bottom, Color color1, int variation)
		{
			return (ClickablePoint)FastFindHelper.PixelSearch(left, top, right, bottom, color1, variation);
		}

		static public bool CheckPixelColor(DetectablePoint data)
		{
			return FastFindHelper.IsInColorRange(data, data.Color, data.ShadeVariation);			
		}

		[Obsolete("Add a DetectablePoint variable in Data.ScreenData.ScreenData for that. All screen coordinates should be centralized and named. No hard-coded coordinates anywhere else!")]
		static public bool CheckPixelColorBad(Point point, Color color, int shadeVariation)
		{
			return FastFindHelper.IsInColorRange(point, color, shadeVariation);
		}

		static public Color GetPixelColor(ClickablePoint point)
		{
			return FastFindHelper.GetPixelColor(point);
		}

		static public bool IsInColorRange(ClickablePoint point, Color color, int shadeVariation=0)
		{
			return FastFindHelper.IsInColorRange(point, color, shadeVariation);
		}

		static public bool SameColor(Color color1, Color color2, int shadeVariation = 0)
		{
			return FastFindHelper.SameColor(color1, color2, shadeVariation);
		}

		/// <summary>
		/// To be called at start of the bot, before any other call from this helper
		/// </summary>
		static public void Initialize()
		{
			FastFindHelper.SetHWndProvider(BlueStacksHelper.GetBlueStacksWindowHandle); // Set this so the wrapper can find the window when needed
		}

		static public bool MakeFullScreenCapture(string targetFile)
		{
			if (!FastFindHelper.TakeFullScreenCapture(true)) return false;
			return FastFindWrapper.SaveJPG(0, targetFile, 100);
		}
	}
}
