namespace CoC.Bot.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;
    using System.Threading;

    using Tools;
    using ViewModels;
	using CoC.Bot.Data;

    internal class MainScreen
    {
        public static void CheckMainScreen()
        {
			Main.Bot.WriteToOutput(Properties.Resources.OutputTryingToLocateMainScreen, GlobalVariables.OutputStates.Information);

            while (!Tools.CoCHelper.CheckPixelColor(ScreenData.IsMain)) // FIX VARIATION
            {
                Thread.Sleep(1000);

                if (!CheckObstacles())
                {
                    Tools.CoCHelper.ClickBad(new Point(126, 700), 1);
//TODO:             OPEN APP AGAIN
                }

                WaitForMainScreen();
            }

			Main.Bot.WriteToOutput("Main Screen Located", GlobalVariables.OutputStates.Information);
        }

        public static void ZoomOut()
        {
            for (int x = 0; x < 5; x++)
            {
                KeyboardHelper.SendVirtualKeyToBS(KeyboardHelper.VirtualKeys.VK_DOWN);
            }
        }

        public static void WaitForMainScreen()
        {
			Main.Bot.WriteToOutput("Waiting for Main Screen");
            for (int i = 0; i < 150; i++)
            {
				if (!Tools.CoCHelper.CheckPixelColor(ScreenData.IsMain))
                {
                    Thread.Sleep(2000);
                    if (CheckObstacles())
                        i = 0;
                }
                else
                    return;
            }

			//Other.SetLog("Unable to load Clash of Clans, Restarting...", Color.Red); // TODO: will add colours later
			Main.Bot.WriteToOutput("Unable to load Clash of Clans, Restarting...");
//TODO:     OPEN APP AGAIN
            Thread.Sleep(10000);
        }

        public static bool CheckObstacles()
        {
			ClickablePoint messagePos = Tools.CoCHelper.SearchPixelInRect(ScreenData.Inactivity);
            if (!messagePos.IsEmpty)
            {
                Tools.CoCHelper.Click(ScreenData.ReloadButton);
                Thread.Sleep(7000);
                return true;
            }

            if (Tools.CoCHelper.CheckPixelColor(ScreenData.Attacked))
            {
                Tools.CoCHelper.Click(ScreenData.AttackedBtn);
                return true;
            }

			// The main screen
            if (Tools.CoCHelper.CheckPixelColor(ScreenData.IsMainGrayed))
            {
				Tools.CoCHelper.Click(ScreenData.TopLeftClient);
                return true;
            }

			// If we have a screen with a small x to cancel it, like when you start a fight. 
			if (Tools.CoCHelper.CheckPixelColor(ScreenData.SomeXCancelBtn))
            {
				Tools.CoCHelper.Click(ScreenData.SomeXCancelBtn);
                return true;
            }

			// If a fight is on going, than cancel it. 
			if (Tools.CoCHelper.CheckPixelColor(ScreenData.CancelFight) || Tools.CoCHelper.CheckPixelColor(ScreenData.CancelFight2))
            {
                Tools.CoCHelper.Click(ScreenData.CancelFight);
                return true;
            }

            if (Tools.CoCHelper.CheckPixelColorBad(new Point(331, 330), Color.FromArgb(240, 160, 59), 20))
            {
                Tools.CoCHelper.ClickBad(new Point(331, 330), 1);
                Thread.Sleep(1000);
                return true;
            }

            if (Tools.CoCHelper.CheckPixelColorBad(new Point(429, 519), Color.FromArgb(184, 227, 95), 20))
            {
                Tools.CoCHelper.ClickBad(new Point(429, 519), 1);
                return true;
            }

            if (Tools.CoCHelper.CheckPixelColorBad(new Point(71, 530), Color.FromArgb(192, 0, 0), 20))
            {
                Tools.CoCHelper.ClickBad(new Point(331, 330), 1);
                ReturnHome(false, false);
                return true;
            }
            
            return false;
        }

        public static void ReturnHome(bool takeSS = true, bool goldChangeCheck = true)
        {
            if(goldChangeCheck)
            {
//TODO:         CHECK KING AND QUEENS POWER
            }

//TODO:     SET KING AND QUEEN POWER TO FALSE
			Main.Bot.WriteToOutput("Returning Home...");

            Tools.CoCHelper.ClickBad(new Point(62, 519), 1);
            Thread.Sleep(500);
            Tools.CoCHelper.ClickBad(new Point(512, 394), 1);
            Thread.Sleep(2000);

            if(takeSS)
            {
				Main.Bot.WriteToOutput("Taking snapshot of your loot");
                
                DateTime now = DateTime.Now;
				string date = string.Format("{0}.{1}.{2}", now.Day, now.Month, now.Year);
				string time = string.Format("{0}.{1}", now.Hour, now.Minute);

                Tools.FastFind.FastFindHelper.TakeFullScreenCapture(true);
				Tools.FastFind.FastFindWrapper.SaveJPG(0, string.Format("{0}/{1} at {2}", GlobalVariables.LogPath, date, time), 100);
            }

            Thread.Sleep(2000);
            Tools.CoCHelper.ClickBad(new Point(428, 544), 1);

            int counter = 0;

            do
            {
                Thread.Sleep(2000);
                if (Tools.CoCHelper.CheckPixelColorBad(new Point(284, 28), Color.FromArgb(65, 177, 205), 20))
                {
                    Tools.CoCHelper.ClickBad(new Point(331, 330), 1);
//TODO:             _GUICtrlEdit_SetText($txtLog, "")
                    return;
                }

                counter++;

                if(counter >= 50)
                {
					Main.Bot.WriteToOutput("Cannot return home...");

                    CheckMainScreen();
                    return;
                }

            } while (true);
        }
    }
}