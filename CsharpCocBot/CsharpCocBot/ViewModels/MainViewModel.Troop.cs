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
		/// [Used in UI for Binding] Gets All Attack Troops.
		/// </summary>
		/// <value>All Attack Troops.</value>
		public static IEnumerable<TroopModel> AllAttackTroops { get { return DataCollection.TroopTiers.SelectMany(tt => tt.Troops).Distinct(); } }

		private int _troopCapacity;
		/// <summary>
		/// Gets the Total Troop capacity.
		/// </summary>
		/// <value>The total troop capacity.</value>
		public int TroopCapacity
		{
			get
			{
				if (_troopCapacity == 0)
					_troopCapacity = CalculateTroopCapacity();

				return _troopCapacity;
			}
			set
			{
				if (_troopCapacity != value)
				{
					_troopCapacity = value;
					OnPropertyChanged();
				}
			}
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
		public static BindingList<Model> BarrackTroops { get; private set; }

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
					System.Diagnostics.Debug.WriteLine(_selectedBarrack1.Name);
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
					System.Diagnostics.Debug.WriteLine(_selectedBarrack2.Name);
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
					System.Diagnostics.Debug.WriteLine(_selectedBarrack3.Name);
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
					System.Diagnostics.Debug.WriteLine(_selectedBarrack4.Name);
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
		public static BindingList<Model> DarkBarrackTroops { get; private set; }

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
					System.Diagnostics.Debug.WriteLine(_selectedDarkBarrack1.Name);
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
					System.Diagnostics.Debug.WriteLine(_selectedDarkBarrack2.Name);
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

		#region Commands

		public ICommand TrainQuantityTextChangedCommand
		{
			get { return new RelayCommand<object>(val => TrainQuantityTextExecuted(val)); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Fires when TrainQuantity TextChanged is executed.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		private void TrainQuantityTextExecuted(object parameter)
		{
			System.Diagnostics.Debug.WriteLine(parameter);

			TroopCapacity = CalculateTroopCapacity();
		}

		/// <summary>
		/// Calculates the total troop capacity.
		/// </summary>
		/// <returns>System.Int32.</returns>
		private static int CalculateTroopCapacity()
		{
			int value = 0;

			value += DataCollection.TroopTiers[TroopType.Tier1].Troops[Troop.Barbarian].TrainQuantity * Troop.Barbarian.CampSlots();
			value += DataCollection.TroopTiers[TroopType.Tier1].Troops[Troop.Archer].TrainQuantity * Troop.Archer.CampSlots();
			value += DataCollection.TroopTiers[TroopType.Tier1].Troops[Troop.Goblin].TrainQuantity * Troop.Goblin.CampSlots();

			value += DataCollection.TroopTiers[TroopType.Tier2].Troops[Troop.Giant].TrainQuantity * Troop.Giant.CampSlots();
			value += DataCollection.TroopTiers[TroopType.Tier2].Troops[Troop.WallBreaker].TrainQuantity * Troop.WallBreaker.CampSlots();
			value += DataCollection.TroopTiers[TroopType.Tier2].Troops[Troop.Balloon].TrainQuantity * Troop.Balloon.CampSlots();
			value += DataCollection.TroopTiers[TroopType.Tier2].Troops[Troop.Wizard].TrainQuantity * Troop.Wizard.CampSlots();

			value += DataCollection.TroopTiers[TroopType.Tier3].Troops[Troop.Healer].TrainQuantity * Troop.Healer.CampSlots();
			value += DataCollection.TroopTiers[TroopType.Tier3].Troops[Troop.Dragon].TrainQuantity * Troop.Dragon.CampSlots();
			value += DataCollection.TroopTiers[TroopType.Tier3].Troops[Troop.Pekka].TrainQuantity * Troop.Pekka.CampSlots();

			value += DataCollection.TroopTiers[TroopType.DarkTroops].Troops[Troop.Minion].TrainQuantity * Troop.Minion.CampSlots();
			value += DataCollection.TroopTiers[TroopType.DarkTroops].Troops[Troop.HogRider].TrainQuantity * Troop.HogRider.CampSlots();
			value += DataCollection.TroopTiers[TroopType.DarkTroops].Troops[Troop.Valkyrie].TrainQuantity * Troop.Valkyrie.CampSlots();
			value += DataCollection.TroopTiers[TroopType.DarkTroops].Troops[Troop.Golem].TrainQuantity * Troop.Golem.CampSlots();
			value += DataCollection.TroopTiers[TroopType.DarkTroops].Troops[Troop.Witch].TrainQuantity * Troop.Witch.CampSlots();
			value += DataCollection.TroopTiers[TroopType.DarkTroops].Troops[Troop.LavaHound].TrainQuantity * Troop.LavaHound.CampSlots();

			return value;
		}

		#endregion
	}
}