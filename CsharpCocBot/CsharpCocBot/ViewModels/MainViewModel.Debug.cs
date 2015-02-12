namespace CoC.Bot.ViewModels
{
    using System;
    using System.Collections.Generic;
	using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
	using System.Windows;
    using System.Windows.Input;
	using System.Windows.Media;
	using System.Xml.Linq;
	using Point = Win32.POINT;

    using Data;
    using Tools;
    using Tools.FastFind;
    using UI.Commands;
	using UI.Services;
	using CoC.Bot.Data.Type;
	using CoC.Bot.Data.ColorList.Tools;
	using CoC.Bot.Data.ColorList;
	using System.IO;

    /// <summary>
    /// Provides functionality for the MainWindow
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {

		// All the debug stuff is hard-coded here
		#region Debug tab Stuff

		/// <summary>
		/// [Used in UI for Binding] Gets the custom Wave.
		/// </summary>
		/// <value>Custom Wave.</value>
		public static ObservableCollection<DebugDataModel> DebugData { get { return DataCollection.DebugData; } }


		public ICommand LauchCurrentScreenAnalysisCommand
		{
			get { return new RelayCommand(() => LauchCurrentScreenAnalysis()); }
		}

		/// <summary>
		/// Adds the Troop for Custom Wave.
		/// </summary>
		private void LauchCurrentScreenAnalysis()
		{
			ExtBitmap.ExtBitmap bitmap = new ExtBitmap.ExtBitmap();
			
			bitmap.SnapShot(false);
			if (!string.IsNullOrEmpty(AppSettings.AnalysisDirectory) && Directory.Exists(AppSettings.AnalysisDirectory))
				AllColorLists.MakeStatsFromDirectory("STATS.csv", AppSettings.AnalysisDirectory);
			else
				AllColorLists.MakeStats("STATS.cvs", bitmap);
			// Just do it !
		}

		#endregion Debug tab Stuff

	}
}
