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

        public void CollectResources()
        {

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

        public void RequestTroops()
        {

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

                    while(TrainIt(GlobalVariables.barrackTroop[i], 5))
                    {
                        Thread.Sleep(50);
                    }
                }

                Thread.Sleep(500);
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(1, 1), 2, 250);
            }

            Main.Bot.Output = "Training Troops Complete...";
        }

        public static bool TrainIt(string troopKind, int count)
        {
            Point pos = GetTrainPos(troopKind);

            if(!pos.IsEmpty)
            {
//TODO:         If CheckPixel($pos) Then
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, pos, count);
                Thread.Sleep(500);
                return true;
//TODO:         EndIf
            }

            return false;
        }

        public static Point GetTrainPos(string troopKind)
        {
            switch(troopKind)
            {
                case "Barbarian":
                    return new Point(261, 366);
                case "Archer":
                    return new Point(369, 366);
                case "Giant":
                    return new Point(475, 366);
                case "Goblin":
                    return new Point(581, 366);
                case "Wallbreaker":
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
