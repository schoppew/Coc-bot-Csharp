using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using CoC.Bot.Data;
using CoC.Bot.Properties;
using CoC.Bot.Tools;
using CoC.Bot.ViewModels;
using FastFind;

namespace CoC.Bot.BotEngine
{
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
            // Store in properties so we can access in the SubFunctions
            Bot = vm;
            Bot.ClearOutput();

            Bot.WriteToOutput(string.Format(Resources.OutputWelcomeMessage, Resources.AppName));
            Bot.WriteToOutput(Resources.OutputBotIsStarting);

            // Check if BlueStacks is running
            if (!BlueStacksHelper.IsBlueStacksRunning)
            {
                Bot.WriteToOutput(Resources.OutputBSNotFound, GlobalVariables.OutputStates.Error);

                Bot.IsExecuting = false;
                return;
            }

            if (!BlueStacksHelper.IsRunningWithRequiredDimensions)
            {
                Bot.WriteToOutput(Resources.OutputBSNotRunningWithDimensions);
                Bot.WriteToOutput(Resources.OutputBSApplyDimensionsIntoRegistry);

                if (!BlueStacksHelper.SetDimensionsIntoRegistry())
                {
                    // Woops! Something went wrong, log the error!
                    Bot.WriteToOutput(Resources.OutputBSApplyDimensionsError, GlobalVariables.OutputStates.Error);

                    Bot.IsExecuting = false;
                    return;
                }
                else
                    Bot.WriteToOutput(Resources.OutputBSAppliedDimensionsIntoRegistry);

                // Restart BlueStacks
                // Wait until restart and continue...

                BlueStacksHelper.ActivateBlueStacks();
            }

            CreateDirectory(GlobalVariables.LogPath);
            CreateDirectory(GlobalVariables.ScreenshotZombieAttacked);
            CreateDirectory(GlobalVariables.ScreenshotZombieSkipped);

            WriteLicense();

            // Run Everything related to the bot in the background
            var thread = new Thread(() =>
            {
                while (Bot.IsExecuting)
                {
                    FastFindWrapper.SetHWnd(BlueStacksHelper.GetBlueStacksWindowHandle(), true);

                    MainScreen.CheckMainScreen();
                    Thread.Sleep(1000);

                    MainScreen.ZoomOut();
                    Thread.Sleep(1000);

                    CoCHelper.Click(ScreenData.OpenChatBtn);
                    Thread.Sleep(1000);
                    string str = ReadText.GetString(151);
                    System.Windows.MessageBox.Show(str);

                    Village.ReArmTraps();
                    Thread.Sleep(1000);

                    Barrack.TrainTroops();
                    Thread.Sleep(1000);

                    Barrack.Boost();
                    Thread.Sleep(1000);

                    RequestAndDonate.RequestTroops();
                    Thread.Sleep(1000);

                    RequestAndDonate.DonateCC();
                    Thread.Sleep(1000);

                    Village.CollectResources();
                    Thread.Sleep(1000);

                    Village.UpgradeWalls();
                    Thread.Sleep(1000);

                    Village.Idle();
                    Thread.Sleep(1000);

                    //Attack.AttackMain();
                };
            })
            {
                IsBackground = true
            };
            thread.Start();
        }

        #region Properties

        /// <summary>
        /// Gets the MainViewModel.
        /// </summary>
        /// <value>The MainViewModel.</value>
        internal static MainViewModel Bot { get; private set; }

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
            if (!File.Exists(Path.Combine(GlobalVariables.AppPath, "LICENSE")))
            {
                try
                {
                    File.WriteAllText(Path.Combine(GlobalVariables.AppPath, "LICENSE"), Resources.LICENSE, Encoding.UTF8);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Checks if latest version
        /// </summary>
        private static void IsUpdateAvailable()
        {
            string version = new WebClient().DownloadString("http://gamebot.org/CSharp/Version.txt");
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string currVersion = fvi.FileVersion;

            if (!version.Equals(currVersion))
            {
                if (MessageBox.Show("Update Available! Would you like to download the latest version?",
                        "Update Available!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    Process.Start("http://gamebot.org");
                }
            }
        }



        #endregion
    }
}
