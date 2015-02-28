using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Documents;
using CoC.Bot.Chart;
using CoC.Bot.Data;

namespace CoC.Bot.BotEngine
{
    class Attack
    {
        private static Dictionary<Troop, int> troopDict = new Dictionary<Troop, int>();
 
		public static void AttackMain()
		{
			Search.PrepareSearch();
			Thread.Sleep(1000);

		    if (Search.VillageSearch())
		    {
                PrepareAttack(); //TODO:
                Thread.Sleep(1000);

                Start(); //TODO:
                Thread.Sleep(1000);

                MainScreen.ReturnHome();
                Thread.Sleep(1000);
		    }
		    else
		    {
		        Main.Bot.WriteToOutput("Timeout...");
		    }

		}

        public static void Start()
        {
            Main.Bot.WriteToOutput("====== Beginning Attack ======", GlobalVariables.OutputStates.Verified);
            
            
        
        }

        public static void DropClanCastle()
        {

        }

        public static void DropHeroes()
        {

        }

        public static bool GoldElixirChange()
        {
            return false;
        }

        public static void PrepareAttack()
        {
            Main.Bot.WriteToOutput("Preparing Attack...");

            for (int i = 0; i < 9; i++)
            {
                Troop troopKind = IdentifyTroop(i);
                int troopQuantity = ReadTroopQuantity(i);

                if (!troopKind.Equals(Troop.None))
                    troopDict.Add(troopKind, troopQuantity);
            }
        }

        public static Troop IdentifyTroop(int slot)
        {
            Color troopPixel = Tools.CoCHelper.GetPixelColor(new ClickablePoint(68 + (72*slot), 565));

            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(248, 176, 32), 5)) return Troop.Barbarian;
            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(216, 63, 104), 5)) return Troop.Archer;
            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(123, 201, 80), 5)) return Troop.Goblin;
            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(248, 212, 158), 5)) return Troop.Giant;
            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(96, 164, 208), 5)) return Troop.WallBreaker;
            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(248, 235, 121), 5)) return Troop.King;

            Color otherPixel = Tools.CoCHelper.GetPixelColor(new ClickablePoint(68 + (72*slot), 588));

            if (Tools.CoCHelper.SameColor(otherPixel, Color.FromArgb(112, 49, 240), 5) ||
                Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(66, 30, 63), 5)) return Troop.Queen;
            if (Tools.CoCHelper.SameColor(Tools.CoCHelper.GetPixelColor(new ClickablePoint(68 + (72*slot), 585)),
                Color.FromArgb(104, 172, 212), 5)) return Troop.CastleClan;
            if (Tools.CoCHelper.SameColor(Tools.CoCHelper.GetPixelColor(new ClickablePoint(68 + (72 * slot), 632)),
                Color.FromArgb(4, 38, 236), 5)) return Troop.SpellLightning;

            return Troop.None;
        }

        public static int ReadTroopQuantity(int slot)
        {
            return int.Parse(ReadText.GetNormal(40 + (72*slot), 565));
        }
    }
}
