using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// The namespace is just CoC.Bot.Data in order to simplify usage of them
namespace CoC.Bot.Data
{
	public class DetectableArea
	{
		public DetectableArea(int left, int top, int right, int bottom, int color, int shadeVariation = 0)
		{
			Left = left; Right = right; Bottom = bottom; Top = top; Color = Color.FromArgb(color); ShadeVariation = shadeVariation;
		}
		public DetectableArea(int left, int top, int right, int bottom, Color color, int shadeVariation = 0)
		{
			Left = left; Right = right; Bottom = bottom; Top = top; Color = color; ShadeVariation = shadeVariation;
		}

		// Will make that cleaner later
		public int Left, Top, Right, Bottom, ShadeVariation;
		public Color Color;
	}
}
