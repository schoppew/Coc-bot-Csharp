namespace CoC.Bot.Data
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Linq;

    /// <summary>
    /// The Data Collection.
    /// </summary>
    internal static class DataCollection
    {
        /// <summary>
        /// The Troop Compositions
        /// </summary>
        internal static BindingList<Model> TroopCompositions = new BindingList<Model>();

        /// <summary>
        /// The Barracks Troops
        /// </summary>
        internal static BindingList<Model> BarracksTroops = new BindingList<Model>();

        /// <summary>
        /// The Dark Barracks troops
        /// </summary>
        internal static BindingList<Model> DarkBarracksTroops = new BindingList<Model>();

        /// <summary>
        /// The Deploy Strategies
        /// </summary>
        internal static BindingList<Model> DeployStrategies = new BindingList<Model>();

        /// <summary>
        /// The Deploy Troops
        /// </summary>
        internal static BindingList<Model> DeployTroops = new BindingList<Model>();

		/// <summary>
		/// The Custom Wave
		/// </summary>
		internal static ObservableCollection<WaveModel> CustomWave = new ObservableCollection<WaveModel>();

        /// <summary>
        /// The Troop Tiers and child Troops
        /// </summary>
        internal static TroopTierModel.TroopTierModelBindingList TroopTiers = new TroopTierModel.TroopTierModelBindingList();
    }
}