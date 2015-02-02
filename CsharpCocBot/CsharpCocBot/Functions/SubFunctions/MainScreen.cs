using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CoC.Bot.Tools;

namespace CoC.Bot.Functions
{
    class MainScreen
    {
        public static void ZoomOut()
        {
            //ADD SEND KEYBOARD KEY TO BLUESTACKS
          KeyboardHelper.SendVirtualKeyToBS(KeyboardHelper.VirtualKeys.VK_DOWN);
          KeyboardHelper.SendVirtualKeyToBS(KeyboardHelper.VirtualKeys.VK_DOWN);
          KeyboardHelper.SendVirtualKeyToBS(KeyboardHelper.VirtualKeys.VK_DOWN);
        }

        public static void WaitForMainScreen()
        {
            Functions.Other.SetLog("Waiting for Main Screen", Color.Orange);
            for(int i = 0; i < 150; i++)
            {
                GlobalVariables.hBitmap = GlobalVariables.screenCap.SnapShot(GlobalVariables.backgroundMode == true ? true : false);
                if (!Functions.Pixels.ColorCheck(Functions.Pixels.GetPixelColor(284, 28), Color.FromArgb(0, 0, 0), 20))
                {
                    Functions.Other.Sleep(2000);
                }
                else
                {
                    return;
                }

                CheckObstacles();
            }

            Functions.Other.SetLog("Unable to load Clash of Clans, Restarting...", Color.Red);
            //RESTART APP
            Functions.Other.Sleep(5000);

            do
            {
                //FIX UP
            } while (false); 
        }

        public static bool CheckObstacles()
        {
            return false;
        }

        public static void CheckMainScreen()
        {
            Functions.Other.SetLog("Trying to locate Main Screen", Color.Blue);
            GlobalVariables.hBitmap = GlobalVariables.screenCap.SnapShot(GlobalVariables.backgroundMode == true ? true : false);

            while (!Functions.Pixels.ColorCheck(Functions.Pixels.GetPixelColor(284, 28), Color.FromArgb(0, 0, 0), 20)) // FIX VARIATION
            {
                GlobalVariables.HWnD = Tools.BlueStackHelper.GetBlueStackWindowHandle();
                Functions.Other.Sleep(1000);

                if (!Functions.MainScreen.CheckObstacles())
                {
                    Tools.MouseHelper.ClickOnPoint2(GlobalVariables.HWnD, new Point(126, 700), 1);
                    //RESTART APP?
                }

                WaitForMainScreen();
            }

            Functions.Other.SetLog("Main Screen Located", Color.Blue);
        }
    }
}
