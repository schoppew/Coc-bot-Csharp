using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtBitmap
{
	public partial class ExtBitmap2
	{
		private void GetRGBOutOfInt(int color, out byte red, out byte green, out byte blue)
		{
			red = (byte)((color >> 16) & 0x00FF);
			green = (byte)((color >> 8) & 0x00FF);
			blue = (byte)(color & 0x00FF);
		}

		private int GetIntFromRGB(byte red, byte green, byte blue)
		{
			return (red << 16) | (green << 8) | blue;
		}

		private bool BytesAreCloseEnough(byte b1, byte b2, int maxVariation)
		{
			return (b1 <= (b2 + maxVariation)) && (b2 <= (b1 + maxVariation));
		}

		private bool IsInShadeVariation(byte red, byte green, byte blue, byte red2, byte green2, byte blue2, int shadeVariation)
		{
			if (shadeVariation == 0)
				return red == red2 && blue == blue2 && green == green2;
			return BytesAreCloseEnough(red, red2, shadeVariation) &&
					BytesAreCloseEnough(blue, blue2, shadeVariation) &&
					BytesAreCloseEnough(green, green2, shadeVariation);
		}

		private bool IsInShadeVariation(int PixelColor, int ColorToFind, int shadeVariation)
		{
			if (shadeVariation <= 0) return (PixelColor & 0x00FFFFFF) == (ColorToFind & 0x00FFFFFF);
			return BytesAreCloseEnough((byte)(PixelColor >> 16), (byte)(ColorToFind >> 16), shadeVariation) &&
				   BytesAreCloseEnough((byte)(PixelColor >>  8), (byte)(ColorToFind >>  8), shadeVariation) &&
				   BytesAreCloseEnough((byte)(PixelColor      ), (byte)(ColorToFind      ), shadeVariation);
		}

		private int FindFirstPixel(int color, int fromPos, int shadeVariation)
		{
			if (shadeVariation == 0)
				return FindFirstPixel(color, fromPos);
			int lastPos = size - bytesPerPixel;
			while (fromPos <= lastPos)
			{
				if (IsInShadeVariation(color, Data[fromPos], shadeVariation)) return fromPos;
				fromPos++;
			}
			return -1;
		}

		private int FindFirstPixel(int color, int fromPos)
		{
			int lastPos = size - bytesPerPixel;
			while (fromPos <= lastPos)
			{
				if (color == (Data[fromPos] & 0x00FFFFFF)) return fromPos;
				fromPos++;
			}
			return -1;
		}
	

		private bool CompareColorWithPixelAtPos(int color, int pos, int shadeVariation)
		{
			if (shadeVariation == 0)
				return color == (Data[pos] & 0x00FFFFFF);
			return IsInShadeVariation(color, Data[pos], shadeVariation);
		}

		/// <summary>
		/// right and bottom INCLUDED
		/// </summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		/// <param name="color"></param>
		/// <param name="shadeVariation"></param>
		/// <returns></returns>
		private int FindPixelInRect(int left, int top, int right, int bottom, int color, int shadeVariation)
		{
			int pos1 = PosFromPoint(left, top);
			while (bottom >= top)
			{
				int cursor = pos1;
				int x = left;
				while (x++ <= right)
				{
					if (CompareColorWithPixelAtPos(color, cursor, shadeVariation))
					{
						return cursor;
					}
					cursor++;
				}
				pos1 += stride;
				bottom--;
			}
			return -1;
		}

		private int PosFromPoint(int x, int y)
		{
			return x * bytesPerPixel + stride * y;
		}

		private int PosFromPoint(Win32.POINT point)
		{
			return point.X + stride * point.Y;
		}

		private Win32.POINT GetPointFromPos(int pos)
		{
			if (pos == -1 || pos >= size) return Win32.POINT.Empty;
			int y = pos / stride;
			int x = pos % stride;
			return new Win32.POINT(x, y);
		}

		private int CountPixels(int color, int shadeVariation)
		{
			int lastPos = size - bytesPerPixel;
			int pos = 0;
			int count = 0;
			//if (shadeVariation == 0)
			//{
			//	while (pos <= lastPos)
			//	{
			//		if (blue == data[pos] && green == data[pos+1] && red == data[pos+2]) count++;					
			//		pos += bytesPerPixel;
			//	}			
			//	return count;
			//}
			while (pos <= lastPos)
			{
				if (IsInShadeVariation(color, Data[pos], shadeVariation))
					count++;

				pos++;
			}
			return count;
		}

	}
}
