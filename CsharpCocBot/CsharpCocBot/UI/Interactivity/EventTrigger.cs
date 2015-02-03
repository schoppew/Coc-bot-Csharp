namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;

	public class EventTrigger : EventTriggerBase<object>
	{
		public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register("EventName", typeof(string), typeof(EventTrigger), new FrameworkPropertyMetadata("Loaded", new PropertyChangedCallback(EventTrigger.OnEventNameChanged)));

		public EventTrigger()
		{
		}

		public EventTrigger(string eventName)
		{
			EventName = eventName;
		}

		protected override string GetEventName()
		{
			return EventName;
		}

		private static void OnEventNameChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			((EventTrigger)sender).OnEventNameChanged((string)args.OldValue, (string)args.NewValue);
		}

		public string EventName
		{
			get { return (string)base.GetValue(EventNameProperty); }
			set { base.SetValue(EventNameProperty, value); }
		}
	}
}