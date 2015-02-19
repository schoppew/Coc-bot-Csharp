namespace CoC.Bot.ViewModels
{
    using System;
    using System.Collections.Generic;
	using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
	using System.Windows;
    using System.Windows.Input;
	using System.Windows.Media;
	using System.Xml.Linq;
	using Point = Win32.POINT;

    using Data;
    using Tools;
    using Tools.FastFind;
    using UI.Commands;
	using UI.Services;
	using CoC.Bot.BotEngine;

    /// <summary>
    /// Provides functionality for the MainWindow
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
			/*
			 * -------------------------------------------------------------------------------------------------------------
			 * UI Usage Notes
			 * -------------------------------------------------------------------------------------------------------------
			 * 
			 * 
			 * HowTo: Start/Stop/Hide etc
			 * ------------------------------------------------------------------------------------------------------------
			 * Under 'Main Methods' you will find the Start(), Stop(), Hide(), etc methods.
			 * All those methods are already vinculed to the UI by using Commands
			 * You can make them async if you wish.
			 * 
			 * 
			 * HowTo: Access a specific value or setting in the UI 
			 * ------------------------------------------------------------------------------------------------------------
			 * All UI properties (User settings) are defined in Properties, no need to access the controls directly.
			 * For example:
			 *          (bool)  MeetGold
			 *          (int)   MinimumGold
			 *          (Model) SelectedDeployStrategy
			 *          (int)   SelectedTroopComposition.Id     <--- The Id is defined in Data.TroopComposition enum
			 *                  DataCollection.TroopTiers       <--- Contains the Troop Tier (Tier 1, Tier 2, Tier 3, etc)
			 *                  DataCollection.TroopTiers.Troop <--- ontains Troops per Tier (Barbs, Archs, ... in Tier 1)
			 *          
			 * 
			 * HowTo: Pass a value or values (Properties) into another method/class for accessing it
			 * ------------------------------------------------------------------------------------------------------------
			 * Just use as parameter the MainViewModel. See Samples.GettingAroundTheUI for a code example.
			 * We can access all values by passing the MainViewModel as parameter.
			 * We can retrieve or change a property's value, those will get reflected automatically in the UI.
			 * Ex.:
			 *          Samples.GettingAroundTheUI.UseValuesInUI(this);
			 * 
			 * 
			 * HowTo: Access the Troops data in the Donate Settings
			 * ------------------------------------------------------------------------------------------------------------
			 * Each Troop is stored in TroopTier which is exposed by the TroopTiers property.
			 * You can access a TroopTier by using (either one) as example:
			 *          var t1 = DataCollection.TroopTiers[(int)TroopType.Tier1];
			 *          var t1 = DataCollection.TroopTiers.Get(TroopType.Tier1);
			 *          var t1 = DataCollection.TroopTiers.Where(tt => tt.Id == (int)TroopType.Tier1).FirstOrDefault();
			 *          
			 * You can access a Troop by using (either one) as example (same as TroopTier):
			 *          var troop = DataCollection.TroopTiers[(int)TroopType.Tier1].Troops[Troop.Barbarian];
			 *          var troop = DataCollection.TroopTiers.Get(TroopType.Tier1).Troops.Get(Troop.Barbarian);
			 *          var troop = DataCollection.TroopTiers.AllTroops().Get(Troop.Barbarian).IsSelectedForDonate;
			 *          var troop = DataCollection.TroopTiers.SelectMany(tt => tt.Troops).Where(t => t.Id == (int)Troop.Barbarian);
			 *          
			 * You can acces a specific Troop setting, for example:
			 *          var troop = DataCollection.TroopTiers.AllTroops().Get(Troop.Barbarian).IsDonateAll;
			 *          var troop = DataCollection.TroopTiers.AllTroops().Get(Troop.Barbarian).DonateKeywords;
			 * 
			 * 
			 * HowTo: Write to the Output (Window Log)
			 * ------------------------------------------------------------------------------------------------------------
			 * Just set a string value into the Output property.
			 * For example:
			 *          WriteToOutput("Hello there!");
			 *          WriteToOutput("Hello there!", GlobalVariables.OutputStates.Information);	<-- Color Blue
			 *          WriteToOutput("Hello there!", GlobalVariables.OutputStates.Verified);		<-- Color Green
			 *          WriteToOutput("Hello there!", GlobalVariables.OutputStates.Warning);		<-- Color Yellow
			 *          WriteToOutput("Hello there!", GlobalVariables.OutputStates.Error);			<-- Color Red
			 * 
			 * 
			 * ------------------------------------------------------------------------------------------------------------
			 */

			Init();
            GetUserSettings();

			GlobalVariables.PluginLoader = new PluginMngr.PluginLoader(this);

            Message = Properties.Resources.StartMessage;
        }

        #region Properties

        /// <summary>
        /// Gets the application title.
        /// </summary>
        /// <value>The application title.</value>
        public static string AppTitle { get { return string.Format("{0} v{1}", Properties.Resources.AppName, typeof(App).Assembly.GetName().Version.ToString(3)); } }

		public static string AppTitleGeneral { get { return Properties.Resources.AppTitleGeneral; } }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <value>The application settings.</value>
        internal static Properties.Settings AppSettings { get { return Properties.Settings.Default; } }

        internal LogWriter Log { get; private set; }

		public static bool IsDebug { get { return GlobalVariables.IsDebug; } }

        #region Behaviour Properties

        /// <summary>
        /// Gets or sets a value indicating whether this bot is executing.
        /// </summary>
        /// <value><c>true</c> if this bot is executing; otherwise, <c>false</c>.</value>
        public bool IsExecuting { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether BlueStacks is hidden.
		/// </summary>
		/// <value><c>true</c> if BlueStacks is hidden; otherwise, <c>false</c>.</value>
		public bool IsBlueStacksHidden { get; set; }

        /// <summary>
		/// Gets or sets a value indicating the BlueStacks Hide/Restore State.
        /// </summary>
		/// <value><c>true</c> if BlueStacks is hidden; otherwise, <c>false</c>.</value>
		public bool HideRestoreBlueStacksState
		{
			get { return IsBlueStacksHidden ? true : false; }
		}

        /// <summary>
        /// Gets a value indicating the Start/Stop State.
        /// </summary>
        /// <value><c>true</c> if Executing; otherwise, <c>false</c>.</value>
        public bool StartStopState
        {
            get { return IsExecuting ? true : false; }
        }

		private bool _isBusy;
		/// <summary>
		/// Gets or sets a value indicating whether this bot is busy.
		/// </summary>
		/// <value><c>true</c> if this bot is busy; otherwise, <c>false</c>.</value>
		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				if (_isBusy != value)
				{
					_isBusy = value;
					OnPropertyChanged();
				}
			}
		}

        #endregion

        #region General Properties

		private static string _output;
		/// <summary>
		/// [For use in UI only] Gets or sets the Output (Window Log).
		/// </summary>
		/// <value>The Output (Window Log).</value>
		public string Output
		{
			get { return _output; }
			set
			{
				if (_output != value)
				{
					_output = value;
					OnPropertyChanged();
				}
			}
		}

        private string _message;
        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>The status message.</value>
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _maxTrophies;
        /// <summary>
        /// Gets or sets the maximum Trophies.
        /// </summary>
        /// <value>The maximum Trophies.</value>
        public int MaxTrophies
        {
            get { return _maxTrophies; }
            set
            {
                if (_maxTrophies != value)
                {
                    _maxTrophies = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

		#endregion

		#region Commands

		private AboutCommand _aboutCommand = new AboutCommand();
        public ICommand AboutCommand
        {
            get { return _aboutCommand; }
        }

		public ICommand MouseDownCommand
		{
			get { return new RelayCommand<MouseButtonEventArgs>(e => MouseDownDragAndMoveWindow(e)); }
		}

		public ICommand MinimizeToTrayCommand
		{
			get { return new RelayCommand(() => MinimizeToTray()); }
		}

		public ICommand MinimizeCommand
		{
			get { return new RelayCommand(() => Minimize()); }
		}

        public ICommand ExitCommand
        {
            get { return new RelayCommand(() => Exit()); }
        }

        #region General Settings Commands

        public ICommand StartStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    StartStop();
					OnPropertyChanged(() => StartStopState);
                });
            }
        }

		public ICommand HideRestoreBlueStacksCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					HideRestoreBlueStacks();
					OnPropertyChanged(() => HideRestoreBlueStacksState);
				});
			}
		}

        #endregion

		#endregion

        #region Initialization

        /// <summary>
        /// Initializes everything that is needed.
        /// </summary>
        private void Init()
        {
            GlobalVariables.Log.WriteToLog(Properties.Resources.LogBotInitializing);

            // Fill the Deploy Strategies
            if (DataCollection.DeployStrategies.Count == 0)
            {
                foreach (var ds in Enum.GetValues(typeof(DeployStrategy)))
                {
                    DataCollection.DeployStrategies.Add(Model.CreateNew((int)ds, ((DeployStrategy)ds).Name()));
                }
            }

            // Fill the Deploy Troops
            if (DataCollection.DeployTroops.Count == 0)
            {
                foreach (var dt in Enum.GetValues(typeof(DeployTroop)))
                {
                    DataCollection.DeployTroops.Add(Model.CreateNew((int)dt, ((DeployTroop)dt).Name()));
                }
            }
            
            // Fill the Troop Tiers
            if (DataCollection.TroopTiers.Count == 0)
            {
                foreach (var tier in Enum.GetValues(typeof(TroopType)))
                {
                    switch ((TroopType)tier)
                    {
                        case TroopType.Tier1:
                            DataCollection.TroopTiers.Add(TroopTierModel.CreateNew((int)tier, TroopType.Tier1.Name()));

                            var t1 = DataCollection.TroopTiers.First(tt => tt.Id == (int)tier);
                            t1.Troops.Add(TroopModel.CreateNew((int)Troop.Barbarian, Troop.Barbarian.Name(), AppSettings.TroopsQtyBarbarians, AppSettings.IsDonateBarbarians, AppSettings.IsDonateAllBarbarians, AppSettings.DonateKeywordsBarbarians, AppSettings.MaxDonationsPerRequestBarbarians));
                            t1.Troops.Add(TroopModel.CreateNew((int)Troop.Archer, Troop.Archer.Name(), AppSettings.TroopsQtyArchers, AppSettings.IsDonateArchers, AppSettings.IsDonateAllArchers, AppSettings.DonateKeywordsArchers, AppSettings.MaxDonationsPerRequestArchers));
                            t1.Troops.Add(TroopModel.CreateNew((int)Troop.Goblin, Troop.Goblin.Name(), AppSettings.TroopsQtyGoblins, AppSettings.IsDonateGoblins, AppSettings.IsDonateAllGoblins, AppSettings.DonateKeywordsGoblins, AppSettings.MaxDonationsPerRequestGoblins));
                            break;
                        case TroopType.Tier2:
                            DataCollection.TroopTiers.Add(TroopTierModel.CreateNew((int)tier, TroopType.Tier2.Name()));

                            var t2 = DataCollection.TroopTiers.First(tt => tt.Id == (int)tier);
                            t2.Troops.Add(TroopModel.CreateNew((int)Troop.Giant, Troop.Giant.Name(), AppSettings.TroopsQtyGiants, AppSettings.IsDonateGiants, AppSettings.IsDonateAllGiants, AppSettings.DonateKeywordsGiants, AppSettings.MaxDonationsPerRequestGiants));
                            t2.Troops.Add(TroopModel.CreateNew((int)Troop.WallBreaker, Troop.WallBreaker.Name(), AppSettings.TroopsQtyWallBreakers, AppSettings.IsDonateWallBreakers, AppSettings.IsDonateAllWallBreakers, AppSettings.DonateKeywordsWallBreakers, AppSettings.MaxDonationsPerRequestWallBreakers));
                            t2.Troops.Add(TroopModel.CreateNew((int)Troop.Balloon, Troop.Balloon.Name(), AppSettings.TroopsQtyBalloons, AppSettings.IsDonateBalloons, AppSettings.IsDonateAllBalloons, AppSettings.DonateKeywordsBalloons, AppSettings.MaxDonationsPerRequestBalloons));
                            t2.Troops.Add(TroopModel.CreateNew((int)Troop.Wizard, Troop.Wizard.Name(), AppSettings.TroopsQtyWizards, AppSettings.IsDonateWizards, AppSettings.IsDonateAllWizards, AppSettings.DonateKeywordsWizards, AppSettings.MaxDonationsPerRequestWizards));
                            break;
                        case TroopType.Tier3:
                            DataCollection.TroopTiers.Add(TroopTierModel.CreateNew((int)tier, TroopType.Tier3.Name()));

                            var t3 = DataCollection.TroopTiers.First(tt => tt.Id == (int)tier);
                            t3.Troops.Add(TroopModel.CreateNew((int)Troop.Healer, Troop.Healer.Name(), AppSettings.TroopsQtyHealers, AppSettings.IsDonateHealers, AppSettings.IsDonateAllHealers, AppSettings.DonateKeywordsHealers, AppSettings.MaxDonationsPerRequestHealers));
                            t3.Troops.Add(TroopModel.CreateNew((int)Troop.Dragon, Troop.Dragon.Name(), AppSettings.TroopsQtyDragons, AppSettings.IsDonateDragons, AppSettings.IsDonateAllDragons, AppSettings.DonateKeywordsDragons, AppSettings.MaxDonationsPerRequestDragons));
                            t3.Troops.Add(TroopModel.CreateNew((int)Troop.Pekka, Troop.Pekka.Name(), AppSettings.TroopsQtyPekkas, AppSettings.IsDonatePekkas, AppSettings.IsDonateAllPekkas, AppSettings.DonateKeywordsPekkas, AppSettings.MaxDonationsPerRequestPekkas));
                            break;
                        case TroopType.DarkTroops:
                            DataCollection.TroopTiers.Add(TroopTierModel.CreateNew((int)tier, TroopType.DarkTroops.Name()));

                            var dt = DataCollection.TroopTiers.First(tt => tt.Id == (int)tier);
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.Minion, Troop.Minion.Name(), AppSettings.TroopsQtyMinions, AppSettings.IsDonateMinions, AppSettings.IsDonateAllMinions, AppSettings.DonateKeywordsMinions, AppSettings.MaxDonationsPerRequestMinions));
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.HogRider, Troop.HogRider.Name(), AppSettings.TroopsQtyHogRiders, AppSettings.IsDonateHogRiders, AppSettings.IsDonateAllHogRiders, AppSettings.DonateKeywordsHogRiders, AppSettings.MaxDonationsPerRequestHogRiders));
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.Valkyrie, Troop.Valkyrie.Name(), AppSettings.TroopsQtyValkyries, AppSettings.IsDonateValkyries, AppSettings.IsDonateAllValkyries, AppSettings.DonateKeywordsValkyries, AppSettings.MaxDonationsPerRequestValkyries));
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.Golem, Troop.Golem.Name(), AppSettings.TroopsQtyGolems, AppSettings.IsDonateGolems, AppSettings.IsDonateAllGolems, AppSettings.DonateKeywordsGolems, AppSettings.MaxDonationsPerRequestGolems));
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.Witch, Troop.Witch.Name(), AppSettings.TroopsQtyWitches, AppSettings.IsDonateWitches, AppSettings.IsDonateAllWitches, AppSettings.DonateKeywordsWitches, AppSettings.MaxDonationsPerRequestWitches));
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.LavaHound, Troop.LavaHound.Name(), AppSettings.TroopsQtyLavaHounds, AppSettings.IsDonateLavaHounds, AppSettings.IsDonateAllLavaHounds, AppSettings.DonateKeywordsLavaHounds, AppSettings.MaxDonationsPerRequestLavaHounds));
                            break;
                        default:
                            // Troop Type Heroes, do nothing!
                            break;
                    }
                }
            }

            // Fill the Troop Compositions
            if (DataCollection.TroopCompositions.Count == 0)
            {
                foreach (var tc in Enum.GetValues(typeof(TroopComposition)))
                {
                    DataCollection.TroopCompositions.Add(Model.CreateNew((int)tc, ((TroopComposition)tc).Name()));
                }
            }

            // Fill the Barracks Troops
            if (DataCollection.BarracksTroops.Count == 0)
            {
                DataCollection.BarracksTroops.Add(Model.CreateNew((int)Troop.Barbarian, Properties.Resources.Barbarians));
                DataCollection.BarracksTroops.Add(Model.CreateNew((int)Troop.Archer, Properties.Resources.Archers));
                DataCollection.BarracksTroops.Add(Model.CreateNew((int)Troop.Goblin, Properties.Resources.Goblins));
                DataCollection.BarracksTroops.Add(Model.CreateNew((int)Troop.Giant, Properties.Resources.Giants));
                DataCollection.BarracksTroops.Add(Model.CreateNew((int)Troop.WallBreaker, Properties.Resources.WallBreakers));
                DataCollection.BarracksTroops.Add(Model.CreateNew((int)Troop.Balloon, Properties.Resources.Balloons));
                DataCollection.BarracksTroops.Add(Model.CreateNew((int)Troop.Wizard, Properties.Resources.Wizards));
                DataCollection.BarracksTroops.Add(Model.CreateNew((int)Troop.Healer, Properties.Resources.Healers));
                DataCollection.BarracksTroops.Add(Model.CreateNew((int)Troop.Dragon, Properties.Resources.Dragons));
                DataCollection.BarracksTroops.Add(Model.CreateNew((int)Troop.Pekka, Properties.Resources.Pekkas));
            }

            // Fill the Dark Barracks Troops
            if (DataCollection.DarkBarracksTroops.Count == 0)
            {
                DataCollection.DarkBarracksTroops.Add(Model.CreateNew((int)Troop.Minion, Properties.Resources.Minions));
                DataCollection.DarkBarracksTroops.Add(Model.CreateNew((int)Troop.HogRider, Properties.Resources.HogRiders));
                DataCollection.DarkBarracksTroops.Add(Model.CreateNew((int)Troop.Valkyrie, Properties.Resources.Valkyries));
                DataCollection.DarkBarracksTroops.Add(Model.CreateNew((int)Troop.Golem, Properties.Resources.Golems));
                DataCollection.DarkBarracksTroops.Add(Model.CreateNew((int)Troop.Witch, Properties.Resources.Witches));
                DataCollection.DarkBarracksTroops.Add(Model.CreateNew((int)Troop.LavaHound, Properties.Resources.LavaHounds));
            }

			// Fill Building Locations
			if (DataCollection.BuildingPoints.Count == 0)
			{
				foreach (var bt in Enum.GetValues(typeof(Building)))
				{
					switch ((Building)bt)
					{
						case Building.TownHall:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.TownHall, BuildingType.Other, AppSettings.LocationTownHall.ToPOINT()));
							break;
						case Building.ClanCastle:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.ClanCastle, BuildingType.Other, AppSettings.LocationClanCastle.ToPOINT()));
							break;
						case Building.Barrack1:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Barrack1, BuildingType.Extractor, AppSettings.LocationBarrack1.ToPOINT()));
							break;
						case Building.Barrack2:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Barrack2, BuildingType.Extractor, AppSettings.LocationBarrack2.ToPOINT()));
							break;
						case Building.Barrack3:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Barrack3, BuildingType.Extractor, AppSettings.LocationBarrack3.ToPOINT()));
							break;
						case Building.Barrack4:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Barrack4, BuildingType.Extractor, AppSettings.LocationBarrack4.ToPOINT()));
							break;
						case Building.DarkBarrack1:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.DarkBarrack1, BuildingType.Extractor, AppSettings.LocationDarkBarrack1.ToPOINT()));
							break;
						case Building.DarkBarrack2:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.DarkBarrack2, BuildingType.Extractor, AppSettings.LocationDarkBarrack2.ToPOINT()));
							break;
						case Building.Elixir1:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Elixir1, BuildingType.Extractor, AppSettings.LocationElixir1.ToPOINT()));
							break;
						case Building.Elixir2:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Elixir2, BuildingType.Extractor, AppSettings.LocationElixir2.ToPOINT()));
							break;
						case Building.Elixir3:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Elixir3, BuildingType.Extractor, AppSettings.LocationElixir3.ToPOINT()));
							break;
						case Building.Elixir4:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Elixir4, BuildingType.Extractor, AppSettings.LocationElixir4.ToPOINT()));
							break;
						case Building.Elixir5:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Elixir5, BuildingType.Extractor, AppSettings.LocationElixir5.ToPOINT()));
							break;
						case Building.Elixir6:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Elixir6, BuildingType.Extractor, AppSettings.LocationElixir6.ToPOINT()));
							break;
						case Building.Elixir7:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Elixir7, BuildingType.Extractor, AppSettings.LocationElixir7.ToPOINT()));
							break;
						case Building.Gold1:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Gold1, BuildingType.Extractor, AppSettings.LocationGold1.ToPOINT()));
							break;
						case Building.Gold2:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Gold2, BuildingType.Extractor, AppSettings.LocationGold2.ToPOINT()));
							break;
						case Building.Gold3:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Gold3, BuildingType.Extractor, AppSettings.LocationGold3.ToPOINT()));
							break;
						case Building.Gold4:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Gold4, BuildingType.Extractor, AppSettings.LocationGold4.ToPOINT()));
							break;
						case Building.Gold5:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Gold5, BuildingType.Extractor, AppSettings.LocationGold5.ToPOINT()));
							break;
						case Building.Gold6:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Gold6, BuildingType.Extractor, AppSettings.LocationGold6.ToPOINT()));
							break;
						case Building.Gold7:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Gold7, BuildingType.Extractor, AppSettings.LocationGold7.ToPOINT()));
							break;
						case Building.Drill1:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Drill1, BuildingType.Extractor, AppSettings.LocationDrill1.ToPOINT()));
							break;
						case Building.Drill2:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Drill2, BuildingType.Extractor, AppSettings.LocationDrill2.ToPOINT()));
							break;
						case Building.Drill3:
							DataCollection.BuildingPoints.Add(BuildingPointModel.CreateNew(Building.Drill3, BuildingType.Extractor, AppSettings.LocationDrill3.ToPOINT()));
							break;
						default:
							// All other building types excluded!
							break;
					}
				}
			}
			GlobalVariables.Log.WriteToLog(Properties.Resources.LogBotInitialized);
        }

        #endregion

        #region Main Methods

        /// <summary>
        /// Starts or Stops the bot execution.
        /// </summary>
        private void StartStop()
        {
            if (!IsExecuting)
            {
                IsExecuting = true;
                Start();
            }
            else
            {
                IsExecuting = false;

				if (IsBlueStacksHidden)
				{
					HideRestoreBlueStacks();
					OnPropertyChanged(() => HideRestoreBlueStacksState);
				}

                Stop();
            }
        }

		/// <summary>
		/// Hides or Restores BlueStacks.
		/// </summary>
		private void HideRestoreBlueStacks()
		{
			if (!IsBlueStacksHidden)
			{
				IsBlueStacksHidden = true;
				HideBlueStacks();
			}
			else
			{
				IsBlueStacksHidden = false;
				RestoreBlueStacks();
			}
		}

        /// <summary>
        /// Starts the bot functionality
        /// </summary>
        private void Start()
        {
			Main.Initialize(this); // <--- Main entry point
        }

        /// <summary>
        /// Stops the bot functionality
        /// </summary>
        private void Stop()
        {
			WriteToOutput(Properties.Resources.OutputBotIsStopping, GlobalVariables.OutputStates.Information);
            
            // Any extra stuff we need to do goes here!

            WriteToOutput(Properties.Resources.OutputBotStopped, GlobalVariables.OutputStates.Verified);
        }

        /// <summary>
        /// Hide BlueStacks
        /// </summary>
        private void HideBlueStacks()
        {
			WriteToOutput(Properties.Resources.OutputHideBlueStacks, GlobalVariables.OutputStates.Information);
			BlueStacksHelper.HideBlueStacks();
        }

		/// <summary>
		/// Show BlueStacks
		/// </summary>
		private void RestoreBlueStacks()
		{
			WriteToOutput(Properties.Resources.OutputRestoreBlueStacks, GlobalVariables.OutputStates.Information);
			BlueStacksHelper.RestoreBlueStacks();
		}

		#endregion

		#region App Specific

		private void MouseDownDragAndMoveWindow(MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
				Application.Current.MainWindow.DragMove();
		}

		/// <summary>
		/// Minimizes to application to tray.
		/// </summary>
		private void MinimizeToTray()
		{
			Notify(Properties.Resources.NotificationRunningInTray);
			Application.Current.MainWindow.Hide();
		}

		/// <summary>
		/// Minimizes the application.
		/// </summary>
		private void Minimize()
		{
			Application.Current.MainWindow.WindowState = WindowState.Minimized;
		}

		/// <summary>
		/// Exits the application.
		/// </summary>
		private void Exit()
		{
			if (IsExecuting)
				Stop();

			// Remember Window Position
			AppSettings.WindowPlacement = UI.Utils.WindowPlacementState.GetPlacement(new System.Windows.Interop.WindowInteropHelper(Application.Current.MainWindow).Handle);

			SaveUserSettings();
			Application.Current.MainWindow.Close();
		}

		#endregion

		#region Output and Notify Methods

		/// <summary>
		/// Clears the Output.
		/// </summary>
		public void ClearOutput()
		{
			_outputProcessed = null;
			Output = null;
		}

		private XNamespace ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
		private static string _outputProcessed;

		/*
		 * For reference only: How an RTF looks in Xml format
		 * Just in case there's something else to add, we know the attributes
		 * 
		 * <Section xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space="preserve" TextAlignment="Left" LineHeight="Auto" 
		 *		IsHyphenationEnabled="False" xml:lang="en-us" FlowDirection="LeftToRight" NumberSubstitution.CultureSource="User" 
		 *		NumberSubstitution.Substitution="AsCulture" FontFamily="Segoe UI" FontStyle="Normal" FontWeight="Normal" FontStretch="Normal" 
		 *		FontSize="12" Foreground="#FFDDDDDD" Typography.StandardLigatures="True" Typography.ContextualLigatures="True" 
		 *		Typography.DiscretionaryLigatures="False" Typography.HistoricalLigatures="False" Typography.AnnotationAlternates="0" 
		 *		Typography.ContextualAlternates="True" Typography.HistoricalForms="False" Typography.Kerning="True" 
		 *		Typography.CapitalSpacing="False" Typography.CaseSensitiveForms="False" Typography.StylisticSet1="False" 
		 *		Typography.StylisticSet2="False" Typography.StylisticSet3="False" Typography.StylisticSet4="False" 
		 *		Typography.StylisticSet5="False" Typography.StylisticSet6="False" Typography.StylisticSet7="False" 
		 *		Typography.StylisticSet8="False" Typography.StylisticSet9="False" Typography.StylisticSet10="False" 
		 *		Typography.StylisticSet11="False" Typography.StylisticSet12="False" Typography.StylisticSet13="False" 
		 *		Typography.StylisticSet14="False" Typography.StylisticSet15="False" Typography.StylisticSet16="False" 
		 *		Typography.StylisticSet17="False" Typography.StylisticSet18="False" Typography.StylisticSet19="False" 
		 *		Typography.StylisticSet20="False" Typography.Fraction="Normal" Typography.SlashedZero="False" 
		 *		Typography.MathematicalGreek="False" Typography.EastAsianExpertForms="False" Typography.Variants="Normal" 
		 *		Typography.Capitals="Normal" Typography.NumeralStyle="Normal" Typography.NumeralAlignment="Normal" 
		 *		Typography.EastAsianWidths="Normal" Typography.EastAsianLanguage="Normal" Typography.StandardSwashes="0" 
		 *		Typography.ContextualSwashes="0" Typography.StylisticAlternates="0">
		 *		<Paragraph>
		 *			<Run Foreground="red">[22:43:04] this is a another another title</Run>
		 *		</Paragraph>
		 *	</Section>"
		 */

		/// <summary>
		/// Writes to the Output.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="brush">The SolidColorBrush.</param>
		/// <param name="fontWight">The density of the typeface.</param>
		private void WriteToOutput(string message, SolidColorBrush brush, FontWeight fontWight)
		{
			XElement root;
			if (string.IsNullOrEmpty(_outputProcessed))
			{
				// Create Empty Xml for the RTF
				root = new XElement(ns + "Section", new XAttribute(XNamespace.Xml + "space", "preserve"));
			}
			else
			{
				// We read the Xml we already have
				var doc = XDocument.Parse(_outputProcessed);
				root = doc.Root;
			}

			var par = new XElement(ns + "Paragraph");

			var run = new XElement(ns + "Run");

			if (brush != null)
				run.Attr("Foreground", brush);

			if (fontWight != null)
			{
				// Make the time output font weight consistent
				if (fontWight == FontWeights.Normal)
				{
					run.Attr("FontWeight", fontWight);

					run.Value = string.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, message);
					par.Add(run);
				}
				else
				{
					run.Value = string.Format("[{0:HH:mm:ss}] ", DateTime.Now);
					par.Add(run);

					run = new XElement(ns + "Run");

					if (brush != null)
						run.Attr("Foreground", brush);

					run.Attr("FontWeight", fontWight);

					run.Value = message;
					par.Add(run);
				}
			}
			else
			{
				run.Value = string.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, message);
				par.Add(run);
			}

			root.Add(par);

			var reader = root.CreateReader();
			reader.MoveToContent();

			Output = _outputProcessed = reader.ReadOuterXml();

			Message = message;
			GlobalVariables.Log.WriteToLog(message);
		}

		/// <summary>
		/// Writes to the output.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="state">The output state.</param>
		public void WriteToOutput(string message, GlobalVariables.OutputStates state = GlobalVariables.OutputStates.Normal)
		{
			switch (state)
			{
				case GlobalVariables.OutputStates.Normal:
					WriteToOutput(message, null, FontWeights.Normal);
					break;
				case GlobalVariables.OutputStates.Information:
					WriteToOutput(message, Brushes.RoyalBlue, FontWeights.Normal);
					break;
				case GlobalVariables.OutputStates.Verified:
					WriteToOutput(message, Brushes.SeaGreen, FontWeights.Normal);
					break;
				case GlobalVariables.OutputStates.Warning:
					WriteToOutput(message, Brushes.Khaki, FontWeights.Normal);
					break;
				case GlobalVariables.OutputStates.Error:
					WriteToOutput(message, Brushes.Salmon, FontWeights.Bold);
					break;
			}
		}

		/// <summary>
		/// Send a Notify using specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Notify(string message)
		{
			//var notifyService = GetService<INotifyService>();
			//if (notifyService != null)
			//	notifyService.Notify(message);

			GetService<INotifyService>().Notify(message);
		}

        #endregion

		#region Application User Settings Methods

		/// <summary>
        /// Gets the application user settings.
        /// </summary>
        private void GetUserSettings()
        {
            // General
            MaxTrophies = AppSettings.MaxTrophies;

            // Search Settings
            IsMeetGold = AppSettings.IsMeetGold;
            IsMeetElixir = AppSettings.IsMeetElixir;
            IsMeetDarkElixir = AppSettings.IsMeetDarkElixir;
            IsMeetTrophyCount = AppSettings.IsMeetTrophyCount;
            IsMeetTownhallLevel = AppSettings.IsMeetTownhallLevel;

            MinimumGold = AppSettings.MinGold;
            MinimumElixir = AppSettings.MinElixir;
            MinimumDarkElixir = AppSettings.MinDarkElixir;
            MinimumTrophyCount = AppSettings.MinTrophyCount;
            MaximumTownhallLevel = AppSettings.MinTownhallLevel;

            IsAlertWhenBaseFound = AppSettings.IsAlertWhenBaseFound;
			IsTakeSnapshotAllTowns = AppSettings.IsTakeSnapshotAllTowns;
			IsTakeSnapshotAllLoots = AppSettings.IsTakeSnapshotAllLoots;

            // Attack Settings
            SelectedMaxCannonLevel = AppSettings.MaxCannonLevel;
            SelectedMaxArcherTowerLevel = AppSettings.MaxArcherTowerLevel;
            SelectedMaxMortarLevel = AppSettings.MaxMortarLevel;
            SelectedMaxWizardTowerLevel = AppSettings.MaxWizardTowerLevel;
            SelectedMaxXbowLevel = AppSettings.MaxXbowLevel;

            IsAttackTheirKing = AppSettings.IsAttackTheirKing;
            IsAttackTheirQueen = AppSettings.IsAttackTheirQueen;

            SelectedAttackMode = (AttackMode)AppSettings.SelectedAttackMode;

            SelectedKingAttackMode = (HeroAttackMode)AppSettings.SelectedKingAttackMode;
            SelectedQueenAttackMode = (HeroAttackMode)AppSettings.SelectedQueenAttackMode;
            IsAttackUsingClanCastle = AppSettings.IsAttackUsingClanCastle;

            SelectedDeployStrategy = DataCollection.DeployStrategies.Where(ds => ds.Id == AppSettings.SelectedDeployStrategy).DefaultIfEmpty(DataCollection.DeployStrategies.Last()).First();
            SelectedDeployTroop = DataCollection.DeployTroops.Where(dt => dt.Id == AppSettings.SelectedDeployTroop).DefaultIfEmpty(DataCollection.DeployTroops.First()).First();
            IsAttackTownhall = AppSettings.IsAttackTownhall;

            // Troop Settings
            SelectedTroopComposition = DataCollection.TroopCompositions.Where(tc => tc.Id == AppSettings.SelectedTroopComposition).DefaultIfEmpty(DataCollection.TroopCompositions.First()).First();

            IsUseBarracks1 = AppSettings.IsUseBarracks1;
            IsUseBarracks2 = AppSettings.IsUseBarracks2;
            IsUseBarracks3 = AppSettings.IsUseBarracks3;
            IsUseBarracks4 = AppSettings.IsUseBarracks4;

            SelectedBarrack1 = DataCollection.BarracksTroops.Where(b1 => b1.Id == AppSettings.SelectedBarrack1).DefaultIfEmpty(DataCollection.BarracksTroops.First()).First();
            SelectedBarrack2 = DataCollection.BarracksTroops.Where(b2 => b2.Id == AppSettings.SelectedBarrack2).DefaultIfEmpty(DataCollection.BarracksTroops.First()).First();
            SelectedBarrack3 = DataCollection.BarracksTroops.Where(b3 => b3.Id == AppSettings.SelectedBarrack3).DefaultIfEmpty(DataCollection.BarracksTroops.First()).First();
            SelectedBarrack4 = DataCollection.BarracksTroops.Where(b4 => b4.Id == AppSettings.SelectedBarrack4).DefaultIfEmpty(DataCollection.BarracksTroops.First()).First();

            IsUseDarkBarracks1 = AppSettings.IsUseDarkBarracks1;
            IsUseDarkBarracks2 = AppSettings.IsUseDarkBarracks2;

            SelectedDarkBarrack1 = DataCollection.DarkBarracksTroops.Where(b1 => b1.Id == AppSettings.SelectedDarkBarrack1).DefaultIfEmpty(DataCollection.DarkBarracksTroops.First()).First();
            SelectedDarkBarrack2 = DataCollection.DarkBarracksTroops.Where(b2 => b2.Id == AppSettings.SelectedDarkBarrack2).DefaultIfEmpty(DataCollection.DarkBarracksTroops.First()).First();

			// Wave Settings
			IsCustomWave = AppSettings.IsCustomWave;

            // Donate Settings
            IsRequestTroops = AppSettings.IsRequestTroops;
            RequestTroopsMessage = AppSettings.RequestTroopsMessage;

			// Other Settings
			IsRearmTraps = AppSettings.IsRearmTraps;
			IsRearmXbows = AppSettings.IsRearmXbows;
			IsRearmInfernos = AppSettings.IsRearmInfernos;
        }

        /// <summary>
        /// Saves the application user settings.
        /// </summary>
        private void SaveUserSettings()
        {
            // General
            AppSettings.MaxTrophies = MaxTrophies;

            // Search Settings
            AppSettings.IsMeetGold = IsMeetGold;
            AppSettings.IsMeetElixir = IsMeetElixir;
            AppSettings.IsMeetDarkElixir = IsMeetDarkElixir;
            AppSettings.IsMeetTrophyCount = IsMeetTrophyCount;
            AppSettings.IsMeetTownhallLevel = IsMeetTownhallLevel;

            AppSettings.MinGold = MinimumGold;
            AppSettings.MinElixir = MinimumElixir;
            AppSettings.MinDarkElixir = MinimumDarkElixir;
            AppSettings.MinTrophyCount = MinimumTrophyCount;
            AppSettings.MinTownhallLevel = MaximumTownhallLevel;

            AppSettings.IsAlertWhenBaseFound = IsAlertWhenBaseFound;
			AppSettings.IsTakeSnapshotAllTowns = IsTakeSnapshotAllTowns;
			AppSettings.IsTakeSnapshotAllLoots = IsTakeSnapshotAllLoots;

            // Attack Settings
            AppSettings.MaxCannonLevel = SelectedMaxCannonLevel;
            AppSettings.MaxArcherTowerLevel = SelectedMaxArcherTowerLevel;
            AppSettings.MaxMortarLevel = SelectedMaxMortarLevel;
            AppSettings.MaxWizardTowerLevel = SelectedMaxWizardTowerLevel;
            AppSettings.MaxXbowLevel = SelectedMaxXbowLevel;

            AppSettings.IsAttackTheirKing = IsAttackTheirKing;
            AppSettings.IsAttackTheirQueen = IsAttackTheirQueen;

            AppSettings.SelectedAttackMode = (int)SelectedAttackMode;

            AppSettings.SelectedKingAttackMode = (int)SelectedKingAttackMode;
            AppSettings.SelectedQueenAttackMode = (int)SelectedQueenAttackMode;
            AppSettings.IsAttackUsingClanCastle = IsAttackUsingClanCastle;

            AppSettings.SelectedDeployStrategy = SelectedDeployStrategy.Id;
            AppSettings.SelectedDeployTroop = SelectedDeployTroop.Id;
            AppSettings.IsAttackTownhall = IsAttackTownhall;

            // Troop Settings
            AppSettings.SelectedTroopComposition = SelectedTroopComposition.Id;

            AppSettings.IsUseBarracks1 = IsUseBarracks1;
            AppSettings.IsUseBarracks2 = IsUseBarracks2;
            AppSettings.IsUseBarracks3 = IsUseBarracks3;
            AppSettings.IsUseBarracks4 = IsUseBarracks4;

            AppSettings.SelectedBarrack1 = SelectedBarrack1.Id;
            AppSettings.SelectedBarrack2 = SelectedBarrack2.Id;
            AppSettings.SelectedBarrack3 = SelectedBarrack3.Id;
            AppSettings.SelectedBarrack4 = SelectedBarrack4.Id;

            AppSettings.IsUseDarkBarracks1 = IsUseDarkBarracks1;
            AppSettings.IsUseDarkBarracks2 = IsUseDarkBarracks2;

            AppSettings.SelectedDarkBarrack1 = SelectedDarkBarrack1.Id;
            AppSettings.SelectedDarkBarrack2 = SelectedDarkBarrack2.Id;

			// Wave Settings
			AppSettings.IsCustomWave = IsCustomWave;

            // Donate Settings
            AppSettings.IsRequestTroops = IsRequestTroops;
            AppSettings.RequestTroopsMessage = RequestTroopsMessage;

            foreach (var tier in Enum.GetValues(typeof(TroopType)))
            {
                switch ((TroopType)tier)
                {
                    case TroopType.Tier1:
                        AppSettings.TroopsQtyBarbarians = DataCollection.TroopTiers[(int)tier].Troops[Troop.Barbarian].TrainQuantity;
                        AppSettings.TroopsQtyArchers = DataCollection.TroopTiers[(int)tier].Troops[Troop.Archer].TrainQuantity;
                        AppSettings.TroopsQtyGoblins = DataCollection.TroopTiers[(int)tier].Troops[Troop.Goblin].TrainQuantity;

                        AppSettings.IsDonateAllBarbarians = DataCollection.TroopTiers[(int)tier].Troops[Troop.Barbarian].IsDonateAll;
                        AppSettings.IsDonateAllArchers = DataCollection.TroopTiers[(int)tier].Troops[Troop.Archer].IsDonateAll;
                        AppSettings.IsDonateAllGoblins = DataCollection.TroopTiers[(int)tier].Troops[Troop.Goblin].IsDonateAll;

                        AppSettings.DonateKeywordsBarbarians = DataCollection.TroopTiers[(int)tier].Troops[Troop.Barbarian].DonateKeywords;
                        AppSettings.DonateKeywordsArchers = DataCollection.TroopTiers[(int)tier].Troops[Troop.Archer].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[Troop.Goblin].DonateKeywords;
                        
                        AppSettings.MaxDonationsPerRequestBarbarians = DataCollection.TroopTiers[(int)tier].Troops[Troop.Barbarian].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestArchers = DataCollection.TroopTiers[(int)tier].Troops[Troop.Archer].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestGoblins = DataCollection.TroopTiers[(int)tier].Troops[Troop.Goblin].MaxDonationsPerRequest;
                        break;
                    case TroopType.Tier2:
                        AppSettings.TroopsQtyGiants = DataCollection.TroopTiers[(int)tier].Troops[Troop.Giant].TrainQuantity;
                        AppSettings.TroopsQtyWallBreakers = DataCollection.TroopTiers[(int)tier].Troops[Troop.WallBreaker].TrainQuantity;
                        AppSettings.TroopsQtyBalloons = DataCollection.TroopTiers[(int)tier].Troops[Troop.Balloon].TrainQuantity;
                        AppSettings.TroopsQtyWizards = DataCollection.TroopTiers[(int)tier].Troops[Troop.Wizard].TrainQuantity;

                        AppSettings.IsDonateAllGiants = DataCollection.TroopTiers[(int)tier].Troops[Troop.Giant].IsDonateAll;
                        AppSettings.IsDonateAllWallBreakers = DataCollection.TroopTiers[(int)tier].Troops[Troop.WallBreaker].IsDonateAll;
                        AppSettings.IsDonateAllBalloons = DataCollection.TroopTiers[(int)tier].Troops[Troop.Balloon].IsDonateAll;
                        AppSettings.IsDonateAllWizards = DataCollection.TroopTiers[(int)tier].Troops[Troop.Wizard].IsDonateAll;

                        AppSettings.DonateKeywordsBarbarians = DataCollection.TroopTiers[(int)tier].Troops[Troop.Giant].DonateKeywords;
                        AppSettings.DonateKeywordsArchers = DataCollection.TroopTiers[(int)tier].Troops[Troop.WallBreaker].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[Troop.Balloon].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[Troop.Wizard].DonateKeywords;

                        AppSettings.MaxDonationsPerRequestGiants = DataCollection.TroopTiers[(int)tier].Troops[Troop.Giant].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestWallBreakers = DataCollection.TroopTiers[(int)tier].Troops[Troop.WallBreaker].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestBalloons = DataCollection.TroopTiers[(int)tier].Troops[Troop.Balloon].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestWizards = DataCollection.TroopTiers[(int)tier].Troops[Troop.Wizard].MaxDonationsPerRequest;
                        break;
                    case TroopType.Tier3:
                        AppSettings.TroopsQtyHealers = DataCollection.TroopTiers[(int)tier].Troops[Troop.Healer].TrainQuantity;
                        AppSettings.TroopsQtyDragons = DataCollection.TroopTiers[(int)tier].Troops[Troop.Dragon].TrainQuantity;
                        AppSettings.TroopsQtyPekkas = DataCollection.TroopTiers[(int)tier].Troops[Troop.Pekka].TrainQuantity;

                        AppSettings.IsDonateAllHealers = DataCollection.TroopTiers[(int)tier].Troops[Troop.Healer].IsDonateAll;
                        AppSettings.IsDonateAllDragons = DataCollection.TroopTiers[(int)tier].Troops[Troop.Dragon].IsDonateAll;
                        AppSettings.IsDonateAllPekkas = DataCollection.TroopTiers[(int)tier].Troops[Troop.Pekka].IsDonateAll;

                        AppSettings.DonateKeywordsBarbarians = DataCollection.TroopTiers[(int)tier].Troops[Troop.Healer].DonateKeywords;
                        AppSettings.DonateKeywordsArchers = DataCollection.TroopTiers[(int)tier].Troops[Troop.Dragon].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[Troop.Pekka].DonateKeywords;

                        AppSettings.MaxDonationsPerRequestHealers = DataCollection.TroopTiers[(int)tier].Troops[Troop.Healer].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestDragons = DataCollection.TroopTiers[(int)tier].Troops[Troop.Dragon].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestPekkas = DataCollection.TroopTiers[(int)tier].Troops[Troop.Pekka].MaxDonationsPerRequest;
                        break;
                    case TroopType.DarkTroops:
                        AppSettings.TroopsQtyMinions = DataCollection.TroopTiers[(int)tier].Troops[Troop.Minion].TrainQuantity;
                        AppSettings.TroopsQtyHogRiders = DataCollection.TroopTiers[(int)tier].Troops[Troop.HogRider].TrainQuantity;
                        AppSettings.TroopsQtyValkyries = DataCollection.TroopTiers[(int)tier].Troops[Troop.Valkyrie].TrainQuantity;
                        AppSettings.TroopsQtyGolems = DataCollection.TroopTiers[(int)tier].Troops[Troop.Golem].TrainQuantity;
                        AppSettings.TroopsQtyWitches = DataCollection.TroopTiers[(int)tier].Troops[Troop.Witch].TrainQuantity;
                        AppSettings.TroopsQtyLavaHounds = DataCollection.TroopTiers[(int)tier].Troops[Troop.LavaHound].TrainQuantity;

                        AppSettings.IsDonateAllMinions = DataCollection.TroopTiers[(int)tier].Troops[Troop.Minion].IsDonateAll;
                        AppSettings.IsDonateAllHogRiders = DataCollection.TroopTiers[(int)tier].Troops[Troop.HogRider].IsDonateAll;
                        AppSettings.IsDonateAllValkyries = DataCollection.TroopTiers[(int)tier].Troops[Troop.Valkyrie].IsDonateAll;
                        AppSettings.IsDonateAllGolems = DataCollection.TroopTiers[(int)tier].Troops[Troop.Golem].IsDonateAll;
                        AppSettings.IsDonateAllWitches = DataCollection.TroopTiers[(int)tier].Troops[Troop.Witch].IsDonateAll;
                        AppSettings.IsDonateAllLavaHounds = DataCollection.TroopTiers[(int)tier].Troops[Troop.LavaHound].IsDonateAll;

                        AppSettings.DonateKeywordsBarbarians = DataCollection.TroopTiers[(int)tier].Troops[Troop.Minion].DonateKeywords;
                        AppSettings.DonateKeywordsArchers = DataCollection.TroopTiers[(int)tier].Troops[Troop.HogRider].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[Troop.Valkyrie].DonateKeywords;
                        AppSettings.DonateKeywordsBarbarians = DataCollection.TroopTiers[(int)tier].Troops[Troop.Golem].DonateKeywords;
                        AppSettings.DonateKeywordsArchers = DataCollection.TroopTiers[(int)tier].Troops[Troop.Witch].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[Troop.LavaHound].DonateKeywords;

                        AppSettings.MaxDonationsPerRequestMinions = DataCollection.TroopTiers[(int)tier].Troops[Troop.Minion].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestHogRiders = DataCollection.TroopTiers[(int)tier].Troops[Troop.HogRider].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestValkyries = DataCollection.TroopTiers[(int)tier].Troops[Troop.Valkyrie].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestGolems = DataCollection.TroopTiers[(int)tier].Troops[Troop.Golem].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestWitches = DataCollection.TroopTiers[(int)tier].Troops[Troop.Witch].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestLavaHounds = DataCollection.TroopTiers[(int)tier].Troops[Troop.LavaHound].MaxDonationsPerRequest;
                        break;
                    default:
                        // Troop Type Heroes, do nothing!
                        break;
                }
            }

			// Location Settings
			var defaultPoint = BuildingPointModel.CreateNew(Building.Unknown, BuildingType.Other, new Point());

			AppSettings.LocationTownHall = DataCollection.BuildingPoints.Where(b => b.Building == Building.TownHall).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();

			AppSettings.LocationClanCastle = DataCollection.BuildingPoints.Where(b => b.Building == Building.ClanCastle).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();

			AppSettings.LocationBarrack1 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Barrack1).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationBarrack2 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Barrack2).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationBarrack3 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Barrack3).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationBarrack4 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Barrack4).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();

			AppSettings.LocationDarkBarrack1 = DataCollection.BuildingPoints.Where(b => b.Building == Building.DarkBarrack1).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationDarkBarrack2 = DataCollection.BuildingPoints.Where(b => b.Building == Building.DarkBarrack2).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();

			AppSettings.LocationElixir1 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Elixir1).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationElixir2 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Elixir2).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationElixir3 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Elixir3).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationElixir4 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Elixir4).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationElixir5 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Elixir5).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationElixir6 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Elixir6).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationElixir7 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Elixir7).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();

			AppSettings.LocationGold1 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Gold1).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationGold2 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Gold2).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationGold3 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Gold3).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationGold4 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Gold4).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationGold5 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Gold5).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationGold6 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Gold6).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationGold7 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Gold7).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();

			AppSettings.LocationDrill1 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Drill1).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationDrill2 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Drill2).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();
			AppSettings.LocationDrill3 = DataCollection.BuildingPoints.Where(b => b.Building == Building.Drill3).DefaultIfEmpty(defaultPoint).First().Coordinates.ToPoint();

			// Other Settings
			AppSettings.IsRearmTraps = IsRearmTraps;
			AppSettings.IsRearmXbows = IsRearmXbows;
			AppSettings.IsRearmInfernos = IsRearmInfernos;

            // Save it!
            AppSettings.Save();
        }

        #endregion
	}
}
