using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Win32;
using MouseAndKeyboard;
using Microsoft.Win32;
using System.IO;

namespace CoC.Bot.Tools
{	
	public static class BlueStacksHelper
	{
		private static IntPtr bshandle = IntPtr.Zero;

		public static bool IsBlueStacksFound { get { return bshandle != IntPtr.Zero; } }
		public static IntPtr GetBlueStacksWindowHandle()
		{
			return GetBlueStacksWindowHandle(false);
		}
		public static IntPtr GetBlueStacksWindowHandle(bool force)
		{
			if (bshandle == IntPtr.Zero || force)
				bshandle = Win32.Win32.FindWindow("WindowsForms10.Window.8.app.0.33c0d9d", "BlueStacks App Player"); // First try
			if (bshandle == IntPtr.Zero)
				bshandle = Win32.Win32.FindWindow(null, "BlueStacks App Player"); // Maybe the class name has changes
			if (bshandle == IntPtr.Zero)
			{
				Process[] proc = Process.GetProcessesByName("BlueStacks App Player"); // If failed, then try with .NET functions
				if (proc == null || proc.Count() == 0)
					return IntPtr.Zero;
				bshandle = proc[0].MainWindowHandle;
			}
			return bshandle;
		}

		// This is the app that is run when you start BlueStacks from Desktop
		public static string GetBluestackLaunchExePath
		{
			get
			{
				var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\BlueStacks", true);
				if (key==null) return null;
				string path = Path.Combine((string)Registry.GetValue(key.Name, "InstallDir",  @"C:\Program Files (x86)\BlueStacks\"), "HD-StartLauncher.exe");
				return path;	
			}		
		}

		// this will start Bluestack and Clash Of Clans in it (if they are both installed). 
		// TODO
		public static bool StartClashOfClanAndWait(int maxDelayMs = 20000)
		{
			var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\BlueStacks", true);
			if (key==null) return false;
			string path = Path.Combine((string)Registry.GetValue(key.Name, "InstallDir", @"C:\Program Files (x86)\BlueStacks\"), "HD-RunApp.exe");
			string commandLine = "Android com.supercell.clashofclans com.supercell.clashofclans.GameApp"; // Clash of Clan app
			ProcessStartInfo psi = new ProcessStartInfo(path,commandLine);
			var p = new Process(); p.StartInfo = psi;

			if (!p.Start()) return false; //start the process
			return p.WaitForExit(maxDelayMs); // wait for the installation to finish, 20 seconds max by default			
		}

		
		static public bool IsBlueStackInstalled()
		{
			var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\BlueStacks\Guests\Android\FrameBuffer\0", true);
			return (key != null);				
		}

		public static bool SetDimensionsIntoRegistry()
		{
			try
			{
				bool value = false;

				var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\BlueStacks\Guests\Android\FrameBuffer\0", true);
				if (key != null)
				{
					Registry.SetValue(key.Name, "WindowWidth", 0x0000035C, RegistryValueKind.DWord);
					Registry.SetValue(key.Name, "WindowHeight", 0x000002D0, RegistryValueKind.DWord);
					Registry.SetValue(key.Name, "GuestWidth", 0x0000035C, RegistryValueKind.DWord);
					Registry.SetValue(key.Name, "GuestHeight", 0x000002D0, RegistryValueKind.DWord);

					Registry.SetValue(key.Name, "Depth", 0x00000010, RegistryValueKind.DWord);
					Registry.SetValue(key.Name, "FullScreen", 0x00000000, RegistryValueKind.DWord);
					Registry.SetValue(key.Name, "WindowState", 0x00000001, RegistryValueKind.DWord);
					Registry.SetValue(key.Name, "HideBootProgress", 0x00000001, RegistryValueKind.DWord);

					key.Close();

					value = true;
				}

				return value;
			}
			catch (Exception exception)
			{
				Debug.Assert(false, exception.Message);
				return false;
			}
		}

		public static bool Click(int x, int y, int nbClick = 1, int delay = 20)
		{
			return Click(new Win32.POINT(x, y), nbClick, delay);
		}

		public static bool Click(Win32.POINT point, int nbClick = 1, int delay = 20)
		{
			if (bshandle == IntPtr.Zero)
				bshandle = GetBlueStacksWindowHandle();
			if (bshandle == IntPtr.Zero)
				return false;
			return MouseHelper.ClickOnPoint2(bshandle, point, nbClick, delay);			
		}

		public static POINT GetClickPosition(int delay = 20)
		{
			if (bshandle == IntPtr.Zero)
				bshandle = GetBlueStacksWindowHandle();
			if (bshandle == IntPtr.Zero)
				return new POINT();

			return MouseHelper.GetPointOnClick(bshandle, delay);		
		}

		#region Properties

		/// <summary>
		/// Gets a value indicating whether BlueStacks is running.
		/// </summary>
		/// <value><c>true</c> if BlueStacks is running; otherwise, <c>false</c>.</value>
		public static bool IsBlueStacksRunning
		{
			get
			{
				bshandle = IntPtr.Zero;
				return GetBlueStacksWindowHandle() != IntPtr.Zero;
			}
		}

		/// <summary>
		/// Gets a value indicating whether BlueStacks is running with required dimensions.
		/// </summary>
		/// <value><c>true</c> if this BlueStacks is running with required dimensions; otherwise, <c>false</c>.</value>
		public static bool IsRunningWithRequiredDimensions
		{
			get
			{
				var rct = new Win32.RECT();
				Win32.Win32.GetClientRect(bshandle, out rct);

				var width = rct.Right - rct.Left; // in Win32 Rect, right and bottom are considered as excluded from the rect. 
				var height = rct.Bottom - rct.Top;

				return (width != 860) || (height != 720) ? false : true;
			}
		}

		#endregion

		/// <summary>
		/// Activates and displays the window. If the window is 
		/// minimized or maximized, the system restores it to its original size 
		/// and position. An application should use this when restoring 
		/// a minimized window.
		/// </summary>
		/// <returns></returns>
		public static bool RestoreBlueStacks()
		{
			if (!IsBlueStacksRunning) return false;
			return Win32.Win32.ShowWindow(bshandle, Win32.WindowShowStyle.Restore);
		}

		/// <summary>
		/// Activates the window and displays it in its current size and position.
		/// </summary>
		/// <returns></returns>
		public static bool ActivateBlueStacks()
		{
			if (!IsBlueStacksRunning) return false;
			return Win32.Win32.ShowWindow(bshandle, Win32.WindowShowStyle.Show);
		}

		/// <summary>
		/// Hides the BlueStack Window
		/// </summary>
		/// <returns></returns>
		public static bool HideBlueStacks()
		{
			if (!IsBlueStacksRunning) return false;
			return Win32.Win32.ShowWindow(bshandle, Win32.WindowShowStyle.Hide);
		}

	}
}
