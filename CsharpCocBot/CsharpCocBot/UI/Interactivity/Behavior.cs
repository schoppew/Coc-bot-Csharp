namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Windows;
	using System.Windows.Media.Animation;

	public abstract class Behavior : Animatable, IAttachedObject
	{
		private DependencyObject associatedObject;
		private Type associatedType;

		internal event EventHandler AssociatedObjectChanged;

		internal Behavior(Type associatedType)
		{
			this.associatedType = associatedType;
		}

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != AssociatedObject)
			{
				if (AssociatedObject != null)
				{
					throw new InvalidOperationException(Properties.Resources.CannotHostBehaviorMultipleTimesExceptionMessage);
				}
				if ((dependencyObject != null) && !AssociatedType.IsAssignableFrom(dependencyObject.GetType()))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.TypeConstraintViolatedExceptionMessage, new object[] { base.GetType().Name, dependencyObject.GetType().Name, AssociatedType.Name }));
				}
				base.WritePreamble();
				associatedObject = dependencyObject;
				base.WritePostscript();
				OnAssociatedObjectChanged();
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
			OnAssociatedObjectChanged();
		}

		private void OnAssociatedObjectChanged()
		{
			if (AssociatedObjectChanged != null)
			{
				AssociatedObjectChanged(this, new EventArgs());
			}
		}

		protected virtual void OnAttached()
		{
		}

		protected virtual void OnDetaching()
		{
		}

		#region Properties

		protected DependencyObject AssociatedObject
		{
			get
			{
				base.ReadPreamble();
				return associatedObject;
			}
		}

		protected Type AssociatedType
		{
			get
			{
				base.ReadPreamble();
				return associatedType;
			}
		}

		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				return AssociatedObject;
			}
		}

		#endregion
	}

	public abstract class Behavior<T> : Behavior where T : DependencyObject
	{
		protected Behavior() : base(typeof(T))
		{
		}

		protected new T AssociatedObject
		{
			get
			{
				return (T)base.AssociatedObject;
			}
		}
	}
}