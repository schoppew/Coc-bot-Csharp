using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Point = Win32.POINT;
namespace ExtBitmap
{
    public partial class ExtBitmap
    {
        // Beware: use this method only it has the apha chanel preserved.
        public int FindFirstNonTransparentPixelPosition()
        {
            for (int i=0; i<size; i+=bytesPerPixel)
                if (data[i + 3] == 255)
                    return i;
            return -1;
        }

        public int FindNextBitmapPos(ExtBitmap searchedImage, int fromPos, int left, int top, int right, int bottom, int shadeVariation)
        {
			int TODO;
            Point pFrom=GetPointFromPos(fromPos);
            //int pos = PosFromPoint(xFrom, yFrom);
            int xMax = right - searchedImage.Width + 1;
            int yMax = bottom - searchedImage.Width + 1;
            int lastLinePos = PosFromPoint(xMax, pFrom.Y);
			int firstPixel = FindFirstNonTransparentPixelPosition();
			Point firstPixelPt = GetPointFromPos(lastLinePos);
			int firstPixelColor = GetPixel(firstPixelPt.X, firstPixelPt.Y);
			xMax -= firstPixelPt.X;
			yMax -= firstPixelPt.Y;

            while (fromPos < lastLinePos)
            {
                //if (searchedImage. searchedImage.data
                fromPos += bytesPerPixel;
            }
			return -1;
        }
    }
}
