using System;

namespace CoC.Bot.Data
{  
  /// <summary>
  /// The Attack Mode
  /// </summary>  
  public enum Troop
  {
    Barbarian,  // Tier 1
    Archer,
    Goblin,

    Giant,      // Tier 2
    WallBreaker,
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