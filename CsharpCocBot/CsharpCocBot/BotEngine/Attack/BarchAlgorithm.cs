using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoC.Bot.Data;

namespace CoC.Bot.BotEngine
{
    class BarchAlgorithm
    {
        public static void Start(int divisor)
        {
            int numArchersPerSide, numBarbsPerSide, waveCount = 2;

            foreach (var i in Attack.troopDict)
            {
                if (i.Key.Equals(Troop.Barbarian))
                    numBarbsPerSide = i.Value / divisor;
                if (i.Key.Equals(Troop.Barbarian))
                    numArchersPerSide = i.Value / divisor;
            }

            // Dropping Barbs
            for (int i = 0; i < waveCount; i++)
            {
                
            }
        }
    }
}
