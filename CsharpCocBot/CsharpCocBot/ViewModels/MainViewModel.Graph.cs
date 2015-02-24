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

	/// <summary>
	/// Provides functionality for the MainWindow
	/// </summary>
	public partial class MainViewModel : ViewModelBase
	{
		#region Properties

		public ObservableCollection<GraphModel> Resources { get; private set; }

		private GraphModel _selectedResource;
		/// <summary>
		/// Gets or sets the selected Resource.
		/// </summary>
		/// <value>The selected Resource.</value>
		public GraphModel SelectedResource
		{
			get { return _selectedResource; }
			set
			{
				if (_selectedResource != value)
				{
					_selectedResource = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion
	}
}
