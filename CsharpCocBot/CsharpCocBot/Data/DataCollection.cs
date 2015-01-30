namespace CoC.Bot.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    internal static class DataCollection
    {
        /// <summary>
        /// The Troop Compositions
        /// </summary>
        internal static BindingList<Model> TroopCompositions = new BindingList<Model>();

        /// <summary>
        /// The Troops
        /// </summary>
        internal static BindingList<Model> Troops = new BindingList<Model>();
    }
}