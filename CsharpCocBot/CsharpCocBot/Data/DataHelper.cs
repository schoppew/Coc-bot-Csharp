namespace CoC.Bot.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Extention methods for Data.
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Determines whether the specified troop is Elixir.
        /// </summary>
        /// <param name="troop">The troop.</param>
        /// <returns><c>true</c> if the specified troop is Elixir; otherwise, <c>false</c>.</returns>
        public static bool IsNormal(this Troop troop)
        {
            return troop <= Troop.Pekka && troop >= Troop.Barbarian;
        }

        /// <summary>
        /// Determines whether the specified troop is Dark Elixir.
        /// </summary>
        /// <param name="troop">The troop.</param>
        /// <returns><c>true</c> if the specified troop is Dark Elixir; otherwise, <c>false</c>.</returns>
        public static bool IsBlack(this Troop troop)
        {
            return troop <= Troop.LavaHound && troop >= Troop.Minion;
        }

        /// <summary>
        /// Determines whether the specified troop is a hero.
        /// </summary>
        /// <param name="troop">The troop.</param>
        /// <returns><c>true</c> if the specified troop is a hero; otherwise, <c>false</c>.</returns>
        public static bool IsHero(this Troop troop)
        {
            return troop == Troop.King || troop == Troop.Queen;
        }

        /// <summary>
        /// Determines whether the Troop is a specified Troop Type.
        /// </summary>
        /// <param name="troop">The troop.</param>
        /// <param name="type">The troop type.</param>
        /// <returns><c>true</c> if the troop is the specified troop type; otherwise, <c>false</c>.</returns>
        public static bool IsTroopType(this Troop troop, TroopType type)
        {
            switch (type)
            {
                case TroopType.Tier1:
                    return troop <= Troop.Goblin && troop >= Troop.Barbarian;
                case TroopType.Tier2:
                    return troop <= Troop.Wizard && troop >= Troop.Giant;
                case TroopType.Tier3:
                    return troop <= Troop.Pekka && troop >= Troop.Healer;
                case TroopType.DarkTroops:
                    return troop <= Troop.LavaHound && troop >= Troop.Minion;
                case TroopType.Heroes:
                    return troop == Troop.King || troop == Troop.Queen;
                default:
                    return false; // Unknown
            }
        }

        public static int CampSlots(this Troop troop)
        {
            switch (troop)
            {
                case Troop.King:
                case Troop.Queen:
                    return 0; // Heroes take no place
                case Troop.Barbarian:
                case Troop.Archer:
                case Troop.Goblin:
                    return 1;
                case Troop.WallBreaker:
                case Troop.Minion:
                    return 2;
                case Troop.Wizard:
                    return 4;
                case Troop.Giant:
                case Troop.Balloon:
                case Troop.HogRider:
                    return 5;
                case Troop.Valkyrie:
                    return 8;
                case Troop.Witch:
                    return 12;
                case Troop.Dragon:
                    return 20;
                case Troop.Pekka:
                    return 25;
                case Troop.Golem:
                case Troop.LavaHound:
                    return 30;
                default:
                    Debug.Assert(false, string.Format("Oops... one is missing ({0})?", troop.Name()));
                    return 1;
            }
        }

        public static IEnumerable<Troop> EnumTroops(bool ElixirTroop, bool BlackTroops, bool Heroes)
        {
            foreach (Troop troop in Enum.GetValues(typeof(Troop)))
                if (ElixirTroop && troop.IsNormal() || BlackTroops && troop.IsBlack() || Heroes && troop.IsHero())
                    yield return troop;
        }
        public static IEnumerable<string> EnumTroopNames(bool ElixirTroop, bool BlackTroops, bool Heroes)
        {
            foreach (Troop troop in EnumTroops(ElixirTroop, BlackTroops, Heroes))
                yield return troop.Name();
        }

        /// <summary>
        /// Returns the specified troop name.
        /// </summary>
        /// <param name="troop">The troop.</param>
        /// <returns>System.String.</returns>
        public static string Name(this Troop troop)
        {
            switch (troop)
            {
                case Troop.WallBreaker: return Properties.Resources.WallBreaker;
                case Troop.HogRider: return Properties.Resources.HogRider;
                case Troop.Pekka: return Properties.Resources.Pekka;
                case Troop.LavaHound: return Properties.Resources.LavaHound;
                default:
                    return Enum.GetName(typeof(Troop), troop);
            }
        }

        /// <summary>
        /// Returns the specified troop type name.
        /// </summary>
        /// <param name="type">The troop type.</param>
        /// <returns>System.String.</returns>
        public static string Name(this TroopType type)
        {
            switch (type)
            {
                case TroopType.Tier1: return Properties.Resources.Tier1;
                case TroopType.Tier2: return Properties.Resources.Tier2;
                case TroopType.Tier3: return Properties.Resources.Tier3;
                case TroopType.DarkTroops: return Properties.Resources.DarkTroops;
                default:
                    return Enum.GetName(typeof(TroopType), type);
            }
        }

        /// <summary>
        /// Returns the specified troop composition name.
        /// </summary>
        /// <param name="composition">The troop composition.</param>
        /// <returns>System.String.</returns>
        public static string Name(this TroopComposition composition)
        {
            switch (composition)
            {
                case TroopComposition.UseBarracks: return Properties.Resources.UseBarracks;
                case TroopComposition.Barching: return Properties.Resources.Barching;
                case TroopComposition.CustomTroops: return Properties.Resources.CustomTroops;
                default:
                    return Enum.GetName(typeof(TroopType), composition);
            }
        }

        /// <summary>
        /// Returns the specified deploy strategy name.
        /// </summary>
        /// <param name="strategy">The deploy strategy.</param>
        /// <returns>System.String.</returns>
        public static string Name(this DeployStrategy strategy)
        {
            switch (strategy)
            {
                case DeployStrategy.TwoSides: return Properties.Resources.DeployStrategyTwoSides;
                case DeployStrategy.ThreeSides: return Properties.Resources.DeployStrategyThreeSides;
                case DeployStrategy.FourSides: return Properties.Resources.DeployStrategyFourSides;
                default:
                    return Enum.GetName(typeof(DeployStrategy), strategy);
            }
        }

        /// <summary>
        /// Returns the specified deploy troop name.
        /// </summary>
        /// <param name="deploy">The deploy troop.</param>
        /// <returns>System.String.</returns>
        public static string Name(this DeployTroop deploy)
        {
            switch (deploy)
            {
                case DeployTroop.BarbariansAndArchers: return Properties.Resources.DeployTroopsBarbariansAndArchers;
                case DeployTroop.UseAllTroops: return Properties.Resources.DeployTroopsUseAllTroops;
                default:
                    return Enum.GetName(typeof(DeployTroop), deploy);
            }
        }
    }
}