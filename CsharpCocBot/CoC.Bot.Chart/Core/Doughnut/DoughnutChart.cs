/*
 * Modern UI (Metro) Charts for Windows 8, WPF, Silverlight
 * 
 * The charts have been developed by Torsten Mandelkow.
 * http://modernuicharts.codeplex.com/
 */

namespace CoC.Bot.Chart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DoughnutChart : PieChart
    {
        protected override double GridLinesMaxValue
        {
            get
            {
                return 0.0;
            }
        }
    }
}