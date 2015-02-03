namespace CoC.Bot.UI.Behaviours
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls.Primitives;

	/// <summary>
	/// This forces a <see cref="TextBoxBase"/> to scroll to end automatically when the <see cref="TextBoxBase.TextChanged"/>-Event is fired
	/// </summary>
	public static class AutoScrollTextBoxBaseBehaviour
	{
		#region Dependency Properties

		#region IsEnabledProperty

		/// <summary>
		/// Dependency-Property for enabling the autoscroll-feature
		/// </summary>
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(AutoScrollTextBoxBaseBehaviour), new UIPropertyMetadata(false, IsEnabledChangedCallback));
		/// <summary>
		/// Attached Property getter for the IsEnabled property.
		/// </summary>
		public static bool GetIsEnabled(DependencyObject source)
		{
			return (bool)source.GetValue(IsEnabledProperty);
		}
		/// <summary>
		/// Attached Property setter for the IsEnabled property.
		/// </summary>
		public static void SetIsEnabled(DependencyObject source, bool value)
		{
			source.SetValue(IsEnabledProperty, value);
		}
		/// <summary>
		/// The property changed handler for the IsEnabled property.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void IsEnabledChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			TextBoxBase tbb = sender as TextBoxBase;
			if (tbb == null) return;

			tbb.TextChanged -= OnTextChanged;

			bool b = ((e.NewValue != null && e.NewValue.GetType() == typeof(bool))) ? (bool)e.NewValue : false;
			if (b)
			{
				tbb.TextChanged += OnTextChanged;
			}

		}

		#endregion

		#endregion

		#region Private Methods

		/// <summary>
		/// If <see cref="TextBoxBase.TextChanged"/>-Event is fired, scroll to the end
		/// </summary>
		private static void OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			((TextBoxBase)sender).ScrollToEnd();
		}

		#endregion
	}
}