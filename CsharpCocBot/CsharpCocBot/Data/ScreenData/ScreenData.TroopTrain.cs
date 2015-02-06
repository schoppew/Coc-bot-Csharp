using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Functions;

namespace CoC.Bot.Data
{
    public static partial class ScreenData
    {
		public static ClickablePoint GetTrainPos(Data.Troop troopKind)
		{
			switch (troopKind)
			{
				case Data.Troop.Barbarian:
					return new ClickablePoint(224, 323);
				case Data.Troop.Archer:
					return new ClickablePoint(337, 323);
				case Data.Troop.Giant:
					return new ClickablePoint(438, 366);
				case Data.Troop.Goblin:
					return new ClickablePoint(548, 366);
				case Data.Troop.WallBreaker:
					return new ClickablePoint(650, 366);
				case Data.Troop.Balloon:
					return new ClickablePoint(218, 438);
				case Data.Troop.Wizard:
					return new ClickablePoint(326, 438);
				case Data.Troop.Healer:
					return new ClickablePoint(434, 438);
				case Data.Troop.Dragon:
					return new ClickablePoint(536, 438);
				case Data.Troop.Pekka:
					return new ClickablePoint(646, 438);
				case Data.Troop.Minion:
					return new ClickablePoint(224, 323); // THESE
				case Data.Troop.HogRider:
					return new ClickablePoint(337, 323); // MAY
				case Data.Troop.Valkyrie:
					return new ClickablePoint(438, 366); // BE
				case Data.Troop.Golem:
					return new ClickablePoint(548, 366); // WRONG
				case Data.Troop.Witch:
					return new ClickablePoint(650, 366); //
				case Data.Troop.LavaHound:
					return new ClickablePoint(218, 438); //-----------
				default:
                    return new ClickablePoint();
			}
		}
    }
}
