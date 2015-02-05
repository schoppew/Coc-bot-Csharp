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
			Point[] collectorPos = new Point[] { Main.Bot.LocationCollector1, Main.Bot.LocationCollector2, Main.Bot.LocationCollector3, Main.Bot.LocationCollector4, Main.Bot.LocationCollector5, Main.Bot.LocationCollector6, Main.Bot.LocationCollector7, Main.Bot.LocationCollector8, Main.Bot.LocationCollector9, Main.Bot.LocationCollector10, Main.Bot.LocationCollector11, Main.Bot.LocationCollector12, Main.Bot.LocationCollector13, Main.Bot.LocationCollector14, Main.Bot.LocationCollector15, Main.Bot.LocationCollector16, Main.Bot.LocationCollector17 };

			if (collectorPos[0].IsEmpty)
			{
				Main.Bot.LocateCollectors();
				Thread.Sleep(2000); // TODO: Mephobia: We don't need to sleep, we just make sure the user did his job of locating the collectors and then continue
			}

			Main.Bot.WriteToOutput("Collecting Resources...");
			Thread.Sleep(250);
			Tools.CoCHelper.ClickBad(new Point(1, 1));

			for (int i = 0; i < 17; i++)
			{
				Thread.Sleep(250);
				Tools.CoCHelper.ClickBad(collectorPos[i]);
				Thread.Sleep(250);
				Tools.CoCHelper.ClickBad(new Point(1, 1));
			}
		}

		/// <summary>
		/// Make Troop Donations.
		/// </summary>
		public static void DonateCC()
		{
			// NOTE: This is how you access Troop information specified by User

			// Get all Troops that meets this criteria (Selected for Donate)
			var troops = DataCollection.TroopTiers.SelectMany(tt => tt.Troops).Where(t => t.IsSelectedForDonate);

			// We then check if the User selected any for Donate
			if (troops.Count() > 0)
			{
				Main.Bot.WriteToOutput("Donating Troops...", GlobalVariables.OutputStates.Information);

				foreach (var troop in troops)
				{
					DonateCCTroopSpecific(troop);
				}				
			}			

			//bool donate = false; // FIX THIS
			//int _y = 119;

			//Main.Bot.WriteToOutput("Donating Troops...", GlobalVariables.OutputStates.Information);
			//Tools.CoCHelper.ClickBad(new Point(1, 1));

			//if (Tools.CoCHelper.CheckPixelColorBad(new Point(331, 330), Color.FromArgb(240, 160, 59), 20))
			//    Tools.CoCHelper.ClickBad(new Point(19, 349));

			//Thread.Sleep(200);
			//Tools.CoCHelper.ClickBad(new Point(189, 24));
			//Thread.Sleep(200);

			//while(donate)
			//{
			//    byte[][] offColors = new byte[][] {};
			//}  
		}

		/// <summary>
		/// Make Troop Specific Donations.
		/// </summary>
		/// <param name="troop">The troop.</param>
		private static void DonateCCTroopSpecific(TroopModel troop)
		{
			// TODO: Do the clicking Stuff here
			// Remember to get the needed information:
			// troop.DonateKeywords
			// troop.MaxDonationsPerRequest

			Main.Bot.WriteToOutput(string.Format("Donating {0} {1}s...", troop.MaxDonationsPerRequest, ((Troop)troop.Id).Name()), GlobalVariables.OutputStates.Verified);
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
					Tools.CoCHelper.ClickBad(new Point(34, 310));
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
				Thread.Sleep(1000); // TODO: Mephobia: We don't need to sleep, we just make sure the user did his job of locating the CC
			}

			Main.Bot.WriteToOutput("Requesting for Clan Castle Troops...");
			Tools.CoCHelper.ClickBad(ccPos, 1);
			Thread.Sleep(1000);

			ClickablePoint requestTroop = Tools.CoCHelper.SearchPixelInRect(310, 580, 553, 622, System.Drawing.Color.FromArgb(96, 140, 144), 10);
			if (!requestTroop.IsEmpty)
			{
				Tools.CoCHelper.ClickBad(requestTroop, 1);
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

        public static Point GetTrainTroopsButton()
        {
            //196, 558, 469, 85
            int left = 196;
            int top = 558;
            int right = 665;
            int bottom = 643;
            int count = 0;

            do
            {
                Point p1 = FastFind.FastFindHelper.PixelSearch(left, top, right, bottom, Color.FromArgb(67, 38, 3), 4);

                if (FastFind.FastFindHelper.IsInColorRange(new Point(p1.X, p1.Y + 10), Color.FromArgb(255, 255, 255), 4))
                {
                    return p1;
                }
                else
                {
                    if (count >= 6)
                    {
                        break;
                    }
                    else
                    {
                        left = p1.X;
                        top = p1.Y;
                        count++;
                    }
                }
            } while (true);

            return new Point(-1, -1);
        }

        public static void TrainTroops()
        {
            Point[] barrackPos = new Point[] { new Point(358, 255), Main.Bot.LocationBarrack2, Main.Bot.LocationBarrack3, Main.Bot.LocationBarrack4 };
            Point[] darkBarrackPos = new Point[] { Main.Bot.LocationDarkBarrack1, Main.Bot.LocationDarkBarrack2 };
            bool armyFull = false;

            //if (Main.Bot.IsUseBarracks1 && barrackPos[0].IsEmpty)
            //{
            //    Main.Bot.LocateBarracks();
            //}

            //if ((Main.Bot.IsUseDarkBarracks1 && darkBarrackPos[0].IsEmpty) || (Main.Bot.IsUseDarkBarracks2 && darkBarrackPos[1].IsEmpty))
            //{
            //    Main.Bot.LocateDarkBarracks();
            //}

            Main.Bot.WriteToOutput("Training Troops...");

            for (int i = 0; i < 4; i++)
            {
                Tools.CoCHelper.Click(Data.ScreenData.TopLeftClient, 2, 200);
                Thread.Sleep(500);

                Tools.CoCHelper.Click(new Data.DetectablePoint(new Point(barrackPos[i].X, barrackPos[i].Y)), 1);
                Thread.Sleep(500);

                Point trainPos = GetTrainTroopsButton();

                if (trainPos.IsEmpty)
                {
                    Main.Bot.WriteToOutput(string.Format("Barrack {0} is not available...", i + 1));
                }
                else
                {
                    MouseHelper.ClickOnPoint2(Tools.BlueStacksHelper.GetBlueStacksWindowHandle(), trainPos);
                    Thread.Sleep(500);

                    armyFull = CheckFullArmy();

                    if (!armyFull)
                    {
                        int barrackId = 0;

                        if (i == 0)
                            barrackId = Main.Bot.SelectedBarrack1.Id;
                        else if (i == 1)
                            barrackId = Main.Bot.SelectedBarrack2.Id;
                        else if (i == 2)
                            barrackId = Main.Bot.SelectedBarrack3.Id;
                        else if (i == 3)
                            barrackId = Main.Bot.SelectedBarrack4.Id;

                        while (TrainIt(barrackId, 5))
                        {
                            Thread.Sleep(50);
                        }
                    }
                    else
                    {
                        Main.Bot.WriteToOutput("Barracks Full...", GlobalVariables.OutputStates.Normal);
                    }
                }

                Thread.Sleep(500);
                Tools.CoCHelper.Click(Data.ScreenData.TopLeftClient, 2, 250);
            }

            if (!armyFull)
            {
                for (int i = 0; i < 2; i++)
                {
                    Tools.CoCHelper.Click(Data.ScreenData.TopLeftClient, 2, 200);
                    Thread.Sleep(500);

                    Tools.CoCHelper.Click(new Data.DetectablePoint(new Point(barrackPos[i].X, barrackPos[i].Y)), 1);
                    Thread.Sleep(500);

                    Point trainPos = GetTrainTroopsButton();

                    if (trainPos.IsEmpty)
                    {
                        Main.Bot.WriteToOutput(string.Format("Dark Barrack {0} is not available...", i + 1));
                    }
                    else
                    {
                        MouseHelper.ClickOnPoint2(Tools.BlueStacksHelper.GetBlueStacksWindowHandle(), trainPos);
                        Thread.Sleep(500);

                        // MAKE BARRACKS FULL METHOD!
                        armyFull = CheckFullArmy();

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
                    Tools.CoCHelper.ClickBad(new Point(1, 1), 2, 250);
                }
            }

            Main.Bot.WriteToOutput("Training Troops Complete...");
        }

        public static bool TrainIt(int troopKind, int count)
        {
            Point pos = GetTrainPos(troopKind);
            bool armyFull = CheckFullArmy();

            if (!pos.IsEmpty && !armyFull)
            {
                MouseHelper.ClickOnPoint2(Tools.BlueStacksHelper.GetBlueStacksWindowHandle(), pos, count, 100);
                return true;
            }

            return false;
        }

        public static Point GetTrainPos(int troopKind)
        {
            switch ((Data.Troop)troopKind)
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
