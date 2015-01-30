namespace CoC.Bot.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using CoC.Bot.UI.Commands;
    using System.Windows;

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

        private bool _meetGoldAndElixir;
        public bool MeetGoldAndElixir
        {
            get { return _meetGoldAndElixir; }
            set
            {
                if (_meetGoldAndElixir != value)
                {
                    _meetGoldAndElixir = value;
                    OnPropertyChanged("MeetGoldAndElixir");
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

        #endregion

        #region Commands

        private AboutCommand _aboutCommand = new AboutCommand();
        public ICommand AboutCommand
        {
            get { return _aboutCommand; }
        }

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

        #endregion

        #region Main Methods

        /// <summary>
        /// Starts the bot functionality
        /// </summary>
        private void Start()
        {
            
        }

        /// <summary>
        /// Stops the bot functionality
        /// </summary>
        private void Stop()
        {

        }

        /// <summary>
        /// Hide the bot
        /// </summary>
        private void Hide()
        {

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
            MeetGoldAndElixir = Properties.Settings.Default.MeetGoldAndElixir;
            MeetDarkElixir = Properties.Settings.Default.MeetDarkElixir;
            MeetTrophyCount = Properties.Settings.Default.MeetTrophyCount;

            MinimumGold = Properties.Settings.Default.MinGold;
            MinimumElixir = Properties.Settings.Default.MinElixir;
            MinimumDarkElixir = Properties.Settings.Default.MinDarkElixir;
            MinimumTrophyCount = Properties.Settings.Default.MinTrophyCount;

            AlertWhenBaseFound = Properties.Settings.Default.AlertWhenBaseFound;
        }

        /// <summary>
        /// Saves the application user settings.
        /// </summary>
        private void SaveUserSettings()
        {
            // General
            Properties.Settings.Default.MaxTrophies = MaxTrophies;

            // Search Settings
            Properties.Settings.Default.MeetGoldAndElixir = MeetGoldAndElixir;
            Properties.Settings.Default.MeetDarkElixir = MeetDarkElixir;
            Properties.Settings.Default.MeetTrophyCount = MeetTrophyCount;

            Properties.Settings.Default.MinGold = MinimumGold;
            Properties.Settings.Default.MinElixir = MinimumElixir;
            Properties.Settings.Default.MinDarkElixir = MinimumDarkElixir;
            Properties.Settings.Default.MinTrophyCount = MinimumTrophyCount;

            Properties.Settings.Default.AlertWhenBaseFound = AlertWhenBaseFound;

            // Save it!
            Properties.Settings.Default.Save();
        }

        #endregion
    }
}