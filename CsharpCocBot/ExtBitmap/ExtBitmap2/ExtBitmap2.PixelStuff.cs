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
			if (shadeVariation <= 0) return PixelColor == ColorToFind;
			return (Math.Abs(((int)PixelColor & 0x00FF0000) - ((int)ColorToFind & 0x00FF0000)) >> 16 <= shadeVariation) &&
					(Math.Abs(((int)PixelColor & 0x0000FF00) - ((int)ColorToFind & 0x0000FF00)) >> 8 <= shadeVariation) &&
					(Math.Abs(((int)PixelColor & 0x000000FF) - ((int)ColorToFind & 0x000000FF)) <= shadeVariation);
		}

		private int FindFirstPixel(byte red, byte green, byte blue, int fromPos, int shadeVariation)
		{
			if (shadeVariation == 0)
				return FindFirstPixel(red, green, blue, fromPos);
			int lastPos = size - bytesPerPixel;
			while (fromPos <= lastPos)
			{
				if (IsInShadeVariation(red, green, blue, data[fromPos + 2], data[fromPos + 1], data[fromPos], shadeVariation)) return fromPos;
				fromPos += bytesPerPixel;
			}
			return -1;
		}

		private int FindFirstPixel(byte red, byte green, byte blue, int fromPos)
		{
			int lastPos = size - bytesPerPixel;
			while (fromPos <= lastPos)
			{
				if (red == data[fromPos + 2] && green == data[fromPos + 1] && blue == data[fromPos]) return fromPos;
				fromPos += bytesPerPixel;
			}
			return -1;
		}

		private bool CompareColorWithPixelAtPos(byte red, byte green, byte blue, int pos, int shadeVariation)
		{
			if (shadeVariation == 0)
				return blue == data[pos] && green == data[pos + 1] && red == data[pos + 2];
			return IsInShadeVariation(red, green, blue, data[pos + 2], data[pos + 1], data[pos], shadeVariation);
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
			byte red, green, blue;			
			GetRGBOutOfInt(color, out red, out green, out blue);
			int pos1 = PosFromPoint(left, top);
			while (bottom >= top)
			{
				int cursor = pos1;
				int x = left;
				while (x++ <= right)
				{
					if (CompareColorWithPixelAtPos(red, green, blue, cursor, shadeVariation))
					{
						return cursor;
					}
					cursor += bytesPerPixel;
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
			return point.X * bytesPerPixel + stride * point.Y;
		}

		private Win32.POINT GetPointFromPos(int pos)
		{
			if (pos == -1 || pos >= size) return Win32.POINT.Empty;
			int y = pos / stride;
			int x = (pos % stride) / bytesPerPixel;
			return new Win32.POINT(x, y);
		}

		private int CountPixels(byte red, byte green, byte blue, int shadeVariation)
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
				if (IsInShadeVariation(red, green, blue, data[pos + 2], data[pos + 1], data[pos], shadeVariation))
					count++;

				pos += bytesPerPixel;
			}
			return count;
		}

		public int CountPixels(int color, int shadeVariation)
		{
			byte red, green, blue;
			GetRGBOutOfInt(color, out red, out green, out blue);			
			return CountPixels(red, green, blue, shadeVariation);
		}
	}
}
