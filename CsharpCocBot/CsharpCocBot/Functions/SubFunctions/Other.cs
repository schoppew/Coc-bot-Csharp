using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CoC.Bot.Functions
{
    class Other
    {
        public static void Sleep(int ms)
        {
            System.Threading.Thread.Sleep(ms);
        }

        public void CreateLogFile()
        {

        }

        public void DisableBlueStacks()
        {

        }

        public void EnableBlueStacks()
        {

        }

        public Point FindPos()
        {
            return new Point(-1, -1);
        }

        public Point GetBlueStacksPos()
        {
            return new Point(-1, -1);
        }

		[Obsolete("Use Main.Bot.Output which takes care of sending messages to the Output and Log. Will add colours later!", true)]
		public static void SetLog(string msg, Color color)
		{

		}

        public string Tab(int _a, int _b)
        {
            return "";
        }

        public string GetTime()
        {
            return "";
        }
    }
}
