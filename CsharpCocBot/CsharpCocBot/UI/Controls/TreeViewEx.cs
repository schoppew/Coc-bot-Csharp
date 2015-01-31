namespace CoC.Bot.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A TreeView control with Bindable SelectedItem.
    /// </summary>
    public class TreeViewEx : TreeView, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItem", typeof(Object), typeof(TreeViewEx), new PropertyMetadata(null));

        public new Object SelectedItem
        {
            get { return (Object)GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemsProperty, value);
                NotifyPropertyChanged("SelectedItem");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewEx"/> class.
        /// </summary>
        public TreeViewEx() : base()
        {
            base.SelectedItemChanged += new RoutedPropertyChangedEventHandler<Object>(TreeViewEx_SelectedItemChanged);
        }

        private void TreeViewEx_SelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            SelectedItem = base.SelectedItem;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String aPropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(aPropertyName));
        }
    }
}