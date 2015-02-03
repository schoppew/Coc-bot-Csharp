namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public sealed class EventObserver : IDisposable
	{
		private EventInfo eventInfo;
		private Delegate handler;
		private object target;

		public EventObserver(EventInfo eventInfo, object target, Delegate handler)
		{
			if (eventInfo == null)
			{
				throw new ArgumentNullException("eventInfo");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this.eventInfo = eventInfo;
			this.target = target;
			this.handler = handler;
			this.eventInfo.AddEventHandler(this.target, handler);
		}

		public void Dispose()
		{
			eventInfo.RemoveEventHandler(target, handler);
		}
	}
}