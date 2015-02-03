namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls.Primitives;
	using System.Windows.Media.Animation;

	[DefaultTrigger(typeof(UIElement), typeof(EventTrigger), "MouseLeftButtonDown"), DefaultTrigger(typeof(ButtonBase), typeof(EventTrigger), "Click")]
	public abstract class TriggerAction : Animatable, IAttachedObject
	{
		private DependencyObject associatedObject;
		private Type associatedObjectTypeConstraint;
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(TriggerAction), new FrameworkPropertyMetadata(true));
		private bool isHosted;

		internal TriggerAction(Type associatedObjectTypeConstraint)
		{
			this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
		}

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != AssociatedObject)
			{
				if (AssociatedObject != null)
				{
					throw new InvalidOperationException(Properties.Resources.CannotHostTriggerActionMultipleTimesExceptionMessage);
				}
				if ((dependencyObject != null) && !AssociatedObjectTypeConstraint.IsAssignableFrom(dependencyObject.GetType()))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.TypeConstraintViolatedExceptionMessage, new object[] { base.GetType().Name, dependencyObject.GetType().Name, AssociatedObjectTypeConstraint.Name }));
				}
				base.WritePreamble();
				associatedObject = dependencyObject;
				base.WritePostscript();
				OnAttached();
			}
		}

		internal void CallInvoke(object parameter)
		{
			if (IsEnabled)
			{
				Invoke(parameter);
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
		}

		protected abstract void Invoke(object parameter);
		protected virtual void OnAttached()
		{
		}

		protected virtual void OnDetaching()
		{
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

		public bool IsEnabled
		{
			get { return (bool)base.GetValue(IsEnabledProperty); }
			set { base.SetValue(IsEnabledProperty, value); }
		}

		internal bool IsHosted
		{
			get
			{
				base.ReadPreamble();
				return isHosted;
			}
			set
			{
				base.WritePreamble();
				isHosted = value;
				base.WritePostscript();
			}
		}

		DependencyObject IAttachedObject.AssociatedObject
		{
			get { return AssociatedObject; }
		}
	}

	public abstract class TriggerAction<T> : TriggerAction where T : DependencyObject
	{
		protected TriggerAction() : base(typeof(T))
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
			get { return base.AssociatedObjectTypeConstraint; }
		}
	}
}