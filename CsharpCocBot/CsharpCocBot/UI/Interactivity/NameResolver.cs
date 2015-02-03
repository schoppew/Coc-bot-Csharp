namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;

	internal sealed class NameResolver
	{
		private string name;
		private FrameworkElement nameScopeReferenceElement;

		public event EventHandler<NameResolvedEventArgs> ResolvedElementChanged;

		private FrameworkElement GetActualNameScopeReference(FrameworkElement initialReferenceElement)
		{
			FrameworkElement element = initialReferenceElement;
			if (!IsNameScope(initialReferenceElement))
			{
				return element;
			}
			return ((initialReferenceElement.Parent as FrameworkElement) ?? element);
		}

		private bool IsNameScope(FrameworkElement frameworkElement)
		{
			FrameworkElement parent = frameworkElement.Parent as FrameworkElement;
			return ((parent != null) && (parent.FindName(Name) != null));
		}

		private void OnNameScopeReferenceElementChanged(FrameworkElement oldNameScopeReference)
		{
			if (PendingReferenceElementLoad)
			{
				oldNameScopeReference.Loaded -= new RoutedEventHandler(OnNameScopeReferenceLoaded);
				PendingReferenceElementLoad = false;
			}
			HasAttempedResolve = false;
			UpdateObjectFromName(Object);
		}

		private void OnNameScopeReferenceLoaded(object sender, RoutedEventArgs e)
		{
			PendingReferenceElementLoad = false;
			NameScopeReferenceElement.Loaded -= new RoutedEventHandler(OnNameScopeReferenceLoaded);
			UpdateObjectFromName(Object);
		}

		private void OnObjectChanged(DependencyObject oldTarget, DependencyObject newTarget)
		{
			if (ResolvedElementChanged != null)
			{
				ResolvedElementChanged(this, new NameResolvedEventArgs(oldTarget, newTarget));
			}
		}

		private void UpdateObjectFromName(DependencyObject oldObject)
		{
			DependencyObject obj2 = null;
			ResolvedObject = null;
			if (NameScopeReferenceElement != null)
			{
				if (!Interaction.IsElementLoaded(NameScopeReferenceElement))
				{
					NameScopeReferenceElement.Loaded += new RoutedEventHandler(OnNameScopeReferenceLoaded);
					PendingReferenceElementLoad = true;
					return;
				}
				if (!string.IsNullOrEmpty(Name))
				{
					FrameworkElement actualNameScopeReferenceElement = ActualNameScopeReferenceElement;
					if (actualNameScopeReferenceElement != null)
					{
						obj2 = actualNameScopeReferenceElement.FindName(Name) as DependencyObject;
					}
				}
			}
			HasAttempedResolve = true;
			ResolvedObject = obj2;
			if (oldObject != Object)
			{
				OnObjectChanged(oldObject, Object);
			}
		}

		private FrameworkElement ActualNameScopeReferenceElement
		{
			get
			{
				if ((NameScopeReferenceElement != null) && Interaction.IsElementLoaded(NameScopeReferenceElement))
				{
					return GetActualNameScopeReference(NameScopeReferenceElement);
				}
				return null;
			}
		}

		private bool HasAttempedResolve { get; set; }

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				DependencyObject oldObject = Object;
				name = value;
				UpdateObjectFromName(oldObject);
			}
		}

		public FrameworkElement NameScopeReferenceElement
		{
			get
			{
				return nameScopeReferenceElement;
			}
			set
			{
				FrameworkElement nameScopeReferenceElement = NameScopeReferenceElement;
				this.nameScopeReferenceElement = value;
				OnNameScopeReferenceElementChanged(nameScopeReferenceElement);
			}
		}

		public DependencyObject Object
		{
			get
			{
				if (string.IsNullOrEmpty(Name) && HasAttempedResolve)
				{
					return NameScopeReferenceElement;
				}
				return ResolvedObject;
			}
		}

		private bool PendingReferenceElementLoad { get; set; }

		private DependencyObject ResolvedObject { get; set; }
	}
}