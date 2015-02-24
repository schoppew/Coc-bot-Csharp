namespace CoC.Bot.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using ViewModels;

	/// <summary>
	/// The Graph Data Model.
	/// </summary>
	public class GraphModel : ViewModelBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GraphModel"/> class.
        /// </summary>
		public GraphModel()
        {

        }

		/// <summary>
		/// Initializes a new instance of the <see cref="GraphModel"/> class.
		/// </summary>
		/// <param name="resource">The Resource.</param>
		/// <param name="amount">The amount.</param>
		internal GraphModel(Resource resource, int amount)
        {
			_resource = resource;
			_amount = amount;
		}

		/// <summary>
		/// Creates a new GraphModel.
		/// </summary>
		/// <param name="resource">The Resource.</param>
		/// <param name="amount">The amount.</param>
		/// <returns>GraphModel.</returns>
		public static GraphModel CreateNew(Resource resource, int amount)
		{
			return new GraphModel(resource, amount);
		}

		#region Properties

		private Resource _resource;
		public Resource Resource
		{
			get { return _resource; }
			set
			{
				if (_resource != value)
				{
					_resource = value;
					OnPropertyChanged();
				}
			}
		}

		private int _amount;
		public int Amount
		{
			get { return _amount; }
			set
			{
				if (_amount != value)
				{
					_amount = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion
	}
}