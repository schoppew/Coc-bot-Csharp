using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace CoC.Bot.Functions
{
    class Attack
    {
        public static void AttackMain()
        {
            Functions.Search.PrepareSearch();
            Thread.Sleep(1000);
            Functions.Search.VillageSearch(); // DO THIS
            Thread.Sleep(1000);
            PrepareAttack(); // DO THIS
            Thread.Sleep(1000);
            Attack(); // DO THIS
            Thread.Sleep(1000);
            Functions.MainScreen.ReturnHome();
            Thread.Sleep(1000);
        }

        // THIS METHOD IS WRONG, MUST CREATE PROPERTY FOR BARCHING, ALL TROOPS, ETC
        public static void Attack()
        {
            Main.Bot.WriteToOutput("====== Beginning Attack ======", GlobalVariables.OutputStates.Verified);
            
            switch (Main.Bot.SelectedAttackMode)
            {
                case Data.AttackMode.AllBases:
                    break;
                case Data.AttackMode.DeadBases:
                    break;
                case Data.AttackMode.WeakBases:
                    break;
                default:
                {
                    Main.Bot.WriteToOutput("Invalid attack mode...", GlobalVariables.OutputStates.Warning);
                    break;  
                }
            }
        
        }

        public static void DropClanCastle()
        {

        }

        public static void DropHeroes()
        {

        }

        public static bool GoldElixirChange()
        {
            return false;
        }

        public static void PrepareAttack()
        {

        }
    }
}
