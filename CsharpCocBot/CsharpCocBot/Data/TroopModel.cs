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
        /// <param name="isSelectedForDonate">Specify if troop is selected for donation.</param>
        /// <param name="isDonateAll">Specify if donate to all.</param>
        /// <param name="donateKeywords">The donate keywords.</param>
        internal TroopModel(int id, string name, bool isSelectedForDonate, bool isDonateAll, string donateKeywords)
        {
            _id = id;
            _name = name;
            _isSelectedForDonate = isSelectedForDonate;
            _isDonateAll = isDonateAll;
            _donateKeywords = donateKeywords;
        }

        /// <summary>
        /// Creates a new TroopModel.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="isSelectedForDonate">Specify if troop is selected.</param>
        /// <param name="isDonateAll">Specify if donate to all.</param>
        /// <param name="donateKeywords">The donate keywords.</param>
        /// <returns>TroopModel.</returns>
        public static TroopModel CreateNew(int id, string name, bool isSelectedForDonate, bool isDonateAll, string donateKeywords)
        {
            return new TroopModel(id, name, isSelectedForDonate, isDonateAll, donateKeywords);
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

        // This is a workaround for getting the SelectedItem in a TreeView
        // Blame Microsoft!
        private bool _isSelectedInUI;
        public bool IsSelectedInUI
        {
            get { return _isSelectedInUI; }
            set
            {
                _isSelectedInUI = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}