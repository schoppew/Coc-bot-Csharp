using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CoC.Bot.Data;
using Win32;
using CoC.Bot.Tools;

namespace CoC.Bot.BotEngine
{
    internal class ReadText
    {

        public string GetChar(ref int x, int y)
        {
            DetectablePoint dp1;
            DetectablePoint dp2;
            DetectablePoint dp3;
            int width;

            // Search for 'A'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 8, 0xE7E7E7, 6);
                dp2 = new DetectablePoint(x + 5, y + 7, 0xDBDCDB, 6);
                dp3 = new DetectablePoint(x + 6, y + 8, 0xD5D6D5, 6);

                if (CoCHelper.CheckPixelColor(dp1, true) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "A";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'a'
            width = 5;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 5, y + 3, 0xACAEAC, 6);
                dp2 = new DetectablePoint(x + 1, y + 4, 0xC2C3C2, 6);
                dp3 = new DetectablePoint(x + 4, y + 7, 0xDADBDA, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "a";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'B'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 6, y + 1, 0xE2E3E2, 6);
                dp2 = new DetectablePoint(x + 1, y + 1, 0xFBFBFB, 6);
                dp3 = new DetectablePoint(x + 3, y + 4, 0xE8E9E8, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "B";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'b'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 3, y + 4, 0x8D8F8D, 6);
                dp2 = new DetectablePoint(x + 4, y + 7, 0xCACBCA, 6);
                dp3 = new DetectablePoint(x + 1, y + 1, 0xFBFBFB, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "b";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'C'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 1, 0xE8E8E8, 6);
                dp2 = new DetectablePoint(x + 2, y + 2, 0xC9CAC9, 6);
                dp3 = new DetectablePoint(x + 5, y + 3, 0xDBDBDB, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "C";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'c'
            width = 5;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 3, y + 2, 0x818481, 6);
                dp2 = new DetectablePoint(x + 5, y + 8, 0x949694, 6);
                dp3 = new DetectablePoint(x + 4, y + 4, 0xECEDEC, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "c";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'D'
            width = 6;

            for (int i = 0; i < 5; i++)
            {
                dp1 = new DetectablePoint(x + 3, y + 7, 0xACAEAC, 6);
                dp2 = new DetectablePoint(x + 6, y + 1, 0xA0A2A0, 6);
                dp3 = new DetectablePoint(x + 2, y + 3, 0xB6B8B6, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "D";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'd'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 3, y + 4, 0x8D8F8D, 6);
                dp2 = new DetectablePoint(x + 4, y + 7, 0xE4E5E4, 6);
                dp3 = new DetectablePoint(x + 1, y + 3, 0xABADAB, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "d";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'E'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 6, 0xA5A7A5, 6);
                dp2 = new DetectablePoint(x + 2, y + 2, 0xC3C5C3, 6);
                dp3 = new DetectablePoint(x + 3, y + 0, 0x767976, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "E";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'e'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 2, 0x404440, 6);
                dp2 = new DetectablePoint(x + 2, y + 3, 0xF5F5F5, 6);
                dp3 = new DetectablePoint(x + 4, y + 6, 0x7B7D7B, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "e";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'F'
            width = 5;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 3, y + 2, 0x515451, 6);
                dp2 = new DetectablePoint(x + 1, y + 8, 0xE1E1E1, 6);
                dp3 = new DetectablePoint(x + 5, y + 1, 0xC6C7C6, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "F";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'f'
            width = 5;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 5, y + 1, 0xC4C5C4, 6);
                dp2 = new DetectablePoint(x + 2, y + 4, 0xBFC0BF, 6);
                dp3 = new DetectablePoint(x + 2, y + 8, 0x535653, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "f";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'G'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 4, y + 4, 0x707370, 6);
                dp2 = new DetectablePoint(x + 3, y + 0, 0xC4C5C4, 6);
                dp3 = new DetectablePoint(x + 4, y + 5, 0xD1D2D1, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "G";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'g'
            width = 5;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 9, 0xD7D8D7, 6);
                dp2 = new DetectablePoint(x + 4, y + 9, 0xF2F2F2, 6);
                dp3 = new DetectablePoint(x + 4, y + 4, 0xE6E7E6, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "g";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'H'
            width = 8;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 5, y + 8, 0x5F635F, 6);
                dp2 = new DetectablePoint(x + 3, y + 3, 0x464A46, 6);
                dp3 = new DetectablePoint(x + 1, y + 1, 0xFAFAFA, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "H";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'h'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 5, 0x818481, 6);
                dp2 = new DetectablePoint(x + 3, y + 4, 0xD3D4D3, 6);
                dp3 = new DetectablePoint(x + 5, y + 3, 0x646764, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "h";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'I'
            width = 2;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 2, 0xFEFEFE, 6);
                dp2 = new DetectablePoint(x + 2, y + 5, 0xA3A5A3, 6);
                dp3 = new DetectablePoint(x + 1, y + 8, 0xE1E1E1, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "I";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'i'
            width = 2;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 1, 0xF0F1F0, 6);
                dp2 = new DetectablePoint(x + 1, y + 2, 0x454945, 6);
                dp3 = new DetectablePoint(x + 1, y + 8, 0xE1E1E1, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "i";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'J'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 5, 0x8E908E, 6);
                dp2 = new DetectablePoint(x + 4, y + 7, 0xC4C5C4, 6);
                dp3 = new DetectablePoint(x + 6, y + 1, 0xFBFBFB, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "J";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'j'
            width = 2;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 0, 0xA0A2A0, 6);
                dp2 = new DetectablePoint(x + 1, y + 4, 0xFDFDFD, 6);
                dp3 = new DetectablePoint(x + 1, y + 9, 0xFEFEFE, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "j";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'K'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 3, 0xC7C8C7, 6);
                dp2 = new DetectablePoint(x + 3, y + 6, 0x828482, 6);
                dp3 = new DetectablePoint(x + 6, y + 0, 0x7C7E7C, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "K";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'k'
            width = 5;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 1, 0xFBFBFB, 6);
                dp2 = new DetectablePoint(x + 2, y + 1, 0x646764, 6);
                dp3 = new DetectablePoint(x + 4, y + 8, 0xDBDCDB, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "k";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'L'
            width = 4;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 7, 0xFDFDFD, 6);
                dp2 = new DetectablePoint(x + 4, y + 7, 0x828482, 6);
                dp3 = new DetectablePoint(x + 1, y + 0, 0x646764, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "L";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'l'
            width = 2;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 2, 0xFEFEFE, 6);
                dp2 = new DetectablePoint(x + 0, y + 2, 0x909290, 6);
                dp3 = new DetectablePoint(x + 1, y + 7, 0xFEFEFE, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "l";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'M'
            width = 10;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 4, y + 8, 0x8C8E8C, 6);
                dp2 = new DetectablePoint(x + 6, y + 1, 0x6A6D6A, 6);
                dp3 = new DetectablePoint(x + 8, y + 8, 0xA9ABA9, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "M";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'm'
            width = 10;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 8, 0xD5D6D5, 6);
                dp2 = new DetectablePoint(x + 7, y + 2, 0x585C58, 6);
                dp3 = new DetectablePoint(x + 5, y + 4, 0xFFFFFF, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "m";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'N'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 3, y + 1, 0x949694, 6);
                dp2 = new DetectablePoint(x + 5, y + 4, 0x707370, 6);
                dp3 = new DetectablePoint(x + 6, y + 1, 0xBFC0BF, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "N";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'n'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 5, y + 3, 0xBEC0BE, 6);
                dp2 = new DetectablePoint(x + 4, y + 8, 0x8A8D8A, 6);
                dp3 = new DetectablePoint(x + 2, y + 5, 0x7A7D7A, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "n";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'O'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 6, 0xBBBDBB, 6);
                dp2 = new DetectablePoint(x + 2, y + 0, 0x888A88, 6);
                dp3 = new DetectablePoint(x + 5, y + 6, 0xD9DAD9, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "O";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'o'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 4, 0x888A88, 6);
                dp2 = new DetectablePoint(x + 4, y + 5, 0xA9ABA9, 6);
                dp3 = new DetectablePoint(x + 3, y + 2, 0x585C58, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "o";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'P'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 4, y + 2, 0x636663, 6);
                dp2 = new DetectablePoint(x + 2, y + 8, 0x858785, 6);
                dp3 = new DetectablePoint(x + 5, y + 3, 0xE6E7E6, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "P";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'p'
            width = 5;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 4, 0x8D908D, 6);
                dp2 = new DetectablePoint(x + 1, y + 10, 0xFFFFFF, 6);
                dp3 = new DetectablePoint(x + 5, y + 3, 0x898B89, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "p";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'Q'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 4, y + 9, 0xE6E7E6, 6);
                dp2 = new DetectablePoint(x + 2, y + 2, 0xABADAB, 6);
                dp3 = new DetectablePoint(x + 6, y + 4, 0xFFFFFF, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "Q";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'q'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 3, y + 4, 0x696C69, 6);
                dp2 = new DetectablePoint(x + 4, y + 8, 0xFFFFFF, 6);
                dp3 = new DetectablePoint(x + 4, y + 10, 0xC1C2C1, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "q";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'R'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 4, y + 2, 0x575A57, 6);
                dp2 = new DetectablePoint(x + 3, y + 6, 0x939593, 6);
                dp3 = new DetectablePoint(x + 6, y + 1, 0xB8BAB8, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "R";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'r'
            width = 5;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 4, y + 5, 0x747774, 6);
                dp2 = new DetectablePoint(x + 2, y + 6, 0x5F635F, 6);
                dp3 = new DetectablePoint(x + 2, y + 2, 0x464A46, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "r";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'S'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 0, 0x8E908E, 6);
                dp2 = new DetectablePoint(x + 5, y + 4, 0x9C9E9C, 6);
                dp3 = new DetectablePoint(x + 3, y + 7, 0x7C7F7C, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "S";
                }
                x += 1;
            }
            x -= 3;

            // Search for 's'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 2, 0x707370, 6);
                dp2 = new DetectablePoint(x + 4, y + 4, 0xC3C4C3, 6);
                dp3 = new DetectablePoint(x + 2, y + 7, 0x909290, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "s";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'T'
            width = 6;
            x -= 2;

            for (int i = 0; i < 5; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 0, 0x707370, 6);
                dp2 = new DetectablePoint(x + 4, y + 2, 0xB7B8B7, 6);
                dp3 = new DetectablePoint(x + 2, y + 8, 0x969896, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "T";
                }
                x += 1;
            }
            x -= 3;

            // Search for 't'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 1, 0x767976, 6);
                dp2 = new DetectablePoint(x + 1, y + 8, 0xB6B8B6, 6);
                dp3 = new DetectablePoint(x + 4, y + 6, 0xA6A8A6, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "t";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'U'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 1, 0xFBFBFB, 6);
                dp2 = new DetectablePoint(x + 1, y + 8, 0x626562, 6);
                dp3 = new DetectablePoint(x + 5, y + 1, 0xBFC0BF, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "U";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'u'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 3, 0x525652, 6);
                dp2 = new DetectablePoint(x + 2, y + 7, 0xCBCCCB, 6);
                dp3 = new DetectablePoint(x + 4, y + 3, 0x888A88, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "u";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'V'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 2, 0x898C89, 6);
                dp2 = new DetectablePoint(x + 3, y + 5, 0x707370, 6);
                dp3 = new DetectablePoint(x + 5, y + 7, 0xE1E2E1, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "V";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'v'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 3, 0x595C59, 6);
                dp2 = new DetectablePoint(x + 3, y + 6, 0x525652, 6);
                dp3 = new DetectablePoint(x + 5, y + 3, 0xEEEEEE, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "v";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'W'
            width = 11;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 3, 0xAEB0AE, 6);
                dp2 = new DetectablePoint(x + 6, y + 1, 0xC5C6C5, 6);
                dp3 = new DetectablePoint(x + 9, y + 8, 0x989A98, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "W";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'w'
            width = 9;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 3, y + 5, 0x8E918E, 6);
                dp2 = new DetectablePoint(x + 5, y + 7, 0x747674, 6);
                dp3 = new DetectablePoint(x + 8, y + 8, 0x797C79, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "w";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'X'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 2, 0x989B98, 6);
                dp2 = new DetectablePoint(x + 3, y + 7, 0x939593, 6);
                dp3 = new DetectablePoint(x + 5, y + 4, 0xB6B8B6, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "X";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'x'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 4, 0x8C8E8C, 6);
                dp2 = new DetectablePoint(x + 2, y + 8, 0x9EA09E, 6);
                dp3 = new DetectablePoint(x + 5, y + 6, 0x4C4F4C, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "x";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'Y'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 1, 0xA1A3A1, 6);
                dp2 = new DetectablePoint(x + 5, y + 5, 0x8C8E8C, 6);
                dp3 = new DetectablePoint(x + 3, y + 8, 0xD5D6D5, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "Y";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'y'
            width = 6;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 8, 0x5D615D, 6);
                dp2 = new DetectablePoint(x + 3, y + 7, 0xEEEEEE, 6);
                dp3 = new DetectablePoint(x + 3, y + 10, 0xE8E9E8, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "y";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'Z'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 2, 0x505450, 6);
                dp2 = new DetectablePoint(x + 1, y + 7, 0x707370, 6);
                dp3 = new DetectablePoint(x + 5, y + 7, 0x888B88, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "Z";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'Z'
            width = 7;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 2, y + 2, 0x505450, 6);
                dp2 = new DetectablePoint(x + 1, y + 7, 0x707370, 6);
                dp3 = new DetectablePoint(x + 5, y + 7, 0x888B88, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "Z";
                }
                x += 1;
            }
            x -= 3;

            // Search for 'z'
            width = 4;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 4, 0x757775, 6);
                dp2 = new DetectablePoint(x + 3, y + 5, 0x757875, 6);
                dp3 = new DetectablePoint(x + 2, y + 8, 0xDADBDA, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return "z";
                }
                x += 1;
            }
            x -= 3;

            // Search for ','
            width = 3;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 7, 0xE8E8E8, 6);
                dp2 = new DetectablePoint(x + 1, y + 8, 0xFEFEFE, 6);
                dp3 = new DetectablePoint(x + 1, y + 9, 0xA5A7A5, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return ",";
                }
                x += 1;
            }
            x -= 3;

            // Search for '{space}'
            width = 2;

            for (int i = 0; i < 3; i++)
            {
                dp1 = new DetectablePoint(x + 1, y + 3, 0x404440, 6);
                dp2 = new DetectablePoint(x + 1, y + 7, 0x404440, 6);
                dp3 = new DetectablePoint(x + 2, y + 5, 0x404440, 6);

                if (CoCHelper.CheckPixelColor(dp1) && CoCHelper.CheckPixelColor(dp2) &&
                    CoCHelper.CheckPixelColor(dp3))
                {
                    x += width;
                    return " ";
                }
                x += 1;
            }
            x -= 3;

            return "|";
        }

        public int GetDarkElixir(int _x, int _y)
        {
            return -1;
        }

        public int GetDigit(int _x, int _y, string _type)
        {
            return -1;
        }

        public int GetElixir(int _x, int _y)
        {
            return -1;
        }

        public int GetGold(int _x, int _y)
        {
            return -1;
        }

        public int GetNormal(int _x, int _y)
        {
            return -1;
        }

        public static int GetOther(int _x, int _y, string kind)
        {
            return -1;
        }

        public static string GetString(int _y)
        {
            return "";
        }

        public int GetTrophy(int _x, int _y)
        {
            return -1;
        }
    }
}