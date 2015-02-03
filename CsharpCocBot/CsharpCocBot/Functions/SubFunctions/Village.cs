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
                Main.Bot.LocateCollectors();
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
            int trophyCount = Functions.ReadText.GetOther(50, 74, "Trophy");

            while (trophyCount > Main.Bot.MaxTrophies)
            {
                trophyCount = Functions.ReadText.GetOther(50, 74, "Trophy");
                Main.Bot.Output = "Trophy Count: " + trophyCount;
                
                if(trophyCount > Main.Bot.MaxTrophies)
                {
                    Main.Bot.Output = "Dropping Trophies...";
                    Thread.Sleep(2000);
                    Functions.MainScreen.ZoomOut();
                    Functions.Search.PrepareSearch();

                    Thread.Sleep(5000);
                    Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(34, 310), 1);
                    Thread.Sleep(1000);

                    Functions.MainScreen.ReturnHome(false, false);
                }
                else
                {
                    Main.Bot.Output = "Trophy Drop Complete...";
                }
            }
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
            System.Drawing.Point ccPos = Main.Bot.LocationClanCastle;

            if (ccPos.IsEmpty)
            {
                Main.Bot.LocateClanCastle();
                Thread.Sleep(1000);
            }

            Main.Bot.WriteToOutput("Requesting for Clan Castle Troops...");
            Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, ccPos, 1);
            Thread.Sleep(1000);

            System.Drawing.Point requestTroop = Tools.FastFind.FastFindHelper.PixelSearch(310, 580, 553, 622, Color.FromArgb(96, 140, 144), 10);
            if(!requestTroop.IsEmpty)
            {
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, requestTroop, 1);
                Thread.Sleep(1000);
                if (Tools.FastFind.FastFindHelper.IsInColorRange(new System.Drawing.Point(340, 245), Color.FromArgb(204, 64, 16), 20))
                {
                    if (!string.IsNullOrEmpty(Main.Bot.RequestTroopsMessage))
                    {
                        Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new System.Drawing.Point(430, 140), 1);
                        Thread.Sleep(1000);
                        Tools.KeyboardHelper.SendToBS(Main.Bot.RequestTroopsMessage);
                    }
                    Thread.Sleep(1000);
                    Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new System.Drawing.Point(524, 228), 1);
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
            Point[] barrackPos = new Point[] {Main.Bot.LocationBarrack1, Main.Bot.LocationBarrack2, Main.Bot.LocationBarrack3, Main.Bot.LocationBarrack4};
            Point[] darkBarrackPos = new Point[] { Main.Bot.LocationDarkBarrack1, Main.Bot.LocationDarkBarrack2 };

            if(Main.Bot.IsUseBarracks1 && barrackPos[0].IsEmpty)
            {
                Main.Bot.LocateBarracks();
                Thread.Sleep(1000);
            }

            if ((Main.Bot.IsUseDarkBarracks1 && darkBarrackPos[0].IsEmpty) || (Main.Bot.IsUseDarkBarracks2 && darkBarrackPos[1].IsEmpty))
            {
                Main.Bot.LocateDarkBarracks();
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
					Main.Bot.WriteToOutput(string.Format("Barrack {0} is not available...", i + 1));
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

            for (int i = 0; i < 2; i++)
            {
                Thread.Sleep(500);
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(1, 1), 1);
                Thread.Sleep(500);

                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(darkBarrackPos[i].X, darkBarrackPos[i].Y), 1);
                Thread.Sleep(500);

                Point trainPos = Tools.FastFind.FastFindHelper.PixelSearch(155, 603, 694, 605, Color.FromArgb(96, 56, 24), 5);

                if (trainPos.IsEmpty)
                {
					Main.Bot.WriteToOutput(string.Format("Dark Barrack {0} is not available...", i + 1));
                    Thread.Sleep(500);
                }
                else
                {
                    Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, trainPos, 1);
                    Thread.Sleep(500);

                    CheckFullArmy();

                    int barrackId = 0;

                    if (i == 0)
                        barrackId = Main.Bot.SelectedDarkBarrack1.Id;
                    else if (i == 1)
                        barrackId = Main.Bot.SelectedDarkBarrack2.Id;

                    while (TrainIt(barrackId, 5))
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
                    return new Point(224, 323);
                case Data.Troop.Archer:
                    return new Point(337, 323);
                case Data.Troop.Giant:
                    return new Point(438, 366);
                case Data.Troop.Goblin:
                    return new Point(548, 366);
                case Data.Troop.WallBreaker:
                    return new Point(650, 366);
                case Data.Troop.Balloon:
                    return new Point(218, 438);
                case Data.Troop.Wizard:
                    return new Point(326, 438);
                case Data.Troop.Healer:
                    return new Point(434, 438);
                case Data.Troop.Dragon:
                    return new Point(536, 438);
                case Data.Troop.Pekka:
                    return new Point(646, 438);
                case Data.Troop.Minion:
                    return new Point(224, 323); // THESE
                case Data.Troop.HogRider:
                    return new Point(337, 323); // MAY
                case Data.Troop.Valkyrie:
                    return new Point(438, 366); // BE
                case Data.Troop.Golem:
                    return new Point(548, 366); // WRONG
                case Data.Troop.Witch:
                    return new Point(650, 366); //
                case Data.Troop.LavaHound:
                    return new Point(218, 438); //-----------
                default:
                    {
						Main.Bot.WriteToOutput(string.Format("Don't know how to train the troop {0} yet...", troopKind));
                        return Point.Empty;
                    }
            }
        }
    }
}
