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
	}
}