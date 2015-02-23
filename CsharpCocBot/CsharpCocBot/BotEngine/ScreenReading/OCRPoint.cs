using CoC.Bot.Data;
using CoC.Bot.Tools;

namespace CoC.Bot.BotEngine.ScreenReading
{
	internal class OCRPoint
	{
		public OCRPoint(int dx, int dy, int color, int shadeVariation = 6)
		{
			DeltaX = dx;
			DeltaY = dy;
			Color = color;
			ShadeVariation = shadeVariation;
		}
		public int DeltaX { get; private set; }
		public int DeltaY { get; private set; }
		public int Color { get; private set; }
		public int ShadeVariation { get; private set; }

		public bool CheckFrom(int x, int y)
		{
			return CoCHelper.CheckPixelColor(new DetectablePoint(x + DeltaX, y + DeltaY, Color, ShadeVariation));
		}
	}
}
