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

        private bool IsStarted { get; set; }

        private bool IsHidden { get; set; }

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
            MaxTrophies = Properties.Settings.Default.MaxTrophies;
        }

        /// <summary>
        /// Saves the application user settings.
        /// </summary>
        private void SaveUserSettings()
        {
            Properties.Settings.Default.MaxTrophies = MaxTrophies;
            Properties.Settings.Default.Save();
        }

        #endregion
    }
}