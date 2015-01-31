namespace CoC.Bot.Data
{
    using System;

    /// <summary>
    /// The Attack Mode
    /// </summary>
    [Flags]
    public enum Troop
    {
        Barbarian,  // Tier 1
        Archer,
        Goblin,

        Giant,      // Tier 2
        Wallbreaker,
        Balloon,
        Wizard,

        Healer,     // Tier 3
        Dragon,
        Pekka,

        Minion,     // Dark Elixir
        HogRider,
        Valkyrie,
        Golem,
        Witch,
        LavaHound,

        King,       // Heroes
        Queen
    };
}