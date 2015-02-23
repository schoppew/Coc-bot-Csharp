using System.Linq;

namespace CoC.Bot.BotEngine.ScreenReading
{
	internal class OCRChar
	{
		public OCRChar(int width, params OCRPoint[] points)
		{
			Points = points;
			Width = width;
			Loops = 3;
		}
		public OCRChar(int width, int loops, params OCRPoint[] points)
		{
			Points = points;
			Width = width;
			Loops = loops;
		}
		private OCRPoint[] Points { get; set; }
		private int Width { get; set; }
		private int Loops { get; set; }
		public bool ReadChar(ref int x, int y)
		{
			int _x = x;
			for (int i = 0; i < Loops; i++)
				if (Points.All(pt => pt.CheckFrom(_x + i, y)))
				{
					x += Width + i;
					return true;
				}
			return false;
		}
	}
}
