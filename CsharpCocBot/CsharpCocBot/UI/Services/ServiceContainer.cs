namespace CoC.Bot.UI.Services
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// The ServiceContainer.
	/// </summary>
	public class ServiceContainer
	{
		public static readonly ServiceContainer Instance = new ServiceContainer();

		private readonly Dictionary<Type, object> _serviceMap;
		private readonly object _serviceMapLock;

		/// <summary>
		/// Prevents a default instance of the <see cref="ServiceContainer"/> class from being created.
		/// </summary>
		private ServiceContainer()
		{
			_serviceMap = new Dictionary<Type, object>();
			_serviceMapLock = new object();
		}

		/// <summary>
		/// Adds the service to the ServiceContainer.
		/// </summary>
		/// <typeparam name="TServiceContract">The type identifier of the service.</typeparam>
		/// <param name="implementation">The implementation.</param>
		public void AddService<TServiceContract>(TServiceContract implementation) where TServiceContract : class
		{
			lock (_serviceMapLock)
			{
				_serviceMap[typeof(TServiceContract)] = implementation;
			}
		}

		/// <summary>
		/// Gets the service object identified by <typeparamref name="TServiceContract"/> in the ServiceContainer.
		/// </summary>
		/// <typeparam name="TServiceContract">The type identifier of the service.</typeparam>
		/// <returns>TServiceContract.</returns>
		public TServiceContract GetService<TServiceContract>() where TServiceContract : class
		{
			object service;
			lock (_serviceMapLock)
			{
				_serviceMap.TryGetValue(typeof(TServiceContract), out service);
			}
			return service as TServiceContract;
		}
	}
}