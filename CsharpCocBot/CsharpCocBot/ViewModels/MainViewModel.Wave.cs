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
		/// [Used in UI for Binding] Gets available Troops for Wave based on Custom Troops.
		/// </summary>
		/// <value>Available Troops for Wave.</value>
		public static IEnumerable<TroopModel> TroopsForWave { get { return AllAttackTroops.Where(t => t.TrainQuantity > 0).Distinct(); } }

		private TroopModel _selectedTroopForWave;
		/// <summary>
		/// [For use in UI only] Gets or sets the selected troop for wave.
		/// </summary>
		/// <value>The selected troop for wave.</value>
		public TroopModel SelectedTroopForWave
		{
			get { return _selectedTroopForWave; }
			set
			{
				if (_selectedTroopForWave != value)
				{
					_selectedTroopForWave = value;
					OnPropertyChanged();
					OnPropertyChanged(() => SelectedTroopForWaveQuantity);
				}
			}
		}

		//private int _selectedWaveTroopQuantity;
		/// <summary>
		/// [For use in UI only] Gets or sets the selected troop for wave quantity.
		/// </summary>
		/// <value>The selected troop for wave quantity.</value>
		public int SelectedTroopForWaveQuantity
		{
			get
			{
				var troop = (TroopModel)SelectedTroopForWave;
				if (troop == null)
					return 0;

				return troop.TrainQuantity;
			}
			//set
			//{
			//	if (_selectedWaveTroopQuantity != value)
			//	{
			//		var troop = (TroopModel)SelectedWaveTroop;
			//		troop.TrainQuantity = value;

			//		_selectedWaveTroopQuantity = value;
			//		OnPropertyChanged();
			//	}
			//}
		}

		private int _selectedTroopForWaveDelay;
		/// <summary>
		/// [For use in UI only] Gets or sets the selected troop for wave delay.
		/// </summary>
		/// <value>The selected troop for wave delay.</value>
		public int SelectedTroopForWaveDelay
		{
			get { return _selectedTroopForWaveDelay; }
			set
			{
				if (_selectedTroopForWaveDelay != value)
				{
					_selectedTroopForWaveDelay = value;
					OnPropertyChanged();
				}
			}
		}

		/// <summary>
		/// [Used in UI for Binding] Gets the custom Wave.
		/// </summary>
		/// <value>Custom Wave.</value>
		public static ObservableCollection<WaveModel> WaveTroops { get { return DataCollection.CustomWaves; } }

		private WaveModel _selectedWaveTroop;
		/// <summary>
		/// [For use in UI only] Gets or sets the selected wave troop.
		/// </summary>
		/// <value>The selected wave troop.</value>
		public WaveModel SelectedWaveTroop
		{
			get { return _selectedWaveTroop; }
			set
			{
				if (_selectedWaveTroop != value)
				{
					_selectedWaveTroop = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isCustomWave;
		/// <summary>
		/// Gets or sets a value indicating whether it should use custom Wave.
		/// </summary>
		/// <value><c>true</c> if custom Wave; otherwise, <c>false</c>.</value>
		public bool IsCustomWave
		{
			get { return _isCustomWave; }
			set
			{
				if (_isCustomWave != value)
				{
					_isCustomWave = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion

		#region Commands

		public ICommand AddTroopForCustomWaveCommand
		{
			get { return new RelayCommand(() => AddTroopForCustomWave()); }
		}

		public ICommand RemoveTroopForCustomWaveCommand
		{
			get { return new RelayCommand(() => RemoveTroopForCustomWave()); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Adds the Troop for Custom Wave.
		/// </summary>
		private void AddTroopForCustomWave()
		{
			if (SelectedTroopForWave != null)
				WaveTroops.Add(WaveModel.CreateNew(SelectedTroopForWave, SelectedTroopForWaveQuantity, SelectedTroopForWaveDelay));
		}

		/// <summary>
		/// Removes the Troop for Custom Wave.
		/// </summary>
		private void RemoveTroopForCustomWave()
		{
			if (SelectedWaveTroop != null)
				WaveTroops.Remove(SelectedWaveTroop);
		}

		#endregion
	}
}