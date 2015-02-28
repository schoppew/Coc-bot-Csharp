using System;
using CoC.Bot.Data;

namespace CoC.Bot.BotEngine
{
	internal class ReadText
	{

		private static char? GetChar(ref int x, int y)
		{
			foreach (var pair in ScreenData.OCRData)
				if (pair.Value.ReadChar(ref x, y))
					return pair.Key;
			return null; // Don't ask why this...   
		}

		public static string GetString(int y)
		{
			for (int i = 0; i < 4; i++)
			{
				int xTemp = 35;

				if (GetChar(ref xTemp, y) == null)
				{
					y += 1;
				}
				else
				{
					break;
				}
			}

			int x = 35;
			string output = "";
			char? lastChar = null;
			do
			{
				lastChar = GetChar(ref x, y);
				if (lastChar == null) break;
				output += lastChar.Value;
			} while (!output.EndsWith("  "));

			return output;
		}

	    public static string GetGold(int xStart, int yStart)
	    {
	        int x = xStart;
	        int y = yStart;
	        string gold = string.Empty;
	        int i = 0;

	        while (string.IsNullOrEmpty(GetDigit(ref x, y + i, "Gold")))
	        {
	            if (i >= 15)
	                break;

	            i++;
	        }

	        x = xStart;
	        gold += GetDigit(ref x, y + i, "Gold");
            gold += GetDigit(ref x, y + i, "Gold");
            gold += GetDigit(ref x, y + i, "Gold");
	        x += 6;
            gold += GetDigit(ref x, y + i, "Gold");
            gold += GetDigit(ref x, y + i, "Gold");
            gold += GetDigit(ref x, y + i, "Gold");

	        return gold;
	    }

        public static string GetElixir(int xStart, int yStart)
        {
            int x = xStart;
            int y = yStart;
            string elixir = string.Empty;
            int i = 0;

            while (string.IsNullOrEmpty(GetDigit(ref x, y + i, "Elixir")))
            {
                if (i >= 15)
                    break;

                i++;
            }

            x = xStart;
            elixir += GetDigit(ref x, y + i, "Elixir");
            elixir += GetDigit(ref x, y + i, "Elixir");
            elixir += GetDigit(ref x, y + i, "Elixir");
            x += 6;
            elixir += GetDigit(ref x, y + i, "Elixir");
            elixir += GetDigit(ref x, y + i, "Elixir");
            elixir += GetDigit(ref x, y + i, "Elixir");

            return elixir;
        }

        public static string GetDarkElixir(int xStart, int yStart)
        {
            int x = xStart;
            int y = yStart;
            string darkElixir = string.Empty;
            int i = 0;

            while (string.IsNullOrEmpty(GetDigit(ref x, y + i, "DarkElixir")))
            {
                if (i >= 15)
                    break;

                i++;
            }

            x = xStart;
            darkElixir += GetDigit(ref x, y + i, "DarkElixir");
            darkElixir += GetDigit(ref x, y + i, "DarkElixir");
            darkElixir += GetDigit(ref x, y + i, "DarkElixir");
            x += 6;
            darkElixir += GetDigit(ref x, y + i, "DarkElixir");
            darkElixir += GetDigit(ref x, y + i, "DarkElixir");
            darkElixir += GetDigit(ref x, y + i, "DarkElixir");

            return darkElixir;
        }

        public static string GetDigit(ref int x, int y, string type)
        {
            int width;
            int c1, c2, c3;
            DetectablePoint dp1, dp2, dp3;

            // Search for digit 0
            width = 13;

            switch (type)
            {
                case "Gold":
                    c1 = 0x989579;
                    c2 = 0x39382E;
                    c3 = 0x272720;
                    break;
                case "Elixir":
                    c1 = 0x978A96;
                    c2 = 0x393439;
                    c3 = 0x272427;
                    break;
                case "DarkElixir":
                    c1 = 0x909090;
                    c2 = 0x363636;
                    c3 = 0x262626;
                    break;
                case "Builder":
                    c1 = 0x979797;
                    c2 = 0x373737;
                    c3 = 0x262626;
                    break;
                case "Resource":
                    c1 = 0x919191;
                    c2 = 0x373737;
                    c3 = 0x272727;
                    break;
                default:
                    c1 = 0x979797;
                    c2 = 0x393939;
                    c3 = 0x272727;
                    break;
            }

            dp1 = new DetectablePoint(x + 6, y + 4, c1, 20);
            dp2 = new DetectablePoint(x + 7, y + 7, c2, 20);
            dp3 = new DetectablePoint(x + 10, y + 13, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "0";
            }

            x--;
            dp1 = new DetectablePoint(x + 6, y + 4, c1, 20);
            dp2 = new DetectablePoint(x + 7, y + 7, c2, 20);
            dp3 = new DetectablePoint(x + 10, y + 13, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "0";
            }

            x += 2;
            dp1 = new DetectablePoint(x + 6, y + 4, c1, 20);
            dp2 = new DetectablePoint(x + 7, y + 7, c2, 20);
            dp3 = new DetectablePoint(x + 10, y + 13, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "0";
            }
            x--;

            // Search for digit 1
            width = 6;

            switch (type)
            {
                case "Gold":
                    c1 = 0x979478;
                    c2 = 0x313127;
                    c3 = 0xD7D4AC;
                    break;
                case "Elixir":
                    c1 = 0x968895;
                    c2 = 0x312D31;
                    c3 = 0xD8C4D6;
                    break;
                case "DarkElixir":
                    c1 = 0x8F8F8F;
                    c2 = 0x2F2F2F;
                    c3 = 0xCDCDCD;
                    break;
                case "Resource":
                    c1 = 0x919191;
                    c2 = 0x303030;
                    c3 = 0xD0D0D0;
                    break;
                default:
                    c1 = 0x969696;
                    c2 = 0x313131;
                    c3 = 0xD8D8D8;
                    break;
            }

            dp1 = new DetectablePoint(x + 1, y + 1, c1, 20);
            dp2 = new DetectablePoint(x + 1, y + 12, c2, 20);
            dp3 = new DetectablePoint(x + 4, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "1";
            }

            x--;
            dp1 = new DetectablePoint(x + 1, y + 1, c1, 20);
            dp2 = new DetectablePoint(x + 1, y + 12, c2, 20);
            dp3 = new DetectablePoint(x + 4, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "1";
            }

            x += 2;
            dp1 = new DetectablePoint(x + 1, y + 1, c1, 20);
            dp2 = new DetectablePoint(x + 1, y + 12, c2, 20);
            dp3 = new DetectablePoint(x + 4, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "1";
            }
            x--;

            // Search for digit 2
            width = 10;

            switch (type)
            {
                case "Gold":
                    c1 = 0xA09E80;
                    c2 = 0xD8D4AC;
                    c3 = 0x979579;
                    break;
                case "Elixir":
                    c1 = 0xA0919F;
                    c2 = 0xD8C4D6;
                    c3 = 0x978A96;
                    break;
                case "DarkElixir":
                    c1 = 0x989898;
                    c2 = 0xCDCDCD;
                    c3 = 0x909090;
                    break;
                case "Resource":
                    c1 = 0x9E99A0;
                    c2 = 0xD3D3D3;
                    c3 = 0x919191;
                    break;
                default:
                    c1 = 0xA0A0A0;
                    c2 = 0xD8D8D8;
                    c3 = 0x979797;
                    break;
            }

            dp1 = new DetectablePoint(x + 1, y + 7, c1, 20);
            dp2 = new DetectablePoint(x + 3, y + 6, c2, 20);
            dp3 = new DetectablePoint(x + 7, y + 7, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "2";
            }

            x--;
            dp1 = new DetectablePoint(x + 1, y + 7, c1, 20);
            dp2 = new DetectablePoint(x + 3, y + 6, c2, 20);
            dp3 = new DetectablePoint(x + 7, y + 7, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "2";
            }

            x += 2;
            dp1 = new DetectablePoint(x + 1, y + 7, c1, 20);
            dp2 = new DetectablePoint(x + 3, y + 6, c2, 20);
            dp3 = new DetectablePoint(x + 7, y + 7, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "2";
            }
            x--;

            // Search for digit 3
            width = 10;

            switch (type)
            {
                case "Gold":
                    c1 = 0x7F7D65;
                    c2 = 0x070706;
                    c3 = 0x37362C;
                    break;
                case "Elixir":
                    c1 = 0x7F737E;
                    c2 = 0x070607;
                    c3 = 0x373236;
                    break;
                case "DarkElixir":
                    c1 = 0x797979;
                    c2 = 0x070707;
                    c3 = 0x343434;
                    break;
                case "Resource":
                    c1 = 0x7A7A7A;
                    c2 = 0x070707;
                    c3 = 0x373737;
                    break;
                default:
                    c1 = 0x7F7F7F;
                    c2 = 0x070707;
                    c3 = 0x373737;
                    break;
            }

            dp1 = new DetectablePoint(x + 2, y + 3, c1, 20);
            dp2 = new DetectablePoint(x + 4, y + 8, c2, 20);
            dp3 = new DetectablePoint(x + 5, y + 13, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "3";
            }

            x--;
            dp1 = new DetectablePoint(x + 2, y + 3, c1, 20);
            dp2 = new DetectablePoint(x + 4, y + 8, c2, 20);
            dp3 = new DetectablePoint(x + 5, y + 13, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "3";
            }

            x += 2;
            dp1 = new DetectablePoint(x + 2, y + 3, c1, 20);
            dp2 = new DetectablePoint(x + 4, y + 8, c2, 20);
            dp3 = new DetectablePoint(x + 5, y + 13, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "3";
            }
            x--;

            // Search for digit 4
            width = 12;

            switch (type)
            {
                case "Gold":
                    c1 = 0x282720;
                    c2 = 0x080806;
                    c3 = 0x403F33;
                    break;
                case "Elixir":
                    c1 = 0x282428;
                    c2 = 0x080708;
                    c3 = 0x403A40;
                    break;
                case "DarkElixir":
                    c1 = 0x262626;
                    c2 = 0x070707;
                    c3 = 0x3D3D3D;
                    break;
                default:
                    c1 = 0x282828;
                    c2 = 0x080808;
                    c3 = 0x404040;
                    break;
            }

            dp1 = new DetectablePoint(x + 2, y + 3, c1, 20);
            dp2 = new DetectablePoint(x + 3, y + 1, c2, 20);
            dp3 = new DetectablePoint(x + 1, y + 5, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "4";
            }

            x--;
            dp1 = new DetectablePoint(x + 2, y + 3, c1, 20);
            dp2 = new DetectablePoint(x + 3, y + 1, c2, 20);
            dp3 = new DetectablePoint(x + 1, y + 5, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "4";
            }

            x += 2;
            dp1 = new DetectablePoint(x + 2, y + 3, c1, 20);
            dp2 = new DetectablePoint(x + 3, y + 1, c2, 20);
            dp3 = new DetectablePoint(x + 1, y + 5, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "4";
            }
            x--;

            // Search for digit 5
            width = 10;

            switch (type)
            {
                case "Gold":
                    c1 = 0x060604;
                    c2 = 0x040403;
                    c3 = 0xB7B492;
                    break;
                case "Elixir":
                    c1 = 0x060606;
                    c2 = 0x040404;
                    c3 = 0xB7A7B6;
                    break;
                case "DarkElixir":
                    c1 = 0x060606;
                    c2 = 0x040404;
                    c3 = 0xAFAFAF;
                    break;
                default:
                    c1 = 0x060606;
                    c2 = 0x040404;
                    c3 = 0xB7B7B7;
                    break;
            }

            dp1 = new DetectablePoint(x + 5, y + 4, c1, 20);
            dp2 = new DetectablePoint(x + 4, y + 9, c2, 20);
            dp3 = new DetectablePoint(x + 6, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "5";
            }

            x--;
            dp1 = new DetectablePoint(x + 5, y + 4, c1, 20);
            dp2 = new DetectablePoint(x + 4, y + 9, c2, 20);
            dp3 = new DetectablePoint(x + 6, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "5";
            }

            x += 2;
            dp1 = new DetectablePoint(x + 5, y + 4, c1, 20);
            dp2 = new DetectablePoint(x + 4, y + 9, c2, 20);
            dp3 = new DetectablePoint(x + 6, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "5";
            }
            x--;

            // Search for digit 6
            width = 11;

            switch (type)
            {
                case "Gold":
                    c1 = 0x070605;
                    c2 = 0x040403;
                    c3 = 0x181713;
                    break;
                case "Elixir":
                    c1 = 0x070707;
                    c2 = 0x040404;
                    c3 = 0x181618;
                    break;
                case "DarkElixir":
                    c1 = 0x060606;
                    c2 = 0x030303;
                    c3 = 0x161616;
                    break;
                default:
                    c1 = 0x070707;
                    c2 = 0x040404;
                    c3 = 0x181818;
                    break;
            }

            dp1 = new DetectablePoint(x + 5, y + 4, c1, 20);
            dp2 = new DetectablePoint(x + 5, y + 9, c2, 20);
            dp3 = new DetectablePoint(x + 8, y + 5, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "6";
            }

            x--;
            dp1 = new DetectablePoint(x + 5, y + 4, c1, 20);
            dp2 = new DetectablePoint(x + 5, y + 9, c2, 20);
            dp3 = new DetectablePoint(x + 8, y + 5, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "6";
            }

            x += 2;
            dp1 = new DetectablePoint(x + 5, y + 4, c1, 20);
            dp2 = new DetectablePoint(x + 5, y + 9, c2, 20);
            dp3 = new DetectablePoint(x + 8, y + 5, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "6";
            }
            x--;

            // Search for digit 74
            width = 21;

            switch (type)
            {
                case "Gold":
                    c1 = 0x414034;
                    c2 = 0x4C4B3D;
                    c3 = 0xD3D0A9;
                    break;
                case "Elixir":
                    c1 = 0x413E38;
                    c2 = 0x4C4941;
                    c3 = 0xD3CEAB;
                    break;
                case "DarkElixir":
                    c1 = 0x3F3F3F;
                    c2 = 0x4A4A4A;
                    c3 = 0xD1D1D1;
                    break;
                default:
                    c1 = 0x414141;
                    c2 = 0x4C4C4C;
                    c3 = 0xD3D3D3;
                    break;
            }

            dp1 = new DetectablePoint(x + 13, y + 7, c1, 20);
            dp2 = new DetectablePoint(x + 7, y + 7, c2, 20);
            dp3 = new DetectablePoint(x + 1, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "74";
            }

            x--;
            dp1 = new DetectablePoint(x + 13, y + 7, c1, 20);
            dp2 = new DetectablePoint(x + 7, y + 7, c2, 20);
            dp3 = new DetectablePoint(x + 1, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "74";
            }

            x += 2;
            dp1 = new DetectablePoint(x + 13, y + 7, c1, 20);
            dp2 = new DetectablePoint(x + 7, y + 7, c2, 20);
            dp3 = new DetectablePoint(x + 1, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "74";
            }
            x--;

            // Search for digit 7
            width = 10;

            switch (type)
            {
                case "Gold":
                    c1 = 0x5E5C4B;
                    c2 = 0x87856C;
                    c3 = 0x5D5C4B;
                    break;
                case "Elixir":
                    c1 = 0x5F565E;
                    c2 = 0x877B86;
                    c3 = 0x5F565E;
                    break;
                case "DarkElixir":
                    c1 = 0x5A5A5A;
                    c2 = 0x818181;
                    c3 = 0x5A5A5A;
                    break;
                default:
                    c1 = 0x5F5F5F;
                    c2 = 0x878787;
                    c3 = 0x5F5F5F;
                    break;
            }

            dp1 = new DetectablePoint(x + 5, y + 11, c1, 20);
            dp2 = new DetectablePoint(x + 4, y + 3, c2, 20);
            dp3 = new DetectablePoint(x + 7, y + 7, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "7";
            }

            x--;
            dp1 = new DetectablePoint(x + 5, y + 11, c1, 20);
            dp2 = new DetectablePoint(x + 4, y + 3, c2, 20);
            dp3 = new DetectablePoint(x + 7, y + 7, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "7";
            }

            x += 2;
            dp1 = new DetectablePoint(x + 5, y + 11, c1, 20);
            dp2 = new DetectablePoint(x + 4, y + 3, c2, 20);
            dp3 = new DetectablePoint(x + 7, y + 7, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "7";
            }
            x--;

            // Search for digit 8
            width = 11;

            switch (type)
            {
                case "Gold":
                    c1 = 0x27261F;
                    c2 = 0x302F26;
                    c3 = 0x26261F;
                    break;
                case "Elixir":
                    c1 = 0x272427;
                    c2 = 0x302B2F;
                    c3 = 0x26261F;
                    break;
                case "DarkElixir":
                    c1 = 0x252525;
                    c2 = 0x2D2D2D;
                    c3 = 0x242424;
                    break;
                default:
                    c1 = 0x272727;
                    c2 = 0x303030;
                    c3 = 0x262626;
                    break;
            }

            dp1 = new DetectablePoint(x + 5, y + 3, c1, 20);
            dp2 = new DetectablePoint(x + 5, y + 10, c2, 20);
            dp3 = new DetectablePoint(x + 1, y + 6, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "8";
            }

            x--;
            dp1 = new DetectablePoint(x + 5, y + 3, c1, 20);
            dp2 = new DetectablePoint(x + 5, y + 10, c2, 20);
            dp3 = new DetectablePoint(x + 1, y + 6, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "8";
            }

            x += 2;
            dp1 = new DetectablePoint(x + 5, y + 3, c1, 20);
            dp2 = new DetectablePoint(x + 5, y + 10, c2, 20);
            dp3 = new DetectablePoint(x + 1, y + 6, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "8";
            }
            x--;

            // Search for digit 9
            width = 11;

            switch (type)
            {
                case "Gold":
                    c1 = 0x302F26;
                    c2 = 0x050504;
                    c3 = 0x272720;
                    break;
                case "Elixir":
                    c1 = 0x302C30;
                    c2 = 0x050505;
                    c3 = 0x282427;
                    break;
                case "DarkElixir":
                    c1 = 0x2E2E2E;
                    c2 = 0x050505;
                    c3 = 0x262626;
                    break;
                default:
                    c1 = 0x303030;
                    c2 = 0x050505;
                    c3 = 0x272727;
                    break;
            }

            dp1 = new DetectablePoint(x + 5, y + 5, c1, 20);
            dp2 = new DetectablePoint(x + 5, y + 9, c2, 20);
            dp3 = new DetectablePoint(x + 8, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "9";
            }

            x--;
            dp1 = new DetectablePoint(x + 5, y + 5, c1, 20);
            dp2 = new DetectablePoint(x + 5, y + 9, c2, 20);
            dp3 = new DetectablePoint(x + 8, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "9";
            }

            x += 2;
            dp1 = new DetectablePoint(x + 5, y + 5, c1, 20);
            dp2 = new DetectablePoint(x + 5, y + 9, c2, 20);
            dp3 = new DetectablePoint(x + 8, y + 12, c3, 20);

            if (Tools.CoCHelper.CheckPixelColor(dp1) && Tools.CoCHelper.CheckPixelColor(dp2) &&
                Tools.CoCHelper.CheckPixelColor(dp3))
            {
                x += width;
                return "9";
            }
            x--;

            return "";
        }

		#region Not implemented function

		public int GetNormal(int _x, int _y)
		{
			throw new NotImplementedException();
		}

		public static int GetOther(int _x, int _y, string kind)
		{
			throw new NotImplementedException();
		}

		public int GetTrophy(int _x, int _y)
		{
			throw new NotImplementedException();
		}
		#endregion Not implemented function
	}
}