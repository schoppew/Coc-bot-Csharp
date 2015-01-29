using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsharpCocBot.Functions
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

        public static bool ColorCheck(Color color1, Color color2, int variation = 5)
        {
            return false;
        }

        public static Color GetPixelColor(int _x, int _y)
        {
            return Color.Fuchsia;
        }

        public static Point MultiPixelSearch(int left, int top, int right, int bottom, int xSkip, int ySkip, Color color1, Object[] offColor, int variation)
        {
            return new Point(-1, -1);
        }

        public static Point PixelSearch(int left, int top, int right, int bottom, Color color1, int variation)
        {
            return new Point(-1, -1);
        }

        public static bool BoolPixelSearch(Point pixel1, Point pixel2, Point pixel3, int variation = 10)
        {
            return false;
        }
    }
}
