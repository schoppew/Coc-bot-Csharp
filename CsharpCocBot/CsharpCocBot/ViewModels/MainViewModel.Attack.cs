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
	}
}