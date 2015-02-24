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

    public class ChartTitle : ContentControl
    {        
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ChartTitle),
            new PropertyMetadata(null));
        public static readonly DependencyProperty SubTitleProperty =
            DependencyProperty.Register("SubTitle", typeof(string), typeof(ChartTitle),
            new PropertyMetadata(null));
     
        static ChartTitle()
        {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartTitle), new FrameworkPropertyMetadata(typeof(ChartTitle))); 
        }

        public ChartTitle()
        {

        }

        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public string SubTitle
        {
            get
            {
                return (string)GetValue(SubTitleProperty);
            }
            set
            {
                SetValue(SubTitleProperty, value);
            }
        }
    }
}
