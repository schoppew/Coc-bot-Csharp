namespace CoC.Bot.UI.Services
{
	using System;
	using System.Windows;
	using System.Windows.Input;

	using UI.Commands;
	using UI.Controls.NotifyIcon;

	/// <summary>
	/// The Notify Service.
	/// </summary>
	public class NotifyService : INotifyService, IDisposable
	{
		private TaskbarIcon icon = new TaskbarIcon
		{
			Name = "NotifyIcon",
			Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri(@"/Assets/Images/TrayIcon.ico", UriKind.Relative)).Stream),
			DoubleClickCommand = ShowCommand
		};

		/// <summary>
		/// Notifies using the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Notify(string message)
		{

			icon.ShowBalloonTip(Properties.Resources.AppName, message, BalloonIcon.None);
		}

		/// <summary>
		/// Notifies using the specified title and message.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		public void Notify(string title, string message)
		{

			icon.ShowBalloonTip(title, message, BalloonIcon.None);
		}

		/// <summary>
		/// Changes the icon source.
		/// </summary>
		/// <param name="path">The path.</param>
		public void ChangeIconSource(string path)
		{
			icon.Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri((path), UriKind.Relative)).Stream);
		}

		public void Dispose()
		{
			if (icon != null)
			{
				icon.Dispose();
				icon = null;
			}
		}

		private static RelayCommand _showCommand;
		public static ICommand ShowCommand
		{
			get
			{
				if (_showCommand == null)
					_showCommand = new RelayCommand(() => Show(), ShowCanExecute);
				return _showCommand;
			}
		}

		/// <summary>
		/// Determines whether the ShowCommand command can be executed.
		/// </summary>
		/// <returns><c>true</c> if can execute, <c>false</c> otherwise</returns>
		private static bool ShowCanExecute()
		{
			return !Application.Current.MainWindow.IsVisible;
		}

		/// <summary>
		/// Executes the Show Window.
		/// </summary>
		private static void Show()
		{
			Application.Current.MainWindow.Show();
		}
	}
}