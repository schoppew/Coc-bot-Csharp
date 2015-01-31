namespace CoC.Bot.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
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
            Troops = new TroopModelObservableCollection();
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

        public TroopModelObservableCollection Troops { get; set; }

        #endregion

        /// <summary>
        /// A Troop ObservableCollection with custom index implementation.
        /// </summary>
        public class TroopModelObservableCollection : ObservableCollection<TroopModel>
        {
            public TroopModelObservableCollection()
            {
                
            }

            public TroopModelObservableCollection(List<TroopModel> list) : base(list)
            {
                
            }

            public TroopModelObservableCollection(IEnumerable<TroopModel> collection) : base(collection)
            {
                
            }

            public new TroopModel this[int t]
            {
                get { return base.Items.Where(i => i.Id == (int)t).FirstOrDefault(); }
            }

            /// <summary>
            /// Gets the specified Troop, or a default value if the sequence contains no elements.
            /// </summary>
            /// <param name="type">The Troop enum.</param>
            /// <returns>TroopModel.</returns>
            public TroopModel Get(Troop type)
            {
                return base.Items.Where(i => i.Id == (int)type).FirstOrDefault();
            }
        }

        /// <summary>
        /// A TroopTierModel BindingList with custom index implementation.
        /// </summary>
        public class TroopTierModelBindingList : BindingList<TroopTierModel>
        {
            public TroopTierModelBindingList()
            {
                
            }

            public TroopTierModelBindingList(IList<TroopTierModel> list) : base(list)
            {
                
            }

            public new TroopTierModel this[int t]
            {
                get { return base.Items.Where(i => i.Id == (int)t).FirstOrDefault(); }
            }

            /// <summary>
            /// Gets the specified Troop Tier, or a default value if the sequence contains no elements.
            /// </summary>
            /// <param name="type">The TroopType enum.</param>
            /// <returns>TroopTierModel.</returns>
            public TroopTierModel Get(TroopType type)
            {
                return base.Items.Where(i => i.Id == (int)type).FirstOrDefault();
            }
        }
    }
}