namespace CoC.Bot.UI.Services
{
	public interface INotifyService
	{
		void Notify(string message);

		void Notify(string title, string message);

		void ChangeIconSource(string path);
	}
}