using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using CoC.Bot.Data;
using MouseAndKeyboard;
using Point = Win32.POINT;
using System.Windows;

namespace CoC.Bot.Functions
{
    internal partial class Village
    {
        public static void CollectResources()
        {
            Point[] collectorPos = new Point[] { Main.Bot.LocationCollector1, Main.Bot.LocationCollector2, Main.Bot.LocationCollector3, Main.Bot.LocationCollector4, Main.Bot.LocationCollector5, Main.Bot.LocationCollector6, Main.Bot.LocationCollector7, Main.Bot.LocationCollector8, Main.Bot.LocationCollector9, Main.Bot.LocationCollector10, Main.Bot.LocationCollector11, Main.Bot.LocationCollector12, Main.Bot.LocationCollector13, Main.Bot.LocationCollector14, Main.Bot.LocationCollector15, Main.Bot.LocationCollector16, Main.Bot.LocationCollector17 };

            if (collectorPos[0].IsEmpty || collectorPos[0].X == 0 || collectorPos[0].Y == 0)
            {
                Main.Bot.LocateExtractors();
            }

            // FF, do not change this to just checking if collectorPos[0].isEmpty. It needs to check if the x or y values are 0 as well to work.
            if (!collectorPos[0].IsEmpty && collectorPos[0].X != 0 && collectorPos[0].Y != 0)
            {
                Main.Bot.WriteToOutput("Collecting Resources...");
                Thread.Sleep(250);

                for (int i = 0; i < 17; i++)
                {
                    Tools.CoCHelper.Click(ScreenData.TopLeftClient, 2, 50);
                    Thread.Sleep(250);
                    Tools.CoCHelper.Click(new ClickablePoint(collectorPos[i]));
                    Thread.Sleep(250);
                }
            }
            else
            {
                Main.Bot.WriteToOutput("Collectors Unavailable...", GlobalVariables.OutputStates.Normal);
        
            }
        }


        public static void DropTrophies()
        {
            int trophyCount = ReadText.GetOther(50, 74, "Trophy");

            while (trophyCount > Main.Bot.MaxTrophies)
            {
                trophyCount = ReadText.GetOther(50, 74, "Trophy");
                Main.Bot.WriteToOutput("Trophy Count: " + trophyCount, GlobalVariables.OutputStates.Normal);

                if (trophyCount > Main.Bot.MaxTrophies)
                {
                    Main.Bot.WriteToOutput("Dropping Trophies...", GlobalVariables.OutputStates.Information);
                    Thread.Sleep(2000);
                    MainScreen.ZoomOut();
                    Search.PrepareSearch();

                    Thread.Sleep(5000);
                    Tools.CoCHelper.Click(ScreenData.DropSingleBarb);
                    Thread.Sleep(1000);

                    MainScreen.ReturnHome(false, false);
                }
                else
                {
                    Main.Bot.WriteToOutput("Trophy Drop Complete...", GlobalVariables.OutputStates.Information);
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
                Main.Bot.WriteToOutput("~~~ Waiting for full army ~~~", GlobalVariables.OutputStates.Verified);
                while (!Barrack.CheckFullArmy(false))
                {
                    sw.Start();

                    Thread.Sleep(1000);
                    MainScreen.CheckMainScreen();

                    Thread.Sleep(1000);
                    MainScreen.ZoomOut();

                    Main.Bot.WriteToOutput("Going idle for 30 seconds...", GlobalVariables.OutputStates.Information);
                    Thread.Sleep(30000);
                    CollectResources();

                    Barrack.TrainTroops();
                    if (Barrack.CheckFullArmy(false))
                        break;

                    Thread.Sleep(1000);
                    DropTrophies();

                    Thread.Sleep(1000);
					RequestAndDonate.DonateCC();

                    sw.Stop();

                    double idleTime = (double)sw.ElapsedMilliseconds / 1000;
                    TimeSpan ts = TimeSpan.FromSeconds(idleTime);

                    string output = string.Format("Time Idle: {0:D2} hours {1:D2} minutes {2:D2} seconds", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
                    Main.Bot.WriteToOutput(output, GlobalVariables.OutputStates.Verified);
                }
            }
        }        
                
    }
}
