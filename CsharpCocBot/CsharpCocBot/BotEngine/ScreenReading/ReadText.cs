﻿using System;
using CoC.Bot.Data;

namespace CoC.Bot.BotEngine
{
	internal class ReadText
	{

		private static char? GetChar(ref int x, int y)
		{
			foreach (var pair in ScreenData.OCRData)
				if (pair.Value.ReadChar(ref x, y))
					return pair.Key;
			return null; // Don't ask why this...   
		}

		public static string GetString(int y)
		{
			for (int i = 0; i < 4; i++)
			{
				int xTemp = 35;

				if (GetChar(ref xTemp, y) == null)
				{
					y += 1;
				}
				else
				{
					break;
				}
			}

			int x = 35;
			string output = "";
			char? lastChar = null;
			do
			{
				lastChar = GetChar(ref x, y);
				if (lastChar == null) break;
				output += lastChar.Value;
			} while (!output.EndsWith("  "));

			return output;
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

		public int GetTrophy(int _x, int _y)
		{
			throw new NotImplementedException();
		}
		#endregion Not implemented function
	}
}