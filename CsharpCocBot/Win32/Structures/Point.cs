using System;
using System.Runtime.InteropServices;

namespace Win32
{
	// POINT structure required by WINDOWPLACEMENT structure
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		/// <summary>
		/// X coordinate.
		/// </summary>
		public int X;

		/// <summary>
		/// Y coordinate.
		/// </summary>
		public int Y;


		public POINT(int x, int y)
		{
			X = x;
			Y = y;
		}

		public POINT(System.Drawing.Point p)
		{
			X = p.X;
			Y = p.Y;
		}

		public static POINT Empty = new POINT(-1, -1);
		public bool IsEmpty 
		{
			get
			{
				return (X == Empty.X) && (Y == Empty.Y);
			}
		}

		public bool IsEmptyOrZero
		{
			get
			{
				return ((X == Empty.X) && (Y == Empty.Y)) || ((X == 0) && (Y == 0));
			}
		}

		
		public override bool Equals(object obj)
		{
			if (obj is POINT) return Equals((POINT)obj);
			return false;
		}

		public bool Equals(POINT other)
		{
			return other.X == X && other.Y == Y;
		}
		public bool Equals(System.Drawing.Point other)
		{
			return other.X == X && other.Y == Y;
		}

		public static bool operator ==(POINT a, POINT b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(POINT a, POINT b)
		{
			return !a.Equals(b);
		}

		public static bool operator ==(POINT a, System.Drawing.Point b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(POINT a, System.Drawing.Point b)
		{
			return !a.Equals(b);
		}

		public static bool operator !=(System.Drawing.Point a, POINT b)
		{
			return !a.Equals(b);
		}
		public static bool operator ==(System.Drawing.Point a, POINT b)
		{
			return a.Equals(b);
		}

		public static implicit operator POINT(System.Drawing.Point p)
		{
			return new POINT(p);			
		}

		public static implicit operator System.Drawing.Point(POINT p)
		{
			return new System.Drawing.Point(p.X, p.Y);
		}

		public override int GetHashCode()
		{
			return (X<<16)+Y;
		}
	}
}
