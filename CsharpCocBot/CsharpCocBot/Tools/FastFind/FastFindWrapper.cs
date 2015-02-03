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
		public const int DEBUG_NONE				= 0x0000;
		public const int DEBUG_CONSOLE		= 0x0001;
		public const int DEBUG_FILE				= 0x0002;
		public const int DEBUG_GRAPHIC		= 0x0004;
		public const int DEBUG_MSGBOX			= 0x0008;
		public const int DEBUG_CHANNEL_BITS= 0x000F; // Mask for bits restricting the channels used for debugging
		
		public const int DEBUG_NOSYSTEM		= 0x0010; // Internal messages from the DLL
		public const int DEBUG_NOSYSTEM_DETAILS = 0x0020; // Internal detailed messages from the DLL (détails)
		public const int DEBUG_NOUSER			= 0x0040; // Messages from the application (external)
		public const int DEBUG_NOTHING_EXCEPT_ERRORS=0x0070; // Disable all messages except errors
		public const int DEBUG_ERROR			= 0x0080; // Error messages (highest priority)
		public const int DEBUG_ORIGIN_BITS= 0x00F0; // Mask for the origin of the message
		
		public const int DEBUG_SAME_LINE	= 0x80000; // Trick to continue a message on the same line (won't work with timestamp)
		
		public const int DEBUG_STREAM     = 0x03; // Console and File
		
		public const int DEBUG_STREAM_SYSTEM= 0x13; // Console and File - System Message
		public const int DEBUG_STREAM_SYSTEM_DETAIL=  0x33; // Console and File - Detailed System Message
		public const int DEBUG_MB_SYSTEM  = 0x3B; // Console, File and MB - System Message
		public const int DEBUG_SYSTEM_ERROR= 0x7F8F; // Console, File and MB - Any Message
		public const int DEBUG_USER_MESSAGE= 0x43; // Console and File - User Message
		public const int DEBUG_USER_MB    = 0x4B; // Console, File and MB - User Message
		public const int DEBUG_USER_ERROR = 0x7F8F; // Console and File and MB - Any Message

    [DllImport(fastFindDllName)]
    public static extern void SetDebugMode(int debugMode);

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
	public static extern bool SaveBMP(int NoSnapShot, [MarshalAs(UnmanagedType.AnsiBStr)]string szFileName /* With no extension (xxx.bmp added)*/);
	[DllImport(fastFindDllName)]
	public static extern bool SaveJPG(int NoSnapShot, [MarshalAs(UnmanagedType.AnsiBStr)]string szFileName /* With no extension*/, UInt32 uQuality);
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
