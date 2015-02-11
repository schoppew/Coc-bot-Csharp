namespace CoC.Bot.Data
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Linq;
	using CoC.Bot.Data.Type;

    /// <summary>
    /// The Data Collection additions for Debug tab.
    /// </summary>
    internal static partial class DataCollection
    {
		/// <summary>
		/// The Debug Data (string/int pairs should do it for now)
		/// </summary>
		internal static ObservableCollection<DebugDataModel> DebugData = new ObservableCollection<DebugDataModel>();
    }
}