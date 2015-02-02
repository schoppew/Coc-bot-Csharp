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

            // TODO

            GlobalVariables.hBitmap = GlobalVariables.screenCap.SnapShot(GlobalVariables.backgroundMode == true ? true : false);

            while (!Pixels.ColorCheck(Pixels.GetPixelColor(284, 28), Color.FromArgb(0, 0, 0), 20)) // FIX VARIATION
            {
                GlobalVariables.HWnD = Tools.BlueStackHelper.GetBlueStackWindowHandle();
                Other.Sleep(1000);

                if (!MainScreen.CheckObstacles())
                {
                    Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(126, 700), 1);
                    //RESTART APP?
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
                GlobalVariables.hBitmap = GlobalVariables.screenCap.SnapShot(GlobalVariables.backgroundMode == true ? true : false);
                if (!Pixels.ColorCheck(Pixels.GetPixelColor(284, 28), Color.FromArgb(0, 0, 0), 20))
                {
                    Other.Sleep(2000);
                }
                else
                {
                    return;
                }

                CheckObstacles();
            }

            Other.SetLog("Unable to load Clash of Clans, Restarting...", Color.Red);
            //RESTART APP
            Other.Sleep(5000);

            do
            {
                //FIX UP
            } while (false);
        }

        public static bool CheckObstacles()
        {
            return false;
        }
    }
}