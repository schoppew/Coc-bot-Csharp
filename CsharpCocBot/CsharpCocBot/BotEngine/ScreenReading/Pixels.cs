using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CoC.Bot.Tools.FastFind;

namespace CoC.Bot.BotEngine
{
    class Pixels
    {
        public static Bitmap CaptureRegion(int left = 0, int top = 0, int right = 720, int bottom = 720, bool returnBmp = false)
        {
            Bitmap b = null;

            if(returnBmp)
            {
                return b;
            }
            else
            {
                return null;
            }
        }

      [Obsolete("This method is deprecated. Use FastFindHelper.IsInColorRange(Point point, Color color, int shadeVariation) instead.")] 
      public static bool ColorCheck(Color color1, Color color2, int variation = 5)
        {
            return false;
        }

      
        [Obsolete("This method is deprecated. Have to recode this, the structure used in the Autoit project is much too clumsy. Use")]
        public static Point MultiPixelSearch(int left, int top, int right, int bottom, int xSkip, int ySkip, Color color1, Object[] offColor, int variation)
        {
            return new Point(-1, -1);
        }

        

        [Obsolete("Have to be coded first. Is it that useful?!")]
        public static bool BoolPixelSearch(Point pixel1, Point pixel2, Point pixel3, int variation = 10)
        {
            return false;
        }
    }
}
