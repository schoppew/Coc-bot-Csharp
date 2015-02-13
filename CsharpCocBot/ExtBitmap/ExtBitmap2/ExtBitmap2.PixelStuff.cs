using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtBitmap
{
	public partial class ExtBitmap2
	{
		private void GetRGBOutOfInt(int color, out byte red, out byte green, out byte blue)
		{
			red = (byte)(color >> 16);
			green = (byte)(color >> 8);
			blue = (byte)color;
		}

		private int GetIntFromRGB(byte red, byte green, byte blue)
		{
			return (red << 16) | (green << 8) | blue;
		}

		private bool BytesAreCloseEnough(byte b1, byte b2, int maxVariation)
		{
			return (b1 <= (b2 + maxVariation)) && (b2 <= (b1 + maxVariation));
		}

		private bool IsInShadeVariation(int PixelColor, int ColorToFind, int shadeVariation)
		{
			return BytesAreCloseEnough((byte)(PixelColor >> 16), (byte)(ColorToFind >> 16), shadeVariation) &&
						 BytesAreCloseEnough((byte)(PixelColor >> 8), (byte)(ColorToFind >> 8), shadeVariation) &&
						 BytesAreCloseEnough((byte)(PixelColor), (byte)(ColorToFind), shadeVariation);			
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
			color = color & 0x00FFFFFF;
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
			color = color & 0x00FFFFFF;
			if (shadeVariation == 0)
			{
				while (pos <= lastPos)
					if (color == (Data[pos++] & 0x00FFFFFF)) count++;
				return count;
			}
			while (pos <= lastPos)
			{
				if (IsInShadeVariation(color, Data[pos++], shadeVariation))
					count++;
			}
			return count;
		}

		private int ParallelCountPixels(int color, int shadeVariation)
		{
			int count = 0;

			Parallel.For<int>(0,
				 Height,
				 () => 0,
				 (j, loop, subtotal) =>
				 {
					 int pos1 = j * stride;
					 int pos2 = pos1 + stride;
					 //int subCount = 0;
					 if (shadeVariation == 0)
					 {
						 for (int _pos = pos1; _pos < pos2; _pos++)
							 if (color == (Data[_pos] & 0x00FFFFFF)) subtotal++;
					 }
					 else
						 for (int _pos = pos1; _pos < pos2; _pos++)
							 if (IsInShadeVariation(color, Data[_pos], shadeVariation))
								 subtotal++;
					 return subtotal;

				 },
				(x) => Interlocked.Add(ref count, x)
				);
			return count;
		}

		unsafe private int UnsafeCountPixels(int color, int shadeVariation)
		{
			int lastPos = size - bytesPerPixel;
			int pos = 0;
			int count = 0;
			color = color & 0x00FFFFFF;
			if (shadeVariation == 0)
			{
				while (pos <= lastPos)
					if (color == (Data[pos++] & 0x00FFFFFF)) count++;
				return count;
			}
			while (pos <= lastPos)
			{
				if (IsInShadeVariation(color, Data[pos++], shadeVariation))
					count++;
			}
			return count;
		}

		unsafe private int ParallelUnsafeCountPixels(int color, int shadeVariation)
		{
			int count = 0;

			Parallel.For<int>(0,
				 Height,
				 () => 0,
				 (j, loop, subtotal) =>
				 {
					 //int pos1 = j * stride;
					 //int pos2 = pos1 + stride;
					 fixed (int* pData = Data)
					 {
						 int* pos = pData + j * stride;
						 int* pos2 = pos + stride;
						 //int subCount = 0;
						 if (shadeVariation == 0)
						 {
							 while(pos<pos2)
							 //for (int _pos = pos1; _pos < pos2; _pos++)
								 if (color == (*(pos++) & 0x00FFFFFF)) subtotal++;
						 }
						 else
							 while (pos < pos2)
								 if (IsInShadeVariation(color, *(pos++), shadeVariation))
									 subtotal++;
					 }
					 return subtotal;

				 },
				(x) => Interlocked.Add(ref count, x)
				);
			return count;
		}
		public int CountPixels(int color, int shadeVariation, bool parallelProcessing)
		{
			byte red, green, blue;
			GetRGBOutOfInt(color, out red, out green, out blue);
			if (parallelProcessing)
				return ParallelUnsafeCountPixels(color, shadeVariation);
			return CountPixels(color, shadeVariation);
		}
	}
}
