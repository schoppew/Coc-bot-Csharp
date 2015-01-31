namespace CoC.Bot.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading;
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
            // Ex. DataCollection.TroopTiers   <--- Contains the Troop Tier (Tier 1, Tier 2, Tier 3, etc) and 
            //     DataCollection.TroopTiers.Troop  Contains Troops per Tier (Barbs, Archs, Goblins in Tier 1, etc) and
            //                                      Contiain properties like: DonateAll, DonateKeywords
            //
            // Under Main Methods you will find the Start(), Stop(), Hide(), etc methods
            // You can make them async if you wish

            Init();

            GetUserSettings();

            Output = "Hey there!";

            Message = "Click on Start to initialize the bot";
        }

        #region Properties

        public static string AppTitle { get { return string.Format("{0} v{1}", Properties.Resources.AppName, typeof(App).Assembly.GetName().Version.ToString(3)); } }

        internal static Properties.Settings AppSettings { get { return Properties.Settings.Default; } }

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
                if (_output == null)
                    _output = new StringBuilder(value);
                else
                    WriteLine(value);

                OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        private bool _meetTownhallLevel;
        public bool MeetTownhallLevel
        {
            get { return _meetTownhallLevel; }
            set
            {
                if (_meetTownhallLevel != value)
                {
                    _meetTownhallLevel = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        private int _minimumTownhallLevel;
        public int MinimumTownhallLevel
        {
            get { return _minimumTownhallLevel; }
            set
            {
                if (_minimumTownhallLevel != value)
                {
                    _minimumTownhallLevel = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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

        /// <summary>
        /// Gets the Troop Tier.
        /// </summary>
        /// <value>The Troop Compositions.</value>
        public static BindingList<TroopTierModel> TroopTiers { get { return DataCollection.TroopTiers; } }

        private bool _requestTroops;
        public bool RequestTroops
        {
            get { return _requestTroops; }
            set
            {
                if (_requestTroops != value)
                {
                    _requestTroops = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _requestTroopsMessage;
        public string RequestTroopsMessage
        {
            get { return _requestTroopsMessage; }
            set
            {
                if (_requestTroopsMessage != value)
                {
                    _requestTroopsMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private object _selectedTroopForDonate;
        public object SelectedTroopForDonate
        {
            get { return _selectedTroopForDonate; }
            set
            {
                if (_selectedTroopForDonate != value)
                {
                    _selectedTroopForDonate = value;
                    if (_selectedTroopForDonate is TroopModel)
                    {
                        ShouldHideDonateControls = Visibility.Visible;
                        ShouldHideTierInfoMessage = Visibility.Collapsed;

                        var troop = (TroopModel)_selectedTroopForDonate;
                        IsCurrentDonateAll = troop.IsDonateAll;
                        CurrentDonateKeywords = troop.DonateKeywords.Replace(@"|", Environment.NewLine);
                    }
                    else
                    {
                        var tt = (TroopTierModel)_selectedTroopForDonate;

                        switch ((TroopType)tt.Id)
                        {
                            case TroopType.Tier1:
                                TroopTierSelectedInfoMessage = Properties.Resources.Tier1;
                                break;
                            case TroopType.Tier2:
                                TroopTierSelectedInfoMessage = Properties.Resources.Tier2;
                                break;
                            case TroopType.Tier3:
                                TroopTierSelectedInfoMessage = Properties.Resources.Tier3;
                                break;
                            case TroopType.DarkTroops:
                                TroopTierSelectedInfoMessage = Properties.Resources.DarkTroops;
                                break;
                            default:
                                TroopTierSelectedInfoMessage = string.Empty;
                                break;
                        }

                        ShouldHideDonateControls = Visibility.Collapsed;
                        ShouldHideTierInfoMessage = Visibility.Visible;
                    }

                    OnPropertyChanged();
                }
            }
        }

        private bool _isCurrentDonateAll;
        public bool IsCurrentDonateAll
        {
            get { return _isCurrentDonateAll; }
            set
            {
                if (_isCurrentDonateAll != value)
                {
                    // TODO: Persist changes to DataCollection.TroopTiers.Troop.IsDonateAll
                    _isCurrentDonateAll = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _currentDonateKeywords;
        public string CurrentDonateKeywords
        {
            get { return _currentDonateKeywords; }
            set
            {
                if (_currentDonateKeywords != value)
                {
                    // TODO: Persist changes to DataCollection.TroopTiers.Troop.DonateKeywords
                    _currentDonateKeywords = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isTroopTierDonateSelected;
        public bool IsTroopTierDonateSelected
        {
            get { return _isTroopTierDonateSelected; }
            set
            {
                if (_isTroopTierDonateSelected != value)
                {
                    _isTroopTierDonateSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _shouldHideDonateControls = Visibility.Collapsed;
        public Visibility ShouldHideDonateControls
        {
            get { return _shouldHideDonateControls; }
            set
            {
                if (_shouldHideDonateControls != value)
                {
                    _shouldHideDonateControls = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _shouldHideTierInfoMessage = Visibility.Visible;
        public Visibility ShouldHideTierInfoMessage
        {
            get { return _shouldHideTierInfoMessage; }
            set
            {
                if (_shouldHideTierInfoMessage != value)
                {
                    _shouldHideTierInfoMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _troopTierSelectedInfoMessage;
        public string TroopTierSelectedInfoMessage
        {
            get { return _troopTierSelectedInfoMessage; }
            set
            {
                if (_troopTierSelectedInfoMessage != value)
                {
                    _troopTierSelectedInfoMessage = value;
                    OnPropertyChanged();
                }
            }
        }

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

        #region Initialization

        /// <summary>
        /// Initializes everything that is needed.
        /// </summary>
        private void Init()
        {
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

            // TODO: Create the DonateAll(Troop) in Settings
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
                            t1.Troops.Add(TroopModel.CreateNew((int)Troop.Barbarian, Troop.Barbarian.Name(), AppSettings.DonateBarbarians, false, AppSettings.DonateKeyboardsBarbarians));
                            t1.Troops.Add(TroopModel.CreateNew((int)Troop.Archer, Troop.Archer.Name(), AppSettings.DonateArchers, false, AppSettings.DonateKeywordsArchers));
                            t1.Troops.Add(TroopModel.CreateNew((int)Troop.Goblin, Troop.Goblin.Name(), AppSettings.DonateGoblins, false, AppSettings.DonateKeywordsGoblins));
                            break;
                        case TroopType.Tier2:
                            DataCollection.TroopTiers.Add(TroopTierModel.CreateNew((int)tier, TroopType.Tier2.Name()));

                            var t2 = DataCollection.TroopTiers.First(tt => tt.Id == (int)tier);
                            t2.Troops.Add(TroopModel.CreateNew((int)Troop.Giant, Troop.Giant.Name(), AppSettings.DonateGiants, false, AppSettings.DonateKeywordsGiants));
                            t2.Troops.Add(TroopModel.CreateNew((int)Troop.Wallbreaker, Troop.Wallbreaker.Name(), AppSettings.DonateWallBreakers, false, AppSettings.DonateKeywordsWallBreakers));
                            t2.Troops.Add(TroopModel.CreateNew((int)Troop.Balloon, Troop.Balloon.Name(), AppSettings.DonateBalloons, false, AppSettings.DonateKeywordsBalloons));
                            t2.Troops.Add(TroopModel.CreateNew((int)Troop.Wizard, Troop.Wizard.Name(), AppSettings.DonateWizards, false, AppSettings.DonateKeywordsWizards));
                            break;
                        case TroopType.Tier3:
                            DataCollection.TroopTiers.Add(TroopTierModel.CreateNew((int)tier, TroopType.Tier3.Name()));

                            var t3 = DataCollection.TroopTiers.First(tt => tt.Id == (int)tier);
                            t3.Troops.Add(TroopModel.CreateNew((int)Troop.Healer, Troop.Healer.Name(), AppSettings.DonateHealers, false, AppSettings.DonateKeywordsHealers));
                            t3.Troops.Add(TroopModel.CreateNew((int)Troop.Dragon, Troop.Dragon.Name(), AppSettings.DonateDragons, false, AppSettings.DonateKeywordsDragons));
                            t3.Troops.Add(TroopModel.CreateNew((int)Troop.Pekka, Troop.Pekka.Name(), AppSettings.DonatePekkas, false, AppSettings.DonateKeywordsPekkas));
                            break;
                        case TroopType.DarkTroops:
                            DataCollection.TroopTiers.Add(TroopTierModel.CreateNew((int)tier, TroopType.DarkTroops.Name()));

                            var dt = DataCollection.TroopTiers.First(tt => tt.Id == (int)tier);
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.Minion, Troop.Minion.Name(), AppSettings.DonateMinions, false, AppSettings.DonateKeywordsMinions));
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.HogRider, Troop.HogRider.Name(), AppSettings.DonateHogRiders, false, AppSettings.DonateKeywordsHogRiders));
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.Valkyrie, Troop.Valkyrie.Name(), AppSettings.DonateValkyries, false, AppSettings.DonateKeywordsValkyries));
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.Golem, Troop.Golem.Name(), AppSettings.DonateGolems, false, AppSettings.DonateKeywordsGolems));
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.Witch, Troop.Witch.Name(), AppSettings.DonateWitches, false, AppSettings.DonateKeywordsWitches));
                            dt.Troops.Add(TroopModel.CreateNew((int)Troop.LavaHound, Troop.LavaHound.Name(), AppSettings.DonateLavaHounds, false, AppSettings.DonateKeywordsLavaHounds));
                            break;
                        default:
                            // Troop Type Heroes, do nothing!
                            break;
                    }
                }
            }
        }

        #endregion

        #region Main Methods

        /// <summary>
        /// Starts the bot functionality
        /// </summary>
        private void Start()
        {
            Output = "Trying some simple captures within FastFind, and Keyboard injection";
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
            Output = "Stop bot execution...";
            MessageBox.Show("You clicked on the Stop button!", "Stop", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Hide the bot
        /// </summary>
        private void Hide()
        {
            Output = "Hidding...";
            MessageBox.Show("You clicked on the Hide button!", "Hide", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Starts the Search Mode.
        /// </summary>
        private void SearchMode()
        {
            Output = "Search Mode...";
            MessageBox.Show("You clicked on the Search Mode button!", "Search Mode", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Manually locates the Clan Castle.
        /// </summary>
        private void LocateClanCastle()
        {
            Output = "Locating Clan Castle...";
            MessageBox.Show("You clicked on the Locate Clan Castle Manually button!", "Locate Clan Castle Manually", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Manually locates the Collectors and Mines.
        /// </summary>
        private void LocateCollectors()
        {
            Output = "Locate Collectors and Gold Mines...";
            MessageBox.Show("You clicked on the Locate Collectors Manually button!", "Locate Collectors Manually", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Manually locates the Barracks.
        /// </summary>
        private void LocateBarracks()
        {
            Output = "Locate Barracks...";
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
        /// Writes a new line to the output textbox (Should not use this directly, use Output property instead).
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="args">The arguments.</param>
        private static void WriteLine(string text, params object[] args)
        {
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
            MaxTrophies = AppSettings.MaxTrophies;

            // Search Settings
            MeetGold = AppSettings.MeetGold;
            MeetElixir = AppSettings.MeetElixir;
            MeetDarkElixir = AppSettings.MeetDarkElixir;
            MeetTrophyCount = AppSettings.MeetTrophyCount;
            MeetTownhallLevel = AppSettings.MeetTownhallLevel;

            MinimumGold = AppSettings.MinGold;
            MinimumElixir = AppSettings.MinElixir;
            MinimumDarkElixir = AppSettings.MinDarkElixir;
            MinimumTrophyCount = AppSettings.MinTrophyCount;
            MinimumTownhallLevel = AppSettings.MinTownhallLevel;

            AlertWhenBaseFound = AppSettings.AlertWhenBaseFound;

            // Attack Settings
            SelectedMaxCannonLevel = AppSettings.MaxCannonLevel;
            SelectedMaxArcherTowerLevel = AppSettings.MaxArcherTowerLevel;
            SelectedMaxMortarLevel = AppSettings.MaxMortarLevel;
            SelectedWizardTowerLevel = AppSettings.MaxWizardTowerLevel;
            SelectedXbowLevel = AppSettings.MaxXbowLevel;

            AttackTheirKing = AppSettings.AttackTheirKing;
            AttackTheirQueen = AppSettings.AttackTheirQueen;

            SelectedAttackMode = (AttackMode)AppSettings.SelectedAttackMode;

            SelectedKingAttackMode = (HeroAttackMode)AppSettings.SelectedKingAttackMode;
            SelectedQueenAttackMode = (HeroAttackMode)AppSettings.SelectedQueenAttackMode;
            AttackUsingClanCastle = AppSettings.AttackUsingClanCastle;

            SelectedDeployStrategy = DataCollection.DeployStrategies.Where(ds => ds.Id == AppSettings.SelectedDeployStrategy).DefaultIfEmpty(DataCollection.DeployStrategies.Last()).First();
            SelectedDeployTroop = DataCollection.DeployTroops.Where(dt => dt.Id == AppSettings.SelectedDeployTroop).DefaultIfEmpty(DataCollection.DeployTroops.First()).First();
            AttackTownhall = AppSettings.AttackTownhall;

            // Donate Settings
            RequestTroops = AppSettings.RequestTroops;
            RequestTroopsMessage = AppSettings.RequestTroopsMessage;

            // Troop Settings
            BarbariansPercent = AppSettings.BarbariansPercent;
            ArchersPercent = AppSettings.ArchersPercent;
            GoblinsPercent = AppSettings.GoblinsPercent;

            SelectedTroopComposition = DataCollection.TroopCompositions.Where(tc => tc.Id == AppSettings.SelectedTroopComposition).DefaultIfEmpty(DataCollection.TroopCompositions.First()).First();

            NumberOfGiants = AppSettings.NumberOfGiants;
            NumberOfWallBreakers = AppSettings.NumberOfWallBreakers;

            SelectedBarrack1 = DataCollection.Troops.Where(b1 => b1.Id == AppSettings.SelectedBarrack1).DefaultIfEmpty(DataCollection.Troops.First()).First();
            SelectedBarrack2 = DataCollection.Troops.Where(b2 => b2.Id == AppSettings.SelectedBarrack2).DefaultIfEmpty(DataCollection.Troops.First()).First();
            SelectedBarrack3 = DataCollection.Troops.Where(b3 => b3.Id == AppSettings.SelectedBarrack3).DefaultIfEmpty(DataCollection.Troops.First()).First();
            SelectedBarrack4 = DataCollection.Troops.Where(b4 => b4.Id == AppSettings.SelectedBarrack4).DefaultIfEmpty(DataCollection.Troops.First()).First();
        }

        /// <summary>
        /// Saves the application user settings.
        /// </summary>
        private void SaveUserSettings()
        {
            // General
            AppSettings.MaxTrophies = MaxTrophies;

            // Search Settings
            AppSettings.MeetGold = MeetGold;
            AppSettings.MeetElixir = MeetElixir;
            AppSettings.MeetDarkElixir = MeetDarkElixir;
            AppSettings.MeetTrophyCount = MeetTrophyCount;
            AppSettings.MeetTownhallLevel = MeetTownhallLevel;

            AppSettings.MinGold = MinimumGold;
            AppSettings.MinElixir = MinimumElixir;
            AppSettings.MinDarkElixir = MinimumDarkElixir;
            AppSettings.MinTrophyCount = MinimumTrophyCount;
            AppSettings.MinTownhallLevel = MinimumTownhallLevel;

            AppSettings.AlertWhenBaseFound = AlertWhenBaseFound;

            // Attack Settings
            AppSettings.MaxCannonLevel = SelectedMaxCannonLevel;
            AppSettings.MaxArcherTowerLevel = SelectedMaxArcherTowerLevel;
            AppSettings.MaxMortarLevel = SelectedMaxMortarLevel;
            AppSettings.MaxWizardTowerLevel = SelectedWizardTowerLevel;
            AppSettings.MaxXbowLevel = SelectedXbowLevel;

            AppSettings.AttackTheirKing = AttackTheirKing;
            AppSettings.AttackTheirQueen = AttackTheirQueen;

            AppSettings.SelectedAttackMode = (int)SelectedAttackMode;

            AppSettings.SelectedKingAttackMode = (int)SelectedKingAttackMode;
            AppSettings.SelectedQueenAttackMode = (int)SelectedQueenAttackMode;
            AppSettings.AttackUsingClanCastle = AttackUsingClanCastle;

            AppSettings.SelectedDeployStrategy = SelectedDeployStrategy.Id;
            AppSettings.SelectedDeployTroop = SelectedDeployTroop.Id;
            AppSettings.AttackTownhall = AttackTownhall;

            // Donate Settings
            AppSettings.RequestTroops = RequestTroops;
            AppSettings.RequestTroopsMessage = RequestTroopsMessage;

            // Troop Settings
            AppSettings.BarbariansPercent = BarbariansPercent;
            AppSettings.ArchersPercent = ArchersPercent;
            AppSettings.GoblinsPercent = GoblinsPercent;

            AppSettings.SelectedTroopComposition = SelectedTroopComposition.Id;

            AppSettings.NumberOfGiants = NumberOfGiants;
            AppSettings.NumberOfWallBreakers = NumberOfWallBreakers;

            AppSettings.SelectedBarrack1 = SelectedBarrack1.Id;
            AppSettings.SelectedBarrack2 = SelectedBarrack2.Id;
            AppSettings.SelectedBarrack3 = SelectedBarrack3.Id;
            AppSettings.SelectedBarrack4 = SelectedBarrack4.Id;

            // Save it!
            AppSettings.Save();
        }

        #endregion
    }
}