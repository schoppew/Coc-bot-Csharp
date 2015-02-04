namespace CoC.Bot.Functions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;

    class Attack
    {
		public static void AttackMain()
		{
			Search.PrepareSearch();
			Thread.Sleep(1000);
			Search.VillageSearch(); // DO THIS
			Thread.Sleep(1000);
			PrepareAttack(); // DO THIS
			Thread.Sleep(1000);
			Start(); // DO THIS
			Thread.Sleep(1000);
			MainScreen.ReturnHome();
			Thread.Sleep(1000);
		}

        // THIS METHOD IS WRONG, MUST CREATE PROPERTY FOR BARCHING, ALL TROOPS, ETC
        public static void Start()
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
