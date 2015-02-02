namespace CoC.Bot.Data
{
    using System;

    /// <summary>
    /// The Troop
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
        Queen = 22
    };
}