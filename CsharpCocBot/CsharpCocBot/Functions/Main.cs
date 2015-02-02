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

    using Tools;
    using Tools.FastFind;
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
            Bot = vm; // Store the ViewModel here for exposing to the SubFunctions

            Bot.Output = string.Format(Properties.Resources.OutputWelcomeMessage, Properties.Resources.AppName);
            Bot.Output = Properties.Resources.OutputBotIsStarting;

            // Check if BlueStack is running
            FastFindWrapper.SetHWnd(BlueStackHelper.GetBlueStackWindowHandle(), true);
            if (!BlueStackHelper.IsBlueStackRunning)
            {
                Bot.Output = Properties.Resources.OutputBSNotFound;

                Bot.IsExecuting = false;
                return;
            }

            if (!BlueStackHelper.IsRunningWithRequiredDimensions)
            {
                Bot.Output = Properties.Resources.OutputBSNotRunningWithDimensions;
                Bot.Output = Properties.Resources.OutputBSApplyDimensionsIntoRegistry;

                if (!BlueStackHelper.SetDimensionsIntoRegistry())
                {
                    // Woops! Something went wrong, log the error!
                    Bot.Output = Properties.Resources.OutputBSApplyDimensionsError;

                    Bot.IsExecuting = false;
                    return;
                }

                // Restart BlueStack
                // Wait until restart and continue...

                BlueStackHelper.ActivateBlueStack();
            }

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
                while (Bot.IsExecuting)
                {
                    Thread.Sleep(1000);
                    Bot.Output = "Loop test...";
                    Bot.Output = "Minimum Gold is: " + Bot.MinimumGold; // Changing values on the fly works as expected
                    Bot.Output = "Try changing values on the fly.";

                    //SubFunctions.MainScreen.CheckMainScreen();

                    //SubFunctions.MainScreen.ZoomOut();

                    //SubFunctions.Village.TrainTroops();

                    //SubFunctions.Village.RequestCC();

                    //SubFunctions.Village.Collect();

                    //Idle();

                    //SubFunctions.Attack.AttackMain();
                };
            })
            {
                IsBackground = true
            };
            thread.Start();
        }

        #region Properties

        internal static MainViewModel Bot { get; private set; }

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
