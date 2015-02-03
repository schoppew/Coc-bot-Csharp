namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Windows;

	public abstract class AttachableCollection<T> : FreezableCollection<T>, IAttachedObject where T: DependencyObject, IAttachedObject
	{
		private DependencyObject associatedObject;
		private Collection<T> snapshot;

		internal AttachableCollection()
		{
			INotifyCollectionChanged changed = this;
			changed.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
			snapshot = new Collection<T>();
		}

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != AssociatedObject)
			{
				if (AssociatedObject != null)
				{
					throw new InvalidOperationException();
				}
				if (Interaction.ShouldRunInDesignMode || !((bool) base.GetValue(DesignerProperties.IsInDesignModeProperty)))
				{
					base.WritePreamble();
					associatedObject = dependencyObject;
					base.WritePostscript();
				}
				OnAttached();
			}
		}

		public void Detach()
		{
			OnDetaching();
			base.WritePreamble();
			associatedObject = null;
			base.WritePostscript();
		}

		internal abstract void ItemAdded(T item);

		internal abstract void ItemRemoved(T item);

		protected abstract void OnAttached();

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (T local in e.NewItems)
					{
						try
						{
							VerifyAdd(local);
							ItemAdded(local);
						}
						finally
						{
							snapshot.Insert(base.IndexOf(local), local);
						}
					}
					break;

				case NotifyCollectionChangedAction.Remove:
					foreach (T local4 in e.OldItems)
					{
						ItemRemoved(local4);
						snapshot.Remove(local4);
					}
					break;

				case NotifyCollectionChangedAction.Replace:
					foreach (T local2 in e.OldItems)
					{
						ItemRemoved(local2);
						snapshot.Remove(local2);
					}
					foreach (T local3 in e.NewItems)
					{
						try
						{
							VerifyAdd(local3);
							ItemAdded(local3);
						}
						finally
						{
							snapshot.Insert(base.IndexOf(local3), local3);
						}
					}
					break;

				case NotifyCollectionChangedAction.Move:
					break;

				case NotifyCollectionChangedAction.Reset:
					foreach (T local5 in snapshot)
					{
						ItemRemoved(local5);
					}
					snapshot = new Collection<T>();
					foreach (T local6 in this)
					{
						VerifyAdd(local6);
						ItemAdded(local6);
					}
					break;

				default:
					return;
			}
		}

		protected abstract void OnDetaching();
		private void VerifyAdd(T item)
		{
			if (snapshot.Contains(item))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.DuplicateItemInCollectionExceptionMessage, new object[] { typeof(T).Name, base.GetType().Name }));
			}
		}

		[Conditional("DEBUG")]
		private void VerifySnapshotIntegrity()
		{
			if (base.Count == snapshot.Count)
			{
				for (int i = 0; i < base.Count; i++)
				{
					if (base[i] != snapshot[i])
					{
						return;
					}
				}
			}
		}

		public DependencyObject AssociatedObject
		{
			get
			{
				base.ReadPreamble();
				return associatedObject;
			}
		}

		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				return AssociatedObject;
			}
		}
	}
}