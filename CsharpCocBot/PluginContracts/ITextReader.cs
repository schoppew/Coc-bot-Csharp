using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.Bot.Plugin
{
	// All Coc plugins that detects the texts should handle this
	public interface ITextReader
	{
		int? TargetElixirQuantity { get; }
		int? OwnElixirQuantity { get; }
		int? TargetGoldQuantity { get; }
		int? OwnGoldQuantity { get; }
		int? TargetDarkElixirQuantity { get; }
		int? OwnDarkElixirQuantity { get; }
		string GetTroopsReadyCount(Troop troop);
	}
}
