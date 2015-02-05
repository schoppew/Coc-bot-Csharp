namespace CoC.Bot.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    using Tools;
    using ViewModels;
	using CoC.Bot.Data;

    internal class MainScreen
    {
        public static void CheckMainScreen()
        {
			Main.Bot.WriteToOutput(Properties.Resources.OutputTryingToLocateMainScreen, GlobalVariables.OutputStates.Information);

            while (!Tools.CoCHelper.CheckPixelColor(ScreenData.IsMain))
            {
                Thread.Sleep(1000);

                if (!CheckObstacles())
                {
                    Data.ClickablePoint appPos = new Data.ClickablePoint(GetAppPos());
                    Tools.CoCHelper.Click(appPos, 1);
                }

                WaitForMainScreen();
            }

			Main.Bot.WriteToOutput("Main Screen Located", GlobalVariables.OutputStates.Information);
        }

        public static Point GetAppPos()
        {
            Point p1 = Tools.FastFind.FastFindHelper.FullScreenPixelSearch(Color.FromArgb(87, 16, 1), 10, true);
            
            if (Tools.FastFind.FastFindHelper.IsInColorRange(new Point(p1.X + 10, p1.Y + 10), Color.FromArgb(169, 90, 46), 10))
            {
                if (Tools.FastFind.FastFindHelper.IsInColorRange(new Point(p1.X - 10, p1.Y - 10), Color.FromArgb(237, 218, 165), 10))
                {
                    return p1;
                }
            }

            return new Point(-1, -1);
        }

        public static void ZoomOut()
        {
            Main.Bot.WriteToOutput("Zooming Out", GlobalVariables.OutputStates.Normal);

            for (int i = 0; i < 15; i++)
            {
                Thread.Sleep(600);
                KeyboardHelper.SendVirtualKeyToBS(KeyboardHelper.VirtualKeys.VK_DOWN);
            }

            Main.Bot.WriteToOutput("Zoomed Out", GlobalVariables.OutputStates.Normal);
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