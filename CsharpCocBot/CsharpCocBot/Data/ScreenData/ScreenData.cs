using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.Bot.Data
{
    /// <summary>
    /// This is where you store all screen informations: clickable points with associated color(s)
    /// ** PLEASE PUT ALL SCREEN POSITIONS AND COLORS TO DETECT HERE **
    /// </summary>
    static public partial class ScreenData
    {
        /*
         */
        public static DetectablePoint BarbarianSlotGrey = new DetectablePoint(194, 315, 0xB3B3B3, 6);
        public static DetectablePoint TrainTroopsButton2 = new DetectablePoint(0, 10, 0xFFFFFF, 4);
        public static DetectableArea TrainTroopsButton = new DetectableArea(196, 558, 665, 643, Color.FromArgb(67, 38, 3), 4);
        public static DetectablePoint RequestTroopsButton2 = new DetectablePoint(0, 10, 0x27291D, 4);
        public static DetectableArea RequestTroopsButton = new DetectableArea(196, 558, 665, 643, Color.FromArgb(185, 54, 48), 4);
        public static ClickablePoint DropSingleBarb = new ClickablePoint(34, 310);
        public static DetectablePoint ArmyFullNotif = new DetectablePoint(121, 149, 0xD4535E, 6);

        public static DetectablePoint IsMain = new DetectablePoint(284, 28, 0x41B1CD, 20);
        public static DetectablePoint IsMainGrayed = new DetectablePoint(IsMain, 0x215B69, 20);
        public static ClickablePoint TopLeftClient = new ClickablePoint(1, 1);
        public static DetectablePoint IsInactive = new DetectablePoint(458, 311, 0x33B5E5, 20);
        public static ClickablePoint ReloadButton = new ClickablePoint(416, 399);
        public static DetectablePoint Attacked = new DetectablePoint(235, 209, 0x9E3826, 20);
        public static ClickablePoint AttackedBtn = new ClickablePoint(429, 493);
        public static DetectablePoint SomeXCancelBtn = new DetectablePoint(819, 55, 0xD80400, 20);
        public static DetectablePoint CancelFight = new DetectablePoint(822, 48, 0xD80408, 20);
        public static DetectablePoint CancelFight2 = new DetectablePoint(830, 59, 0xD80408, 20);

        public static DetectableArea Inactivity = new DetectableArea(457, 300, 458, 330, Color.FromArgb(51, 181, 229), 10);
        public static ClickablePoint AttaqueButton = new ClickablePoint(60, 614);
        public static ClickablePoint MatchButton = new ClickablePoint(217, 510);
        public static DetectablePoint HasShield = new DetectablePoint(513, 416, 0x5DAC10, 50);
        public static ClickablePoint BreakShield = new ClickablePoint(513, 416);
        public static ClickablePoint SurrenderButton = new ClickablePoint(62, 519);
        public static ClickablePoint ConfirmSurrender = new ClickablePoint(512, 394);
        public static ClickablePoint CancelFightBtn = new ClickablePoint(822, 48);
        public static DetectablePoint EndFightScene = new DetectablePoint(429, 519, 0xB8E35F, 20); //Victory or defeat scene
        public static ClickablePoint ReturnHome = new ClickablePoint(428, 544);
        public static DetectablePoint CloseChat = new DetectablePoint(330, 334, 0xF0A03C, 20);

        public static DetectablePoint EndBattleBtn = new DetectablePoint(71, 530, 0xC00000, 20);
        public static DetectablePoint HasClanMessage = new DetectablePoint(31, 313, 0xF80B09, 20);
        public static ClickablePoint OpenChatBtn = new ClickablePoint(10, 334);
        public static DetectablePoint IsClanTabSelected = new DetectablePoint(204, 20, 0x6F6C4F, 20);
        public static DetectablePoint IsClanMessage = new DetectablePoint(26, 320, 0xE70400, 20);

        public static ClickablePoint ClanRequestTextArea = new ClickablePoint(430, 140);
        public static ClickablePoint ConfirmClanTroopsRequest = new ClickablePoint(524, 228);
        public static DetectablePoint CampFull = new DetectablePoint(328, 535, 0xD03840, 20);

        public static ClickablePoint DropTrophiesStartPoint = new ClickablePoint(34, 310);
        public static DetectablePoint TrainBtn = new DetectablePoint(541, 602, 0x728BB0, 20);
        public static DetectablePoint TrainBarbarian = new DetectablePoint(261, 366, 0x39D8E0, 20);
        public static DetectablePoint TrainArcher = new DetectablePoint(369, 366, 0x39D8E0, 20);
        public static DetectablePoint TrainGiant = new DetectablePoint(475, 366, 0x3DD8E0, 20);
        public static DetectablePoint TrainGoblin = new DetectablePoint(581, 366, 0x39D8E0, 20);
        public static DetectablePoint TrainWallbreaker = new DetectablePoint(688, 366, 0x3AD8E0, 20);

        public static ClickablePoint NextBtn = new ClickablePoint(750, 500);

        public static ClickablePoint GetTrainPos(int troopKind)
        {
            switch ((Data.Troop)troopKind)
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
            } while (true);

            return new ClickablePoint();
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
            } while (true);

            return new ClickablePoint();
        }

        // ; Someone asking troupes : Color 0xD0E978 in x = 121


    }
}
