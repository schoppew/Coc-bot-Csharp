using System;
using System.Threading;
using CoC.Bot.Data;
using CoC.Bot.Tools;
using Point = Win32.POINT;
namespace CoC.Bot.BotEngine
{
    class Search
    {
        public static string Gold;
        public static string Elixir;
        public static string DarkElixir;

        public static bool CompareResources()
        {
            bool goalMet = true;

            GetResources();

            try
            {
                if (Main.Bot.IsMeetGold && !(int.Parse(Gold) >= Main.Bot.MinimumGold))
                    goalMet = false;

                if (Main.Bot.IsMeetElixir && !(int.Parse(Elixir) >= Main.Bot.MinimumElixir))
                    goalMet = false;

                if (Main.Bot.IsMeetDarkElixir && !(int.Parse(DarkElixir) >= Main.Bot.MinimumDarkElixir))
                    goalMet = false;
            }
            catch (Exception)
            {
                return false;
            }

            return goalMet;
        }

        public static void GetResources()
        {
            Gold = ReadText.GetGold(51, 66);
            Elixir = ReadText.GetElixir(51, 95);
            DarkElixir = ReadText.GetDarkElixir(51, 123);

            Main.Bot.WriteToOutput("[G]: " + Gold + "; [E]: " + Elixir + "; [DE]: " + DarkElixir + ";");
        }

        public static void PrepareSearch()
        {
            Main.Bot.WriteToOutput("Preparing Search...");

            CoCHelper.Click(ScreenData.AttackButton);
            Thread.Sleep(1000);
            CoCHelper.Click(ScreenData.MatchButton);
            Thread.Sleep(3000);

            if (CoCHelper.CheckPixelColor(ScreenData.HasShield))
            {
                CoCHelper.Click(ScreenData.BreakShield);
            }
        }

        public static bool VillageSearch()
        {
            switch (Main.Bot.SelectedAttackMode)
            {
                case AttackMode.AllBases:
                    Main.Bot.WriteToOutput("============Searching For All Bases============", GlobalVariables.OutputStates.Information);
                    break;
                case AttackMode.DeadBases:
                    Main.Bot.WriteToOutput("============Searching For Dead Bases============", GlobalVariables.OutputStates.Information);
                    break;
                case AttackMode.WeakBases:
                    Main.Bot.WriteToOutput("============Searching For Weak Bases============", GlobalVariables.OutputStates.Information);
                    break;
            }

            Main.Bot.WriteToOutput("~Gold: " + Main.Bot.MinimumGold + "; Elixir: " + Main.Bot.MinimumElixir +"; Dark Elixir: " + Main.Bot.MinimumDarkElixir + "; Trophies: " + Main.Bot.MinimumTrophyCount + ";");

            while (true)
            {
                var timeout = 0;
                while (!CoCHelper.CheckPixelColor(ScreenData.NextBtn))
                {
                    if (timeout >= 20) // After 10 seconds
                        return false;

                    timeout++;
                    Thread.Sleep(500);
                }

                if (CompareResources())
                {
                    if (Main.Bot.SelectedAttackMode.Equals(AttackMode.DeadBases))
                    {
                        if (CheckDeadBase())
                        {
                            Main.Bot.WriteToOutput("~~~~~~~ Dead Base Found! ~~~~~~~");
                            return true;
                        }

                        Main.Bot.WriteToOutput("~~~~~~~ Not Dead Base, Skipping ~~~~~~~");
                        CoCHelper.Click(ScreenData.NextBtn);
                    }
                    else if (Main.Bot.SelectedAttackMode.Equals(AttackMode.WeakBases))
                    {
                        if (CheckWeakBase())
                        {
                            Main.Bot.WriteToOutput("~~~~~~~ Weak Base Found! ~~~~~~~");
                            return true;
                        }

                        Main.Bot.WriteToOutput("~~~~~~~ Not Weak Base, Skipping ~~~~~~~");
                        CoCHelper.Click(ScreenData.NextBtn);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    CoCHelper.Click(ScreenData.NextBtn);
                    Thread.Sleep(1000);
                }
            }
        }

        public static bool CheckDeadBase()
        {
            return true;
        }

        public static bool CheckWeakBase()
        {
            return true;
        }


    }
}
