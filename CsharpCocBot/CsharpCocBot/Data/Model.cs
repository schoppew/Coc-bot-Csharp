namespace CoC.Bot.Data
{
    using CoC.Bot.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A general use Data Model composed only of ID and Name.
    /// </summary>
    public class Model : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        public Model()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        internal Model(short id, string name)
        {
            _id = id;
            _name = name;
        }

        /// <summary>
        /// Creates a new Model.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <returns>Model.</returns>
        public static Model CreateNew(short id, string name)
        {
            return new Model(id, name);
        }

        #region Properties

        private short _id;
        public short Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        #endregion
    }
}