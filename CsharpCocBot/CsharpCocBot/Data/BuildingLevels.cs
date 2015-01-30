namespace CoC.Bot.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The Building Levels.
    /// </summary>
    public static class BuildingLevels
    {
        private static readonly int[] _levels = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

        /// <summary>
        /// Returns All building levels.
        /// </summary>
        /// <value>All building levels.</value>
        public static IEnumerable<int> All
        {
            get { return _levels; }
        }

        /// <summary>
        /// Returns Cannon building levels.
        /// </summary>
        /// <value>Cannon building levels.</value>
        public static IEnumerable<int> Cannon
        {
            get { return _levels.Take(13); }
        }

        /// <summary>
        /// Returns Archer Tower building levels.
        /// </summary>
        /// <value>Archer Tower building levels.</value>
        public static IEnumerable<int> ArcherTower
        {
            get { return _levels.Take(14); }
        }

        /// <summary>
        /// Returns Mortar building levels.
        /// </summary>
        /// <value>Mortar building levels.</value>
        public static IEnumerable<int> Mortar
        {
            get { return _levels.Take(9); }
        }

        /// <summary>
        /// Returns Air Defense building levels.
        /// </summary>
        /// <value>Air Defense building levels.</value>
        public static IEnumerable<int> AirDefense
        {
            get { return _levels.Take(9); }
        }

        /// <summary>
        /// Returns Hidden Tesla building levels.
        /// </summary>
        /// <value>Hidden Tesla building levels.</value>
        public static IEnumerable<int> HiddenTesla
        {
            get { return _levels.Take(9); }
        }

        /// <summary>
        /// Returns Wizard Tower building levels.
        /// </summary>
        /// <value>Wizard Tower building levels.</value>
        public static IEnumerable<int> WizardTower
        {
            get { return _levels.Take(9); }
        }

        /// <summary>
        /// Returns Xbow building levels.
        /// </summary>
        /// <value>Xbow building levels.</value>
        public static IEnumerable<int> Xbow
        {
            get { return _levels.Take(5); }
        }

        /// <summary>
        /// Returns Inferno Tower building levels.
        /// </summary>
        /// <value>Inferno Tower building levels.</value>
        public static IEnumerable<int> InfernoTower
        {
            get { return _levels.Take(4); }
        }

        /// <summary>
        /// Returns Wall building levels.
        /// </summary>
        /// <value>Wall building levels.</value>
        public static IEnumerable<int> Wall
        {
            get { return _levels.Take(12); }
        }
    }
}