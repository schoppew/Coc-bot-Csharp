namespace CoC.Bot
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Threading;
	using CoC.Bot.Tools;
	using Microsoft.Shell;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
	public partial class App : Application, ISingleInstanceApp
    {
		private const string Unique = "CoC-Bot-c977bfea-3c83-4975-930b-d01c7614447a";

		[STAThread]
		public static void Main()
		{
			if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
			{
				var application = new App();
				application.InitializeComponent();
				application.Run();

				// Allow single instance code to perform cleanup operations
				SingleInstance<App>.Cleanup();
			}
		}

		#region ISingleInstanceApp Members

		public bool SignalExternalCommandLineArgs(IList<string> args)
		{
			// Handle command line arguments of second instance

			// Show window if hidden (tray)
			if (!MainWindow.IsVisible)
				MainWindow.Show();

			// Bring window to foreground
			if (MainWindow.WindowState == WindowState.Minimized)
				MainWindow.WindowState = WindowState.Normal;

			MainWindow.Activate();

			return true;
		}

		#endregion

        public App()
        {
            FieldInfo colorCacheField = typeof(SystemColors).GetField("_colorCache", BindingFlags.Static | BindingFlags.NonPublic);
            Color[] _colorCache = (Color[])colorCacheField.GetValue(typeof(SystemColors));
            _colorCache[14] = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			UI.Services.ServiceInjector.InjectServices();

			CoCHelper.Initialize();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if DEBUG
			GlobalVariables.IsDebug = true;
#else
            GlobalVariables.IsDebug = false;
#endif

			// Only launch with Debug tab enabled if is implicity specified
			if (e.Args.Length > 0)
			{
				for (var i = 0; i != e.Args.Length; i++)
				{
					if (e.Args[i].ToLower() == "/debug")
					{
						GlobalVariables.IsDebug = true;
					}
				}
			}
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
