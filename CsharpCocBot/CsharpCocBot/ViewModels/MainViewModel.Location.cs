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
		#region Properties

		/// <summary>
		/// [Used in UI for Binding] Gets the Building Points.
		/// </summary>
		/// <value>The Building points.</value>
		public static ObservableCollection<BuildingPointModel> BuildingPoints { get { return DataCollection.BuildingPoints; } }

		#endregion

		#region Commands

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

		private DelegateCommand _locateMinesCommand;
		public ICommand LocateMinesCommand
		{
			get
			{
				if (_locateMinesCommand == null)
					_locateMinesCommand = new DelegateCommand(() => LocateMines(), LocateMinesCanExecute);
				return _locateMinesCommand;
			}
		}

		private DelegateCommand _locateDrillsCommand;
		public ICommand LocateDrillsCommand
		{
			get
			{
				if (_locateDrillsCommand == null)
					_locateDrillsCommand = new DelegateCommand(() => LocateDrills(), LocateDrillsCanExecute);
				return _locateDrillsCommand;
			}
		}

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

		private DelegateCommand _locateTownHallCommand;
		public ICommand LocateTownHallCommand
		{
			get
			{
				if (_locateTownHallCommand == null)
					_locateTownHallCommand = new DelegateCommand(() => LocateTownHall(), LocateTownHallCanExecute);
				return _locateTownHallCommand;
			}
		}

		public ICommand LocateSingleBuildingCommand
		{
			get { return new RelayCommand(() => LocateSingleBuilding()); }
		}

		public ICommand RelocateSingleBuildingCommand
		{
			get { return new RelayCommand(() => RelocateSingleBuilding()); }
		}

		public ICommand ClearLocationSingleBuildingCommand
		{
			get { return new RelayCommand(() => ClearLocationSingleBuilding()); }
		}

		#endregion

		#region Can Execute Methods

		/// <summary>
		/// Determines whether the LocateCollectorsCommand command can be executed.
		/// </summary>
		/// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
		private bool LocateCollectorsCanExecute()
		{
			return !StartStopState; // only executes if bot is not running
		}

		/// <summary>
		/// Determines whether the LocateMinesCommand command can be executed.
		/// </summary>
		/// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
		private bool LocateMinesCanExecute()
		{
			return !StartStopState; // only executes if bot is not running
		}

		/// <summary>
		/// Determines whether the LocateDrillsCommand command can be executed.
		/// </summary>
		/// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
		private bool LocateDrillsCanExecute()
		{
			return !StartStopState; // only executes if bot is not running
		}

		/// <summary>
		/// Determines whether the LocateClanCastleCommand command can be executed.
		/// </summary>
		/// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
		private bool LocateClanCastleCanExecute()
		{
			return !StartStopState; // only executes if bot is not running
		}

		/// <summary>
		/// Determines whether the LocateBarracksCommand command can be executed.
		/// </summary>
		/// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
		private bool LocateBarracksCanExecute()
		{
			return !StartStopState; // only executes if bot is not running
		}

		/// <summary>
		/// Determines whether the LocateDarkBarracksCommand command can be executed.
		/// </summary>
		/// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
		private bool LocateDarkBarracksCanExecute()
		{
			return !StartStopState; // only executes if bot is not running
		}

		/// <summary>
		/// Determines whether the LocateTownhallCommand command can be executed.
		/// </summary>
		/// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
		private bool LocateTownHallCanExecute()
		{
			return !StartStopState; // only executes if bot is not running
		}

		#endregion

		#region Methods

		/// <summary>
		/// Manually locates the Elixir Collectors.
		/// </summary>
		public void LocateCollectors()
		{
			var msgBox = GetService<IMessageBoxService>();
			if (msgBox != null)
			{
				if (msgBox.Show("Do you want to start locating the Collectors, Mines and Drills?", "Locate Extractors Manually", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
					return;

				var p = BlueStacksHelper.GetClickPosition(2000);
				if (p.IsEmpty) return;

				msgBox.Show(string.Format("You clicked on X={0}, Y={1}. Please test those coords with BlueStacksHelper.Click() method.", p.X, p.Y), "Location Test", MessageBoxButton.OK, MessageBoxImage.Information);

				// TODO: This is a fixed number, use a GlobalVariables so is consistent between all code (Total Collectors = 17)
				/*
				for (int i = 1; i < 18; i++)
				{
					if (msgBox.Show(string.Format("Click OK, then click on your resource extractor # {0}. Press Cancel to abort the process.", i), string.Format("Locate resource extractor # {0}", i), MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.Cancel)
						break;

					var p = BlueStacksHelper.GetClickPosition(2000);
					if (p.IsEmpty) continue;

					string message = string.Format("Resource Extractor # {0} have the following {1},{2} coords.", i, p.X, p.Y);
					System.Diagnostics.Debug.WriteLine(message);

					GlobalVariables.Log.WriteToLog(message);
				}
				 */
			}
		}

		/// <summary>
		/// Manually locates the Gold Mines.
		/// </summary>
		public void LocateMines()
		{
			// Code for showing that it works
			Notify("Locating Gold Mines...");
			System.Diagnostics.Debug.WriteLine("Locate Gold Mines...");
		}

		/// <summary>
		/// Manually locates the Dark Elixir Drills.
		/// </summary>
		public void LocateDrills()
		{
			// Code for showing that it works
			Notify("Locating Dark Elixir Drills...");
			System.Diagnostics.Debug.WriteLine("Locate Dark Elixir Drills...");
		}

		/// <summary>
		/// Manually locates the Clan Castle.
		/// </summary>
		public void LocateClanCastle()
		{
			// Code for showing that it works
			Notify("Locating Clan Castle...");
			System.Diagnostics.Debug.WriteLine("Locate Clan Castle...");
		}

		/// <summary>
		/// Manually locates the Barracks.
		/// </summary>
		public void LocateBarracks()
		{
			// Code for showing that it works
			Notify("Locating Barracks...");
			System.Diagnostics.Debug.WriteLine("Locate Barracks...");
		}

		/// <summary>
		/// Manually locates the Dark Barracks.
		/// </summary>
		public void LocateDarkBarracks()
		{
			// Code for showing that it works
			Notify("Locating Dark Barracks...");
			System.Diagnostics.Debug.WriteLine("Locate Dark Barracks...");
		}

		/// <summary>
		/// Manually locates the Townhall.
		/// </summary>
		private void LocateTownHall()
		{
			// Code for showing that it works
			Notify("Locating Town Hall...");
			System.Diagnostics.Debug.WriteLine("Locate Town Hall...");
		}

		/// <summary>
		/// Manually locates a single building.
		/// </summary>
		/// <returns>System.Object.</returns>
		private void LocateSingleBuilding()
		{
			// Code for showing that it works
			Notify("Locate Building...");
			System.Diagnostics.Debug.WriteLine("Locate Building...");
		}

		/// <summary>
		/// Manually relocates a single building.
		/// </summary>
		/// <returns>System.Object.</returns>
		private void RelocateSingleBuilding()
		{
			// Code for showing that it works
			Notify("Relocate Building...");
			System.Diagnostics.Debug.WriteLine("Relocate Building...");
		}

		/// <summary>
		/// Manually clears a location of a single building.
		/// </summary>
		/// <returns>System.Object.</returns>
		private void ClearLocationSingleBuilding()
		{
			// Code for showing that it works
			Notify("Clear Location...");
			System.Diagnostics.Debug.WriteLine("Clear Location...");
		}

		#endregion
	}
}