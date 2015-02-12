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

    /// <summary>
    /// Provides functionality for the MainWindow
    /// </summary>
    public partial class MainViewModel : ViewModelBase
	{
		#region Properties

		private bool _isRearmTraps;
		/// <summary>
		/// Gets or sets a value indicating whether should rearm Traps.
		/// </summary>
		/// <value><c>true</c> if should rearm traps; otherwise, <c>false</c>.</value>
		public bool IsRearmTraps
		{
			get { return _isRearmTraps; }
			set
			{
				if (_isRearmTraps != value)
				{
					_isRearmTraps = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isRearmXbows;
		/// <summary>
		/// Gets or sets a value indicating whether should rearm X-bows.
		/// </summary>
		/// <value><c>true</c> if should rearm Xbows; otherwise, <c>false</c>.</value>
		public bool IsRearmXbows
		{
			get { return _isRearmXbows; }
			set
			{
				if (_isRearmXbows != value)
				{
					_isRearmXbows = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isRearmInfernos;
		/// <summary>
		/// Gets or sets a value indicating whether should rearm Inferno Towers.
		/// </summary>
		/// <value><c>true</c> if should rearm traps; otherwise, <c>false</c>.</value>
		public bool IsRearmInfernos
		{
			get { return _isRearmInfernos; }
			set
			{
				if (_isRearmInfernos != value)
				{
					_isRearmInfernos = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion
	}
}