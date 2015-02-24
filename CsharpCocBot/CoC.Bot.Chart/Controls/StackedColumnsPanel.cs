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
    using System.Windows.Controls;
    using System.Windows;

    public class StackedColumnsPanel : Grid
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
                child.Measure(availableSize);

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size cellSize = new Size(finalSize.Width / Children.Count, finalSize.Height);
            int row = 0, col = 0;

            double bottomposition = finalSize.Height;
            foreach (UIElement child in Children)
            {
                double width= finalSize.Width;
                double height = child.DesiredSize.Height;
                double x = 0;
                double y = bottomposition - height;
                Rect rect = new Rect(x, y, width, height);
                child.Arrange(rect);

                bottomposition -= height;
                col++;
            }
            return finalSize;
        }
    }
}
