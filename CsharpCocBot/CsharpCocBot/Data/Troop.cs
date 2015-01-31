using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.Bot.Data
{
  /// <summary>
  /// The Attack Mode
  /// </summary>
  public enum Troop
  {
    Barbarian, // Normal troops (Elixir)
    Archer,
    Giant,
    Goblin,
    Wallbreaker,
    Balloon,
    Wizard,
    Healer,
    Dragon,
    Pekka,

    Minion,   // Dark elixir troops
    Hogrider,
    Valkyrie,
    Golem,
    Witch,
    Lavahound,

    King,   // Heroes
    Queen
  };
  
  static public class TroopHelper // Extention methods for troops
  {
    public static bool IsNormal(this Troop troop)
    {
      return troop <= Troop.Pekka && troop >= Troop.Barbarian;        
    }

    public static bool IsBlack(this Troop troop)
    {
      return troop <= Troop.Lavahound && troop >= Troop.Minion;
    }

    public static bool IsHero(this Troop troop)
    {
      return troop == Troop.King || troop == Troop.Queen;
    }

    public static int CampSlots(this Troop troop)
    {
      switch(troop)
      {
        case Troop.King:
        case Troop.Queen:
          return 0; // Heroes take no place
        case Troop.Barbarian:
        case Troop.Archer:
        case Troop.Goblin:
          return 1;
        case Troop.Wallbreaker:
        case Troop.Minion:
          return 2;
        case Troop.Wizard:
          return 4;
        case Troop.Giant:
        case Troop.Balloon:
        case Troop.Hogrider:
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
        case Troop.Lavahound:
          return 30;
        default:
          Debug.Assert(false, "Oops... one is missing ("+troop.Name()+")?" );
          return 1;
      }
    }

    public static string Name(this Troop troop)
    {       
      switch(troop)
      {
        case Troop.Wallbreaker: return "Wall Breaker";
        case Troop.Hogrider: return "Hog Rider";          
        case Troop.Pekka: return "P.E.K.K.A.";          
        case Troop.Lavahound: return "Lava Hound";
        default:
          return Enum.GetName(typeof(Troop), troop);          
      }
    }

    static public IEnumerable<Troop> EnumTroops(bool ElixirTroop, bool BlackTroops, bool Heroes)
    {
      foreach (Troop troop in Enum.GetValues(typeof(Troop)))
        if (ElixirTroop && troop.IsNormal() ||
            BlackTroops && troop.IsBlack() ||
            Heroes && troop.IsHero())
          yield return troop;
    }

    static public IEnumerable<string> EnumTroopNames(bool ElixirTroop, bool BlackTroops, bool Heroes)
    {
      foreach (Troop troop in EnumTroops(ElixirTroop, BlackTroops, Heroes))
          yield return troop.Name();
    }
  }
}