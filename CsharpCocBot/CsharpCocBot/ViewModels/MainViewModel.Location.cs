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
			get { return new RelayCommand<Building>(b => LocateSingleBuilding(b), b => LocationSingleBuildingCanExecute(b)); }
		}

		public ICommand RelocateSingleBuildingCommand
		{
			get { return new RelayCommand<Building>(b => RelocateSingleBuilding(b)); }
		}

		public ICommand ClearLocationSingleBuildingCommand
		{
			get { return new RelayCommand<Building>(b => ClearLocationSingleBuilding(b), b => LocationSingleBuildingCanExecute(b)); }
		}

		public ICommand StopLocatingCommand
		{
			get { return new RelayCommand(() => StopLocating()); }
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

		/// <summary>
		/// Determines whether the LocateSingleBuildingCommand and/or the ClearLocationSingleBuildingCommand command can be executed.
		/// </summary>
		/// <param name="building">The building.</param>
		/// <returns><c>true</c> if can execute, <c>false</c> otherwise.</returns>
		private bool LocationSingleBuildingCanExecute(Building building)
		{
			var bPoint = DataCollection.BuildingPoints.Where(b => b.Building == building).FirstOrDefault();

			if (bPoint == null)
				return false;

			return bPoint.Coordinates.IsEmpty;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Manually locates the Elixir Collectors.
		/// </summary>
		public void LocateCollectors()
		{
			IsBusy = true;

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
			IsBusy = true;

			// Code for showing that it works
			Notify("Locating Gold Mines...");
			System.Diagnostics.Debug.WriteLine("Locate Gold Mines...");
		}

		/// <summary>
		/// Manually locates the Dark Elixir Drills.
		/// </summary>
		public void LocateDrills()
		{
			IsBusy = true;

			// Code for showing that it works
			Notify("Locating Dark Elixir Drills...");
			System.Diagnostics.Debug.WriteLine("Locate Dark Elixir Drills...");
		}

		/// <summary>
		/// Manually locates the Clan Castle.
		/// </summary>
		public void LocateClanCastle()
		{
			IsBusy = true;

			// Code for showing that it works
			Notify("Locating Clan Castle...");
			System.Diagnostics.Debug.WriteLine("Locate Clan Castle...");
		}

		/// <summary>
		/// Manually locates the Barracks.
		/// </summary>
		public void LocateBarracks()
		{
			IsBusy = true;

			// Code for showing that it works
			Notify("Locating Barracks...");
			System.Diagnostics.Debug.WriteLine("Locate Barracks...");
		}

		/// <summary>
		/// Manually locates the Dark Barracks.
		/// </summary>
		public void LocateDarkBarracks()
		{
			IsBusy = true;

			// Code for showing that it works
			Notify("Locating Dark Barracks...");
			System.Diagnostics.Debug.WriteLine("Locate Dark Barracks...");
		}

		/// <summary>
		/// Manually locates the Townhall.
		/// </summary>
		public void LocateTownHall()
		{
			IsBusy = true;
			//Mouse.OverrideCursor = Cursors.Wait;

			// Code for showing that it works
			Notify("Locating Town Hall...");
			System.Diagnostics.Debug.WriteLine("Locate Town Hall...");
		}

		/// <summary>
		/// Manually locates a single building.
		/// </summary>
		/// <param name="building">The Building.</param>
		private void LocateSingleBuilding(Building building)
		{
			System.Diagnostics.Debug.WriteLine(building);

			ProcessSingleBuildingLocation(building, SingleBuildingAction.Select);
		}

		/// <summary>
		/// Manually relocates a single building.
		/// </summary>
		/// <param name="building">The Building.</param>
		private void RelocateSingleBuilding(Building building)
		{
			System.Diagnostics.Debug.WriteLine(building);

			IsBusy = true;

			ProcessSingleBuildingLocation(building, SingleBuildingAction.Relocate);
		}

		/// <summary>
		/// Manually clears a location of a single building.
		/// </summary>
		/// <param name="building">The Building.</param>
		private void ClearLocationSingleBuilding(Building building)
		{
			System.Diagnostics.Debug.WriteLine(building);

			ProcessSingleBuildingLocation(building, SingleBuildingAction.Clear);
		}

		/// <summary>
		/// Stops the locating process.
		/// </summary>
		private void StopLocating()
		{
			IsBusy = false;
			//Mouse.OverrideCursor = null;
			// TODO: Mouse UnHook
		}

		private enum SingleBuildingAction { Select, Relocate, Clear };

		/// <summary>
		/// Processes a single Building location.
		/// </summary>
		/// <param name="building">The Building.</param>
		/// <param name="action">The SingleBuildingAction.</param>
		private void ProcessSingleBuildingLocation(Building building, SingleBuildingAction action)
		{
			var bPoint = DataCollection.BuildingPoints.Where(b => b.Building == building).FirstOrDefault();

			if (bPoint == null)
				return;

			switch (action)
			{
				case SingleBuildingAction.Select:
					Tools.CoCHelper.Click(new ClickablePoint(bPoint.Coordinates));
					break;
				case SingleBuildingAction.Relocate:
					// TODO: Mouse Hook
					break;
				case SingleBuildingAction.Clear:
					bPoint.Coordinates = new Point();
					break;
				default:
					break;
			}
		}

		#endregion
	}
}