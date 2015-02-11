namespace CoC.Bot.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using Point = Win32.POINT;

	using ViewModels;

	/// <summary>
	/// The Building Point Data Model.
	/// </summary>
	public class BuildingPointModel : ViewModelBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BuildingPointModel"/> class.
		/// </summary>
		public BuildingPointModel()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BuildingPointModel"/> class.
		/// </summary>
		/// <param name="building">The BuildingType.</param>
		/// <param name="coordinates">The Point.</param>
		internal BuildingPointModel(Building building, BuildingType type, Point coordinates)
        {
			_building = building;
			_buildingType = type;
			_coordinates = coordinates;
        }

		/// <summary>
		/// Creates a new BuildingPointModel.
		/// </summary>
		/// <param name="building">The BuildingType.</param>
		/// <param name="coordinates">The Point.</param>
		/// <returns>BuildingPointModel.</returns>
		public static BuildingPointModel CreateNew(Building building, BuildingType type, Point coordinates)
		{
			return new BuildingPointModel(building, type, coordinates);
		}

		#region Properties

		private Building _building;
		public Building Building
		{
			get { return _building; }
			set
			{
				if (_building != value)
				{
					_building = value;
					OnPropertyChanged();
				}
			}
		}

		private BuildingType _buildingType;
		public BuildingType BuildingType
		{
			get { return _buildingType; }
			set
			{
				if (_buildingType != value)
				{
					_buildingType = value;
					OnPropertyChanged();
				}
			}
		}

		private Point _coordinates;
		public Point Coordinates
		{
			get { return _coordinates; }
			set
			{
				if (_coordinates != value)
				{
					_coordinates = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion
	}
}