using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtBitmap
{
	unsafe public partial class ExtBitmap
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
			return (Math.Abs(b1 - b2) <= maxVariation);
			//return (b1 <= (b2 + maxVariation)) && (b2 <= (b1 + maxVariation));
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
					 BytesAreCloseEnough((byte)(PixelColor >> 8), (byte)(ColorToFind >> 8), shadeVariation) &&
					 BytesAreCloseEnough((byte)(PixelColor), (byte)(ColorToFind), shadeVariation);
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

		unsafe private int ParallelUnsafeCountPixels(byte red, byte green, byte blue, int shadeVariation)
		{
			int lastPos = size - bytesPerPixel;
			int count = 0;

			Parallel.For<int>(0,
							 Height,
							 () => 0,
							 (j, loop, subtotal) =>
							 {
								 fixed (byte* pos = data)
								 {
									 byte* _pos = pos + j * stride;
									 byte* pos2 = _pos + stride;
									 //byte* pos = ((byte*)data) + j * stride;
									 //int pos1 = j * stride;
									 //int pos2 = pos1 + stride - 1;
									 //int subCount = 0;
									 if (shadeVariation == 0)
									 {
										 do
										 {
											 if (blue == *_pos && green == *(_pos+1) && red == *(_pos+2)) subtotal++;
											 _pos += bytesPerPixel;
										 } while (_pos < pos2);
										 
									 }
									 else
										 do
										 {
											 if (IsInShadeVariation(red, green, blue, *(_pos+2), *(_pos+1), *_pos, shadeVariation))
												 subtotal++;
											 _pos += bytesPerPixel;
										 } while (_pos < pos2);
										 
									 return subtotal;
								 }
							 },
							(x) => Interlocked.Add(ref count, x)
							);
			return count;
		}

		private int ParallelCountPixels(byte red, byte green, byte blue, int shadeVariation)
		{
			int lastPos = size - bytesPerPixel;
			int count = 0;

			Parallel.For<int>(0,
							 Height,
							 ()=>0,
							 (j, loop, subtotal) =>
							{
								int pos1 = j * stride;
								int pos2 = pos1 + stride -1;
								//int subCount = 0;
								if (shadeVariation == 0)
								{
									for (int _pos = pos1; _pos < pos2; _pos += bytesPerPixel)
									{
										if (blue == data[_pos] && green == data[_pos + 1] && red == data[_pos + 2]) subtotal++;										
									}
								}
								else
									for (int _pos = pos1; _pos < pos2; _pos += bytesPerPixel)
										if (IsInShadeVariation(red, green, blue, data[_pos + 2], data[_pos + 1], data[_pos], shadeVariation))
											subtotal++;
								return subtotal;
								
							},
							(x) => Interlocked.Add(ref count, x)
							);
			return count;
		}

		private int CountPixels(byte red, byte green, byte blue, int shadeVariation)
		{
			int lastPos = size - bytesPerPixel;
			int count = 0;
			int pos = 0;
			if (shadeVariation == 0)
			{
				while (pos <= lastPos)
				{
					if (blue == data[pos] && green == data[pos + 1] && red == data[pos + 2]) count++;
					pos += bytesPerPixel;
				}
				return count;
			}

			while (pos <= lastPos)
			{
				if (IsInShadeVariation(red, green, blue, data[pos + 2], data[pos + 1], data[pos], shadeVariation))
					count++;

				pos += bytesPerPixel;
			}
			return count;
		}

		private int UnsafeCountPixels(byte red, byte green, byte blue, int shadeVariation)
		{
			int lastPos = size - bytesPerPixel;
			int count = 0;
			//int pos = 0;
			fixed (byte* pData = data)
			{
				byte* pPos = pData;
				byte* pLastPos = pData + lastPos;

				if (shadeVariation == 0)
				{
					while (pPos <= pLastPos)
					{
						if (blue == *pPos && green == *(pPos+1) && red == *(pPos+2)) count++;
						pPos+= bytesPerPixel;
					}
					return count;
				}

				while (pPos <= pLastPos)
				{
					if (IsInShadeVariation(red, green, blue, *(pPos + 2), *(pPos + 1), *pPos, shadeVariation))
						count++;
					pPos += bytesPerPixel;
				}
			}
			return count;
		}

		public int CountPixels(int color, int shadeVariation, bool parallelProcessing)
		{
			byte red, green, blue;
			GetRGBOutOfInt(color, out red, out green, out blue);
			if (parallelProcessing)
				return ParallelUnsafeCountPixels(red, green, blue, shadeVariation);
			return UnsafeCountPixels(red, green, blue, shadeVariation);
		}
	}
}
