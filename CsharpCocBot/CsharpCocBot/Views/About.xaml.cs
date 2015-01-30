namespace CoC.Bot.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            InitializeComponent();
        }

        private void Bar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void About_Loaded(object sender, EventArgs e)
        {
            tbDisclaimer.Text = "DISCLAIMER:" + "\r\n\r\nThis software is provided strictly for educational purposes only." + "\r\nYou're responsible for the use of the software." + "\r\nThe 'coder' is not responsible for any misuse of the generated information.";
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
