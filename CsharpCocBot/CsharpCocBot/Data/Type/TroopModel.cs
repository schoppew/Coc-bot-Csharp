namespace CoC.Bot.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ViewModels;

    /// <summary>
    /// The Troop Data Model.
    /// </summary>
    public class TroopModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TroopModel"/> class.
        /// </summary>
        public TroopModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TroopModel"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="trainQuantity">The quantity to train.</param>
        /// <param name="isSelectedForDonate">Specify if troop is selected for donation.</param>
        /// <param name="isDonateAll">Specify if donate to all.</param>
        /// <param name="donateKeywords">The donate keywords.</param>
        /// <param name="maxDonationsPerRequest">The maximum donations per request.</param>
        internal TroopModel(int id, string name, int trainQuantity, bool isSelectedForDonate, bool isDonateAll, string donateKeywords, int maxDonationsPerRequest)
        {
            _id = id;
            _name = name;
            _isSelectedForDonate = isSelectedForDonate;
            _isDonateAll = isDonateAll;
            _donateKeywords = donateKeywords;
            _maxDonationsPerRequest = maxDonationsPerRequest;
            _trainQuantity = trainQuantity;
        }

        /// <summary>
        /// Creates a new TroopModel.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="trainQuantity">The quantity to train.</param>
        /// <param name="isSelectedForDonate">Specify if troop is selected.</param>
        /// <param name="isDonateAll">Specify if donate to all.</param>
        /// <param name="donateKeywords">The donate keywords.</param>
        /// <param name="maxDonationsPerRequest">The maximum donations per request.</param>
        /// <returns>TroopModel.</returns>
        public static TroopModel CreateNew(int id, string name, int trainQuantity, bool isSelectedForDonate, bool isDonateAll, string donateKeywords, int maxDonationsPerRequest)
        {
            return new TroopModel(id, name, trainQuantity, isSelectedForDonate, isDonateAll, donateKeywords, maxDonationsPerRequest);
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

        private int _trainQuantity;
        public int TrainQuantity
        {
            get { return _trainQuantity; }
            set
            {
                if (_trainQuantity != value)
                {
                    _trainQuantity = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isSelectedForDonate;
        public bool IsSelectedForDonate
        {
            get { return _isSelectedForDonate; }
            set
            {
                _isSelectedForDonate = value;
                OnPropertyChanged();
            }
        }

        private bool _isDonateAll;
        public bool IsDonateAll
        {
            get { return _isDonateAll; }
            set
            {
                _isDonateAll = value;
                OnPropertyChanged();
            }
        }

        private string _donateKeywords;
        public string DonateKeywords
        {
            get { return _donateKeywords; }
            set
            {
                if (_donateKeywords != value)
                {
                    _donateKeywords = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _maxDonationsPerRequest;
        public int MaxDonationsPerRequest
        {
            get { return _maxDonationsPerRequest; }
            set
            {
                if (_maxDonationsPerRequest != value)
                {
                    _maxDonationsPerRequest = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
