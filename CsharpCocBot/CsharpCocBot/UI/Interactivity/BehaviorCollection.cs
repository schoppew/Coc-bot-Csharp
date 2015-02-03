namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;

	public sealed class BehaviorCollection : AttachableCollection<Behavior>
	{
		internal BehaviorCollection()
		{
		}

		protected override Freezable CreateInstanceCore()
		{
			return new BehaviorCollection();
		}

		internal override void ItemAdded(Behavior item)
		{
			if (AssociatedObject != null)
			{
				item.Attach(AssociatedObject);
			}
		}

		internal override void ItemRemoved(Behavior item)
		{
			if (((IAttachedObject)item).AssociatedObject != null)
			{
				item.Detach();
			}
		}

		protected override void OnAttached()
		{
			foreach (Behavior behavior in this)
			{
				behavior.Attach(AssociatedObject);
			}
		}

		protected override void OnDetaching()
		{
			foreach (Behavior behavior in this)
			{
				behavior.Detach();
			}
		}
	}
}