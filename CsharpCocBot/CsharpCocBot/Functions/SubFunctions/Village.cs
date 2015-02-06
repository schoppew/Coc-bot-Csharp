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

            if (collectorPos[0].IsEmpty)
            {
                Main.Bot.LocateCollectors();
            }

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
                Main.Bot.WriteToOutput("~~~ Waiting for full army ~~~");
                while (!GlobalVariables.fullArmy)
                {
                    sw.Start();

                    Thread.Sleep(1000);
                    MainScreen.CheckMainScreen();
                    Thread.Sleep(1000);
                    MainScreen.ZoomOut();
                    Thread.Sleep(30000);

                    CollectResources();

                    Barrack.TrainTroops();
                    if (GlobalVariables.fullArmy)
                        break;

                    Thread.Sleep(1000);
                    DropTrophies();
                    Thread.Sleep(1000);
                    ClanDonation.DonateCC();
                    sw.Stop();

                    double idleTime = (double)sw.ElapsedMilliseconds * 1000;
                    Main.Bot.WriteToOutput(string.Format("Time Idle: {0} hours {1} minutes {2} seconds", Math.Floor(Math.Floor(idleTime / 60) / 60), Math.Floor(Math.Floor(idleTime / 60) % 60), Math.Floor(idleTime % 60)), GlobalVariables.OutputStates.Warning);
                }
            }
        }

        public static void RequestTroops()
        {
			Point ccPos = Main.Bot.LocationClanCastle;

            if (ccPos.IsEmpty)
            {
                Main.Bot.LocateClanCastle();
            }

            Main.Bot.WriteToOutput("Requesting for Clan Castle Troops...");
            Tools.CoCHelper.ClickBad(ccPos, 1);
            Thread.Sleep(500);
            Tools.CoCHelper.Click(new ClickablePoint(ccPos));

            ClickablePoint requestTroop = GetRequestTroopsButton();

            if (!requestTroop.IsEmpty)
            {
                Tools.CoCHelper.Click(requestTroop);
                Thread.Sleep(1000);
                if (Tools.CoCHelper.CheckPixelColorBad(new Point(340, 245), System.Drawing.Color.FromArgb(204, 64, 16), 20))
                {
                    if (!string.IsNullOrEmpty(Main.Bot.RequestTroopsMessage))
                    {
                        Tools.CoCHelper.ClickBad(new Point(430, 140), 1);
                        Thread.Sleep(1000);
                        KeyboardHelper.SendToBS(Main.Bot.RequestTroopsMessage);
                    }
                    Thread.Sleep(1000);
                    Tools.CoCHelper.ClickBad(new Point(524, 228), 1);
                }
                else
                {
                    Main.Bot.WriteToOutput("Request's already been made...");
                    Tools.CoCHelper.ClickBad(new Point(1, 1), 2);
                }
            }
            else
            {
                Main.Bot.WriteToOutput("Clan Castle not available...");
            }
        }

    }
}
