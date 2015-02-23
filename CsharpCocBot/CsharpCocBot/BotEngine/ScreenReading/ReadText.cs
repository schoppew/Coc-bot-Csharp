using System;
using CoC.Bot.Data;

namespace CoC.Bot.BotEngine
{
	internal class ReadText
	{

		private static char GetChar(ref int x, int y)
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

		public static string GetString(int y)
		{
		    for (int i = 0; i < 4; i++)
		    {
		        int xTemp = 35;
                
		        if (GetChar(ref xTemp, y) == char.Parse("|"))
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

		    do
		    {
		        output += GetChar(ref x, y);
		    } while (output.Substring(output.Length - 3, output.Length - 1).Equals("  "));

		    return output;
		}

		public int GetTrophy(int _x, int _y)
		{
			throw new NotImplementedException();
		}
		#endregion Not implemented function
	}
}