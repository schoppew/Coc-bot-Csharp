namespace CoC.Bot.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using ViewModels;

    /// <summary>
    /// The Troop Tier Data Model.
    /// </summary>
    public class TroopTierModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TroopTierModel"/> class.
        /// </summary>
        public TroopTierModel()
        {
            Troops = new ObservableCollection<TroopModel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TroopTierModel"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        internal TroopTierModel(int id, string name) : this()
        {
            _id = id;
            _name = name;
        }

        /// <summary>
        /// Creates a new TroopTierModel.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <returns>TroopTierModel.</returns>
        public static TroopTierModel CreateNew(int id, string name)
        {
            return new TroopTierModel(id, name);
        }

        #region Properties

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TroopModel> Troops { get; set; }

        #endregion
    }
}