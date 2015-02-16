using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Data;

namespace CoC.Bot.BotEngine.Train
{
	public class TroopToTrainInBarracks : List<TroopToTrainInBarrack>
	{
		public bool AddBarrack(BuildingPointModel model)
		{
			if (model == null || model.Coordinates.IsEmptyOrZero || (model.BuildingType != BuildingType.Barrack && model.BuildingType != BuildingType.DarkBarrack)) return false;
			Add(new TroopToTrainInBarrack(model));
			return true;
		}

		public bool AddTroop(Troop troop, int nb = 1)
		{
			bool dark = troop.IsDark();
			for (int i = 0; i < nb; i++)
			{
				TroopToTrainInBarrack best = this.Where(tttb => ((dark && tttb.IsDark) || (!dark && tttb.IsNormal))).OrderBy(tb => tb.CumulTime).FirstOrDefault();
				if (best == null)
				{
					Main.Bot.WriteToOutput("No barrack found to train " + troop.Name(), GlobalVariables.OutputStates.Error);
					return false;
				}
				best.Push(troop);
			}
			return true;
		}

		/// <summary>
		/// Get total training time, in seconds
		/// </summary>
		public int getTotalTrainingTime
		{
			get
			{
				if (this.Count == 0) return 0;
				int max = this.Select(tb=>tb.CumulTime).Max();
				return max;
			}
		}
	}
}
