namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Windows;
	using System.Windows.Markup;
	using System.Windows.Media.Animation;

	[ContentProperty("Actions")]
	public abstract class TriggerBase : Animatable, IAttachedObject
	{
		private static readonly DependencyPropertyKey ActionsPropertyKey = DependencyProperty.RegisterReadOnly("Actions", typeof(TriggerActionCollection), typeof(TriggerBase), new FrameworkPropertyMetadata());
		public static readonly DependencyProperty ActionsProperty = ActionsPropertyKey.DependencyProperty;
		private DependencyObject associatedObject;
		private Type associatedObjectTypeConstraint;

		public event EventHandler<PreviewInvokeEventArgs> PreviewInvoke;

		internal TriggerBase(Type associatedObjectTypeConstraint)
		{
			this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
			TriggerActionCollection actions = new TriggerActionCollection();
			base.SetValue(ActionsPropertyKey, actions);
		}

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != AssociatedObject)
			{
				if (AssociatedObject != null)
				{
					throw new InvalidOperationException(Properties.Resources.CannotHostTriggerMultipleTimesExceptionMessage);
				}
				if ((dependencyObject != null) && !AssociatedObjectTypeConstraint.IsAssignableFrom(dependencyObject.GetType()))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.TypeConstraintViolatedExceptionMessage, new object[] { base.GetType().Name, dependencyObject.GetType().Name, AssociatedObjectTypeConstraint.Name }));
				}
				base.WritePreamble();
				associatedObject = dependencyObject;
				base.WritePostscript();
				Actions.Attach(dependencyObject);
				OnAttached();
			}
		}

		protected override Freezable CreateInstanceCore()
		{
			return (Freezable)Activator.CreateInstance(base.GetType());
		}

		public void Detach()
		{
			OnDetaching();
			base.WritePreamble();
			associatedObject = null;
			base.WritePostscript();
			Actions.Detach();
		}

		protected void InvokeActions(object parameter)
		{
			if (PreviewInvoke != null)
			{
				PreviewInvokeEventArgs e = new PreviewInvokeEventArgs();
				PreviewInvoke(this, e);
				if (e.Cancelling)
				{
					return;
				}
			}
			foreach (TriggerAction action in Actions)
			{
				action.CallInvoke(parameter);
			}
		}

		protected virtual void OnAttached()
		{
		}

		protected virtual void OnDetaching()
		{
		}

		public TriggerActionCollection Actions
		{
			get { return (TriggerActionCollection)base.GetValue(ActionsProperty); }
		}

		protected DependencyObject AssociatedObject
		{
			get
			{
				base.ReadPreamble();
				return associatedObject;
			}
		}

		protected virtual Type AssociatedObjectTypeConstraint
		{
			get
			{
				base.ReadPreamble();
				return associatedObjectTypeConstraint;
			}
		}

		DependencyObject IAttachedObject.AssociatedObject
		{
			get { return AssociatedObject; }
		}
	}

	public abstract class TriggerBase<T> : TriggerBase where T : DependencyObject
	{
		protected TriggerBase() : base(typeof(T))
		{
		}

		protected new T AssociatedObject
		{
			get
			{
				return (T)base.AssociatedObject;
			}
		}

		protected sealed override Type AssociatedObjectTypeConstraint
		{
			get
			{
				return base.AssociatedObjectTypeConstraint;
			}
		}
	}
}