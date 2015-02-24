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
	using System.Windows.Media;

    public class PiePieceLabel : Control
    {        
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(PiePieceLabel),
            new PropertyMetadata(null));
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(PiePieceLabel),
            new PropertyMetadata(0.0, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(double), typeof(PiePieceLabel),
            new PropertyMetadata(null));
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.Register("ItemBrush", typeof(Brush), typeof(PiePieceLabel),
            new PropertyMetadata(null));
     
        static PiePieceLabel()
        {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(PiePieceLabel), new FrameworkPropertyMetadata(typeof(PiePieceLabel)));
        }

        public PiePieceLabel()
        {

        }

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public Brush ItemBrush
        {
            get { return (Brush)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
    }
}
