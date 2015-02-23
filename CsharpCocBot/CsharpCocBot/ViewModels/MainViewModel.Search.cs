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
	using CoC.Bot.Data.Type;

	/// <summary>
	/// Provides functionality for the MainWindow
	/// </summary>
	public partial class MainViewModel : ViewModelBase
	{
		public enum SearchCondition { Any = 1, All = 2 };

		#region Properties

		private bool _isMeetGold;
		/// <summary>
		/// Gets or sets a value indicating whether should meet Gold conditions.
		/// </summary>
		/// <value><c>true</c> if should meet Gold conditions; otherwise, <c>false</c>.</value>
		public bool IsMeetGold
		{
			get { return _isMeetGold; }
			set
			{
				if (_isMeetGold != value)
				{
					_isMeetGold = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isMeetElixir;
		/// <summary>
		/// Gets or sets a value indicating whether should meet Elixir conditions.
		/// </summary>
		/// <value><c>true</c> if should meet Elixir conditions; otherwise, <c>false</c>.</value>
		public bool IsMeetElixir
		{
			get { return _isMeetElixir; }
			set
			{
				if (_isMeetElixir != value)
				{
					_isMeetElixir = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isMeetDarkElixir;
		/// <summary>
		/// Gets or sets a value indicating whether should meet Dark Elixir conditions.
		/// </summary>
		/// <value><c>true</c> if should meet Dark Elixir conditions; otherwise, <c>false</c>.</value>
		public bool IsMeetDarkElixir
		{
			get { return _isMeetDarkElixir; }
			set
			{
				if (_isMeetDarkElixir != value)
				{
					_isMeetDarkElixir = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isMeetTrophyCount;
		/// <summary>
		/// Gets or sets a value indicating whether should meet Trophy count conditions.
		/// </summary>
		/// <value><c>true</c> if should meet Trophy count conditions; otherwise, <c>false</c>.</value>
		public bool IsMeetTrophyCount
		{
			get { return _isMeetTrophyCount; }
			set
			{
				if (_isMeetTrophyCount != value)
				{
					_isMeetTrophyCount = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isMeetTownhallLevel;
		/// <summary>
		/// Gets or sets a value indicating whether should meet Townhall level conditions.
		/// </summary>
		/// <value><c>true</c> if should meet Townhall level conditions; otherwise, <c>false</c>.</value>
		public bool IsMeetTownhallLevel
		{
			get { return _isMeetTownhallLevel; }
			set
			{
				if (_isMeetTownhallLevel != value)
				{
					_isMeetTownhallLevel = value;
					OnPropertyChanged();
				}
			}
		}

		private SearchCondition _selectedSearchCondition;
		/// <summary>
		/// Gets or sets the selected Search condition.
		/// </summary>
		/// <value>The selected Search condition.</value>
		public SearchCondition SelectedSearchCondition
		{
			get { return _selectedSearchCondition; }
			set
			{
				if (_selectedSearchCondition != value)
				{
					_selectedSearchCondition = value;
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

		private int _maximumTownhallLevel;
		/// <summary>
		/// Gets or sets the minimum Townhall level.
		/// </summary>
		/// <value>The minimum Townhall level.</value>
		public int MaximumTownhallLevel
		{
			get { return _maximumTownhallLevel; }
			set
			{
				if (_maximumTownhallLevel != value)
				{
					_maximumTownhallLevel = value;
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

		private bool _isTakeSnapshotAllTowns;
		/// <summary>
		/// Gets or sets a value indicating whether should take a snapshot of all Townhalls.
		/// </summary>
		/// <value><c>true</c> if take a snapshot of all Townhalls; otherwise, <c>false</c>.</value>
		public bool IsTakeSnapshotAllTowns
		{
			get { return _isTakeSnapshotAllTowns; }
			set
			{
				if (_isTakeSnapshotAllTowns != value)
				{
					_isTakeSnapshotAllTowns = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isTakeSnapshotAllLoots;
		/// <summary>
		/// Gets or sets a value indicating whether should take a snapshot of all Loots.
		/// </summary>
		/// <value><c>true</c> if take a snapshot of all Loots; otherwise, <c>false</c>.</value>
		public bool IsTakeSnapshotAllLoots
		{
			get { return _isTakeSnapshotAllLoots; }
			set
			{
				if (_isTakeSnapshotAllLoots != value)
				{
					_isTakeSnapshotAllLoots = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion

		#region Can Execute Methods

		/// <summary>
		/// Determines whether the SearchModeCommand command can be executed.
		/// </summary>
		/// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
		private bool SearchModeCanExecute()
		{
			return true; // TODO: We need to define this
		}

		#endregion

		#region Commands

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

		#region Methods

		/// <summary>
		/// Starts the Search Mode.
		/// </summary>
		private void SearchMode()
		{
			// Code for showing that it works
			WriteToOutput("Search Mode...");
			Notify("Search Mode...");
			System.Diagnostics.Debug.WriteLine("Search Mode...");
		}

		#endregion
	}
}