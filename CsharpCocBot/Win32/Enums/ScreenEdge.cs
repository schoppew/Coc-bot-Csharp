namespace Win32
{
	public partial class CONST
	{
		public const int ABE_BOTTOM = 3;
		public const int ABE_LEFT = 0;
		public const int ABE_RIGHT = 2;
		public const int ABE_TOP = 1;
	}
	public enum ScreenEdge
	{
		Undefined = -1,
		Left = CONST.ABE_LEFT,
		Top = CONST.ABE_TOP,
		Right = CONST.ABE_RIGHT,
		Bottom = CONST.ABE_BOTTOM
	}
}
