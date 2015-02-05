namespace CoC.Bot.UI.Controls.NotifyIcon.Interop
{
	using System;
	using System.Drawing;
	using System.Runtime.InteropServices;

	using UI.Utils;
	using Point = Win32.POINT;

	/// <summary>
	/// Resolves the current tray position.
	/// </summary>
	public static class TrayInfo
	{
		/// <summary>
		/// Gets the position of the system tray.
		/// </summary>
		/// <returns>Tray coordinates.</returns>
		public static Point GetTrayLocation()
		{
			var info = new AppBarInfo();
			info.GetSystemTaskBarPosition();

			Rectangle rcWorkArea = info.WorkArea;

			int x = 0, y = 0;
			if (info.Edge == Win32.ScreenEdge.Left)
			{
				x = rcWorkArea.Left + 2;
				y = rcWorkArea.Bottom;
			}
			else if (info.Edge == Win32.ScreenEdge.Bottom)
			{
				x = rcWorkArea.Right;
				y = rcWorkArea.Bottom;
			}
			else if (info.Edge == Win32.ScreenEdge.Top)
			{
				x = rcWorkArea.Right;
				y = rcWorkArea.Top;
			}
			else if (info.Edge == Win32.ScreenEdge.Right)
			{
				x = rcWorkArea.Right;
				y = rcWorkArea.Bottom;
			}

			return new Point { X = x, Y = y };
		}
	}

	internal class AppBarInfo
	{
		
		private const int ABM_GETTASKBARPOS = 0x00000005;

		// SystemParametersInfo constants
		private const UInt32 SPI_GETWORKAREA = 0x0030;

		private Win32.APPBARDATA m_data;

		public Win32.ScreenEdge Edge
		{
			get { return (Win32.ScreenEdge)m_data.uEdge; }
		}


		public Rectangle WorkArea
		{
			get
			{
				Int32 bResult = 0;
				var rc = new RECT();
				IntPtr rawRect = Marshal.AllocHGlobal(Marshal.SizeOf(rc));
				bResult = Win32.Win32.SystemParametersInfo(SPI_GETWORKAREA, 0, rawRect, 0);
				rc = (RECT)Marshal.PtrToStructure(rawRect, rc.GetType());

				if (bResult == 1)
				{
					Marshal.FreeHGlobal(rawRect);
					return new Rectangle(rc.Left, rc.Top, rc.Right - rc.Left, rc.Bottom - rc.Top);
				}

				return new Rectangle(0, 0, 0, 0);
			}
		}


		public void GetPosition(string strClassName, string strWindowName)
		{
			m_data = new Win32.APPBARDATA();
			m_data.cbSize = (UInt32)Marshal.SizeOf(m_data.GetType());

			IntPtr hWnd = Win32.Win32.FindWindow(strClassName, strWindowName);

			if (hWnd != IntPtr.Zero)
			{
				UInt32 uResult = Win32.Win32.SHAppBarMessage(ABM_GETTASKBARPOS, ref m_data);

				if (uResult != 1)
				{
					throw new Exception("Failed to communicate with the given AppBar");
				}
			}
			else
			{
				throw new Exception("Failed to find an AppBar that matched the given criteria");
			}
		}


		public void GetSystemTaskBarPosition()
		{
			GetPosition("Shell_TrayWnd", null);
		}

		
	}
}