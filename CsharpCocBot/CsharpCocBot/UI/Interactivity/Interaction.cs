namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;

	public static class Interaction
	{
		private static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("ShadowBehaviors", typeof(BehaviorCollection), typeof(Interaction), new FrameworkPropertyMetadata(new PropertyChangedCallback(Interaction.OnBehaviorsChanged)));
		private static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached("ShadowTriggers", typeof(TriggerCollection), typeof(Interaction), new FrameworkPropertyMetadata(new PropertyChangedCallback(Interaction.OnTriggersChanged)));

		public static BehaviorCollection GetBehaviors(DependencyObject obj)
		{
			BehaviorCollection behaviors = (BehaviorCollection)obj.GetValue(BehaviorsProperty);
			if (behaviors == null)
			{
				behaviors = new BehaviorCollection();
				obj.SetValue(BehaviorsProperty, behaviors);
			}
			return behaviors;
		}

		public static TriggerCollection GetTriggers(DependencyObject obj)
		{
			TriggerCollection triggers = (TriggerCollection)obj.GetValue(TriggersProperty);
			if (triggers == null)
			{
				triggers = new TriggerCollection();
				obj.SetValue(TriggersProperty, triggers);
			}
			return triggers;
		}

		internal static bool IsElementLoaded(FrameworkElement element)
		{
			return element.IsLoaded;
		}

		private static void OnBehaviorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			BehaviorCollection oldValue = (BehaviorCollection)args.OldValue;
			BehaviorCollection newValue = (BehaviorCollection)args.NewValue;
			if (oldValue != newValue)
			{
				if ((oldValue != null) && (oldValue.AssociatedObject != null))
				{
					oldValue.Detach();
				}
				if ((newValue != null) && (obj != null))
				{
					if (newValue.AssociatedObject != null)
					{
						throw new InvalidOperationException(Properties.Resources.CannotHostBehaviorCollectionMultipleTimesExceptionMessage);
					}
					newValue.Attach(obj);
				}
			}
		}

		private static void OnTriggersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			TriggerCollection oldValue = args.OldValue as TriggerCollection;
			TriggerCollection newValue = args.NewValue as TriggerCollection;
			if (oldValue != newValue)
			{
				if ((oldValue != null) && (oldValue.AssociatedObject != null))
				{
					oldValue.Detach();
				}
				if ((newValue != null) && (obj != null))
				{
					if (newValue.AssociatedObject != null)
					{
						throw new InvalidOperationException(Properties.Resources.CannotHostTriggerCollectionMultipleTimesExceptionMessage);
					}
					newValue.Attach(obj);
				}
			}
		}

		internal static bool ShouldRunInDesignMode { get; set; }
	}
}