namespace CoC.Bot.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;

    using Tools;
    using ViewModels;

    internal class MainScreen
    {
        public static void CheckMainScreen()
        {
            Main.Bot.Output = Properties.Resources.OutputTryingToLocateMainScreen;

            while (!Tools.FastFind.FastFindHelper.IsInColorRange(new Point(284, 28), Color.FromArgb(65, 177, 205), 20)) // FIX VARIATION
            {
                Other.Sleep(1000);

                if (!MainScreen.CheckObstacles())
                {
                    Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(126, 700), 1);
                    //OPEN APP AGAIN
                }

                WaitForMainScreen();
            }

            Other.SetLog("Main Screen Located", Color.Blue);
        }

        public static void ZoomOut()
        {
            //ADD SEND KEYBOARD KEY TO BLUESTACKS
            KeyboardHelper.SendVirtualKeyToBS(KeyboardHelper.VirtualKeys.VK_DOWN);
            KeyboardHelper.SendVirtualKeyToBS(KeyboardHelper.VirtualKeys.VK_DOWN);
            KeyboardHelper.SendVirtualKeyToBS(KeyboardHelper.VirtualKeys.VK_DOWN);
        }

        public static void WaitForMainScreen()
        {
            Other.SetLog("Waiting for Main Screen", Color.Orange);
            for (int i = 0; i < 150; i++)
            {
                if (!Tools.FastFind.FastFindHelper.IsInColorRange(new Point(284, 28), Color.FromArgb(65, 177, 205), 20))
                {
                    Other.Sleep(2000);
                    if (CheckObstacles())
                        i = 0;
                }
                else
                    return;
            }

            Other.SetLog("Unable to load Clash of Clans, Restarting...", Color.Red);
            //OPEN APP AGAIN
            Other.Sleep(10000);
        }

        public static bool CheckObstacles()
        {
            Point messagePos = Tools.FastFind.FastFindHelper.PixelSearch(457, 300, 458, 330, Color.FromArgb(51, 181, 229), 10);
            if (!messagePos.IsEmpty)
            {
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(416, 399), 1);
                Other.Sleep(7000);
                return true;
            }

            if (Tools.FastFind.FastFindHelper.IsInColorRange(new Point(235, 209), Color.FromArgb(158, 56, 38), 20))
            {
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(429, 493), 1);
                return true;
            }

            if (Tools.FastFind.FastFindHelper.IsInColorRange(new Point(284, 28), Color.FromArgb(33, 91, 105), 20))
            {
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(1, 1), 1);
                return true;
            }

            if (Tools.FastFind.FastFindHelper.IsInColorRange(new Point(819, 55), Color.FromArgb(216, 4, 0), 20))
            {
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(819, 55), 1);
                return true;
            }

            if (Tools.FastFind.FastFindHelper.IsInColorRange(new Point(822, 48), Color.FromArgb(216, 4, 8), 20) || Tools.FastFind.FastFindHelper.IsInColorRange(new Point(830, 59), Color.FromArgb(216, 4, 8), 20))
            {
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(822, 48), 1);
                return true;
            }

            if (Tools.FastFind.FastFindHelper.IsInColorRange(new Point(331, 330), Color.FromArgb(240, 160, 59), 20))
            {
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(331, 330), 1);
                Other.Sleep(1000);
                return true;
            }

            if (Tools.FastFind.FastFindHelper.IsInColorRange(new Point(429, 519), Color.FromArgb(184, 227, 95), 20))
            {
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(429, 519), 1);
                return true;
            }

            if (Tools.FastFind.FastFindHelper.IsInColorRange(new Point(71, 530), Color.FromArgb(192, 0, 0), 20))
            {
                Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(331, 330), 1);
                ReturnHome(false, false);
                return true;
            }
            
            return false;
        }

        public static void ReturnHome(bool takeSS = true, bool goldChangeCheck = true)
        {
            if(goldChangeCheck)
            {
                // CHECK KING AND QUEENS POWER
            }

            //SET KING AND QUEEN POWER TO FALSE
            Other.SetLog("Returning Home...", Color.Blue);

            Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(62, 519), 1);
            Other.Sleep(500);
            Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(512, 394), 1);
            Other.Sleep(2000);

            if(takeSS)
            {
                Other.SetLog("Taking snapshot of your loot", Color.Orange);
                
                DateTime now = DateTime.Now;
                string date = now.Day.ToString() + "." + now.Month.ToString() + "." + now.Year.ToString();
                string time = now.Hour.ToString() + "." + now.Minute.ToString();

                Tools.FastFind.FastFindHelper.TakeFullScreenCapture(true);
                //FINISH Tools.FastFind.FastFindWrapper.SaveJPG(0, GlobalVariables.LogPath.ToString() + "/" date.ToString() + " at " + time.ToString(), 100);
            }


        }
    }
}