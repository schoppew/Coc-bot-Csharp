﻿namespace CoC.Bot.Plugin
{    
    /// <summary>
    /// The Troop (can be saftely casted into Coc.Bot.Data.Troop, should be synchronized)
    /// </summary>
    public enum Troop
    {
        Barbarian = 1,  // Tier 1
        Archer = 2,
        Goblin = 3,

        Giant = 4,      // Tier 2
        WallBreaker = 5,
        Balloon = 6,
        Wizard = 7,

        Healer = 8,     // Tier 3
        Dragon = 9,
        Pekka = 10,

        Minion = 11,    // Dark Elixir
        HogRider = 12,
        Valkyrie = 13,
        Golem = 14,
        Witch = 15,
        LavaHound = 16,

        King = 21,      // Heroes
        Queen = 22,


		CastleClan = -1, // Special "troops" (can be dropped, and trained for the spells). 
		SpellLightning = -2,
		SpellHeal = -3,
		SpellRage = -4,
		SpellJump = -5,
		SpellIce = -6,

		None = 0

    };
}