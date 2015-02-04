using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Data.ScreenData;

namespace CoC.Bot.Tools
{
	static public class CoCHelper
	{
		static public bool Click(ClickablePoint point, int nbClick = 0, int delay = 0)
		{
			return BlueStackHelper.Click(point, nbClick, delay);
		}
		
		[Obsolete("All screen coordinates should be stored in Data.ScreenData.ScreenData. No hard-coded coordinates anywhere else!")]
		static public bool ClickBad(Point point, int nbClick = 0, int delay = 0)
		{
			return Click((ClickablePoint)point, nbClick, delay);
		}


	}
}
