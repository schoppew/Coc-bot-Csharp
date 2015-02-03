namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.Linq;
	using System.Windows;

	public abstract class TargetedTriggerAction : TriggerAction
	{
		private bool isTargetChangedRegistered;
		public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string), typeof(TargetedTriggerAction), new FrameworkPropertyMetadata(new PropertyChangedCallback(TargetedTriggerAction.OnTargetNameChanged)));
		public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(TargetedTriggerAction), new FrameworkPropertyMetadata(new PropertyChangedCallback(TargetedTriggerAction.OnTargetObjectChanged)));
		private NameResolver targetResolver;
		private Type targetTypeConstraint;

		internal TargetedTriggerAction(Type targetTypeConstraint)
			: base(typeof(DependencyObject))
		{
			this.targetTypeConstraint = targetTypeConstraint;
			targetResolver = new NameResolver();
			RegisterTargetChanged();
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			DependencyObject associatedObject = AssociatedObject;
			Behavior behavior = associatedObject as Behavior;
			RegisterTargetChanged();
			if (behavior != null)
			{
				associatedObject = ((IAttachedObject)behavior).AssociatedObject;
				behavior.AssociatedObjectChanged += new EventHandler(OnBehaviorHostChanged);
			}
			TargetResolver.NameScopeReferenceElement = associatedObject as FrameworkElement;
		}

		private void OnBehaviorHostChanged(object sender, EventArgs e)
		{
			TargetResolver.NameScopeReferenceElement = ((IAttachedObject)sender).AssociatedObject as FrameworkElement;
		}

		protected override void OnDetaching()
		{
			Behavior associatedObject = AssociatedObject as Behavior;
			base.OnDetaching();
			OnTargetChangedImpl(TargetResolver.Object, null);
			UnregisterTargetChanged();
			if (associatedObject != null)
			{
				associatedObject.AssociatedObjectChanged -= new EventHandler(OnBehaviorHostChanged);
			}
			TargetResolver.NameScopeReferenceElement = null;
		}

		private void OnTargetChanged(object sender, NameResolvedEventArgs e)
		{
			if (AssociatedObject != null)
			{
				OnTargetChangedImpl(e.OldObject, e.NewObject);
			}
		}

		internal virtual void OnTargetChangedImpl(object oldTarget, object newTarget)
		{
		}

		private static void OnTargetNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			TargetedTriggerAction action = (TargetedTriggerAction)obj;
			action.TargetResolver.Name = (string)args.NewValue;
		}

		private static void OnTargetObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			((TargetedTriggerAction)obj).OnTargetChanged(obj, new NameResolvedEventArgs(args.OldValue, args.NewValue));
		}

		private void RegisterTargetChanged()
		{
			if (!IsTargetChangedRegistered)
			{
				TargetResolver.ResolvedElementChanged += new EventHandler<NameResolvedEventArgs>(OnTargetChanged);
				IsTargetChangedRegistered = true;
			}
		}

		private void UnregisterTargetChanged()
		{
			if (IsTargetChangedRegistered)
			{
				TargetResolver.ResolvedElementChanged -= new EventHandler<NameResolvedEventArgs>(OnTargetChanged);
				IsTargetChangedRegistered = false;
			}
		}

		protected sealed override Type AssociatedObjectTypeConstraint
		{
			get
			{
				TypeConstraintAttribute attribute = TypeDescriptor.GetAttributes(base.GetType())[typeof(TypeConstraintAttribute)] as TypeConstraintAttribute;
				if (attribute != null)
				{
					return attribute.Constraint;
				}
				return typeof(DependencyObject);
			}
		}

		private bool IsTargetChangedRegistered
		{
			get
			{
				return isTargetChangedRegistered;
			}
			set
			{
				isTargetChangedRegistered = value;
			}
		}

		private bool IsTargetNameSet
		{
			get
			{
				if (string.IsNullOrEmpty(TargetName))
				{
					return (base.ReadLocalValue(TargetNameProperty) != DependencyProperty.UnsetValue);
				}
				return true;
			}
		}

		protected object Target
		{
			get
			{
				object associatedObject = AssociatedObject;
				if (TargetObject != null)
				{
					associatedObject = TargetObject;
				}
				else if (IsTargetNameSet)
				{
					associatedObject = TargetResolver.Object;
				}
				if ((associatedObject != null) && !TargetTypeConstraint.IsAssignableFrom(associatedObject.GetType()))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.RetargetedTypeConstraintViolatedExceptionMessage, new object[] { base.GetType().Name, associatedObject.GetType(), TargetTypeConstraint, "Target" }));
				}
				return associatedObject;
			}
		}

		public string TargetName
		{
			get
			{
				return (string)base.GetValue(TargetNameProperty);
			}
			set
			{
				base.SetValue(TargetNameProperty, value);
			}
		}

		public object TargetObject
		{
			get
			{
				return base.GetValue(TargetObjectProperty);
			}
			set
			{
				base.SetValue(TargetObjectProperty, value);
			}
		}

		private NameResolver TargetResolver
		{
			get { return targetResolver; }
		}

		protected Type TargetTypeConstraint
		{
			get
			{
				base.ReadPreamble();
				return targetTypeConstraint;
			}
		}
	}

	public abstract class TargetedTriggerAction<T> : TargetedTriggerAction where T : class
	{
		protected TargetedTriggerAction() : base(typeof(T))
		{
		}

		protected virtual void OnTargetChanged(T oldTarget, T newTarget)
		{
		}

		internal sealed override void OnTargetChangedImpl(object oldTarget, object newTarget)
		{
			base.OnTargetChangedImpl(oldTarget, newTarget);
			this.OnTargetChanged(oldTarget as T, newTarget as T);
		}

		protected new T Target
		{
			get
			{
				return (T)base.Target;
			}
		}
	}
}