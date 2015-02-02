namespace CoC.Bot.Functions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using ViewModels;

    /// <summary>
    /// The Main entry point for the Bot Functions.
    /// </summary>
    public class Main
    {
        /// <summary>
        /// Initializes the Bot.
        /// </summary>
        public static void Initialize(MainViewModel vm)
        {
            MainViewModel = vm;

            // TODO: Check if BlueStack is running

            MainViewModel.Output = string.Format(Properties.Resources.OutputWelcomeMessage, Properties.Resources.AppName);
            MainViewModel.Output = Properties.Resources.OutputBotIsStarting;

            CreateDirectory(LogPath);
            CreateDirectory(ScreenshotZombieAttacked);
            CreateDirectory(ScreenshotZombieSkipped);

            WriteLicense();

            // Create Log File
            // Do more stuff
            // Yay!

            // Run Everything related to the bot in the background
            var thread = new Thread(() =>
            {
                while (MainViewModel.IsExecuting)
                {
                    Thread.Sleep(1000);
                    MainViewModel.Output = "Loop test...";
                };
            })
            {
                IsBackground = true
            };
            thread.Start();
        }

        #region Properties

        internal static MainViewModel MainViewModel { get; private set; }

        internal static string AppPath
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        internal static string LogPath
        {
            get { return Path.Combine(AppPath, "Logs"); }
        }

        internal static string ScreenshotZombieAttacked
        {
            get { return Path.Combine(AppPath, @"Screenshots\Zombies Attacked"); }
        }

        internal static string ScreenshotZombieSkipped
        {
            get { return Path.Combine(AppPath, @"Screenshots\Zombies Skipped"); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the required directories.
        /// </summary>
        /// <param name="path">The path.</param>
        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Writes the License.
        /// </summary>
        private static void WriteLicense()
        {
            if (!File.Exists(Path.Combine(AppPath, "LICENSE")))
            {
                try
                {
                    File.WriteAllText(Path.Combine(AppPath, "LICENSE"), Properties.Resources.LICENSE, Encoding.UTF8);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion
    }
}
