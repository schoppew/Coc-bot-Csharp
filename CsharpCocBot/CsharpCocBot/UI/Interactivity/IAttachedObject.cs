namespace CoC.Bot.UI.Interactivity
{
	using System.Windows;

	public interface IAttachedObject
	{
		void Attach(DependencyObject dependencyObject);

		void Detach();

		DependencyObject AssociatedObject { get; }
	}
}