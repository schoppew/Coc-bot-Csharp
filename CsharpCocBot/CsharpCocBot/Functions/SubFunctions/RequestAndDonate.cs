using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoC.Bot.Data;
using MouseAndKeyboard;
using Point = Win32.POINT;

namespace CoC.Bot.Functions
{
	public static class RequestAndDonate
	{
		#region Donate
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
		#endregion Donate

		#region Request
		public static void RequestTroops()
		{
            ClickablePoint ccPos = (ClickablePoint)Main.Bot.LocationClanCastle;

			if (ccPos.IsEmpty)
			{
				Main.Bot.LocateClanCastle();
			}

			Main.Bot.WriteToOutput("Requesting for Clan Castle Troops...");
			Tools.CoCHelper.Click(ccPos);
			Thread.Sleep(200);

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

		public static ClickablePoint GetRequestTroopsButton()
		{
			int left = ScreenData.RequestTroopsButton.Left;
			int top = ScreenData.RequestTroopsButton.Top;
			int right = ScreenData.RequestTroopsButton.Right;
			int bottom = ScreenData.RequestTroopsButton.Bottom;
			int count = 0;

			do
			{
				DetectableArea area = new DetectableArea(left, top, right, bottom, ScreenData.RequestTroopsButton.Color, ScreenData.RequestTroopsButton.ShadeVariation);
				ClickablePoint p1 = Tools.CoCHelper.SearchPixelInRect(area);

                if(!p1.IsEmpty)
                { 
				    if (Tools.CoCHelper.IsInColorRange(new ClickablePoint(p1.Point.X + ScreenData.RequestTroopsButton2.Point.X, p1.Point.Y + ScreenData.RequestTroopsButton2.Point.Y), ScreenData.RequestTroopsButton2.Color, ScreenData.RequestTroopsButton2.ShadeVariation))
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
		#endregion Request

	}
}
