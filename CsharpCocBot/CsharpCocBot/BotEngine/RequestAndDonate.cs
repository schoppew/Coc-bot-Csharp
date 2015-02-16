using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoC.Bot.Data;
using MouseAndKeyboard;
using Point = Win32.POINT;

namespace CoC.Bot.BotEngine
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

			Main.Bot.WriteToOutput("Finished donating troops...", GlobalVariables.OutputStates.Information);
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
			int _y = 119;

			Tools.CoCHelper.Click(ScreenData.TopLeftClient, 2, 50); // Click out of anything
			Thread.Sleep(300);
			Tools.CoCHelper.Click(ScreenData.OpenChatBtn); // Click Green Chat Tab
			Thread.Sleep(300);
			Tools.CoCHelper.Click((ClickablePoint)ScreenData.IsClanTabSelected); //Clicks Clan Chat Tab
			Thread.Sleep(300);

			ClickablePoint donatePos = GetDonateButton();

			if (!donatePos.IsEmpty)
			{
				string requestText = ReadText.GetString(donatePos.Point.Y - 28);

				if (string.IsNullOrEmpty(requestText))
					requestText = ReadText.GetString(donatePos.Point.Y - 17);
				else
					requestText = requestText + Environment.NewLine + ReadText.GetString(donatePos.Point.Y - 17);

				Main.Bot.WriteToOutput("Requested Troops: " + requestText, GlobalVariables.OutputStates.Information);

				string[] str = troop.DonateKeywords.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

				for (int i = 0; i < str.Length; i++)
				{
					if (requestText.Contains(str[i]))
					{
						ClickablePoint showMore;

						do
						{
							ClickablePoint donateBtn = GetDonateButton();
							showMore = GetNotificationButton();

							if (donateBtn.IsEmpty)
							{
								if (showMore.IsEmpty)
								{
									Main.Bot.WriteToOutput("No Donation Opportunities For " + troop.Name + "s...", GlobalVariables.OutputStates.Normal);
									break;
								}
							}
							else
							{
								//FIX
								Main.Bot.WriteToOutput(string.Format("Donating {0} {1}s...", troop.MaxDonationsPerRequest, troop.Troop.Name()), GlobalVariables.OutputStates.Verified);

								Tools.CoCHelper.Click(donateBtn);
								ClickablePoint barb = new ClickablePoint(donateBtn.Point.X + 108, donateBtn.Point.Y - 58);
								Tools.CoCHelper.Click(barb, troop.MaxDonationsPerRequest);

								if (!showMore.IsEmpty)
								{
									Tools.CoCHelper.Click(showMore);
									Thread.Sleep(500);
									showMore = GetNotificationButton();
								}
							}

						} while (!showMore.IsEmpty);
					}
				}
			}
			else
				Main.Bot.WriteToOutput("No clan members to donate to...", GlobalVariables.OutputStates.Normal);

			Tools.CoCHelper.Click(ScreenData.CloseChat);
		}

		public static ClickablePoint GetNotificationButton()
		{
			int left = ScreenData.ChatNotificationButtonArea.Left;
			int top = ScreenData.ChatNotificationButtonArea.Top;
			int right = ScreenData.ChatNotificationButtonArea.Right;
			int bottom = ScreenData.ChatNotificationButtonArea.Bottom;
			int count = 0;

			do
			{
				DetectableArea area = new DetectableArea(left, top, right, bottom, ScreenData.ChatNotificationButtonArea.Color, ScreenData.ChatNotificationButtonArea.ShadeVariation);
				ClickablePoint p1 = Tools.CoCHelper.SearchPixelInRect(area);

				if (!p1.IsEmpty && !(p1.Point.X == -1 || p1.Point.Y == -1))
				{
					return p1;
				}

				if (count >= 5)
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

		public static ClickablePoint GetDonateButton()
		{
			int left = ScreenData.ChatArea.Left;
			int top = ScreenData.ChatArea.Top;
			int right = ScreenData.ChatArea.Right;
			int bottom = ScreenData.ChatArea.Bottom;
			int count = 0;

			do
			{
				DetectableArea area = new DetectableArea(left, top, right, bottom, ScreenData.ChatArea.Color, ScreenData.ChatArea.ShadeVariation);
				ClickablePoint p1 = Tools.CoCHelper.SearchPixelInRect(area);

				if (!p1.IsEmpty && !(p1.Point.X == -1 || p1.Point.Y == -1))
				{
					if (Tools.CoCHelper.IsInColorRange(new ClickablePoint(p1.Point.X + ScreenData.DonateButtonColor1.Point.X, p1.Point.Y + ScreenData.DonateButtonColor1.Point.Y), ScreenData.DonateButtonColor1.Color, ScreenData.DonateButtonColor1.ShadeVariation))
					{
						if (Tools.CoCHelper.IsInColorRange(new ClickablePoint(p1.Point.X + ScreenData.DonateButtonColor2.Point.X, p1.Point.Y + ScreenData.DonateButtonColor2.Point.Y), ScreenData.DonateButtonColor2.Color, ScreenData.DonateButtonColor2.ShadeVariation))
						{
							if (Tools.CoCHelper.IsInColorRange(new ClickablePoint(p1.Point.X + ScreenData.DonateButtonColor3.Point.X, p1.Point.Y + ScreenData.DonateButtonColor3.Point.Y), ScreenData.DonateButtonColor3.Color, ScreenData.DonateButtonColor3.ShadeVariation))
							{
								return p1;
							}
						}
					}
				}

				if (count >= 100)
				{
					break;
				}
				else
				{
					if (!p1.IsEmpty && !(p1.Point.X == -1 || p1.Point.Y == -1))
					{
						top = p1.Point.Y;
					}

					count++;
				}
			} while (true);

			return new ClickablePoint();
		}

		#endregion Donate

		#region Request

		public static void RequestTroops()
		{
			if (Main.Bot.IsRequestTroops)
			{
				ClickablePoint ccPos = new ClickablePoint(356, 432); //(ClickablePoint)Main.Bot.LocationClanCastle; 

				if (ccPos.IsEmpty)
				{
					Main.Bot.LocateClanCastle();
				}

				Main.Bot.WriteToOutput("Requesting for Clan Castle Troops...");
				Tools.CoCHelper.Click(ccPos);
				Thread.Sleep(500);

				ClickablePoint requestTroop = GetRequestTroopsButton();

				if (!requestTroop.IsEmpty)
				{
					Tools.CoCHelper.Click(requestTroop);
					Thread.Sleep(2000);

					// FF, this only works if forceCapture is Enabled. I modified the CheckPixelColor method slightly until the problem is fixed. => Please check this again, should be fixed
					if (Tools.CoCHelper.CheckPixelColor(ScreenData.RequestTroopsRedCancel))
					{
						if (!string.IsNullOrEmpty(Main.Bot.RequestTroopsMessage))
						{
							Tools.CoCHelper.Click(ScreenData.RequestTroopsText);
							Thread.Sleep(300);
							KeyboardHelper.SendToBS(Main.Bot.RequestTroopsMessage); // TODO: Sending Keystrokes to BlueStacks does not work! Tags: Ph!d, FastFrench
						}
						Thread.Sleep(1000);
						Tools.CoCHelper.Click(ScreenData.RequestTroopsGreenSend);

						Main.Bot.WriteToOutput("Request successfully made...");
					}
					else
					{
						Main.Bot.WriteToOutput("Request's already been made...");
						Tools.CoCHelper.Click(ScreenData.TopLeftClient, 2, 50);
					}
				}
				else
				{
					Main.Bot.WriteToOutput("Clan Castle not available...");
				}
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

				if (!p1.IsEmpty && !(p1.Point.X == -1 || p1.Point.Y == -1))
				{
					if (Tools.CoCHelper.IsInColorRange(new ClickablePoint(p1.Point.X + ScreenData.RequestTroopsButton2.Point.X, p1.Point.Y + ScreenData.RequestTroopsButton2.Point.Y), ScreenData.RequestTroopsButton2.Color, ScreenData.RequestTroopsButton2.ShadeVariation))
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

		#endregion Request
	}
}
