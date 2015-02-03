namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public sealed class TriggerCollection : AttachableCollection<TriggerBase>
	{
		internal TriggerCollection()
		{
		}

		protected override System.Windows.Freezable CreateInstanceCore()
		{
			return new TriggerCollection();
		}

		internal override void ItemAdded(TriggerBase item)
		{
			if (AssociatedObject != null)
			{
				item.Attach(AssociatedObject);
			}
		}

		internal override void ItemRemoved(TriggerBase item)
		{
			if (((IAttachedObject)item).AssociatedObject != null)
			{
				item.Detach();
			}
		}

		protected override void OnAttached()
		{
			foreach (TriggerBase base2 in this)
			{
				base2.Attach(AssociatedObject);
			}
		}

		protected override void OnDetaching()
		{
			foreach (TriggerBase base2 in this)
			{
				base2.Detach();
			}
		}
	}
}