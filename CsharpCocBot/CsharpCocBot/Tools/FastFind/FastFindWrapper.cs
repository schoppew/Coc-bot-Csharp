using System;
using System.Runtime.InteropServices;

namespace CoC.Bot.Tools.FastFind
{
  static public class FastFindWrapper
  {
#if WIN64
    const string fastFindDllName = "../UnmanagedLibs/FastFind64.dll";
#else
      const string fastFindDllName = "../UnmanagedLibs/FastFind.dll";
#endif

    // Exclusion areas
    // ===============
    [DllImport(fastFindDllName)]
    public static extern void AddExcludedArea(int x1, int y1, int x2, int y2);

    [DllImport(fastFindDllName)]
    public static extern bool IsExcluded(int x, int y, IntPtr hWnd);

    [DllImport(fastFindDllName)]
    public static extern bool ResetExcludedAreas();

    // Configuration 
    // ==============
    [DllImport(fastFindDllName)]
    public static extern void SetDebugMode();

    [DllImport(fastFindDllName)]
    public static extern void SetHWnd(IntPtr NewWindowHandle, bool bClientArea);

    /// <summary>
    /// Usage : IntPtr ptr = GetLastErrorMsg();
    ///         string s = Marshal.PtrToStringAnsi(ptr);
    /// If extensively used, make sure there is no suspect memory leaks
    /// </summary>
    /// <returns></returns>
    [DllImport(fastFindDllName, CharSet = CharSet.Auto)]
    public static extern IntPtr GetLastErrorMsg();

    [DllImport(fastFindDllName, CharSet = CharSet.Auto)]
    public static extern IntPtr FFVersion();

    // Basic functions
    [DllImport(fastFindDllName, EntryPoint="FFGetPixel")]
    public static extern int GetPixel(int X, int Y, int NoSnapShot);
    [DllImport(fastFindDllName)]
    public static extern int ColorPixelSearch(ref int XRef, ref int YRef, int ColorToFind, int NoSnapShot);
    [DllImport(fastFindDllName)]
    public static extern int GetPixelFromScreen(int x, int y, int NoSnapShot); // Like GetPixel, but with screen coordinates

    // Snapshots
    [DllImport(fastFindDllName)]
    public static extern int SnapShot(int aLeft, int aTop, int aRight, int aBottom, int NoSnapShot);

    // List of colors management
    [DllImport(fastFindDllName)]
    public static extern int AddColor(int NewColor);
    [DllImport(fastFindDllName)]
    public static extern int RemoveColor(int NewColor);
    [DllImport(fastFindDllName)]
    public static extern void ResetColors();

    // Search function, for multiple colors (color list should be defined first)
    [DllImport(fastFindDllName)]
    public static extern int ColorsPixelSearch(ref int XRef, ref int YRef, int NoSnapShot);

    // ColorsSearch is close to ColorSearch, except it can look for several colors instead of only one. 
    [DllImport(fastFindDllName)]
    public static extern int ColorsSearch(int SizeSearch, ref int NbMatchMin, ref int XRef, ref int YRef, int NoSnapShot);

    // Most generic search function : called in most case. 
    [DllImport(fastFindDllName)]
    public static extern int GenericColorSearch(int SizeSearch, ref int NbMatchMin, ref int XRef, ref int YRef, int ColorToFind, int ShadeVariation, int NoSnapShot);

    // New vith verion 1.4 : more powerful search function : looks for 'spots' instead of pixels
    [DllImport(fastFindDllName)]
    public static extern int ProgressiveSearch(int SizeSearch, ref int NbMatchMin, int NbMatchMax, ref int XRef, ref int YRef, int ColorToFind/*-1 if several colors*/, int ShadeVariation, int NoSnapShot);

    // Count pixels with a given color
    [DllImport(fastFindDllName)]
    public static extern int ColorCount(int ColorToFind, int NoSnapShot, int ShadeVariation);

    /// // SnapShot saving into bitmap file
    [DllImport(fastFindDllName)]
    public static extern bool SaveBMP(int NoSnapShot, [MarshalAs(UnmanagedType.LPTStr)]string szFileName /* With no extension (xxx.bmp added)*/);
    [DllImport(fastFindDllName)]
    public static extern bool SaveJPG(int NoSnapShot, [MarshalAs(UnmanagedType.LPTStr)]string szFileName /* With no extension*/, UInt32 uQuality);
    [DllImport(fastFindDllName)]
    public static extern int GetLastFileSuffix();

    // Raw SnapShot rata retrieval
    [DllImport(fastFindDllName)]
    public static extern IntPtr GetRawData(int NoSnapShot, ref int NbBytes);

    // Detection of changes between two SnapShots
    [DllImport(fastFindDllName)]
    public static extern int KeepChanges(int NoSnapShot, int NoSnapShot2, int ShadeVariation);  // ** Changed in version 2.0 : ShadeVariation added **
    [DllImport(fastFindDllName)]
    public static extern int KeepColor(int NoSnapShot, int ColorToFind, int ShadeVariation);
    [DllImport(fastFindDllName)]
    public static extern int HasChanged(int NoSnapShot, int NoSnapShot2, int ShadeVariation);  // ** Changed in version 2.0 : ShadeVariation added **
    [DllImport(fastFindDllName)]
    public static extern int LocalizeChanges(int NoSnapShot, int NoSnapShot2, ref int xMin, ref int yMin, ref int xMax, ref int yMax, ref int nbFound, int ShadeVariation);  // ** Changed in version 2.0 : ShadeVariation added **

    // Display of a SnapShot
    [DllImport(fastFindDllName)]
    public static extern bool DrawSnapShot(int NoSnapShot);
    [DllImport(fastFindDllName)]
    public static extern bool DrawSnapShotXY(int NoSnapShot, int X, int Y); // ** New in version 2.0 **

    // Change of the color of a pixel (hyper-omptimized function)
    [DllImport(fastFindDllName)]
    public static extern bool FFSetPixel(int x, int y, int Color, int NoSnapShot);

    // SnapShot duplication
    [DllImport(fastFindDllName)]
    public static extern bool DuplicateSnapShot(int Src, int Dst);

    // Misc functions, new in version 2.0
    [DllImport(fastFindDllName)]
    public static extern int ComputeMeanValues(int NoSnapShot, ref int MeanRed, ref int MeanGreen, ref int MeanBlue);
    [DllImport(fastFindDllName)]
    public static extern int ApplyFilterOnSnapShot(int NoSnapShot, int Red, int Green, int Blue);
  }
}
