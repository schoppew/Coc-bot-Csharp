﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Data;

namespace CoC.Bot.Tools.FastFind
{
    static public class FastFindHelper
    {
        // The default delay in ms between to captures. If calling for a capture more often, then by default will provide the previous one. 
        const int MINIMUM_DELAY_BETWEEN_CAPTURES = 1000;
        public const int DEFAULT_SNAP = 0;
        public const int CUSTOM_SNAP = 0;


        static private Stopwatch lastFullCapture = null;

        /// <summary>
        /// Takes a screen shot of the full client area of the BlueStack window
        /// </summary>
        /// <returns></returns>
        static public bool TakeFullScreenCapture(bool forceNew = false)
        {
            if ((lastFullCapture != null && lastFullCapture.ElapsedMilliseconds > MINIMUM_DELAY_BETWEEN_CAPTURES) || forceNew)
            {
                FastFindWrapper.SetHWnd(BlueStackHelper.GetBlueStackWindowHandle(), true); // Bind FastFind with BlueStack window, considers only ClientArea
                if (FastFindWrapper.SnapShot(0, 0, 0, 0, DEFAULT_SNAP) == 0)
                {
                    lastFullCapture = null;
                    Debug.Assert(false, "FF Capture failed");
                    return false;
                }
                lastFullCapture = Stopwatch.StartNew();
            }
            return true;
        }

        /// <summary>
        /// Takes a screen shot of the full client area of the BlueStack window
        /// </summary>
        /// <returns></returns>
        static private bool TakeCustomCapture(int left, int top, int right, int bottom)
        {
            FastFindWrapper.SetHWnd(BlueStackHelper.GetBlueStackWindowHandle(), true); // Bind FastFind with BlueStack window, considers only ClientArea
            if (FastFindWrapper.SnapShot(left, top, right, bottom, CUSTOM_SNAP) == 0)
            {
                Debug.Assert(false, "FF Capture failed");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a pixel from the client area of BlueStack. 
        /// Capture will be done or refreshed automatically has needed     
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="forceCapture">If set to true, then a new Capture of the full client area will be done first</param>
        /// <returns>A C# Color structure representing the color of that pixel. Returns Color.Empty as an error</returns>
        static public Color GetPixelColor(int x, int y, bool forceCapture = false)
        {
            if (!TakeFullScreenCapture(forceCapture)) return Color.Empty;
            int value = FastFindWrapper.GetPixel(x, y, DEFAULT_SNAP);
            return System.Drawing.Color.FromArgb(value);
        }

        /// <summary>
        /// Returns a pixel from the client area of BlueStack. 
        /// Capture will be done or refreshed automatically has needed     
        /// </summary>
        /// <param name="point"></param>
        /// <param name="forceCapture">If set to true, then a new Capture of the full client area will be done first</param>
        /// <returns>A C# Color structure representing the color of that pixel. Returns Color.Empty as an error</returns>
        static public Color GetPixelColor(Point point, bool forceCapture = false)
        {
            if (!TakeFullScreenCapture(forceCapture)) return Color.Empty;
            int value = FastFindWrapper.GetPixel(point.X, point.Y, DEFAULT_SNAP);
            return System.Drawing.Color.FromArgb(value);
        }

        /// <summary>
        /// Returns a pixel from the client area of BlueStack. 
        /// Capture will be done or refreshed automatically has needed     
        /// </summary>
        /// <param name="point"></param>
        /// <param name="forceCapture">If set to true, then a new Capture of the full client area will be done first</param>
        /// <returns>An int 0x00RRGGBB, or -1 as an error</returns>
        static public int GetPixel(Point point, bool forceCapture = false)
        {
            if (!TakeFullScreenCapture(forceCapture)) return -1;
            return FastFindWrapper.GetPixel(point.X, point.Y, DEFAULT_SNAP);
        }

        /// <summary>
        /// Returns a pixel from the client area of BlueStack. 
        /// Capture will be done or refreshed automatically has needed     
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="forceCapture">If set to true, then a new Capture of the full client area will be done first</param>
        /// <returns>An int 0x00RRGGBB, or -1 as an error</returns>
        static public int GetPixel(int x, int y, bool forceCapture = false)
        {
            if (!TakeFullScreenCapture(forceCapture)) return -1;
            return FastFindWrapper.GetPixel(x, y, DEFAULT_SNAP);
        }

        /// <summary>
        /// Beware: do not use this function extensively, at it does a screen shot each time. Better use higher level functions when needed.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="color1"></param>
        /// <param name="variation"></param>
        /// <param name="forceCapture"></param>
        /// <returns></returns>
        static public Point PixelSearch(int left, int top, int right, int bottom, Color color1, int variation)
        {
            if (!TakeCustomCapture(left, top, right, bottom)) return Point.Empty;
            int nbMatchMin = 1, xRef = (left + right) / 2, yRef = (top + bottom) / 2;
            if (FastFindWrapper.GenericColorSearch(1, ref nbMatchMin, ref xRef, ref yRef, color1.ToArgb(), variation, CUSTOM_SNAP) == 0 || nbMatchMin == 0) return Point.Empty;
            return new Point(xRef, yRef);
        }

        /// <summary>
        /// Search for a given color within all the client area of BS. Faster then PixelSearch, as it doesn't do a new capture each time. 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="color1"></param>
        /// <param name="variation"></param>
        /// <returns></returns>
        static public Point FullScreenPixelSearch(Color color1, int variation, bool forceCapture = false)
        {
            if (!TakeFullScreenCapture(forceCapture)) return Point.Empty;
            int nbMatchMin = 1, xRef = 0, yRef = 0;
            if (FastFindWrapper.GenericColorSearch(1, ref nbMatchMin, ref xRef, ref yRef, color1.ToArgb(), variation, DEFAULT_SNAP) == 0 || nbMatchMin == 0) return Point.Empty;
            return new Point(xRef, yRef);
        }

        /// <summary>
        /// Beware: do not use this function extensively, at it does a screen shot each time. 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="color1"></param>
        /// <param name="variation"></param>
        /// <param name="forceCapture"></param>
        /// <returns></returns>
		static public Point PixelSearch(int left, int top, int right, int bottom, ColorSet colors, int variation)
        {
            if (!TakeCustomCapture(left, top, right, bottom)) return Point.Empty;
            FastFindWrapper.ResetColors();
            foreach (Color color in colors)
                FastFindWrapper.AddColor(color.ToArgb());
            int xRef = (left + right) / 2, yRef = (top + bottom) / 2;
            if (FastFindWrapper.ColorsPixelSearch(ref xRef, ref yRef, CUSTOM_SNAP) == 0) return Point.Empty;
            return new Point(xRef, yRef);
        }

        /// <summary>
        /// Search for a given color within a color list, scanning all the client area of BS. Faster then PixelSearch, as it doesn't do a new capture each time. 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="color1"></param>
        /// <param name="variation"></param>
        /// <param name="forceCapture"></param>
        /// <returns></returns>
		static public Point FullScreenPixelSearch(ColorSet colors, int variation, bool forceCapture = false)
        {
            if (!TakeFullScreenCapture(forceCapture)) return Point.Empty;
            FastFindWrapper.ResetColors();
            foreach (Color color in colors)
                FastFindWrapper.AddColor(color.ToArgb());
            int xRef = 0, yRef = 0;
            if (FastFindWrapper.ColorsPixelSearch(ref xRef, ref yRef, DEFAULT_SNAP) == 0) return Point.Empty;
            return new Point(xRef, yRef);
        }

        static private bool IsInShadeVariation(int PixelColor, int ColorToFind, int ShadeVariation)
        {
            if (ShadeVariation <= 0) return PixelColor == ColorToFind;
            return (Math.Abs(((int)PixelColor & 0x00FF0000) - ((int)ColorToFind & 0x00FF0000)) >> 16 <= ShadeVariation) &&
                    (Math.Abs(((int)PixelColor & 0x0000FF00) - ((int)ColorToFind & 0x0000FF00)) >> 8 <= ShadeVariation) &&
                    (Math.Abs(((int)PixelColor & 0x000000FF) - ((int)ColorToFind & 0x000000FF)) <= ShadeVariation);
        }

        static public bool IsInColorRange(Point point, Color color, int shadeVariation = 0)
        {
            int pixel = GetPixel(point);
            if (shadeVariation == 0)
                return pixel == color.ToArgb();
            return IsInShadeVariation(pixel, color.ToArgb(), shadeVariation);
        }
    }
}
