namespace CoC.Bot.UI.Services
{
	using System;
	using System.Windows;

	/// <summary>
	/// A service that shows message boxes.
	/// </summary>
	internal class MessageBoxService : IMessageBoxService
	{
		MessageBoxResult IMessageBoxService.Show(string text, string caption, MessageBoxButton button, MessageBoxImage image)
		{
			return MessageBox.Show(text, caption, button, image);
		}

		MessageBoxResult IMessageBoxService.Show(string text, string caption, MessageBoxButton button, MessageBoxImage image, MessageBoxResult defaultResult)
		{
			return MessageBox.Show(text, caption, button, image, defaultResult);
		}
	}
}