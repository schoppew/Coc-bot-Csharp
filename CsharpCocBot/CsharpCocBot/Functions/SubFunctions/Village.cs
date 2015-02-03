using System;
using System.Collections.Generic;
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
            Point[] collectorPos = new Point[] {};

            if (collectorPos[0].IsEmpty)
            {
                // LOCATE COLLECTORS
                // SAVE CONFIG
                Thread.Sleep(2000);
            }

            Main.Bot.Output = "Collecting Resources...";
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

        public void DonateCC()
        {

        }

        public void DropTrophies()
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

        public static void RequestTroops()
        {
            Point ccPos = new Point(-1, -1);

            if (ccPos.IsEmpty)
            {
                // LOCATE CLAN CASTLE
                // SAVE CONFIG
                Thread.Sleep(1000);
            }

            Main.Bot.Output = "Requesting for Clan Castle Troops...";
            Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, ccPos, 1);
            Thread.Sleep(1000);

            Point requestTroop = Tools.FastFind.FastFindHelper.PixelSearch(310, 580, 553, 622, Color.FromArgb(96, 140, 144), 10);
            if(!requestTroop.IsEmpty)
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
                    Main.Bot.Output = "Request's already been made...";
                    Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(1, 1), 2);
                }
            }
            else
            {
                Main.Bot.Output = "Clan Castle not available...";
            }
        }

        public static void TrainTroops()
        {
            if(GlobalVariables.barrackPos[0].IsEmpty)
            {
//TODO:         LOCATE BARRACKS
//TODO:         SAVE CONFIG
                Thread.Sleep(1000);
            }
           Main.Bot.Output = "Training Troops...";

            for(int i = 0; i < 4; i++)
            {
                Thread.Sleep(500);
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(1, 1), 1);
                Thread.Sleep(500);

                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(GlobalVariables.barrackPos[i].X, GlobalVariables.barrackPos[i].Y), 1);
                Thread.Sleep(500);

                Point trainPos = Tools.FastFind.FastFindHelper.PixelSearch(155, 603, 694, 605, Color.FromArgb(96, 56, 24), 5);
                
                if(trainPos.IsEmpty)
                {
                    Main.Bot.Output = "Barrack " + (i + 1).ToString() + " is not available...";
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

            Main.Bot.Output = "Training Troops Complete...";
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
                        Main.Bot.Output = "Don't know how to train the troop " + troopKind + " yet...";
                        return new Point(0, 0);
                    }
            }
        }
    }
}
