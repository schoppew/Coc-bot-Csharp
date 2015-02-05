namespace CoC.Bot.Tools
{
	using System;
	using System.Windows;
	using Win32;

	public static class PointHelper
	{
		/// <summary>
		/// Converts a <see cref="System.Windows.Point"/> to <see cref="Win32.POINT"/>.
		/// </summary>
		/// <param name="point">The <see cref="System.Windows.Point"/></param>
		/// <returns>A <see cref="Win32.POINT"/></returns>
		public static POINT ToPOINT(this Point point)
		{
			return new POINT((int)point.X, (int)point.Y);
		}
		/// <summary>
		/// Converts a <see cref="Win32.POINT"/> to <see cref="System.Windows.Point"/>.
		/// </summary>
		/// <param name="point">The <see cref="Chipmunk.Win32.POINT"/></param>
		/// <returns>A <see cref="System.Windows.Point"/></returns>
		public static Point ToPoint(this POINT point)
		{
			return new Point(point.X, point.Y);
		}
	}
}