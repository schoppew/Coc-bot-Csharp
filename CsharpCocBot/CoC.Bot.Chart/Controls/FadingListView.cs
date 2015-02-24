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
	using System.Windows.Media.Animation;

    public class FadingListView : ItemsControl
    {
        public static readonly DependencyProperty RealWidthProperty =
            DependencyProperty.Register("RealWidth", typeof(double), typeof(FadingListView),
            new PropertyMetadata(0.0));
        public static readonly DependencyProperty RealHeightProperty =
            DependencyProperty.Register("RealHeight", typeof(double), typeof(FadingListView),
            new PropertyMetadata(0.0));

        static FadingListView()
        {
			//DefaultStyleKeyProperty.OverrideMetadata(typeof(FadingListView), new FrameworkPropertyMetadata(typeof(FadingListView))); 
        }

        public FadingListView()
        {
            SizeChanged += FadingListView_SizeChanged;
        }

        public double RealWidth
        {
            get
            {
                return (double)GetValue(RealWidthProperty);
            }
            set
            {
                SetValue(RealWidthProperty, value);
            }
        }

        public double RealHeight
        {
            get
            {
                return (double)GetValue(RealHeightProperty);
            }
            set
            {
                SetValue(RealHeightProperty, value);
            }
        }

        void FadingListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RealWidth = ActualWidth;
            RealHeight = ActualHeight;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (Items != null)
            {
                if (Items.Count < 100)
                {
                    int index = ItemContainerGenerator.IndexFromContainer(element);
                    var lb = (ContentPresenter)element;

                    TimeSpan waitTime = TimeSpan.FromMilliseconds(index * (500.0 / Items.Count));

                    lb.Opacity = 0.0;
                    DoubleAnimation anm = new DoubleAnimation();
                    anm.From = 0;
                    anm.To = 1;
                    anm.Duration = TimeSpan.FromMilliseconds(250);
                    anm.BeginTime = waitTime;

                    Storyboard storyda = new Storyboard();
                    storyda.Children.Add(anm);
                    Storyboard.SetTarget(storyda, lb);
					Storyboard.SetTargetProperty(storyda, new PropertyPath(OpacityProperty));
                    storyda.Begin();
                }
            }

            base.PrepareContainerForItemOverride(element, item);
        }
    }
}
