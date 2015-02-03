using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace CoC.Bot.Functions
{
    internal class Village
    {
        public static bool CheckFullArmy()
        {
            return false;
        }

        public static void CollectResources()
        {
            Point[] collectorPos = new Point[] { Main.Bot.LocationCollector1, Main.Bot.LocationCollector2, Main.Bot.LocationCollector3, Main.Bot.LocationCollector4, Main.Bot.LocationCollector5, Main.Bot.LocationCollector6, Main.Bot.LocationCollector7, Main.Bot.LocationCollector8, Main.Bot.LocationCollector9, Main.Bot.LocationCollector10, Main.Bot.LocationCollector11, Main.Bot.LocationCollector12, Main.Bot.LocationCollector13, Main.Bot.LocationCollector14, Main.Bot.LocationCollector15, Main.Bot.LocationCollector16, Main.Bot.LocationCollector17};

            if (collectorPos[0].IsEmpty)
            {
                // LOCATE COLLECTORS
                // SAVE CONFIG
                Thread.Sleep(2000);
            }

            Main.Bot.WriteToOutput("Collecting Resources...");
            Thread.Sleep(250);
            Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(1, 1), 1);

            for(int i = 0; i < 17; i++)
            {
                Thread.Sleep(250);
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, collectorPos[i], 1);
                Thread.Sleep(250);
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(1, 1), 1);
            }
        }

        public static void DonateCC()
        {

        }

        public static void DropTrophies()
        {

        }

        public int GetTownHallLevel()
        {
            return -1;
        }

        public bool IsElixirFull()
        {
            return false;
        }

        public bool IsGoldFull()
        {
            return false;
        }

        public void LocateBarracks()
        {

        }

        public void LocateCollectors()
        {

        }

        public void LocateClanCastle()
        {

        }

        public static void Idle()
        {
            Stopwatch sw = new Stopwatch();
            
            if (!GlobalVariables.fullArmy)
            {
                Main.Bot.WriteToOutput("~~~ Waiting for full army ~~~");
                while (!GlobalVariables.fullArmy)
                {
                    sw.Start();

                    Thread.Sleep(1000);
                    Functions.MainScreen.CheckMainScreen();
                    Thread.Sleep(1000);
                    Functions.MainScreen.ZoomOut();
                    Thread.Sleep(30000);
                    
//TODO:             if ($iCollectCounter > $COLLECTATCOUNT) {
//TODO:                 CollectResources();
//TODO:                 Thread.Sleep(1000);
//TODO:                 $iCollectCounter = 0;
//TODO:             }
//TODO:             $iCollectCounter++;

                    TrainTroops();
                    if (GlobalVariables.fullArmy)
                        break;

                    Thread.Sleep(1000);
                    DropTrophies();
                    Thread.Sleep(1000);
                    DonateCC();
                    sw.Stop();

                    double idleTime = (double) sw.ElapsedMilliseconds * 1000;
					Main.Bot.WriteToOutput(string.Format("Time Idle: {0} hours {1} minutes {2} seconds", Math.Floor(Math.Floor(idleTime / 60) / 60), Math.Floor(Math.Floor(idleTime / 60) % 60), Math.Floor(idleTime % 60)), GlobalVariables.OutputStates.Warning);
                }
            }
        }

		public static void RequestTroops()
		{
			Point ccPos = Main.Bot.LocationClanCastle;

			if (ccPos.IsEmpty)
			{
				// LOCATE CLAN CASTLE
				// SAVE CONFIG
				Thread.Sleep(1000);
			}

			Main.Bot.WriteToOutput("Requesting for Clan Castle Troops...");
			Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, ccPos, 1);
			Thread.Sleep(1000);

			Point requestTroop = Tools.FastFind.FastFindHelper.PixelSearch(310, 580, 553, 622, Color.FromArgb(96, 140, 144), 10);
			if (!requestTroop.IsEmpty)
			{
				Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, requestTroop, 1);
				Thread.Sleep(1000);
				if (Tools.FastFind.FastFindHelper.IsInColorRange(new Point(340, 245), Color.FromArgb(204, 64, 16), 20))
				{
					if (!string.IsNullOrEmpty(Main.Bot.RequestTroopsMessage))
					{
						Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(430, 140), 1);
						Thread.Sleep(1000);
						Tools.KeyboardHelper.SendToBS(Main.Bot.RequestTroopsMessage);
					}
					Thread.Sleep(1000);
					Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(524, 228), 1);
				}
				else
				{
					Main.Bot.WriteToOutput("Request's already been made...");
					Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(1, 1), 2);
				}
			}
			else
			{
				Main.Bot.WriteToOutput("Clan Castle not available...");
			}
		}

        public static void TrainTroops()
        {
            Point[] barrackPos = new Point[] {Main.Bot.LocationBarrack1, Main.Bot.LocationBarrack2, Main.Bot.LocationBarrack3, Main.Bot.LocationBarrack4, Main.Bot.LocationDarkBarrack1, Main.Bot.LocationDarkBarrack2};

            if(barrackPos[0].IsEmpty)
            {
//TODO:         LOCATE BARRACKS
//TODO:         SAVE CONFIG
                Thread.Sleep(1000);
            }

            Main.Bot.WriteToOutput("Training Troops...");

            for(int i = 0; i < 4; i++)
            {
                Thread.Sleep(500);
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(1, 1), 1);
                Thread.Sleep(500);

                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(barrackPos[i].X, barrackPos[i].Y), 1);
                Thread.Sleep(500);

                Point trainPos = Tools.FastFind.FastFindHelper.PixelSearch(155, 603, 694, 605, Color.FromArgb(96, 56, 24), 5);
                
                if(trainPos.IsEmpty)
                {
                    Main.Bot.WriteToOutput("Barrack " + (i + 1).ToString() + " is not available...");
                    Thread.Sleep(500);
                }
                else
                {
                    Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, trainPos, 1);
                    Thread.Sleep(500);

                    CheckFullArmy();

                    int barrackId = 0;

                    if (i == 0)
                        barrackId = Main.Bot.SelectedBarrack1.Id;
                    else if (i == 1)
                        barrackId = Main.Bot.SelectedBarrack2.Id;
                    else if (i == 2)
                        barrackId = Main.Bot.SelectedBarrack3.Id;
                    else if (i == 3)
                        barrackId = Main.Bot.SelectedBarrack4.Id;

                    while(TrainIt(barrackId, 5))
                    {
                        Thread.Sleep(50);
                    }
                }

                Thread.Sleep(500);
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(1, 1), 2, 250);
            }

            Main.Bot.WriteToOutput("Training Troops Complete...");
        }

        public static bool TrainIt(int troopKind, int count)
        {
            Point pos = GetTrainPos(troopKind);

            if(!pos.IsEmpty)
            {
//TODO:         If CheckPixel($pos) Then :: I was confused by this conditional because the CheckPixel method says its 1 parameter should be an array[4], but this position variable has a 2 values
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, pos, count);
                Thread.Sleep(500);
                return true;
//TODO:         EndIf
            }

            return false;
        }

        public static Point GetTrainPos(int troopKind)
        {
            switch((Data.Troop) troopKind)
            {
                case Data.Troop.Barbarian:
                    return new Point(261, 366);
                case Data.Troop.Archer:
                    return new Point(369, 366);
                case Data.Troop.Giant:
                    return new Point(475, 366);
                case Data.Troop.Goblin:
                    return new Point(581, 366);
                case Data.Troop.WallBreaker:
                    return new Point(688, 366);
                default:
                    {
                        Main.Bot.WriteToOutput("Don't know how to train the troop " + troopKind + " yet...");
                        return new Point(0, 0);
                    }
            }
        }
    }
}
