using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Data;

namespace CoC.Bot.Functions
{
	internal partial class Village
	{

		public static ClickablePoint GetRequestTroopsButton()
		{
			int left = ScreenData.RequestTroopsButton.Left;
			int top = ScreenData.RequestTroopsButton.Top;
			int right = ScreenData.RequestTroopsButton.Right;
			int bottom = ScreenData.RequestTroopsButton.Bottom;
			int count = 0;

			do
			{
				DetectableArea area = new DetectableArea(left, top, right, bottom, ScreenData.RequestTroopsButton.Color, ScreenData.RequestTroopsButton.ShadeVariation);
				ClickablePoint p1 = Tools.CoCHelper.SearchPixelInRect(area);

				if (Tools.CoCHelper.IsInColorRange(new ClickablePoint(p1.Point.X + ScreenData.RequestTroopsButton2.Point.X, p1.Point.Y + ScreenData.RequestTroopsButton2.Point.Y), ScreenData.RequestTroopsButton2.Color, ScreenData.RequestTroopsButton2.ShadeVariation))
				{
					return p1;
				}
				else
				{
					if (count >= 6)
					{
						break;
					}
					else
					{
						left = p1.Point.X;
						top = p1.Point.Y;
						count++;
					}
				}
			} while (true);

			return new ClickablePoint();
		}

		
	}
}
