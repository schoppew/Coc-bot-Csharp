using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.Bot.Data.ScreenData
{
	/// <summary>
	/// 
	/// </summary>
	public class DetectablePoint : ClickablePoint
	{
		public DetectablePoint()
			: base()
		{
			Color = Color.Empty;
			ShadeVariation = 0;
		}

		public DetectablePoint(Point point)
			: this(point, Color.Empty)
		{			
		}

		public DetectablePoint(Point point, Color color, int shadeVariation=0)
			: base(point)
		{
			Color = color;
			ShadeVariation = shadeVariation;
		}

		public DetectablePoint(Point point, int color, int shadeVariation = 0)
			: this(point.X, point.Y, color, shadeVariation)
		{
		}

		public DetectablePoint(int x, int y, int color, int shadeVariation=0)
			: base(x, y)
		{
			Color = Color.FromArgb(color);
			ShadeVariation = shadeVariation;
		}

		public Color Color {get; private set; }
		public int ShadeVariation { get; set; }

		override public bool IsEmpty { get { return base.IsEmpty || Color.IsEmpty; } }
	}
}
