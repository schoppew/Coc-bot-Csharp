namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;
	using System.Windows;

	public abstract class EventTriggerBase : TriggerBase
	{
		private MethodInfo eventHandlerMethodInfo;
		private bool isSourceChangedRegistered;
		public static readonly DependencyProperty SourceNameProperty = DependencyProperty.Register("SourceName", typeof(string), typeof(EventTriggerBase), new PropertyMetadata(new PropertyChangedCallback(EventTriggerBase.OnSourceNameChanged)));
		private NameResolver sourceNameResolver;
		public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register("SourceObject", typeof(object), typeof(EventTriggerBase), new PropertyMetadata(new PropertyChangedCallback(EventTriggerBase.OnSourceObjectChanged)));
		private Type sourceTypeConstraint;

		internal EventTriggerBase(Type sourceTypeConstraint) : base(typeof(DependencyObject))
		{
			this.sourceTypeConstraint = sourceTypeConstraint;
			sourceNameResolver = new NameResolver();
			RegisterSourceChanged();
		}

		protected abstract string GetEventName();
		private static bool IsValidEvent(EventInfo eventInfo)
		{
			Type eventHandlerType = eventInfo.EventHandlerType;
			if (!typeof(Delegate).IsAssignableFrom(eventInfo.EventHandlerType))
			{
				return false;
			}
			ParameterInfo[] parameters = eventHandlerType.GetMethod("Invoke").GetParameters();
			return (((parameters.Length == 2) && typeof(object).IsAssignableFrom(parameters[0].ParameterType)) && typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType));
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			DependencyObject associatedObject = AssociatedObject;
			Behavior behavior = associatedObject as Behavior;
			FrameworkElement element = associatedObject as FrameworkElement;
			RegisterSourceChanged();
			if (behavior != null)
			{
				associatedObject = ((IAttachedObject)behavior).AssociatedObject;
				behavior.AssociatedObjectChanged += new EventHandler(OnBehaviorHostChanged);
			}
			else if ((SourceObject != null) || (element == null))
			{
				try
				{
					OnSourceChanged(null, Source);
				}
				catch (InvalidOperationException)
				{
				}
			}
			else
			{
				SourceNameResolver.NameScopeReferenceElement = element;
			}
			if (((string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) == 0) && (element != null)) && !Interaction.IsElementLoaded(element))
			{
				RegisterLoaded(element);
			}
		}

		private void OnBehaviorHostChanged(object sender, EventArgs e)
		{
			SourceNameResolver.NameScopeReferenceElement = ((IAttachedObject)sender).AssociatedObject as FrameworkElement;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			Behavior associatedObject = AssociatedObject as Behavior;
			FrameworkElement associatedElement = AssociatedObject as FrameworkElement;
			try
			{
				OnSourceChanged(Source, null);
			}
			catch (InvalidOperationException)
			{
			}
			UnregisterSourceChanged();
			if (associatedObject != null)
			{
				associatedObject.AssociatedObjectChanged -= new EventHandler(OnBehaviorHostChanged);
			}
			SourceNameResolver.NameScopeReferenceElement = null;
			if ((string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) == 0) && (associatedElement != null))
			{
				UnregisterLoaded(associatedElement);
			}
		}

		protected virtual void OnEvent(EventArgs eventArgs)
		{
			InvokeActions(eventArgs);
		}

		private void OnEventImpl(object sender, EventArgs eventArgs)
		{
			OnEvent(eventArgs);
		}

		internal void OnEventNameChanged(string oldEventName, string newEventName)
		{
			if (AssociatedObject != null)
			{
				FrameworkElement source = Source as FrameworkElement;
				if ((source != null) && (string.Compare(oldEventName, "Loaded", StringComparison.Ordinal) == 0))
				{
					UnregisterLoaded(source);
				}
				else if (!string.IsNullOrEmpty(oldEventName))
				{
					UnregisterEvent(Source, oldEventName);
				}
				if ((source != null) && (string.Compare(newEventName, "Loaded", StringComparison.Ordinal) == 0))
				{
					RegisterLoaded(source);
				}
				else if (!string.IsNullOrEmpty(newEventName))
				{
					RegisterEvent(Source, newEventName);
				}
			}
		}

		private void OnSourceChanged(object oldSource, object newSource)
		{
			if (AssociatedObject != null)
			{
				OnSourceChangedImpl(oldSource, newSource);
			}
		}

		internal virtual void OnSourceChangedImpl(object oldSource, object newSource)
		{
			if (!string.IsNullOrEmpty(GetEventName()) && (string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) != 0))
			{
				if ((oldSource != null) && SourceTypeConstraint.IsAssignableFrom(oldSource.GetType()))
				{
					UnregisterEvent(oldSource, GetEventName());
				}
				if ((newSource != null) && SourceTypeConstraint.IsAssignableFrom(newSource.GetType()))
				{
					RegisterEvent(newSource, GetEventName());
				}
			}
		}

		private static void OnSourceNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EventTriggerBase base2 = (EventTriggerBase)obj;
			base2.SourceNameResolver.Name = (string)args.NewValue;
		}

		private void OnSourceNameResolverElementChanged(object sender, NameResolvedEventArgs e)
		{
			if (SourceObject == null)
			{
				OnSourceChanged(e.OldObject, e.NewObject);
			}
		}

		private static void OnSourceObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EventTriggerBase base2 = (EventTriggerBase)obj;
			object newSource = base2.SourceNameResolver.Object;
			if (args.NewValue == null)
			{
				base2.OnSourceChanged(args.OldValue, newSource);
			}
			else
			{
				if ((args.OldValue == null) && (newSource != null))
				{
					base2.UnregisterEvent(newSource, base2.GetEventName());
				}
				base2.OnSourceChanged(args.OldValue, args.NewValue);
			}
		}

		private void RegisterEvent(object obj, string eventName)
		{
			EventInfo eventInfo = obj.GetType().GetEvent(eventName);
			if (eventInfo == null)
			{
				if (SourceObject != null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.EventTriggerCannotFindEventNameExceptionMessage, new object[] { eventName, obj.GetType().Name }));
				}
			}
			else if (!IsValidEvent(eventInfo))
			{
				if (SourceObject != null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.EventTriggerBaseInvalidEventExceptionMessage, new object[] { eventName, obj.GetType().Name }));
				}
			}
			else
			{
				eventHandlerMethodInfo = typeof(EventTriggerBase).GetMethod("OnEventImpl", BindingFlags.NonPublic | BindingFlags.Instance);
				eventInfo.AddEventHandler(obj, Delegate.CreateDelegate(eventInfo.EventHandlerType, this, eventHandlerMethodInfo));
			}
		}

		private void RegisterLoaded(FrameworkElement associatedElement)
		{
			if (!IsLoadedRegistered && (associatedElement != null))
			{
				associatedElement.Loaded += new RoutedEventHandler(OnEventImpl);
				IsLoadedRegistered = true;
			}
		}

		private void RegisterSourceChanged()
		{
			if (!IsSourceChangedRegistered)
			{
				SourceNameResolver.ResolvedElementChanged += new EventHandler<NameResolvedEventArgs>(OnSourceNameResolverElementChanged);
				IsSourceChangedRegistered = true;
			}
		}

		private void UnregisterEvent(object obj, string eventName)
		{
			if (string.Compare(eventName, "Loaded", StringComparison.Ordinal) == 0)
			{
				FrameworkElement associatedElement = obj as FrameworkElement;
				if (associatedElement != null)
				{
					UnregisterLoaded(associatedElement);
				}
			}
			else
			{
				UnregisterEventImpl(obj, eventName);
			}
		}

		private void UnregisterEventImpl(object obj, string eventName)
		{
			Type type = obj.GetType();
			if (eventHandlerMethodInfo != null)
			{
				EventInfo info = type.GetEvent(eventName);
				info.RemoveEventHandler(obj, Delegate.CreateDelegate(info.EventHandlerType, this, eventHandlerMethodInfo));
				eventHandlerMethodInfo = null;
			}
		}

		private void UnregisterLoaded(FrameworkElement associatedElement)
		{
			if (IsLoadedRegistered && (associatedElement != null))
			{
				associatedElement.Loaded -= new RoutedEventHandler(OnEventImpl);
				IsLoadedRegistered = false;
			}
		}

		private void UnregisterSourceChanged()
		{
			if (IsSourceChangedRegistered)
			{
				SourceNameResolver.ResolvedElementChanged -= new EventHandler<NameResolvedEventArgs>(OnSourceNameResolverElementChanged);
				IsSourceChangedRegistered = false;
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

		private bool IsLoadedRegistered { get; set; }

		private bool IsSourceChangedRegistered
		{
			get { return isSourceChangedRegistered; }
			set { isSourceChangedRegistered = value; }
		}

		private bool IsSourceNameSet
		{
			get
			{
				if (string.IsNullOrEmpty(SourceName))
				{
					return (base.ReadLocalValue(SourceNameProperty) != DependencyProperty.UnsetValue);
				}
				return true;
			}
		}

		public object Source
		{
			get
			{
				object associatedObject = AssociatedObject;
				if (SourceObject != null)
				{
					return SourceObject;
				}
				if (IsSourceNameSet)
				{
					associatedObject = SourceNameResolver.Object;
					if ((associatedObject != null) && !SourceTypeConstraint.IsAssignableFrom(associatedObject.GetType()))
					{
						throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.RetargetedTypeConstraintViolatedExceptionMessage, new object[] { base.GetType().Name, associatedObject.GetType(), SourceTypeConstraint, "Source" }));
					}
				}
				return associatedObject;
			}
		}

		public string SourceName
		{
			get { return (string)base.GetValue(SourceNameProperty); }
			set { base.SetValue(SourceNameProperty, value); }
		}

		private NameResolver SourceNameResolver
		{
			get { return sourceNameResolver; }
		}

		public object SourceObject
		{
			get { return base.GetValue(SourceObjectProperty); }
			set { base.SetValue(SourceObjectProperty, value); }
		}

		protected Type SourceTypeConstraint
		{
			get { return sourceTypeConstraint; }
		}
	}

	public abstract class EventTriggerBase<T> : EventTriggerBase where T : class
	{
		protected EventTriggerBase() : base(typeof(T))
		{
		}

		protected virtual void OnSourceChanged(T oldSource, T newSource)
		{
		}

		internal sealed override void OnSourceChangedImpl(object oldSource, object newSource)
		{
			base.OnSourceChangedImpl(oldSource, newSource);
			this.OnSourceChanged(oldSource as T, newSource as T);
		}

		public new T Source
		{
			get { return (T)base.Source; }
		}
	}
}