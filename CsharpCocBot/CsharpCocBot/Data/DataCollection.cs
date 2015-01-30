namespace CoC.Bot.Data
{
    using System;
    using System.Collections.Generic;
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
        /// The Troop Types
        /// </summary>
        internal static BindingList<Model> Troops = new BindingList<Model>();

        /// <summary>
        /// The Deploy Strategies
        /// </summary>
        internal static BindingList<Model> DeployStrategies = new BindingList<Model>();

        /// <summary>
        /// The Deploy Troops
        /// </summary>
        internal static BindingList<Model> DeployTroops = new BindingList<Model>();
    }
}