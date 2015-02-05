using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtBitmap
{
	public partial class ExtBitmap
	{
		private bool BytesAreCloseEnough(byte b1, byte b2, int maxVariation)
		{
			return (b1 <= (b2 + maxVariation)) && (b2 <= (b1 + maxVariation));
		}

		private bool IsInShadeVariation(byte red, byte green, byte blue, byte red2, byte green2, byte blue2, int shadeVariation)
		{
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
	}
}
