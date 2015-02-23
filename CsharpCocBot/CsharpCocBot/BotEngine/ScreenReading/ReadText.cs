using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CoC.Bot.Data;
using Win32;
using CoC.Bot.Tools;
using CoC.Bot.BotEngine.ScreenReading;

namespace CoC.Bot.BotEngine
{
	internal class ReadText
	{

		public char GetChar(ref int x, int y)
		{
			foreach (var pair in ScreenData.OCRData)
				if (pair.Value.ReadChar(ref x, y))
					return pair.Key;
			return '|'; // Don't ask why this...   
		}

		#region Not implemented function
		public int GetDarkElixir(int _x, int _y)
		{
			throw new NotImplementedException();
		}

		public int GetDigit(int _x, int _y, string _type)
		{
			throw new NotImplementedException();
		}

		public int GetElixir(int _x, int _y)
		{
			throw new NotImplementedException();
		}

		public int GetGold(int _x, int _y)
		{
			throw new NotImplementedException();
		}

		public int GetNormal(int _x, int _y)
		{
			throw new NotImplementedException();
		}

		public static int GetOther(int _x, int _y, string kind)
		{
			throw new NotImplementedException();
		}

		public static string GetString(int _y)
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