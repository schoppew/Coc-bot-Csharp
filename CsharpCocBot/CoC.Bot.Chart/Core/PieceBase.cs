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
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Controls;

    [TemplateVisualState(Name = BarPiece.StateSelectionUnselected, GroupName = BarPiece.GroupSelectionStates)]
    [TemplateVisualState(Name = BarPiece.StateSelectionSelected, GroupName = BarPiece.GroupSelectionStates)]
    public abstract class PieceBase : Control
    {
        #region Fields

        internal const string StateSelectionUnselected = "Unselected";
        internal const string StateSelectionSelected = "Selected";
        internal const string GroupSelectionStates = "SelectionStates";

        public static readonly DependencyProperty ClientHeightProperty =
            DependencyProperty.Register("ClientHeight", typeof(double), typeof(PieceBase),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnSizeChanged)));
        
        public static readonly DependencyProperty ClientWidthProperty =
            DependencyProperty.Register("ClientWidth", typeof(double), typeof(PieceBase),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnSizeChanged)));

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(PieceBase),
            new PropertyMetadata(null));
        
        public static readonly DependencyProperty ParentChartProperty =
            DependencyProperty.Register("ParentChart", typeof(ChartBase), typeof(PieceBase),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(PieceBase),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedChanged)));
        
        public static readonly DependencyProperty IsClickedByUserProperty =
            DependencyProperty.Register("IsClickedByUser", typeof(bool), typeof(PieceBase),
            new PropertyMetadata(false));

        #endregion Fields

        #region Properties

        public double ClientHeight
        {
            get { return (double)GetValue(ClientHeightProperty); }
            set { SetValue(ClientHeightProperty, value); }
        }

        public double ClientWidth
        {
            get { return (double)GetValue(ClientWidthProperty); }
            set { SetValue(ClientWidthProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public bool IsClickedByUser
        {
            get { return (bool)GetValue(IsClickedByUserProperty); }
            set { SetValue(IsClickedByUserProperty, value); }
        }

        public ChartBase ParentChart
        {
            get { return (ChartBase)GetValue(ParentChartProperty); }
            set { SetValue(ParentChartProperty, value); }
        }

        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        public string Caption
        {
            get
            {
                return (DataContext as DataPoint).DisplayName;
            }
        }

        #endregion Properties

        #region Methods

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PieceBase).DrawGeometry(false);
        }

        protected virtual void DrawGeometry(bool withAnimation = true)
        {
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PieceBase source = (PieceBase)d;
            bool oldValue = (bool)e.OldValue;
            bool newValue = (bool)e.NewValue;
            source.OnIsSelectedPropertyChanged(oldValue, newValue);
        }

        protected virtual void OnIsSelectedPropertyChanged(bool oldValue, bool newValue)
        {
            IsClickedByUser = false;
            VisualStateManager.GoToState(this, newValue ? StateSelectionSelected : StateSelectionUnselected, true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InternalOnApplyTemplate();
        }

        protected virtual void InternalOnApplyTemplate()
        {
        }

        protected void RegisterMouseEvents(UIElement slice)
        {

            if (slice != null)
            {
				slice.MouseLeftButtonUp += delegate
                {
                    InternalMousePressed();
                };

				slice.MouseMove += delegate
                {
                    InternalMouseMoved();
                };
            }
        }

        private void InternalMousePressed()
        {
            SetValue(PieceBase.IsClickedByUserProperty, true);
        }

        private void InternalMouseMoved()
        {
            //SetValue(PieceBase.Is, true);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            HandleMouseDown();
            e.Handled = true;
        }

        private void HandleMouseDown()
        {
            IsClickedByUser = true;
        }

        #endregion Methods
    }
}
