using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Tools;
using CoC.Bot.Tools.FastFind;

namespace CoC.Bot.Data
{
	static public partial class ScreenData
	{

		static ClickablePoint DropTroopPixelByPosition(int buttonNumber)
		{
			return new ClickablePoint(68 + (72 * buttonNumber), 595);
		}

		static public bool SelectDropTroop(int buttonNumber)
		{
			return CoCHelper.Click(DropTroopPixelByPosition(buttonNumber));
		}

		static public Troop IdentifyTroopKind(int buttonNumber)
		{
			Color troopPixel = CoCHelper.GetPixelColor(DropTroopPixelByPosition(buttonNumber));
			if (CoCHelper.SameColor(troopPixel, Color.FromArgb(0xF8B020), 5)) return Troop.Barbarian;
			if (CoCHelper.SameColor(troopPixel, Color.FromArgb(0xD83F68), 5)) return Troop.Archer;
			if (CoCHelper.SameColor(troopPixel, Color.FromArgb(0x7BC950), 5)) return Troop.Goblin;
			if (CoCHelper.SameColor(troopPixel, Color.FromArgb(0xF8D49E), 5)) return Troop.Giant;
			if (CoCHelper.SameColor(troopPixel, Color.FromArgb(0x60A4D0), 5)) return Troop.WallBreaker;
			if (CoCHelper.SameColor(troopPixel, Color.FromArgb(0xF8EB79), 5)) return Troop.King;			
			if (CoCHelper.IsInColorRange(new ClickablePoint(68 + (72 * buttonNumber), 588), Color.FromArgb(0x7031F0), 5)) return Troop.Queen;
			if (CoCHelper.IsInColorRange(new ClickablePoint(68 + (72 * buttonNumber), 585), Color.FromArgb(0x68ACD4), 5)) return Troop.CastleClan;
			if (CoCHelper.IsInColorRange(new ClickablePoint(68 + (72 * buttonNumber), 632), Color.FromArgb(0x0426EC), 5)) return Troop.SpellLightning;
			return Troop.None;
		}
	}

}
