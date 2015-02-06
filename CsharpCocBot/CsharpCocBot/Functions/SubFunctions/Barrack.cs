using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoC.Bot.Data;
using Point = Win32.POINT;

namespace CoC.Bot.Functions
{
	public static class Barrack
	{
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

                if(!p1.IsEmpty)
                { 
				    if (Tools.CoCHelper.IsInColorRange(new ClickablePoint(p1.Point.X + ScreenData.TrainTroopsButton2.Point.X, p1.Point.Y + ScreenData.TrainTroopsButton2.Point.Y), ScreenData.TrainTroopsButton2.Color, ScreenData.TrainTroopsButton2.ShadeVariation))
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
						    left = p1.Point.X;
						    top = p1.Point.Y;
						    count++;
					    }
				    }
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

            if(!barrackOpen)
            {
                Tools.CoCHelper.Click(ScreenData.TopLeftClient, 2, 100);
                Thread.Sleep(200);
                Tools.CoCHelper.Click(new ClickablePoint(Main.Bot.LocationBarrack1));
                Thread.Sleep(200);
                Tools.CoCHelper.Click(GetTrainTroopsButton());
            }
            
            campsFull = Tools.CoCHelper.SameColor(Tools.CoCHelper.GetPixelColor(ScreenData.ArmyFullNotif), ScreenData.ArmyFullNotif.Color, 6);
            return campsFull;
		}


		public static void TrainTroops()
		{
			ClickablePoint[] barrackPos = new ClickablePoint[] { (ClickablePoint)Main.Bot.LocationBarrack1, (ClickablePoint)Main.Bot.LocationBarrack2, (ClickablePoint)Main.Bot.LocationBarrack3, (ClickablePoint)Main.Bot.LocationBarrack4 };
			ClickablePoint[] darkBarrackPos = new ClickablePoint[] { (ClickablePoint)Main.Bot.LocationDarkBarrack1, (ClickablePoint)Main.Bot.LocationDarkBarrack2 };
			bool armyFull = false;

			if (Main.Bot.IsUseBarracks1 && barrackPos[0].IsEmpty)
			{
				Main.Bot.LocateBarracks();
			}

			if ((Main.Bot.IsUseDarkBarracks1 && darkBarrackPos[0].IsEmpty) || (Main.Bot.IsUseDarkBarracks2 && darkBarrackPos[1].IsEmpty))
			{
				Main.Bot.LocateDarkBarracks();
			}

			Main.Bot.WriteToOutput("Training Troops...");

			for (int i = 0; i < 4; i++)
				if (barrackPos[i].IsEmpty)
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

			// Train Dark Barracks only if the army isn't full
			if (!armyFull)
			{
				for (int i = 0; i < 2; i++)
					if (darkBarrackPos[i].IsEmpty)
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

			Main.Bot.WriteToOutput("Training Troops Complete...");
		}

	}
}
