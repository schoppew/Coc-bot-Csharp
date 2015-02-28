using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Windows.Media.Animation;
using CoC.Bot.Data;
using Point = Win32.POINT;
namespace CoC.Bot.BotEngine
{
    class Search
    {
        public static string Gold;
        public static string Elixir;
        public static string DarkElixir;

        public bool CheckNextButton()
        {
            return false;
        }

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
            DarkElixir = "";

            Main.Bot.WriteToOutput("[G]: " + Gold + "; [E]: " + Elixir + "; [DE]: " + DarkElixir + ";");
        }

        public static void PrepareSearch()
        {
            Tools.CoCHelper.Click(ScreenData.AttackButton);
            Thread.Sleep(1000);
            Tools.CoCHelper.Click(ScreenData.MatchButton);
            Thread.Sleep(3000);

            if (Tools.CoCHelper.CheckPixelColor(ScreenData.HasShield))
            {
                Tools.CoCHelper.Click(ScreenData.BreakShield);
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
                while (!Tools.CoCHelper.CheckPixelColor(ScreenData.NextBtn))
                {
                    if (timeout >= 20) // After 10 seconds
                    {
                        return false;
                    }

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
                        Tools.CoCHelper.Click(ScreenData.NextBtn);
                    }
                    else if (Main.Bot.SelectedAttackMode.Equals(AttackMode.WeakBases))
                    {
                        if (CheckWeakBase())
                        {
                            Main.Bot.WriteToOutput("~~~~~~~ Weak Base Found! ~~~~~~~");
                            return true;
                        }

                        Main.Bot.WriteToOutput("~~~~~~~ Not Weak Base, Skipping ~~~~~~~");
                        Tools.CoCHelper.Click(ScreenData.NextBtn);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    Tools.CoCHelper.Click(ScreenData.NextBtn);
                    Thread.Sleep(500);
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
