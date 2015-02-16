using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Data;

namespace CoC.Bot.BotEngine.Train
{
	public class TrainingElement
	{
		public TrainingElement(Troop troop, int nb = 1)
		{
			this.Troop = troop;
			TrainingStarted = 0;
			TotalCount = nb;
		}
		public Troop Troop { get; private set; }
		public int TrainingStarted { get; set; }
		public int TotalCount { get; set; }
	}
}
