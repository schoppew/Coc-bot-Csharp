using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.Bot.Plugin
{
	public enum OCRTarget
	{
		None = 0,
		
		MyGold = 1,
		MyElixir = 2,
		MyDarkElixir = 3,
		MyTrophies = 4,
		TargetGold = 11,
		TargetElixir = 12,
		TargetDarkElixir = 13,
		TargetTrophies = 14,
		ClanChat = 20,
		GeneralChat = 21,
		TrophiesToWin = 30,	
	}
}
