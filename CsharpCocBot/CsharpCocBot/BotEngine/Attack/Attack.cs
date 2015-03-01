using System;
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
        public static Dictionary<Troop, int> troopDict = new Dictionary<Troop, int>();
 
		public static void Initialize()
		{
			Search.PrepareSearch();
			Thread.Sleep(1000);

		    if (Search.VillageSearch())
		    {
                PrepareAttack();
                Thread.Sleep(1000);

                Start(); //TODO:
                Thread.Sleep(1000);

                troopDict.Clear();
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
            int divisor=456;

            Main.Bot.WriteToOutput("====== Beginning Attack ======", GlobalVariables.OutputStates.Verified);

            switch (Main.Bot.SelectedDeployStrategy.Id)
            {
                case (int) DeployStrategy.OneSide:
                    divisor = 1;
                    break;
                case (int) DeployStrategy.TwoSides:
                    divisor = 2;
                    break;
                case (int) DeployStrategy.ThreeSides:
                    divisor = 3;
                    break;
                case (int) DeployStrategy.FourSides:
                    divisor = 4;
                    break;
            }

            //barch
            BarchAlgorithm.Start(divisor);
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

            Main.Bot.WriteToOutput("Finished Preparing...");
        }

        public static Troop IdentifyTroop(int slot)
        {
            Color troopPixel = Tools.CoCHelper.GetPixelColor(new ClickablePoint(63 + (72*slot), 596));

            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(248, 176, 32), 5)) return Troop.Barbarian;
            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(198, 56, 96), 5)) return Troop.Archer;
            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(52, 100, 42), 5)) return Troop.Goblin;
            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(248, 218, 33), 5)) return Troop.Giant;
            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(88, 80, 104), 5)) return Troop.WallBreaker;
            if (Tools.CoCHelper.SameColor(troopPixel, Color.FromArgb(248, 235, 121), 5)) return Troop.King;

            return Troop.None;
        }

        public static int ReadTroopQuantity(int slot)
        {
            int output;
            try
            {
                output = int.Parse(ReadText.GetNormal(40 + (72*slot), 565));
            }
            catch (Exception)
            {
                output = 0;
            }

            return output;
        }
    }
}
