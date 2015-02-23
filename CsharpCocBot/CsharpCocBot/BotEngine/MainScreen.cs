using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

using CoC.Bot.Tools;
using CoC.Bot.ViewModels;
using CoC.Bot.Data;
using MouseAndKeyboard;
using Point = Win32.POINT;

namespace CoC.Bot.BotEngine
{
    
    internal class MainScreen
    {
        public static void CheckMainScreen()
        {
            Main.Bot.WriteToOutput(Properties.Resources.OutputTryingToLocateMainScreen, GlobalVariables.OutputStates.Information);

            while (!CoCHelper.CheckPixelColor(ScreenData.IsMain))
            {
                Thread.Sleep(1000);

                if (!CheckObstacles())
                {
                    ClickablePoint appPos = GetAppPos();
                    CoCHelper.Click(appPos, 1);
                }

                WaitForMainScreen();
            }

            Main.Bot.WriteToOutput("Main Screen Located", GlobalVariables.OutputStates.Information);
        }

        public static void ZoomOut()
        {
            Main.Bot.WriteToOutput("Zooming Out");

            int count = 0;

            while(!CoCHelper.SameColor(CoCHelper.GetPixelColor(ScreenData.TopLeftClient), Color.Black))
            {
                if(count >= 15)
                    break;
                else
                {
                    KeyboardHelper.SendVirtualKeyToBS(KeyboardHelper.VirtualKeys.VK_DOWN);
                    Thread.Sleep(300);
                    count++;
                }
            }

            Main.Bot.WriteToOutput("Zoomed Out");
        }

        public static void WaitForMainScreen()
        {
			Main.Bot.WriteToOutput("Waiting for Main Screen");
            for (int i = 0; i < 150; i++)
            {
				if (!CoCHelper.CheckPixelColor(ScreenData.IsMain))
                {
                    Thread.Sleep(2000);
                    if (CheckObstacles())
                        i = 0;
                }
                else
                    return;
            }

			Main.Bot.WriteToOutput("Unable to load Clash of Clans, Restarting...");

            ClickablePoint appPos = GetAppPos();
            CoCHelper.Click(appPos, 1);

            Thread.Sleep(10000);
        }

        public static bool CheckObstacles()
        {
			ClickablePoint messagePos = CoCHelper.SearchPixelInRect(ScreenData.Inactivity);
            if (!messagePos.IsEmpty)
            {
                CoCHelper.Click(ScreenData.ReloadButton);
                Thread.Sleep(7000);
                return true;
            }

            if (CoCHelper.CheckPixelColor(ScreenData.Attacked))
            {
                CoCHelper.Click(ScreenData.AttackedBtn);
                return true;
            }

			// The main screen
            if (CoCHelper.CheckPixelColor(ScreenData.IsMainGrayed))
            {
				CoCHelper.Click(ScreenData.TopLeftClient);
                return true;
            }

			// If we have a screen with a small x to cancel it, like when you start a fight. 
			if (CoCHelper.CheckPixelColor(ScreenData.SomeXCancelBtn))
            {
				CoCHelper.Click(ScreenData.SomeXCancelBtn);
                return true;
            }

			// If a fight is on going, than cancel it. 
			if (CoCHelper.CheckPixelColor(ScreenData.CancelFight) || CoCHelper.CheckPixelColor(ScreenData.CancelFight2))
            {
                CoCHelper.Click(ScreenData.CancelFight);
                return true;
            }

            if (CoCHelper.CheckPixelColorBad(new Point(331, 330), Color.FromArgb(240, 160, 59), 20))
            {
                CoCHelper.ClickBad(new Point(331, 330), 1);
                Thread.Sleep(1000);
                return true;
            }

            if (CoCHelper.CheckPixelColorBad(new Point(429, 519), Color.FromArgb(184, 227, 95), 20))
            {
                CoCHelper.ClickBad(new Point(429, 519), 1);
                return true;
            }

            if (CoCHelper.CheckPixelColorBad(new Point(71, 530), Color.FromArgb(192, 0, 0), 20))
            {
                CoCHelper.ClickBad(new Point(331, 330), 1);
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

            CoCHelper.ClickBad(new Point(62, 519), 1);
            Thread.Sleep(500);
            CoCHelper.ClickBad(new Point(512, 394), 1);
            Thread.Sleep(2000);

            if(takeSS)
            {
				Main.Bot.WriteToOutput("Taking snapshot of your loot");
                
                DateTime now = DateTime.Now;
				string date = string.Format("{0}.{1}.{2}", now.Day, now.Month, now.Year);
				string time = string.Format("{0}.{1}", now.Hour, now.Minute);
				CoCHelper.MakeFullScreenCapture(string.Format("{0}/{1} at {2}", GlobalVariables.LogPath, date, time));                
            }

            Thread.Sleep(2000);
            CoCHelper.ClickBad(new Point(428, 544), 1);

            int counter = 0;

            do
            {
                Thread.Sleep(2000);
                if (CoCHelper.CheckPixelColorBad(new Point(284, 28), Color.FromArgb(65, 177, 205), 20))
                {
                    CoCHelper.ClickBad(new Point(331, 330), 1);
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

        public static ClickablePoint GetAppPos()
        {
            ClickablePoint p1 = CoCHelper.SearchPixelInRect(ScreenData.ClashApp);

            if (CoCHelper.IsInColorRange(new ClickablePoint(p1.Point.X + ScreenData.ClashApp2.Point.X, p1.Point.Y + ScreenData.ClashApp2.Point.Y), ScreenData.ClashApp2.Color, ScreenData.ClashApp2.ShadeVariation))
            {
                if (CoCHelper.IsInColorRange(new ClickablePoint(p1.Point.X + ScreenData.ClashApp3.Point.X, p1.Point.Y + ScreenData.ClashApp3.Point.Y), ScreenData.ClashApp3.Color, ScreenData.ClashApp3.ShadeVariation))
                {
                    return p1;
                }
            }

            return new ClickablePoint();
        }
    }
}