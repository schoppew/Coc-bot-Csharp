namespace CoC.Bot.UI.Services
{
	/// <summary>
	/// The ServiceInjector.
	/// </summary>
	public static class ServiceInjector
	{
		/// <summary>
		/// Loads service objects into the ServiceContainer on startup.
		/// </summary>
		public static void InjectServices()
		{
			ServiceContainer.Instance.AddService<IMessageBoxService>(new MessageBoxService());
			ServiceContainer.Instance.AddService<INotifyService>(new NotifyService());
		}
	}
}