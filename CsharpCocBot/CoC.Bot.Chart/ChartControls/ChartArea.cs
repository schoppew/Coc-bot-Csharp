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
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows;

    public class ChartArea : ContentControl
    {
        static ChartArea()
        {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartArea), new FrameworkPropertyMetadata(typeof(ChartArea)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieChart"/> class.
        /// </summary>
        public ChartArea()
        {
			
        }

        public static readonly DependencyProperty ParentChartProperty =
            DependencyProperty.Register("ParentChart",
            typeof(ChartBase),
            typeof(ChartArea),
            new PropertyMetadata(null));
        public static readonly DependencyProperty ChartLegendItemStyleProperty =
            DependencyProperty.Register("ChartLegendItemStyle",
            typeof(Style),
            typeof(ChartArea),
            new PropertyMetadata(null));

        public ChartBase ParentChart
        {
            get { return (ChartBase)GetValue(ParentChartProperty); }
            set { SetValue(ParentChartProperty, value); }
        }

        public ObservableCollection<string> GridLines
        {
            get
            {
                return ParentChart.GridLines;
            }
        }

        public ObservableCollection<DataPointGroup> DataPointGroups
        {
            get
            {
                return ParentChart.DataPointGroups;
            }
        }

        public ObservableCollection<ChartLegendItemViewModel> ChartLegendItems
        {
            get
            {
                return ParentChart.ChartLegendItems;
            }
        } 

        public Style ChartLegendItemStyle
        {
            get { return (Style)GetValue(ChartLegendItemStyleProperty); }
            set { SetValue(ChartLegendItemStyleProperty, value); }
        }
    }
}
