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
	using System.Windows.Documents;
    using System.Windows.Input;
	using System.Windows.Media;
	using System.Xml.Linq;

    using Data;
    using Tools;
    using Tools.FastFind;
    using UI.Commands;
	using UI.Services;

    /// <summary>
    /// Provides functionality for the MainWindow
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
		private readonly INotifyService notifyService = new NotifyService();

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
			
            Message = Properties.Resources.StartMessage;
        }

        #region Properties

        /// <summary>
        /// Gets the application title.
        /// </summary>
        /// <value>The application title.</value>
        public static string AppTitle { get { return string.Format("{0} v{1}", Properties.Resources.AppName, typeof(App).Assembly.GetName().Version.ToString(3)); } }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <value>The application settings.</value>
        internal static Properties.Settings AppSettings { get { return Properties.Settings.Default; } }

        internal LogWriter Log { get; private set; }

		internal static bool IsDebug { get { return GlobalVariables.IsDebug; } }

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
			get
			{
				return IsBlueStacksHidden ? true : false;
			}
		}

        /// <summary>
        /// Gets a value indicating the Start/Stop State.
        /// </summary>
        /// <value><c>true</c> if Executing; otherwise, <c>false</c>.</value>
        public bool StartStopState
        {
            get { return IsExecuting ? true : false; }
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

        #region Search Settings Properties

        private bool _meetGold;
        /// <summary>
        /// Gets or sets a value indicating whether should meet Gold conditions.
        /// </summary>
        /// <value><c>true</c> if should meet Gold conditions; otherwise, <c>false</c>.</value>
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
        /// <summary>
        /// Gets or sets a value indicating whether should meet Elixir conditions.
        /// </summary>
        /// <value><c>true</c> if should meet Elixir conditions; otherwise, <c>false</c>.</value>
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
        /// <summary>
        /// Gets or sets a value indicating whether should meet Dark Elixir conditions.
        /// </summary>
        /// <value><c>true</c> if should meet Dark Elixir conditions; otherwise, <c>false</c>.</value>
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
        /// <summary>
        /// Gets or sets a value indicating whether should meet Trophy count conditions.
        /// </summary>
        /// <value><c>true</c> if should meet Trophy count conditions; otherwise, <c>false</c>.</value>
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
        /// <summary>
        /// Gets or sets a value indicating whether should meet Townhall level conditions.
        /// </summary>
        /// <value><c>true</c> if should meet Townhall level conditions; otherwise, <c>false</c>.</value>
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
        /// <summary>
        /// Gets or sets the minimum Gold.
        /// </summary>
        /// <value>The minimum Gold.</value>
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
        /// <summary>
        /// Gets or sets the minimum Elixir.
        /// </summary>
        /// <value>The minimum Elixir.</value>
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
        /// <summary>
        /// Gets or sets the minimum Dark Elixir.
        /// </summary>
        /// <value>The minimum Dark Elixir.</value>
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
        /// <summary>
        /// Gets or sets the minimum Trophy count.
        /// </summary>
        /// <value>The minimum Trophy count.</value>
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
        /// <summary>
        /// Gets or sets the minimum Townhall level.
        /// </summary>
        /// <value>The minimum Townhall level.</value>
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

        private bool _isAlertWhenBaseFound;
        /// <summary>
        /// Gets or sets a value indicating whether should alert when base found.
        /// </summary>
        /// <value><c>true</c> if alert when base found; otherwise, <c>false</c>.</value>
        public bool IsAlertWhenBaseFound
        {
            get { return _isAlertWhenBaseFound; }
            set
            {
                if (_isAlertWhenBaseFound != value)
                {
                    _isAlertWhenBaseFound = value;
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
        /// <summary>
        /// Gets or sets the maximum Cannon level.
        /// </summary>
        /// <value>The maximum Cannon level.</value>
        public int SelectedMaxCannonLevel
        {
            get { return _selectedMaxCannonLevel; }
            set
            {
                if (_selectedMaxCannonLevel != value)
                {
                    _selectedMaxCannonLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedMaxArcherTowerLevel;
        /// <summary>
        /// Gets or sets the maximum Archer Tower level.
        /// </summary>
        /// <value>The maximum Archer Tower level.</value>
        public int SelectedMaxArcherTowerLevel
        {
            get { return _selectedMaxArcherTowerLevel; }
            set
            {
                if (_selectedMaxArcherTowerLevel != value)
                {
                    _selectedMaxArcherTowerLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedMaxMortarLevel;
        /// <summary>
        /// Gets or sets the maximum Mortar level.
        /// </summary>
        /// <value>The maximum Mortar level.</value>
        public int SelectedMaxMortarLevel
        {
            get { return _selectedMaxMortarLevel; }
            set
            {
                if (_selectedMaxMortarLevel != value)
                {
                    _selectedMaxMortarLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedMaxWizardTowerLevel;
        /// <summary>
        /// Gets or sets the maximum Wizard Tower level.
        /// </summary>
        /// <value>The maximum Wizard Tower level.</value>
        public int SelectedMaxWizardTowerLevel
        {
            get { return _selectedMaxWizardTowerLevel; }
            set
            {
                if (_selectedMaxWizardTowerLevel != value)
                {
                    _selectedMaxWizardTowerLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedMaxXbowLevel;
        /// <summary>
        /// Gets or sets the maximum Xbow level.
        /// </summary>
        /// <value>The maximum Xbow level.</value>
        public int SelectedMaxXbowLevel
        {
            get { return _selectedMaxXbowLevel; }
            set
            {
                if (_selectedMaxXbowLevel != value)
                {
                    _selectedMaxXbowLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isAttackTheirKing;
        /// <summary>
        /// Gets or sets a value indicating whether to attack their King.
        /// </summary>
        /// <value><c>true</c> if attack their King; otherwise, <c>false</c>.</value>
        public bool IsAttackTheirKing
        {
            get { return _isAttackTheirKing; }
            set
            {
                if (_isAttackTheirKing != value)
                {
                    _isAttackTheirKing = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isAttackTheirQueen;
        /// <summary>
        /// Gets or sets a value indicating whether to attack their Queen.
        /// </summary>
        /// <value><c>true</c> if attack their Queen; otherwise, <c>false</c>.</value>
        public bool IsAttackTheirQueen
        {
            get { return _isAttackTheirQueen; }
            set
            {
                if (_isAttackTheirQueen != value)
                {
                    _isAttackTheirQueen = value;
                    OnPropertyChanged();
                }
            }
        }

        private AttackMode _selectedAttackMode;
        /// <summary>
        /// Gets or sets the selected attack mode.
        /// </summary>
        /// <value>The selected attack mode.</value>
        public AttackMode SelectedAttackMode
        {
            get { return _selectedAttackMode; }
            set
            {
                if (_selectedAttackMode != value)
                {
                    _selectedAttackMode = value;
                    OnPropertyChanged();
                }
            }
        }

        private HeroAttackMode _selectedKingAttackMode;
        /// <summary>
        /// Gets or sets the selected King attack mode.
        /// </summary>
        /// <value>The selected King attack mode.</value>
        public HeroAttackMode SelectedKingAttackMode
        {
            get { return _selectedKingAttackMode; }
            set
            {
                if (_selectedKingAttackMode != value)
                {
                    _selectedKingAttackMode = value;
                    OnPropertyChanged();
                }
            }
        }

        private HeroAttackMode _selectedQueenAttackMode;
        /// <summary>
        /// Gets or sets the selected Queen attack mode.
        /// </summary>
        /// <value>The selected Queen attack mode.</value>
        public HeroAttackMode SelectedQueenAttackMode
        {
            get { return _selectedQueenAttackMode; }
            set
            {
                if (_selectedQueenAttackMode != value)
                {
                    _selectedQueenAttackMode = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// [Used in UI for Binding] Gets the Deploy Strategies.
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
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// [Used in UI for Binding] Gets the Deploy Troops.
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
                    OnPropertyChanged();
                }
            }
        }

        private bool _isAttackTownhall;
        /// <summary>
        /// Gets or sets a value indicating whether to attack the Townhall.
        /// </summary>
        /// <value><c>true</c> if attack to Townhall; otherwise, <c>false</c>.</value>
        public bool IsAttackTownhall
        {
            get { return _isAttackTownhall; }
            set
            {
                if (_isAttackTownhall != value)
                {
                    _isAttackTownhall = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isAttackUsingClanCastle;
        /// <summary>
        /// Gets or sets a value indicating whether to attack using clan castle troops.
        /// </summary>
        /// <value><c>true</c> if attack using clan castle troops; otherwise, <c>false</c>.</value>
        public bool IsAttackUsingClanCastle
        {
            get { return _isAttackUsingClanCastle; }
            set
            {
                if (_isAttackUsingClanCastle != value)
                {
                    _isAttackUsingClanCastle = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Donate Settings Properties

        /// <summary>
        /// [Used in UI for Binding] Gets the Troop Tier.
        /// </summary>
        /// <value>The Troop Compositions.</value>
        public static BindingList<TroopTierModel> TroopTiers { get { return DataCollection.TroopTiers; } }

        private bool _isRequestTroops;
        /// <summary>
        /// Gets or sets a value indicating whether it should request for troops.
        /// </summary>
        /// <value><c>true</c> if request for troops; otherwise, <c>false</c>.</value>
        public bool IsRequestTroops
        {
            get { return _isRequestTroops; }
            set
            {
                if (_isRequestTroops != value)
                {
                    _isRequestTroops = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _requestTroopsMessage;
        /// <summary>
        /// Gets or sets the request troops message.
        /// </summary>
        /// <value>The request troops message.</value>
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
        /// <summary>
        /// [For use in UI only] Gets or sets the selected troop for donate.
        /// </summary>
        /// <value>The selected troop for donate.</value>
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
                        ShouldHideDonateControls = true;
                        ShouldHideTierInfoMessage = false;

                        var troop = (TroopModel)_selectedTroopForDonate;
                        IsCurrentDonateAll = troop.IsDonateAll;
                        CurrentDonateKeywords = troop.DonateKeywords.Replace(@"|", Environment.NewLine);
                        CurrentMaxDonationsPerRequest = troop.MaxDonationsPerRequest;
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

                        ShouldHideDonateControls = false;
                        ShouldHideTierInfoMessage = true;
                    }

                    OnPropertyChanged();
                }
            }
        }

        private bool _isCurrentDonateAll;
        /// <summary>
        /// [For use in UI only] Gets or sets a value indicating whether the selected troop is for donate all.
        /// </summary>
        /// <value><c>true</c> if selected troop is for donate all; otherwise, <c>false</c>.</value>
        public bool IsCurrentDonateAll
        {
            get { return _isCurrentDonateAll; }
            set
            {
                if (_isCurrentDonateAll != value)
                {
                    if (SelectedTroopForDonate is TroopModel)
                    {
                        var troop = (TroopModel)SelectedTroopForDonate;
                        troop.IsDonateAll = value;
                    }

                    _isCurrentDonateAll = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _currentDonateKeywords;
        /// <summary>
        /// [For use in UI only] Gets or sets the selected troop donate keywords.
        /// </summary>
        /// <value>The selected troop donate keywords.</value>
        public string CurrentDonateKeywords
        {
            get { return _currentDonateKeywords; }
            set
            {
                if (_currentDonateKeywords != value)
                {
                    if (SelectedTroopForDonate is TroopModel)
                    {
                        var troop = (TroopModel)SelectedTroopForDonate;
                        troop.DonateKeywords = value.Replace(Environment.NewLine, @"|");
                    }

                    _currentDonateKeywords = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _currentMaxDonationsPerRequest;
        /// <summary>
        /// [For use in UI only] Gets or sets the maximum number of troops to donate for the selected troop.
        /// </summary>
        /// <value>The maximum number of troops to donate for the selected troop.</value>
        public int CurrentMaxDonationsPerRequest
        {
            get { return _currentMaxDonationsPerRequest; }
            set
            {
                if (_currentMaxDonationsPerRequest != value)
                {
                    if (SelectedTroopForDonate is TroopModel)
                    {
                        var troop = (TroopModel)SelectedTroopForDonate;
                        troop.MaxDonationsPerRequest = value;
                    }

                    _currentMaxDonationsPerRequest = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _shouldHideDonateControls;
        /// <summary>
        /// [For use in UI only] Gets or sets if should hide donate controls.
        /// </summary>
        /// <value>If should hide donate controls.</value>
        public bool ShouldHideDonateControls
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

        private bool _shouldHideTierInfoMessage = true;
        /// <summary>
        /// [For use in UI only] Gets or sets if should hide tier information message.
        /// </summary>
        /// <value>If should hide tier information message.</value>
        public bool ShouldHideTierInfoMessage
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
        /// <summary>
        /// [For use in UI only] Gets or sets the troop tier selected information message.
        /// </summary>
        /// <value>The troop tier selected information message.</value>
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

		/// <summary>
		/// [Used in UI for Binding] Gets All Attack Troops.
		/// </summary>
		/// <value>All Attack Troops.</value>
		public static IEnumerable<TroopModel> AllAttackTroops { get { return DataCollection.TroopTiers.SelectMany(tt => tt.Troops).Distinct(); } }

		private TroopModel _selectedAttackTroop;
		/// <summary>
		/// [For use in UI only] Gets or sets the selected attack troop.
		/// </summary>
		/// <value>The selected attack troop.</value>
		public TroopModel SelectedAttackTroop
		{
			get { return _selectedAttackTroop; }
			set
			{
				if (_selectedAttackTroop != value)
				{
					_selectedAttackTroop = value;
					OnPropertyChanged();
					OnPropertyChanged(() => TroopCapacity);
				}
			}
		}

		/// <summary>
		/// Gets the Total Troop capacity.
		/// </summary>
		/// <value>The total troop capacity.</value>
		public int TroopCapacity
		{
			get { return CalculateTroopCapacity(); }
		}

        /// <summary>
        /// [Used in UI for Binding] Gets the Troop Compositions.
        /// </summary>
        /// <value>The Troop Compositions.</value>
        public static BindingList<Model> TroopCompositions { get { return DataCollection.TroopCompositions; } }

        private Model _selectedTroopComposition;
        /// <summary>
        /// Gets or sets the selected Troop Composition.
        /// </summary>
        /// <value>The selected Troop Composition.</value>
        public Model SelectedTroopComposition
        {
            get { return _selectedTroopComposition; }
            set
            {
                if (_selectedTroopComposition != value)
                {
                    _selectedTroopComposition = value;
                    OnPropertyChanged();
                    OnPropertyChanged(() => IsUseBarracksEnabled);
                    OnPropertyChanged(() => IsCustomTroopsEnabled);
                }
            }
        }

        /// <summary>
		/// [Used in UI for Binding] Gets the Troops.
        /// </summary>
        /// <value>The Troops.</value>
        public static BindingList<Model> BarrackTroops { get { return DataCollection.BarracksTroops; } }

        private bool _isUseBarracks1;
        /// <summary>
        /// Gets or sets a value indicating whether it should use Barracks 1.
        /// </summary>
        /// <value><c>true</c> if it should use Barracks 1; otherwise, <c>false</c>.</value>
        public bool IsUseBarracks1
        {
            get { return _isUseBarracks1; }
            set
            {
                if (_isUseBarracks1 != value)
                {
                    _isUseBarracks1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isUseBarracks2;
        /// <summary>
        /// Gets or sets a value indicating whether it should use Barracks 2.
        /// </summary>
        /// <value><c>true</c> if it should use Barracks 2; otherwise, <c>false</c>.</value>
        public bool IsUseBarracks2
        {
            get { return _isUseBarracks2; }
            set
            {
                if (_isUseBarracks2 != value)
                {
                    _isUseBarracks2 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isUseBarracks3;
        /// <summary>
        /// Gets or sets a value indicating whether it should use Barracks 3.
        /// </summary>
        /// <value><c>true</c> if it should use Barracks 3; otherwise, <c>false</c>.</value>
        public bool IsUseBarracks3
        {
            get { return _isUseBarracks3; }
            set
            {
                if (_isUseBarracks3 != value)
                {
                    _isUseBarracks3 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isUseBarracks4;
        /// <summary>
        /// Gets or sets a value indicating whether it should use Barracks 4.
        /// </summary>
        /// <value><c>true</c> if it should use Barracks 4; otherwise, <c>false</c>.</value>
        public bool IsUseBarracks4
        {
            get { return _isUseBarracks4; }
            set
            {
                if (_isUseBarracks4 != value)
                {
                    _isUseBarracks4 = value;
                    OnPropertyChanged();
                }
            }
        }

        private Model _selectedBarrack1;
        /// <summary>
        /// Gets or sets the Selected Troops in Barrack 1.
        /// </summary>
        /// <value>The Selected Troops in Barrack 1.</value>
        public Model SelectedBarrack1
        {
            get { return _selectedBarrack1; }
            set
            {
                if (_selectedBarrack1 != value)
                {
                    _selectedBarrack1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private Model _selectedBarrack2;
        /// <summary>
        /// Gets or sets the Selected Troops in Barrack 2.
        /// </summary>
        /// <value>The Selected Troops in Barrack 2.</value>
        public Model SelectedBarrack2
        {
            get { return _selectedBarrack2; }
            set
            {
                if (_selectedBarrack2 != value)
                {
                    _selectedBarrack2 = value;
                    OnPropertyChanged();
                }
            }
        }

        private Model _selectedBarrack3;
        /// <summary>
        /// Gets or sets the Selected Troops in Barrack 3.
        /// </summary>
        /// <value>The Selected Troops in Barrack 3.</value>
        public Model SelectedBarrack3
        {
            get { return _selectedBarrack3; }
            set
            {
                if (_selectedBarrack3 != value)
                {
                    _selectedBarrack3 = value;
                    OnPropertyChanged();
                }
            }
        }

        private Model _selectedBarrack4;
        /// <summary>
        /// Gets or sets the Selected Troops in Barrack 4.
        /// </summary>
        /// <value>The Selected Troops in Barrack 4.</value>
        public Model SelectedBarrack4
        {
            get { return _selectedBarrack4; }
            set
            {
                if (_selectedBarrack4 != value)
                {
                    _selectedBarrack4 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isUseDarkBarracks1;
        /// <summary>
        /// Gets or sets a value indicating whether it should use Dark Barracks 1.
        /// </summary>
        /// <value><c>true</c> if it should use Dark Barracks 1; otherwise, <c>false</c>.</value>
        public bool IsUseDarkBarracks1
        {
            get { return _isUseDarkBarracks1; }
            set
            {
                if (_isUseDarkBarracks1 != value)
                {
                    _isUseDarkBarracks1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isUseDarkBarracks2;
        /// <summary>
        /// Gets or sets a value indicating whether it should use Dark Barracks 2.
        /// </summary>
        /// <value><c>true</c> if it should use Dark Barracks 2; otherwise, <c>false</c>.</value>
        public bool IsUseDarkBarracks2
        {
            get { return _isUseDarkBarracks2; }
            set
            {
                if (_isUseDarkBarracks2 != value)
                {
                    _isUseDarkBarracks2 = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
		/// [Used in UI for Binding] Gets the Dark Troops.
        /// </summary>
        /// <value>The Dark Troops.</value>
        public static BindingList<Model> DarkBarrackTroops { get { return DataCollection.DarkBarracksTroops; } }

        private Model _selectedDarkBarrack1;
        /// <summary>
        /// Gets or sets the Selected Troops in Dark Barrack 1.
        /// </summary>
        /// <value>The Selected Troops in Dark Barrack 1.</value>
        public Model SelectedDarkBarrack1
        {
            get { return _selectedDarkBarrack1; }
            set
            {
                if (_selectedDarkBarrack1 != value)
                {
                    _selectedDarkBarrack1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private Model _selectedDarkBarrack2;
        /// <summary>
        /// Gets or sets the Selected Troops in Dark Barrack 2.
        /// </summary>
        /// <value>The Selected Troops in Dark Barrack 2.</value>
        public Model SelectedDarkBarrack2
        {
            get { return _selectedDarkBarrack2; }
            set
            {
                if (_selectedDarkBarrack2 != value)
                {
                    _selectedDarkBarrack2 = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// [For use in UI only] Gets a value indicating whether the use of barracks is enabled.
        /// </summary>
        /// <value><c>true</c> if the use of barracks is enabled; otherwise, <c>false</c>.</value>
        public bool IsUseBarracksEnabled
        {
            get
            {
                if (SelectedTroopComposition.Id == (int)TroopComposition.UseBarracks)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// [For use in UI only] Gets a value indicating whether the use of custom troops is enabled.
        /// </summary>
        /// <value><c>true</c> if the use of custom troops enabled; otherwise, <c>false</c>.</value>
        public bool IsCustomTroopsEnabled
        {
            get
            {
                if (SelectedTroopComposition.Id == (int)TroopComposition.CustomTroops)
                    return true;
                else
                    return false;
            }
        }

        #endregion

		#region Location Properties

		private System.Drawing.Point _locationClanCastle;
		public System.Drawing.Point LocationClanCastle
		{
			get { return _locationClanCastle; }
			set
			{
				if (_locationClanCastle != value)
				{
					_locationClanCastle = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationBarrack1;
		public System.Drawing.Point LocationBarrack1
		{
			get { return _locationBarrack1; }
			set
			{
				if (_locationBarrack1 != value)
				{
					_locationBarrack1 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationBarrack2;
		public System.Drawing.Point LocationBarrack2
		{
			get { return _locationBarrack2; }
			set
			{
				if (_locationBarrack2 != value)
				{
					_locationBarrack2 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationBarrack3;
		public System.Drawing.Point LocationBarrack3
		{
			get { return _locationBarrack3; }
			set
			{
				if (_locationBarrack3 != value)
				{
					_locationBarrack3 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationBarrack4;
		public System.Drawing.Point LocationBarrack4
		{
			get { return _locationBarrack4; }
			set
			{
				if (_locationBarrack4 != value)
				{
					_locationBarrack4 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationDarkBarrack1;
		public System.Drawing.Point LocationDarkBarrack1
		{
			get { return _locationDarkBarrack1; }
			set
			{
				if (_locationDarkBarrack1 != value)
				{
					_locationDarkBarrack1 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationDarkBarrack2;
		public System.Drawing.Point LocationDarkBarrack2
		{
			get { return _locationDarkBarrack2; }
			set
			{
				if (_locationDarkBarrack2 != value)
				{
					_locationDarkBarrack2 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationAllTrapsRearm;
		public System.Drawing.Point LocationAllTrapsRearm
		{
			get { return _locationAllTrapsRearm; }
			set
			{
				if (_locationAllTrapsRearm != value)
				{
					_locationAllTrapsRearm = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationAllXbowsRearm;
		public System.Drawing.Point LocationAllXbowsRearm
		{
			get { return _locationAllXbowsRearm; }
			set
			{
				if (_locationAllXbowsRearm != value)
				{
					_locationAllXbowsRearm = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector1;
		public System.Drawing.Point LocationCollector1
		{
			get { return _locationCollector1; }
			set
			{
				if (_locationCollector1 != value)
				{
					_locationCollector1 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector2;
		public System.Drawing.Point LocationCollector2
		{
			get { return _locationCollector2; }
			set
			{
				if (_locationCollector2 != value)
				{
					_locationCollector2 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector3;
		public System.Drawing.Point LocationCollector3
		{
			get { return _locationCollector3; }
			set
			{
				if (_locationCollector3 != value)
				{
					_locationCollector3 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector4;
		public System.Drawing.Point LocationCollector4
		{
			get { return _locationCollector4; }
			set
			{
				if (_locationCollector4 != value)
				{
					_locationCollector4 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector5;
		public System.Drawing.Point LocationCollector5
		{
			get { return _locationCollector5; }
			set
			{
				if (_locationCollector5 != value)
				{
					_locationCollector5 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector6;
		public System.Drawing.Point LocationCollector6
		{
			get { return _locationCollector6; }
			set
			{
				if (_locationCollector6 != value)
				{
					_locationCollector6 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector7;
		public System.Drawing.Point LocationCollector7
		{
			get { return _locationCollector7; }
			set
			{
				if (_locationCollector7 != value)
				{
					_locationCollector7 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector8;
		public System.Drawing.Point LocationCollector8
		{
			get { return _locationCollector8; }
			set
			{
				if (_locationCollector8 != value)
				{
					_locationCollector8 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector9;
		public System.Drawing.Point LocationCollector9
		{
			get { return _locationCollector9; }
			set
			{
				if (_locationCollector9 != value)
				{
					_locationCollector9 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector10;
		public System.Drawing.Point LocationCollector10
		{
			get { return _locationCollector10; }
			set
			{
				if (_locationCollector10 != value)
				{
					_locationCollector10 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector11;
		public System.Drawing.Point LocationCollector11
		{
			get { return _locationCollector11; }
			set
			{
				if (_locationCollector11 != value)
				{
					_locationCollector11 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector12;
		public System.Drawing.Point LocationCollector12
		{
			get { return _locationCollector12; }
			set
			{
				if (_locationCollector12 != value)
				{
					_locationCollector12 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector13;
		public System.Drawing.Point LocationCollector13
		{
			get { return _locationCollector13; }
			set
			{
				if (_locationCollector13 != value)
				{
					_locationCollector13 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector14;
		public System.Drawing.Point LocationCollector14
		{
			get { return _locationCollector14; }
			set
			{
				if (_locationCollector14 != value)
				{
					_locationCollector14 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector15;
		public System.Drawing.Point LocationCollector15
		{
			get { return _locationCollector15; }
			set
			{
				if (_locationCollector15 != value)
				{
					_locationCollector15 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector16;
		public System.Drawing.Point LocationCollector16
		{
			get { return _locationCollector16; }
			set
			{
				if (_locationCollector16 != value)
				{
					_locationCollector16 = value;
					OnPropertyChanged();
				}
			}
		}

		private System.Drawing.Point _locationCollector17;
		public System.Drawing.Point LocationCollector17
		{
			get { return _locationCollector17; }
			set
			{
				if (_locationCollector17 != value)
				{
					_locationCollector17 = value;
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

		//public ICommand MouseDownCommand
		//{
		//	get { return new RelayCommand<MouseButtonEventArgs>(MouseDownDragAndMoveWindow); }
		//}

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

        #region Search Settings Commands

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

        private DelegateCommand _locateDarkBarracksCommand;
        public ICommand LocateDarkBarracksCommand
        {
            get
            {
                if (_locateDarkBarracksCommand == null)
                    _locateDarkBarracksCommand = new DelegateCommand(() => LocateDarkBarracks(), LocateDarkBarracksCanExecute);
                return _locateDarkBarracksCommand;
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
        /// Determines whether the LocateCollectorsCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
        private bool LocateCollectorsCanExecute()
        {
            return !StartStopState; // only executes if bot is not working?
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
            return !StartStopState; // only executes if bot is not working?
        }

        /// <summary>
        /// Determines whether the LocateBarracksCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
        private bool LocateBarracksCanExecute()
        {
            return !StartStopState; // only executes if bot is not working?
        }

        /// <summary>
        /// Determines whether the LocateDarkBarracksCommand command can be executed.
        /// </summary>
        /// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
        private bool LocateDarkBarracksCanExecute()
        {
            return !StartStopState; // only executes if bot is not working?
        }

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
            ClearOutput(); // Clear everything before we start

			// Colored Output test
            //WriteToOutput("this is title.");
            //WriteToOutput("this is another title");
            //WriteToOutput("this is colored title", GlobalVariables.OutputStates.Information);
            //WriteToOutput("this is colored title", GlobalVariables.OutputStates.Verified);
            //WriteToOutput("this is colored bold title", GlobalVariables.OutputStates.Error);
            //WriteToOutput("this is colored title", GlobalVariables.OutputStates.Warning);

			//var color = CoC.Bot.UI.Utils.OutputColors.Salmon;

			Functions.Main.Initialize(this); // <--- Main entry point

			// Sample for getting familiar with the UI (used for accessing the properties/user settings values)
			//Samples.GettingAroundTheUI.UseValuesInUI(this);

            //WriteToOutput("Trying some simple captures within FastFind, and Keyboard injection");
            //MessageBox.Show("Trying some simple captures within FastFind, and Keyboard injection", "Start", MessageBoxButton.OK, MessageBoxImage.Information);
            //FastFindTesting.Test();
            //KeyboardHelper.BSTest();
            //KeyboardHelper.BSTest2();
            //KeyboardHelper.NotePadTest();
        }

        /// <summary>
        /// Stops the bot functionality
        /// </summary>
        private void Stop()
        {
            WriteToOutput(Properties.Resources.OutputBotIsStopping);
            
            MessageBox.Show("You clicked on the Stop button!", "Stop", MessageBoxButton.OK, MessageBoxImage.Information);

            WriteToOutput(Properties.Resources.OutputBotStopped);
        }

        /// <summary>
        /// Hide the bot
        /// </summary>
        private void HideBlueStacks()
        {
			WriteToOutput(Properties.Resources.OutputHideBlueStacks);
			BlueStackHelper.HideBlueStack();
        }

		private void RestoreBlueStacks()
		{
			WriteToOutput(Properties.Resources.OutputRestoreBlueStacks);
			BlueStackHelper.RestoreBlueStack();
		}

        /// <summary>
        /// Manually locates the Collectors and Mines.
        /// </summary>
        public void LocateCollectors()
        {
			Notify("Locate Collectors and Gold Mines...");
            WriteToOutput("Locate Collectors and Gold Mines...");
            MessageBox.Show("You clicked on the Locate Collectors Manually button!", "Locate Collectors Manually", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Starts the Search Mode.
        /// </summary>
        private void SearchMode()
        {
            WriteToOutput("Search Mode...");
            MessageBox.Show("You clicked on the Search Mode button!", "Search Mode", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Manually locates the Clan Castle.
        /// </summary>
        public void LocateClanCastle()
        {
			Notify("Locating Clan Castle...");
            WriteToOutput("Locating Clan Castle...");
            MessageBox.Show("You clicked on the Locate Clan Castle Manually button!", "Locate Clan Castle Manually", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Manually locates the Barracks.
        /// </summary>
        public void LocateBarracks()
        {
			Notify("Locating Barracks...");
            WriteToOutput("Locate Barracks...");
            MessageBox.Show("You clicked on the Locate Barracks Manually button!", "Locate Barracks Manually", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Manually locates the Dark Barracks.
        /// </summary>
        public void LocateDarkBarracks()
        {
			Notify("Locating Dark Barracks...");
            WriteToOutput("Locate Dark Barracks...");
            MessageBox.Show("You clicked on the Locate Dark Barracks Manually button!", "Locate Dark Barracks Manually", MessageBoxButton.OK, MessageBoxImage.Information);
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
				run.Attr("FontWeight", fontWight);

			run.Value = string.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, message);
			par.Add(run);

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
			notifyService.Notify(message);
		}

        #endregion

		#region Calculation Methods

		/// <summary>
		/// Calculates the total troop capacity.
		/// </summary>
		/// <returns>System.Int32.</returns>
		private static int CalculateTroopCapacity()
		{
			int value = 0;

			AllAttackTroops.Sum(s => s.TrainQuantity);

			foreach (var tier in Enum.GetValues(typeof(TroopType)))
			{
				switch((TroopType)tier)
				{
					case TroopType.Tier1:
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Barbarian].TrainQuantity * Troop.Barbarian.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Archer].TrainQuantity * Troop.Archer.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Goblin].TrainQuantity * Troop.Goblin.CampSlots();
						break;
					case TroopType.Tier2:
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Giant].TrainQuantity * Troop.Giant.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.WallBreaker].TrainQuantity * Troop.WallBreaker.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Balloon].TrainQuantity * Troop.Balloon.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Wizard].TrainQuantity * Troop.Wizard.CampSlots();
						break;
					case TroopType.Tier3:
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Healer].TrainQuantity * Troop.Healer.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Dragon].TrainQuantity * Troop.Dragon.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Pekka].TrainQuantity * Troop.Pekka.CampSlots();
						break;
					case TroopType.DarkTroops:
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Minion].TrainQuantity * Troop.Minion.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.HogRider].TrainQuantity * Troop.HogRider.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Valkyrie].TrainQuantity * Troop.Valkyrie.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Golem].TrainQuantity * Troop.Golem.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Witch].TrainQuantity * Troop.Witch.CampSlots();
						value += DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.LavaHound].TrainQuantity * Troop.LavaHound.CampSlots();
						break;
					default:
						// Troop Type Heroes, do nothing!
						break;
				}
			}

			return value;
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

            IsAlertWhenBaseFound = AppSettings.IsAlertWhenBaseFound;

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

            // Donate Settings
            IsRequestTroops = AppSettings.IsRequestTroops;
            RequestTroopsMessage = AppSettings.RequestTroopsMessage;

			// Location Settings
			// TODO: Change the type from Windows.Drawing.Point to System.Windows.Point and get rid of the reference
			LocationClanCastle = AppSettings.LocationClanCastle;

			LocationBarrack1 = AppSettings.LocationBarrack1;
			LocationBarrack2 = AppSettings.LocationBarrack2;
			LocationBarrack3 = AppSettings.LocationBarrack3;
			LocationBarrack4 = AppSettings.LocationBarrack4;

			LocationDarkBarrack1 = AppSettings.LocationDarkBarrack1;
			LocationDarkBarrack2 = AppSettings.LocationDarkBarrack2;

			LocationAllTrapsRearm = AppSettings.LocationAllTrapsRearm;
			LocationAllXbowsRearm = AppSettings.LocationAllXbowsRearm;

			LocationCollector1 = AppSettings.LocationCollector1;
			LocationCollector2 = AppSettings.LocationCollector2;
			LocationCollector3 = AppSettings.LocationCollector3;
			LocationCollector4 = AppSettings.LocationCollector4;
			LocationCollector5 = AppSettings.LocationCollector5;
			LocationCollector6 = AppSettings.LocationCollector6;
			LocationCollector7 = AppSettings.LocationCollector7;
			LocationCollector8 = AppSettings.LocationCollector8;
			LocationCollector9 = AppSettings.LocationCollector9;
			LocationCollector10 = AppSettings.LocationCollector10;
			LocationCollector11 = AppSettings.LocationCollector11;
			LocationCollector12 = AppSettings.LocationCollector12;
			LocationCollector13 = AppSettings.LocationCollector13;
			LocationCollector14 = AppSettings.LocationCollector14;
			LocationCollector15 = AppSettings.LocationCollector15;
			LocationCollector16 = AppSettings.LocationCollector16;
			LocationCollector17 = AppSettings.LocationCollector17;
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

            AppSettings.IsAlertWhenBaseFound = IsAlertWhenBaseFound;

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

            // Donate Settings
            AppSettings.IsRequestTroops = IsRequestTroops;
            AppSettings.RequestTroopsMessage = RequestTroopsMessage;

            foreach (var tier in Enum.GetValues(typeof(TroopType)))
            {
                switch ((TroopType)tier)
                {
                    case TroopType.Tier1:
                        AppSettings.TroopsQtyBarbarians = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Barbarian].TrainQuantity;
                        AppSettings.TroopsQtyArchers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Archer].TrainQuantity;
                        AppSettings.TroopsQtyGoblins = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Goblin].TrainQuantity;

                        AppSettings.IsDonateAllBarbarians = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Barbarian].IsDonateAll;
                        AppSettings.IsDonateAllArchers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Archer].IsDonateAll;
                        AppSettings.IsDonateAllGoblins = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Goblin].IsDonateAll;

                        AppSettings.DonateKeywordsBarbarians = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Barbarian].DonateKeywords;
                        AppSettings.DonateKeywordsArchers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Archer].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Goblin].DonateKeywords;
                        
                        AppSettings.MaxDonationsPerRequestBarbarians = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Barbarian].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestArchers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Archer].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestGoblins = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Goblin].MaxDonationsPerRequest;
                        break;
                    case TroopType.Tier2:
                        AppSettings.TroopsQtyGiants = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Giant].TrainQuantity;
                        AppSettings.TroopsQtyWallBreakers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.WallBreaker].TrainQuantity;
                        AppSettings.TroopsQtyBalloons = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Balloon].TrainQuantity;
                        AppSettings.TroopsQtyWizards = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Wizard].TrainQuantity;

                        AppSettings.IsDonateAllGiants = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Giant].IsDonateAll;
                        AppSettings.IsDonateAllWallBreakers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.WallBreaker].IsDonateAll;
                        AppSettings.IsDonateAllBalloons = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Balloon].IsDonateAll;
                        AppSettings.IsDonateAllWizards = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Wizard].IsDonateAll;

                        AppSettings.DonateKeywordsBarbarians = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Giant].DonateKeywords;
                        AppSettings.DonateKeywordsArchers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.WallBreaker].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Balloon].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Wizard].DonateKeywords;

                        AppSettings.MaxDonationsPerRequestGiants = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Giant].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestWallBreakers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.WallBreaker].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestBalloons = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Balloon].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestWizards = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Wizard].MaxDonationsPerRequest;
                        break;
                    case TroopType.Tier3:
                        AppSettings.TroopsQtyHealers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Healer].TrainQuantity;
                        AppSettings.TroopsQtyDragons = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Dragon].TrainQuantity;
                        AppSettings.TroopsQtyPekkas = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Pekka].TrainQuantity;

                        AppSettings.IsDonateAllHealers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Healer].IsDonateAll;
                        AppSettings.IsDonateAllDragons = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Dragon].IsDonateAll;
                        AppSettings.IsDonateAllPekkas = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Pekka].IsDonateAll;

                        AppSettings.DonateKeywordsBarbarians = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Healer].DonateKeywords;
                        AppSettings.DonateKeywordsArchers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Dragon].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Pekka].DonateKeywords;

                        AppSettings.MaxDonationsPerRequestHealers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Healer].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestDragons = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Dragon].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestPekkas = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Pekka].MaxDonationsPerRequest;
                        break;
                    case TroopType.DarkTroops:
                        AppSettings.TroopsQtyMinions = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Minion].TrainQuantity;
                        AppSettings.TroopsQtyHogRiders = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.HogRider].TrainQuantity;
                        AppSettings.TroopsQtyValkyries = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Valkyrie].TrainQuantity;
                        AppSettings.TroopsQtyGolems = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Golem].TrainQuantity;
                        AppSettings.TroopsQtyWitches = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Witch].TrainQuantity;
                        AppSettings.TroopsQtyLavaHounds = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.LavaHound].TrainQuantity;

                        AppSettings.IsDonateAllMinions = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Minion].IsDonateAll;
                        AppSettings.IsDonateAllHogRiders = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.HogRider].IsDonateAll;
                        AppSettings.IsDonateAllValkyries = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Valkyrie].IsDonateAll;
                        AppSettings.IsDonateAllGolems = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Golem].IsDonateAll;
                        AppSettings.IsDonateAllWitches = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Witch].IsDonateAll;
                        AppSettings.IsDonateAllLavaHounds = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.LavaHound].IsDonateAll;

                        AppSettings.DonateKeywordsBarbarians = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Minion].DonateKeywords;
                        AppSettings.DonateKeywordsArchers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.HogRider].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Valkyrie].DonateKeywords;
                        AppSettings.DonateKeywordsBarbarians = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Golem].DonateKeywords;
                        AppSettings.DonateKeywordsArchers = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Witch].DonateKeywords;
                        AppSettings.DonateKeywordsGoblins = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.LavaHound].DonateKeywords;

                        AppSettings.MaxDonationsPerRequestMinions = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Minion].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestHogRiders = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.HogRider].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestValkyries = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Valkyrie].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestGolems = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Golem].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestWitches = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.Witch].MaxDonationsPerRequest;
                        AppSettings.MaxDonationsPerRequestLavaHounds = DataCollection.TroopTiers[(int)tier].Troops[(int)Troop.LavaHound].MaxDonationsPerRequest;
                        break;
                    default:
                        // Troop Type Heroes, do nothing!
                        break;
                }
            }

			// Location Settings
			// TODO: Change the type from Windows.Drawing.Point to System.Windows.Point and get rid of the reference
			AppSettings.LocationClanCastle = LocationClanCastle;

			AppSettings.LocationBarrack1 = LocationBarrack1;
			AppSettings.LocationBarrack2 = LocationBarrack2;
			AppSettings.LocationBarrack3 = LocationBarrack3;
			AppSettings.LocationBarrack4 = LocationBarrack4;

			AppSettings.LocationDarkBarrack1 = LocationDarkBarrack1;
			AppSettings.LocationDarkBarrack2 = LocationDarkBarrack2;

			AppSettings.LocationAllTrapsRearm = LocationAllTrapsRearm;
			AppSettings.LocationAllXbowsRearm = LocationAllXbowsRearm;

			AppSettings.LocationCollector1 = LocationCollector1;
			AppSettings.LocationCollector2 = LocationCollector2;
			AppSettings.LocationCollector3 = LocationCollector3;
			AppSettings.LocationCollector4 = LocationCollector4;
			AppSettings.LocationCollector5 = LocationCollector5;
			AppSettings.LocationCollector6 = LocationCollector6;
			AppSettings.LocationCollector7 = LocationCollector7;
			AppSettings.LocationCollector8 = LocationCollector8;
			AppSettings.LocationCollector9 = LocationCollector9;
			AppSettings.LocationCollector10 = LocationCollector10;
			AppSettings.LocationCollector11 = LocationCollector11;
			AppSettings.LocationCollector12 = LocationCollector12;
			AppSettings.LocationCollector13 = LocationCollector13;
			AppSettings.LocationCollector14 = LocationCollector14;
			AppSettings.LocationCollector15 = LocationCollector15;
			AppSettings.LocationCollector16 = LocationCollector16;
			AppSettings.LocationCollector17 = LocationCollector17;

            // Save it!
            AppSettings.Save();
        }

        #endregion
    }
}
