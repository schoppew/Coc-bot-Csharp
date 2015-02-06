using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.Bot.Data.ColorList.Tools
{
	public static class ColorListsHelper
	{
		static public decimal Count(ColorListItem item, ExtBitmap.ExtBitmap bitmap, int shadeVariation)
		{
			if (item.ColorCount == 0) return 0;
			int count = 0;
			foreach (int color in item.Colors)
				count += bitmap.CountPixels(color, shadeVariation);
			return (decimal)(count * 100.0)/item.MinPixelCount;			
		}
	}
}
