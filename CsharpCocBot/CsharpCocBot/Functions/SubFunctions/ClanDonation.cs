using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Data;

namespace CoC.Bot.Functions
{
	public static class ClanDonation
	{
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

	}
}
