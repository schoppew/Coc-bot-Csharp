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
    using System.Windows;  
    using System.Windows.Controls;
    using System.Windows.Media.Animation;

    public class ColumnPiece : PieceBase
    {
        #region Fields

        private Border slice = null;

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(double), typeof(ColumnPiece),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnPercentageChanged)));
        
        public static readonly DependencyProperty ColumnHeightProperty =
            DependencyProperty.Register("ColumnHeight", typeof(double), typeof(ColumnPiece),
            new PropertyMetadata(0.0));
        
        #endregion Fields

        #region Constructors

        static ColumnPiece()        
        {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ColumnPiece), new FrameworkPropertyMetadata(typeof(ColumnPiece)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnPiece"/> class.
        /// </summary>
        public ColumnPiece()
        {
			Loaded += ColumnPiece_Loaded;
        }

        #endregion Constructors

        #region Properties

        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public double ColumnHeight
        {
            get { return (double)GetValue(ColumnHeightProperty); }
            set { SetValue(ColumnHeightProperty, value); }
        }
 
        #endregion Properties

        #region Methods

        private static void OnPercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ColumnPiece).DrawGeometry();
        }

        protected override void InternalOnApplyTemplate()
        {
            slice = GetTemplateChild("Slice") as Border;
            RegisterMouseEvents(slice);
        }

        void ColumnPiece_Loaded(object sender, RoutedEventArgs e)
        {
            DrawGeometry();
        }

        protected override void DrawGeometry(bool withAnimation = true)
        {    
            try
            {
                if (ClientWidth <= 0.0)
                {
                    return;
                }
                if (ClientHeight <= 0.0)
                {
                    return;
                }

                double startHeight = 0;
                if (slice.Height > 0)
                {
                    startHeight = slice.Height;
                }

                DoubleAnimation scaleAnimation = new DoubleAnimation();
                scaleAnimation.From = startHeight;
                scaleAnimation.To = ClientHeight * Percentage;
                scaleAnimation.Duration = TimeSpan.FromMilliseconds(withAnimation ? 500: 0);
                scaleAnimation.EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseOut };
                Storyboard storyScaleX = new Storyboard();
                storyScaleX.Children.Add(scaleAnimation);

                Storyboard.SetTarget(storyScaleX, slice);
				Storyboard.SetTargetProperty(storyScaleX, new PropertyPath("Height"));
                storyScaleX.Begin();
   
            }
            catch (Exception ex)
            {
            }
        }

        #endregion Methods
    }
}