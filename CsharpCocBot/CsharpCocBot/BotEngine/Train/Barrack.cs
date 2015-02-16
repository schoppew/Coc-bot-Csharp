using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoC.Bot.BotEngine.Train;
using CoC.Bot.Data;
using Point = Win32.POINT;

namespace CoC.Bot.BotEngine
{
	public static class Barrack
	{
		static public bool RemoveAllTraining()
		{
			// Todo: do the proper clicks to remove all the troops in queues in all barracks. 
			// This should be called before an attack. 
			return false;
		}
		
		
		static public void TrainPlanner()
		{
			// Todo: This collection must be stored in some global shared place. 
			//       It is needed to train troops, and need to be updated when troops are donated
			TroopToTrainInBarracks barracks = new TroopToTrainInBarracks(); 
			foreach (var building in DataCollection.BuildingPoints)
				barracks.AddBarrack(building);
			
			// Todo: fill each barrack according to number of each troop to be done
			int TellMeHowToGetThisDataFromUI;
			IEnumerable<TroopModel> TroopsToBeTrained=null;


			// All the magic is here ;=)			
			foreach (TroopModel troop in TroopsToBeTrained.OrderByDescending(t=>t.Troop.TimeToTrain()))
			{
				barracks.AddTroop(troop.Troop);
			}

			// Here you can retreive in each barrack what has to be trained. 
			// Just like that:
			//TrainingElement te;
			//foreach(var barrack in barracks)
			//	while ((te = barrack.Pop()) != null)
			//	{
			//		Troop troopToTrain = te.Troop;
			//		int Quantity = te.TotalCount - te.TrainingStarted;
			//		<Train It>
			//		te.TrainingStarted = te.TotalCount;
			//	}
				
		}

		static public Troop GetTroopToBeTrainedInBarrack(int barrackId, bool dark)
		{
			if (barrackId < 0 || barrackId > 3) throw new ArgumentException("barrackId should be between 0 and 3");
			if (dark)
				if (barrackId < 0 || barrackId > 1) throw new ArgumentException("barrackId should be between 0 and 1 when dark is true");
			switch (barrackId)
			{
				case 0:
					if (dark)
						return (Troop)Main.Bot.SelectedDarkBarrack1.Id;
					return (Troop)Main.Bot.SelectedBarrack1.Id;
				case 1:
					if (dark)
						return (Troop)Main.Bot.SelectedDarkBarrack2.Id;
					return (Troop)Main.Bot.SelectedBarrack2.Id;
				case 2:
					return (Troop)Main.Bot.SelectedBarrack3.Id;
				case 3:
					return (Troop)Main.Bot.SelectedBarrack4.Id;
			}
			return Troop.None;
		}

		public static bool TrainIt(Troop troopKind, int count)
		{
			ClickablePoint pos = ScreenData.GetTrainPos(troopKind);

			if (!pos.IsEmpty)
			{
				Tools.CoCHelper.Click(pos, count, 100);
				return true;
			}

			return false;
		}

		public static ClickablePoint GetTrainTroopsButton()
		{
			int left = ScreenData.TrainTroopsButton.Left;
			int top = ScreenData.TrainTroopsButton.Top;
			int right = ScreenData.TrainTroopsButton.Right;
			int bottom = ScreenData.TrainTroopsButton.Bottom;
			int count = 0;

			do
			{
				DetectableArea area = new DetectableArea(left, top, right, bottom, ScreenData.TrainTroopsButton.Color, ScreenData.TrainTroopsButton.ShadeVariation);
				ClickablePoint p1 = Tools.CoCHelper.SearchPixelInRect(area);

				if (!p1.IsEmpty && !(p1.Point.X == -1 || p1.Point.Y == -1))
				{
					if (Tools.CoCHelper.IsInColorRange(new ClickablePoint(p1.Point.X + ScreenData.TrainTroopsButton2.Point.X, p1.Point.Y + ScreenData.TrainTroopsButton2.Point.Y), ScreenData.TrainTroopsButton2.Color, ScreenData.TrainTroopsButton2.ShadeVariation))
					{
						return p1;
					}
				}

				if (count >= 6)
				{
					break;
				}
				else
				{
					if (!p1.IsEmpty && !(p1.Point.X == -1 || p1.Point.Y == -1))
					{
						left = p1.Point.X;
						top = p1.Point.Y;
					}

					count++;
				}
			} while (true);

			return new ClickablePoint();
		}

		public static bool CheckBarrackFull()
		{
			// BARRACK MUST BE OPEN FOR THIS
			return Tools.CoCHelper.CheckPixelColor(ScreenData.BarbarianSlotGrey);
		}

		public static bool CheckFullArmy(bool barrackOpen)
		{
			bool campsFull;

			if (!barrackOpen)
			{
				Tools.CoCHelper.Click(ScreenData.TopLeftClient, 2, 100);
				Thread.Sleep(200);
				Tools.CoCHelper.Click(new ClickablePoint(DataCollection.BuildingPoints.Where(b => b.Building == Building.Barrack1).First().Coordinates));
				//Tools.CoCHelper.Click(new ClickablePoint(Main.Bot.LocationBarrack1));
				Thread.Sleep(200);
				Tools.CoCHelper.Click(GetTrainTroopsButton());
			}

			campsFull = Tools.CoCHelper.SameColor(Tools.CoCHelper.GetPixelColor(ScreenData.ArmyFullNotif), ScreenData.ArmyFullNotif.Color, 6);
			return campsFull;
		}


		public static void TrainTroops()
		{
			var barracks = DataCollection.BuildingPoints.Where(b => b.BuildingType == BuildingType.Barrack);
			var darkBarracks = DataCollection.BuildingPoints.Where(b => b.BuildingType == BuildingType.DarkBarrack);

			if (barracks.Count() <= 0)
				return; // The DataCollection.BuildingPoints is empty. Something is wrong!

			if (darkBarracks.Count() > 0)
				return; // The DataCollection.BuildingPoints is empty. Something is wrong!

			var barrackPos = barracks.Select(x => new ClickablePoint(x.Coordinates)).ToArray();
			var darkBarrackPos = darkBarracks.Select(x => new ClickablePoint(x.Coordinates)).ToArray();
			//ClickablePoint[] barrackPos = new ClickablePoint[] { (ClickablePoint)Main.Bot.LocationBarrack1, (ClickablePoint)Main.Bot.LocationBarrack2, (ClickablePoint)Main.Bot.LocationBarrack3, (ClickablePoint)Main.Bot.LocationBarrack4 };
			//ClickablePoint[] darkBarrackPos = new ClickablePoint[] { (ClickablePoint)Main.Bot.LocationDarkBarrack1, (ClickablePoint)Main.Bot.LocationDarkBarrack2 };

			bool armyFull = false;

			// FF, do not change this to just checking if barrackPos[0].isEmpty. It needs to check if the x or y values are 0 as well to work.
			if (Main.Bot.IsUseBarracks1 && (barrackPos[0].IsEmpty || barrackPos[0].Point.X == 0 || barrackPos[0].Point.Y == 0))
			{
				Main.Bot.LocateBarracks();
			}

			// FF, do not change this to just checking if darkBarrackPos[0].isEmpty. It needs to check if the x or y values are 0 as well to work.
			if ((Main.Bot.IsUseDarkBarracks1 && (darkBarrackPos[0].IsEmpty || darkBarrackPos[0].Point.X == 0 || darkBarrackPos[0].Point.Y == 0)) || (Main.Bot.IsUseDarkBarracks2 && (darkBarrackPos[1].IsEmpty || darkBarrackPos[1].Point.X == 0 || darkBarrackPos[1].Point.Y == 0)))
			{
				Main.Bot.LocateDarkBarracks();
			}

			Main.Bot.WriteToOutput("Training Barracks Troops...");

			for (int i = 0; i < 4; i++)
			{
				// FF, do not change this to just checking if barrackPos[0].isEmpty. It needs to check if the x or y values are 0 as well to work.
				if (barrackPos[i].IsEmpty || barrackPos[i].Point.X == 0 || barrackPos[i].Point.Y == 0)
					Main.Bot.WriteToOutput(string.Format("Barrack {0} is not set...", i + 1));
				else
				{
					Tools.CoCHelper.Click(Data.ScreenData.TopLeftClient, 2, 200);
					Thread.Sleep(500);

					Tools.CoCHelper.Click(barrackPos[i], 1);
					Thread.Sleep(500);

					Point trainPos = Barrack.GetTrainTroopsButton();

					if (trainPos.IsEmpty)
					{
						Main.Bot.WriteToOutput(string.Format("Barrack {0} is not available...", i + 1));
					}
					else
					{
						Tools.CoCHelper.Click(new ClickablePoint(trainPos));
						Thread.Sleep(500);

						armyFull = CheckFullArmy(true);

						if (!armyFull)
						{
							Troop troop = GetTroopToBeTrainedInBarrack(i, false);

							while (!CheckBarrackFull())
							{
								TrainIt(troop, 5);
								Thread.Sleep(50);
							}
						}
						else
						{
							Main.Bot.WriteToOutput("Barracks Full...", GlobalVariables.OutputStates.Normal);
						}
					}

					Tools.CoCHelper.Click(Data.ScreenData.TopLeftClient, 2, 250);
				}
			}

			// Train Dark Barracks only if the army isn't full
			if (!armyFull)
			{
				Main.Bot.WriteToOutput("Training Dark Barracks Troops...");

				for (int i = 0; i < 2; i++)
				{
					// FF, do not change this to just checking if darkBarrackPos[0].isEmpty. It needs to check if the x or y values are 0 as well to work.
					if (darkBarrackPos[i].IsEmpty || darkBarrackPos[i].Point.X == 0 || darkBarrackPos[i].Point.Y == 0)
						Main.Bot.WriteToOutput(string.Format("Dark Barrack {0} is not set...", i + 1));
					else
					{
						Tools.CoCHelper.Click(Data.ScreenData.TopLeftClient, 2, 200);
						Thread.Sleep(500);

						Tools.CoCHelper.Click(darkBarrackPos[i], 1);
						Thread.Sleep(500);

						Point trainPos = GetTrainTroopsButton();

						if (trainPos.IsEmpty)
						{
							Main.Bot.WriteToOutput(string.Format("Dark Barrack {0} is not available...", i + 1));
						}
						else
						{
							Tools.CoCHelper.Click(new ClickablePoint(trainPos));
							Thread.Sleep(50);

							if (!armyFull)
							{
								Troop troop = GetTroopToBeTrainedInBarrack(i, true);

								while (!CheckBarrackFull())
								{
									TrainIt(troop, 5);
									Thread.Sleep(50);
								}
							}
						}

						Tools.CoCHelper.Click(Data.ScreenData.TopLeftClient, 2, 250);
					}
				}
			}

			Main.Bot.WriteToOutput("Training Troops Complete...");
		}

		public static void Boost()
		{
			//TODO: Create UI Elements/Properties for Boosting Barracks. TAGS: Ph!d
		}

	}
}
