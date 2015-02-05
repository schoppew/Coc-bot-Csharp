using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Point = Win32.POINT;
namespace CoC.Bot.Functions
{
    class Search
    {

        public bool CheckNextButton()
        {
            return false;
        }

        public bool CompareResources()
        {
            return false;
        }

        public void GetResources()
        {

        }

        public static void PrepareSearch()
        {
            Tools.CoCHelper.ClickBad(new Point(60, 614), 1);
            Thread.Sleep(1000);
            Tools.CoCHelper.ClickBad(new Point(217, 510), 1);
            Thread.Sleep(3000);

            if (Tools.CoCHelper.CheckPixelColorBad(new Point(513, 416), Color.FromArgb(93, 172, 16), 50))
            {
                Tools.CoCHelper.ClickBad(new Point(513, 416), 1);
            }
        }

        public static void VillageSearch()
        {

        }
    }
}
