namespace CoC.Bot.UI.Services
{
	using System.Windows;

	public interface IMessageBoxService
	{
		MessageBoxResult Show(string message, string title, MessageBoxButton button, MessageBoxImage image);

		MessageBoxResult Show(string message, string title, MessageBoxButton button, MessageBoxImage image, MessageBoxResult defaultResult);
	}
}