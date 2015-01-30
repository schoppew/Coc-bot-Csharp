namespace CoC.Bot.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Input;
  using CoC.Bot.Data;
  using CoC.Bot.Tools;
  using CoC.Bot.Tools.FastFind;
  using CoC.Bot.UI.Commands;

    /// <summary>
    /// Provides functionality for the MainWindow
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private static StringBuilder _output = new StringBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            // Usage Notes for non WPF devs:
            //
            // You access and manipulate the values on the Window/Tabs by accessing their Properties (below). You don't access the controls (-;
            // Ex. MinimumGold, MinimumElixir, etc.
            // Ex. SelectedTroopComposition.Id <--- Returns the selected troop composition ID (attack all sides, etc)
            // Ex. (int)SelectedKingAttackMode <--- Returns the selected king attack mode (dead bases, all bases, none, etc)
            //
            // Under Main Methods you will find the Start(), Stop(), Hide(), etc methods
            // You can make them async if you wish


            // Fill the Troop Compositions
            if (DataCollection.TroopCompositions.Count == 0)
            {
                DataCollection.TroopCompositions.Add(Model.CreateNew(1, Properties.Resources.UseBarracks));
                DataCollection.TroopCompositions.Add(Model.CreateNew(2, Properties.Resources.Barching));
                DataCollection.TroopCompositions.Add(Model.CreateNew(3, Properties.Resources.CustomTroops));
            }

            // Fill the Troops
            if (DataCollection.Troops.Count == 0)
            {
                DataCollection.Troops.Add(Model.CreateNew(1, Properties.Resources.Barbarians));
                DataCollection.Troops.Add(Model.CreateNew(2, Properties.Resources.Archers));
                //DataCollection.Troops.Add(Model.CreateNew(3, Properties.Resources.Goblins));
                //DataCollection.Troops.Add(Model.CreateNew(4, Properties.Resources.Giants));
                //DataCollection.Troops.Add(Model.CreateNew(5, Properties.Resources.WallBreakers));
                //DataCollection.Troops.Add(Model.CreateNew(6, Properties.Resources.Balloons));
                //DataCollection.Troops.Add(Model.CreateNew(7, Properties.Resources.Wizards));
                //DataCollection.Troops.Add(Model.CreateNew(8, Properties.Resources.Healers));
                //DataCollection.Troops.Add(Model.CreateNew(9, Properties.Resources.Dragons));
                //DataCollection.Troops.Add(Model.CreateNew(10, Properties.Resources.Pekkas));
                // add more troop types
            }

            // Fill the Deploy Strategies
            if (DataCollection.DeployStrategies.Count == 0)
            {
                DataCollection.DeployStrategies.Add(Model.CreateNew(1, Properties.Resources.DeployStrategyTwoSides));
                DataCollection.DeployStrategies.Add(Model.CreateNew(2, Properties.Resources.DeployStrategyThreeSides));
                DataCollection.DeployStrategies.Add(Model.CreateNew(3, Properties.Resources.DeployStrategyFourSides));
            }

            // Fill the Deploy Troops
            if (DataCollection.DeployTroops.Count == 0)
            {
                DataCollection.DeployTroops.Add(Model.CreateNew(1, Properties.Resources.DeployTroopsBarbariansAndArchers));
                DataCollection.DeployTroops.Add(Model.CreateNew(2, Properties.Resources.DeployTroopsUseAllTroops));
            }

            GetUserSettings();

            Message = "Click on Start to initialize the bot";
        }

        #region Properties

        public static string AppTitle { get { return string.Format("{0} v{1}", Properties.Resources.AppName, typeof(App).Assembly.GetName().Version.ToString(3)); } }

        #region Behaviour Properties

        private bool IsStarted { get; set; }

        private bool IsHidden { get; set; }

        #endregion

        #region General Properties

        public string Output
        {
            get { return _output.ToString(); }
            set
            {
                _output = new StringBuilder(value);
                OnPropertyChanged("Output");
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        private int _maxTrophies;
        public int MaxTrophies
        {
            get { return _maxTrophies; }
            set
            {
                if (_maxTrophies != value)
                {
                    _maxTrophies = value;
                    OnPropertyChanged("MaxTrophies");
                }
            }
        }

        #endregion

        #region Search Settings Properties

        private bool _meetGold;
        public bool MeetGold
        {
            get { return _meetGold; }
            set
            {
                if (_meetGold != value)
                {
                    _meetGold = value;
                    OnPropertyChanged("MeetGold");
                }
            }
        }

        private bool _meetElixir;
        public bool MeetElixir
        {
            get { return _meetElixir; }
            set
            {
                if (_meetElixir != value)
                {
                    _meetElixir = value;
                    OnPropertyChanged("MeetElixir");
                }
            }
        }

        private bool _meetDarkElixir;
        public bool MeetDarkElixir
        {
            get { return _meetDarkElixir; }
            set
            {
                if (_meetDarkElixir != value)
                {
                    _meetDarkElixir = value;
                    OnPropertyChanged("MeetDarkElixir");
                }
            }
        }

        private bool _meetTrophyCount;
        public bool MeetTrophyCount
        {
            get { return _meetTrophyCount; }
            set
            {
                if (_meetTrophyCount != value)
                {
                    _meetTrophyCount = value;
                    OnPropertyChanged("MeetTrophyCount");
                }
            }
        }

        private int _minimumGold;
        public int MinimumGold
        {
            get { return _minimumGold; }
            set
            {
                if (_minimumGold != value)
                {
                    _minimumGold = value;
                    OnPropertyChanged("MinimumGold");
                }
            }
        }

        private int _minimumElixir;
        public int MinimumElixir
        {
            get { return _minimumElixir; }
            set
            {
                if (_minimumElixir != value)
                {
                    _minimumElixir = value;
                    OnPropertyChanged("MinimumElixir");
                }
            }
        }

        private int _minimumDarkElixir;
        public int MinimumDarkElixir
        {
            get { return _minimumDarkElixir; }
            set
            {
                if (_minimumDarkElixir != value)
                {
                    _minimumDarkElixir = value;
                    OnPropertyChanged("MinimumDarkElixir");
                }
            }
        }

        private int _minimumTrophyCount;
        public int MinimumTrophyCount
        {
            get { return _minimumTrophyCount; }
            set
            {
                if (_minimumTrophyCount != value)
                {
                    _minimumTrophyCount = value;
                    OnPropertyChanged("MinimumTrophyCount");
                }
            }
        }

        private bool _alertWhenBaseFound;
        public bool AlertWhenBaseFound
        {
            get { return _alertWhenBaseFound; }
            set
            {
                if (_alertWhenBaseFound != value)
                {
                    _alertWhenBaseFound = value;
                    OnPropertyChanged("AlertWhenBaseFound");
                }
            }
        }

        #endregion

        #region Attack Settings Properties

        public static IEnumerable<int> CannonLevels { get { return BuildingLevels.Cannon; } }
        public static IEnumerable<int> ArcherTowerLevels { get { return BuildingLevels.ArcherTower; } }
        public static IEnumerable<int> MortarLevels { get { return BuildingLevels.Mortar; } }
        public static IEnumerable<int> WizardTowerLevels { get { return BuildingLevels.WizardTower; } }
        public static IEnumerable<int> XbowLevels { get { return BuildingLevels.Xbow; } }

        private int _selectedMaxCannonLevel;
        public int SelectedMaxCannonLevel
        {
            get { return _selectedMaxCannonLevel; }
            set
            {
                if (_selectedMaxCannonLevel != value)
                {
                    _selectedMaxCannonLevel = value;
                    OnPropertyChanged("SelectedMaxCannonLevel");
                }
            }
        }

        private int _selectedMaxArcherTowerLevel;
        public int SelectedMaxArcherTowerLevel
        {
            get { return _selectedMaxArcherTowerLevel; }
            set
            {
                if (_selectedMaxArcherTowerLevel != value)
                {
                    _selectedMaxArcherTowerLevel = value;
                    OnPropertyChanged("SelectedMaxArcherTowerLevel");
                }
            }
        }

        private int _selectedMaxMortarLevel;
        public int SelectedMaxMortarLevel
        {
            get { return _selectedMaxMortarLevel; }
            set
            {
                if (_selectedMaxMortarLevel != value)
                {
                    _selectedMaxMortarLevel = value;
                    OnPropertyChanged("SelectedMaxMortarLevel");
                }
            }
        }

        private int _selectedWizardTowerLevel;
        public int SelectedWizardTowerLevel
        {
            get { return _selectedWizardTowerLevel; }
            set
            {
                if (_selectedWizardTowerLevel != value)
                {
                    _selectedWizardTowerLevel = value;
                    OnPropertyChanged("SelectedWizardTowerLevel");
                }
            }
        }

        private int _selectedXbowLevel;
        public int SelectedXbowLevel
        {
            get { return _selectedXbowLevel; }
            set
            {
                if (_selectedXbowLevel != value)
                {
                    _selectedXbowLevel = value;
                    OnPropertyChanged("SelectedXbowLevel");
                }
            }
        }

        private bool _attackTheirKing;
        public bool AttackTheirKing
        {
            get { return _attackTheirKing; }
            set
            {
                if (_attackTheirKing != value)
                {
                    _attackTheirKing = value;
                    OnPropertyChanged("AttackTheirKing");
                }
            }
        }

        private bool _attackTheirQueen;
        public bool AttackTheirQueen
        {
            get { return _attackTheirQueen; }
            set
            {
                if (_attackTheirQueen != value)
                {
                    _attackTheirQueen = value;
                    OnPropertyChanged("AttackTheirQueen");
                }
            }
        }

        private AttackMode _selectedAttackMode;
        public AttackMode SelectedAttackMode
        {
            get { return _selectedAttackMode; }
            set
            {
                if (_selectedAttackMode != value)
                {
                    _selectedAttackMode = value;
                    OnPropertyChanged("SelectedAttackMode");
                }
            }
        }

        private HeroAttackMode _selectedKingAttackMode;
        public HeroAttackMode SelectedKingAttackMode
        {
            get { return _selectedKingAttackMode; }
            set
            {
                if (_selectedKingAttackMode != value)
                {
                    _selectedKingAttackMode = value;
                    OnPropertyChanged("SelectedKingAttackMode");
                }
            }
        }

        private HeroAttackMode _selectedQueenAttackMode;
        public HeroAttackMode SelectedQueenAttackMode
        {
            get { return _selectedQueenAttackMode; }
            set
            {
                if (_selectedQueenAttackMode != value)
                {
                    _selectedQueenAttackMode = value;
                    OnPropertyChanged("SelectedQueenAttackMode");
                }
            }
        }

        /// <summary>
        /// Gets the Deploy Strategies.
        /// </summary>
        /// <value>The Deploy Strategies.</value>
        public static BindingList<Model> DeployStrategies { get { return DataCollection.DeployStrategies; } }

        private Model _selectedDeployStrategy;
        public Model SelectedDeployStrategy
        {
            get { return _selectedDeployStrategy; }
            set
            {
                if (_selectedDeployStrategy != value)
                {
                    _selectedDeployStrategy = value;
                    OnPropertyChanged("SelectedDeployStrategy");
                }
            }
        }

        /// <summary>
        /// Gets the Deploy Troops.
        /// </summary>
        /// <value>The Deploy Troops.</value>
        public static BindingList<Model> DeployTroops { get { return DataCollection.DeployTroops; } }

        private Model _selectedDeployTroop;
        public Model SelectedDeployTroop
        {
            get { return _selectedDeployTroop; }
            set
            {
                if (_selectedDeployTroop != value)
                {
                    _selectedDeployTroop = value;
                    OnPropertyChanged("SelectedDeployTroop");
                }
            }
        }

        private bool _attackTownhall;
        public bool AttackTownhall
        {
            get { return _attackTownhall; }
            set
            {
                if (_attackTownhall != value)
                {
                    _attackTownhall = value;
                    OnPropertyChanged("AttackTownhall");
                }
            }
        }

        private bool _attackUsingClanCastle;
        public bool AttackUsingClanCastle
        {
            get { return _attackUsingClanCastle; }
            set
            {
                if (_attackUsingClanCastle != value)
                {
                    _attackUsingClanCastle = value;
                    OnPropertyChanged("AttackUsingClanCastle");
                }
            }
        }

        #endregion

        #region Donate Settings Properties



        #endregion

        #region Troop Settings Properties

        private int _barbariansPercent;
        public int BarbariansPercent
        {
            get { return _barbariansPercent; }
            set
            {
                if (_barbariansPercent != value)
                {
                    _barbariansPercent = value;
                    OnPropertyChanged("BarbariansPercent");
                    OnPropertyChanged("TotalPercent");
                }
            }
        }

        private int _archersPercent;
        public int ArchersPercent
        {
            get { return _archersPercent; }
            set
            {
                if (_archersPercent != value)
                {
                    _archersPercent = value;
                    OnPropertyChanged("ArchersPercent");
                    OnPropertyChanged("TotalPercent");
                }
            }
        }

        private int _goblinsPercent;
        public int GoblinsPercent
        {
            get { return _goblinsPercent; }
            set
            {
                if (_goblinsPercent != value)
                {
                    _goblinsPercent = value;
                    OnPropertyChanged("GoblinsPercent");
                    OnPropertyChanged("TotalPercent");
                }
            }
        }

        public int TotalPercent
        {
            get { return BarbariansPercent + ArchersPercent + GoblinsPercent; }
        }

        /// <summary>
        /// Gets the Troop Compositions.
        /// </summary>
        /// <value>The Troop Compositions.</value>
        public static BindingList<Model> TroopCompositions { get { return DataCollection.TroopCompositions; } }

        private Model _selectedTroopComposition;
        public Model SelectedTroopComposition
        {
            get { return _selectedTroopComposition; }
            set
            {
                if (_selectedTroopComposition != value)
                {
                    _selectedTroopComposition = value;
                    OnPropertyChanged("SelectedTroopComposition");
                    OnPropertyChanged("IsUseBarracksEnabled");
                    OnPropertyChanged("IsCustomTroopsEnabled");
                }
            }
        }

        private int _numberOfGiants;
        public int NumberOfGiants
        {
            get { return _numberOfGiants; }
            set
            {
                if (_numberOfGiants != value)
                {
                    _numberOfGiants = value;
                    OnPropertyChanged("NumberOfGiants");
                }
            }
        }

        private int _numberOfWallBreakers;
        public int NumberOfWallBreakers
        {
            get { return _numberOfWallBreakers; }
            set
            {
                if (_numberOfWallBreakers != value)
                {
                    _numberOfWallBreakers = value;
                    OnPropertyChanged("NumberOfWallBreakers");
                }
            }
        }

        /// <summary>
        /// Gets the Troops.
        /// </summary>
        /// <value>The Troops.</value>
        public static BindingList<Model> Troops { get { return DataCollection.Troops; } }

        private Model _selectedBarrack1;
        public Model SelectedBarrack1
        {
            get { return _selectedBarrack1; }
            set
            {
                if (_selectedBarrack1 != value)
                {
                    _selectedBarrack1 = value;
                    OnPropertyChanged("SelectedBarrack1");
                }
            }
        }

        private Model _selectedBarrack2;
        public Model SelectedBarrack2
        {
            get { return _selectedBarrack2; }
            set
            {
                if (_selectedBarrack2 != value)
                {
                    _selectedBarrack2 = value;
                    OnPropertyChanged("SelectedBarrack2");
                }
            }
        }

        private Model _selectedBarrack3;
        public Model SelectedBarrack3
        {
            get { return _selectedBarrack3; }
            set
            {
                if (_selectedBarrack3 != value)
                {
                    _selectedBarrack3 = value;
                    OnPropertyChanged("SelectedBarrack3");
                }
            }
        }

        private Model _selectedBarrack4;
        public Model SelectedBarrack4
        {
            get { return _selectedBarrack4; }
            set
            {
                if (_selectedBarrack4 != value)
                {
                    _selectedBarrack4 = value;
                    OnPropertyChanged("SelectedBarrack4");
                }
            }
        }

        public bool IsUseBarracksEnabled
        {
            get
            {
                if (SelectedTroopComposition.Id == 1)
                    return true;
                else
                    return false;
            }
        }

        public bool IsCustomTroopsEnabled
        {
            get
            {
                if (SelectedTroopComposition.Id == 3)
                    return true;
                else
                    return false;
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

        private DelegateCommand _exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                    _exitCommand = new DelegateCommand(() => Exit());
                return _exitCommand;
            }
        }

        #region General Settings Commands

        private DelegateCommand _startCommand;
        public ICommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                    _startCommand = new DelegateCommand(() => Start(), StartCanExecute);
                return _startCommand;
            }
        }

        private DelegateCommand _stopCommand;
        public ICommand StopCommand
        {
            get
            {
                if (_stopCommand == null)
                    _stopCommand = new DelegateCommand(() => Stop(), StopCanExecute);
                return _stopCommand;
            }
        }

        private DelegateCommand _hideCommand;
        public ICommand HideCommand
        {
            get
            {
                if (_hideCommand == null)
                    _hideCommand = new DelegateCommand(() => Hide(), HideCanExecute);
                return _hideCommand;
            }
        }

        #endregion

        #region Search Settings Commands

        private DelegateCommand _searchModeCommand;
        public ICommand SearchModeCommand
        {
            get
            {
                if (_searchModeCommand == null)
                    _searchModeCommand = new DelegateCommand(() => SearchMode(), SearchModeCanExecute);
                return _searchModeCommand;
            }
        }

        #endregion

        #region Donate Settings Commands

        private DelegateCommand _locateClanCastleCommand;
        public ICommand LocateClanCastleCommand
        {
            get
            {
                if (_locateClanCastleCommand == null)
                    _locateClanCastleCommand = new DelegateCommand(() => LocateClanCastle(), LocateClanCastleCanExecute);
                return _locateClanCastleCommand;
            }
        }

        #endregion

        #region Troop Settings Commands

        private DelegateCommand _locateCollectorsCommand;
        public ICommand LocateCollectorsCommand
        {
            get
            {
                if (_locateCollectorsCommand == null)
                    _locateCollectorsCommand = new DelegateCommand(() => LocateCollectors(), LocateCollectorsCanExecute);
                return _locateCollectorsCommand;
            }
        }

        private DelegateCommand _locateBarracksCommand;
        public ICommand LocateBarracksCommand
        {
            get
            {
                if (_locateBarracksCommand == null)
                    _locateBarracksCommand = new DelegateCommand(() => LocateBarracks(), LocateBarracksCanExecute);
                return _locateBarracksCommand;
            }
        }

        #endregion

        #endregion

        #region Can Execute Methods

        /// <summary>
        /// Determines whether the Start command can be executed.
        /// </summary>
        /// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
        private bool StartCanExecute()
        {
            return IsStarted == false ? true : false;
        }

        /// <summary>
        /// Determines whether the Stop command can be executed.
        /// </summary>
        /// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
        private bool StopCanExecute()
        {
            return IsStarted;
        }

        /// <summary>
        /// Determines whether the HideCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
        private bool HideCanExecute()
        {
            return IsHidden;
        }

        /// <summary>
        /// Determines whether the SearchModeCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
        private bool SearchModeCanExecute()
        {
            return true; // TODO: We need to define this
        }

        /// <summary>
        /// Determines whether the LocateClanCastleCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
        private bool LocateClanCastleCanExecute()
        {
            return true; // TODO: We need to define this
        }

        /// <summary>
        /// Determines whether the LocateCollectorsCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
        private bool LocateCollectorsCanExecute()
        {
            return true; // TODO: We need to define this
        }

        /// <summary>
        /// Determines whether the LocateBarracksCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
        private bool LocateBarracksCanExecute()
        {
            return true; // TODO: We need to define this
        }

        #endregion

        #region Main Methods

        /// <summary>
        /// Starts the bot functionality
        /// </summary>
        private void Start()
        {
          MessageBox.Show("Trying some simple captures within FastFind, and Keyboard injection", "Start", MessageBoxButton.OK, MessageBoxImage.Information);
          FastFindTesting.Test();
          KeyboardHelper.BSTest();
          KeyboardHelper.BSTest2();
          KeyboardHelper.NotePadTest();          
        }

        /// <summary>
        /// Stops the bot functionality
        /// </summary>
        private void Stop()
        {
            MessageBox.Show("You clicked on the Stop button!", "Stop", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Hide the bot
        /// </summary>
        private void Hide()
        {
            MessageBox.Show("You clicked on the Hide button!", "Hide", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Starts the Search Mode.
        /// </summary>
        private void SearchMode()
        {
            MessageBox.Show("You clicked on the Search Mode button!", "Search Mode", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Manually locates the Clan Castle.
        /// </summary>
        private void LocateClanCastle()
        {
            MessageBox.Show("You clicked on the Locate Clan Castle Manually button!", "Locate Clan Castle Manually", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Manually locates the Collectors and Mines.
        /// </summary>
        private void LocateCollectors()
        {
            MessageBox.Show("You clicked on the Locate Collectors Manually button!", "Locate Collectors Manually", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Manually locates the Barracks.
        /// </summary>
        private void LocateBarracks()
        {
            MessageBox.Show("You clicked on the Locate Barracks Manually button!", "Locate Barracks Manually", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        private void Exit()
        {
            SaveUserSettings();
            Application.Current.MainWindow.Close();
        }

        #endregion

        #region Output Messages Methods

        /// <summary>
        /// Writes a new line to the output textbox.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="args">The arguments.</param>
        private static void WriteLine(string text, params object[] args)
        {
            // TODO: Need to work on this
            _output.AppendFormat(text + Environment.NewLine, args);
        }

        #endregion

        #region Application User Settings Methods

        /// <summary>
        /// Gets the application user settings.
        /// </summary>
        private void GetUserSettings()
        {
            // General
            MaxTrophies = Properties.Settings.Default.MaxTrophies;

            // Search Settings
            MeetGold = Properties.Settings.Default.MeetGold;
            MeetElixir = Properties.Settings.Default.MeetElixir;
            MeetDarkElixir = Properties.Settings.Default.MeetDarkElixir;
            MeetTrophyCount = Properties.Settings.Default.MeetTrophyCount;

            MinimumGold = Properties.Settings.Default.MinGold;
            MinimumElixir = Properties.Settings.Default.MinElixir;
            MinimumDarkElixir = Properties.Settings.Default.MinDarkElixir;
            MinimumTrophyCount = Properties.Settings.Default.MinTrophyCount;

            AlertWhenBaseFound = Properties.Settings.Default.AlertWhenBaseFound;

            // Attack Settings
            SelectedMaxCannonLevel = Properties.Settings.Default.MaxCannonLevel;
            SelectedMaxArcherTowerLevel = Properties.Settings.Default.MaxArcherTowerLevel;
            SelectedMaxMortarLevel = Properties.Settings.Default.MaxMortarLevel;
            SelectedWizardTowerLevel = Properties.Settings.Default.MaxWizardTowerLevel;
            SelectedXbowLevel = Properties.Settings.Default.MaxXbowLevel;

            AttackTheirKing = Properties.Settings.Default.AttackTheirKing;
            AttackTheirQueen = Properties.Settings.Default.AttackTheirQueen;

            SelectedAttackMode = (AttackMode)Properties.Settings.Default.SelectedAttackMode;

            SelectedKingAttackMode = (HeroAttackMode)Properties.Settings.Default.SelectedKingAttackMode;
            SelectedQueenAttackMode = (HeroAttackMode)Properties.Settings.Default.SelectedQueenAttackMode;
            AttackUsingClanCastle = Properties.Settings.Default.AttackUsingClanCastle;

            SelectedDeployStrategy = DataCollection.DeployStrategies.Where(ds => ds.Id == Properties.Settings.Default.SelectedDeployStrategy).DefaultIfEmpty(DataCollection.DeployStrategies.Last()).First();
            SelectedDeployTroop = DataCollection.DeployTroops.Where(dt => dt.Id == Properties.Settings.Default.SelectedDeployTroop).DefaultIfEmpty(DataCollection.DeployTroops.First()).First();
            AttackTownhall = Properties.Settings.Default.AttackTownhall;

            // Donate Settings

            // Troop Settings
            BarbariansPercent = Properties.Settings.Default.BarbariansPercent;
            ArchersPercent = Properties.Settings.Default.ArchersPercent;
            GoblinsPercent = Properties.Settings.Default.GoblinsPercent;

            SelectedTroopComposition = DataCollection.TroopCompositions.Where(tc => tc.Id == Properties.Settings.Default.SelectedTroopComposition).DefaultIfEmpty(DataCollection.TroopCompositions.First()).First();

            NumberOfGiants = Properties.Settings.Default.NumberOfGiants;
            NumberOfWallBreakers = Properties.Settings.Default.NumberOfWallBreakers;

            SelectedBarrack1 = DataCollection.Troops.Where(b1 => b1.Id == Properties.Settings.Default.SelectedBarrack1).DefaultIfEmpty(DataCollection.Troops.First()).First();
            SelectedBarrack2 = DataCollection.Troops.Where(b2 => b2.Id == Properties.Settings.Default.SelectedBarrack2).DefaultIfEmpty(DataCollection.Troops.First()).First();
            SelectedBarrack3 = DataCollection.Troops.Where(b3 => b3.Id == Properties.Settings.Default.SelectedBarrack3).DefaultIfEmpty(DataCollection.Troops.First()).First();
            SelectedBarrack4 = DataCollection.Troops.Where(b4 => b4.Id == Properties.Settings.Default.SelectedBarrack4).DefaultIfEmpty(DataCollection.Troops.First()).First();
        }

        /// <summary>
        /// Saves the application user settings.
        /// </summary>
        private void SaveUserSettings()
        {
            // General
            Properties.Settings.Default.MaxTrophies = MaxTrophies;

            // Search Settings
            Properties.Settings.Default.MeetGold = MeetGold;
            Properties.Settings.Default.MeetElixir = MeetElixir;
            Properties.Settings.Default.MeetDarkElixir = MeetDarkElixir;
            Properties.Settings.Default.MeetTrophyCount = MeetTrophyCount;

            Properties.Settings.Default.MinGold = MinimumGold;
            Properties.Settings.Default.MinElixir = MinimumElixir;
            Properties.Settings.Default.MinDarkElixir = MinimumDarkElixir;
            Properties.Settings.Default.MinTrophyCount = MinimumTrophyCount;

            Properties.Settings.Default.AlertWhenBaseFound = AlertWhenBaseFound;

            // Attack Settings
            Properties.Settings.Default.MaxCannonLevel = SelectedMaxCannonLevel;
            Properties.Settings.Default.MaxArcherTowerLevel = SelectedMaxArcherTowerLevel;
            Properties.Settings.Default.MaxMortarLevel = SelectedMaxMortarLevel;
            Properties.Settings.Default.MaxWizardTowerLevel = SelectedWizardTowerLevel;
            Properties.Settings.Default.MaxXbowLevel = SelectedXbowLevel;

            Properties.Settings.Default.AttackTheirKing = AttackTheirKing;
            Properties.Settings.Default.AttackTheirQueen = AttackTheirQueen;

            Properties.Settings.Default.SelectedAttackMode = (int)SelectedAttackMode;

            Properties.Settings.Default.SelectedKingAttackMode = (int)SelectedKingAttackMode;
            Properties.Settings.Default.SelectedQueenAttackMode = (int)SelectedQueenAttackMode;
            Properties.Settings.Default.AttackUsingClanCastle = AttackUsingClanCastle;

            Properties.Settings.Default.SelectedDeployStrategy = SelectedDeployStrategy.Id;
            Properties.Settings.Default.SelectedDeployTroop = SelectedDeployTroop.Id;
            Properties.Settings.Default.AttackTownhall = AttackTownhall;

            // Donate Settings

            // Troop Settings
            Properties.Settings.Default.BarbariansPercent = BarbariansPercent;
            Properties.Settings.Default.ArchersPercent = ArchersPercent;
            Properties.Settings.Default.GoblinsPercent = GoblinsPercent;

            Properties.Settings.Default.SelectedTroopComposition = SelectedTroopComposition.Id;

            Properties.Settings.Default.NumberOfGiants = NumberOfGiants;
            Properties.Settings.Default.NumberOfWallBreakers = NumberOfWallBreakers;

            Properties.Settings.Default.SelectedBarrack1 = SelectedBarrack1.Id;
            Properties.Settings.Default.SelectedBarrack2 = SelectedBarrack2.Id;
            Properties.Settings.Default.SelectedBarrack3 = SelectedBarrack3.Id;
            Properties.Settings.Default.SelectedBarrack4 = SelectedBarrack4.Id;

            // Save it!
            Properties.Settings.Default.Save();
        }

        #endregion
    }
}