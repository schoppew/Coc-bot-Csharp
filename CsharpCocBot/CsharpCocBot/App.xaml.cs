namespace CoC.Bot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            FieldInfo colorCacheField = typeof(SystemColors).GetField("_colorCache", BindingFlags.Static | BindingFlags.NonPublic);
            Color[] _colorCache = (Color[])colorCacheField.GetValue(typeof(SystemColors));
            _colorCache[14] = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // In case we need to do something when App launches
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show(string.Format(@"Unhandled exception! Message : {0} Stack Trace : {1}", ex.Message, ex.StackTrace), CoC.Bot.Properties.Resources.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, CoC.Bot.Properties.Resources.AppName, MessageBoxButton.OK, MessageBoxImage.Exclamation);

            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
