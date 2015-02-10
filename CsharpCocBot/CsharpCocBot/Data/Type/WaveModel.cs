namespace CoC.Bot.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using ViewModels;

	/// <summary>
	/// The Wave Data Model.
	/// </summary>
	public class WaveModel : ViewModelBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WaveModel"/> class.
		/// </summary>
		public WaveModel()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WaveModel"/> class.
		/// </summary>
		/// <param name="troop">The TroopModel.</param>
		/// <param name="deployQuantity">The deploy quantity.</param>
		/// <param name="deployDelay">The deploy delay in milliseconds.</param>
		internal WaveModel(TroopModel troop, int deployQuantity, int deployDelay)
        {
			_troop = troop;
			_deployQuantity = deployQuantity;
			_deployDelay = deployDelay;
        }

		/// <summary>
		/// Creates the new WaveModel.
		/// </summary>
		/// <param name="troop">The TroopModel.</param>
		/// <param name="deployQuantity">The deploy quantity.</param>
		/// <param name="deployDelay">The deploy delay in milliseconds.</param>
		/// <returns>WaveModel.</returns>
		public static WaveModel CreateNew(TroopModel troop, int deployQuantity, int deployDelay)
		{
			return new WaveModel(troop, deployQuantity, deployDelay);
		}

		#region Properties

		private TroopModel _troop;
		public TroopModel Troop
		{
			get { return _troop; }
			set
			{
				_troop = value;
				OnPropertyChanged();
			}
		}

		private int _deployQuantity;
		public int DeployQuantity
		{
			get { return _deployQuantity; }
			set
			{
				if (_deployQuantity != value)
				{
					_deployQuantity = value;
					OnPropertyChanged();
				}
			}
		}

		private int _deployDelay;
		public int DeployDelay
		{
			get { return _deployDelay; }
			set
			{
				if (_deployDelay != value)
				{
					_deployDelay = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion
	}
}