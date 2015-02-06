namespace CoC.Bot.UI.Behaviours
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using UI.Interactivity;

	public class TextBoxBehaviour : Behavior<TextBox>
    {
        public static readonly DependencyProperty AlwaysScrollToEndProperty = DependencyProperty.RegisterAttached("AlwaysScrollToEnd", typeof(bool), typeof(TextBoxBehaviour), new PropertyMetadata(false, AlwaysScrollToEndChanged));

        public static readonly DependencyProperty SelectAllTextOnFocusProperty = DependencyProperty.RegisterAttached("SelectAllTextOnFocus", typeof(bool), typeof(TextBoxBehaviour), new UIPropertyMetadata(false, OnSelectAllTextOnFocusChanged));

		public static readonly DependencyProperty RegularExpressionProperty = DependencyProperty.Register("RegExBehaviour", typeof(string), typeof(TextBoxBehaviour), null);

        #region Scroll Behaviour

        private static void AlwaysScrollToEndChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
			TextBox tb = sender as TextBox;
            if (tb != null)
            {
                bool alwaysScrollToEnd = (e.NewValue != null) && (bool)e.NewValue;
                if (alwaysScrollToEnd)
                {
                    tb.ScrollToEnd();
                    tb.TextChanged += TextChanged;
                }
                else
                {
                    tb.TextChanged -= TextChanged;
                }
            }
            else
            {
                throw new InvalidOperationException("The attached AlwaysScrollToEnd property can only be applied to TextBox instances.");
            }
        }

        public static bool GetAlwaysScrollToEnd(TextBox textBox)
        {
            if (textBox == null)
                throw new ArgumentNullException("textBox");

            return (bool)textBox.GetValue(AlwaysScrollToEndProperty);
        }

        public static void SetAlwaysScrollToEnd(TextBox textBox, bool alwaysScrollToEnd)
        {
            if (textBox == null)
                throw new ArgumentNullException("textBox");

            textBox.SetValue(AlwaysScrollToEndProperty, alwaysScrollToEnd);
        }

        private static void TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).ScrollToEnd();
        }

        #endregion

        #region Select all Text on Focus Behaviour

        private static void OnSelectAllTextOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox == null) return;

            if (e.NewValue is bool == false) return;

            if ((bool)e.NewValue)
            {
                textBox.GotFocus += SelectAll;
                textBox.PreviewMouseDown += IgnoreMouseButton;
            }
            else
            {
                textBox.GotFocus -= SelectAll;
                textBox.PreviewMouseDown -= IgnoreMouseButton;
            }
        }

        private static void SelectAll(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox == null) return;
            textBox.SelectAll();
        }

        private static void IgnoreMouseButton(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null || textBox.IsKeyboardFocusWithin) return;

            e.Handled = true;
            textBox.Focus();
        }

        public static bool GetSelectAllTextOnFocus(TextBox textBox)
        {
            return (bool)textBox.GetValue(SelectAllTextOnFocusProperty);
        }

        public static void SetSelectAllTextOnFocus(TextBox textBox, bool value)
        {
            textBox.SetValue(SelectAllTextOnFocusProperty, value);
        }

        #endregion

		#region RegEx Behaviour

		#region DependencyProperties

		public string RegularExpression
		{
			get { return (string)GetValue(RegularExpressionProperty); }
			set { SetValue(RegularExpressionProperty, value); }
		}

		public static readonly DependencyProperty MaxLengthProperty =
			DependencyProperty.Register("MaxLength", typeof(int), typeof(TextBoxBehaviour),
											new FrameworkPropertyMetadata(int.MinValue));

		public int MaxLength
		{
			get { return (int)GetValue(MaxLengthProperty); }
			set { SetValue(MaxLengthProperty, value); }
		}

		public static readonly DependencyProperty EmptyValueProperty =
			DependencyProperty.Register("EmptyValue", typeof(string), typeof(TextBoxBehaviour), null);

		public string EmptyValue
		{
			get { return (string)GetValue(EmptyValueProperty); }
			set { SetValue(EmptyValueProperty, value); }
		}

		#endregion

		/// <summary>
		///     Attach our behaviour. Add event handlers
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.PreviewTextInput += PreviewTextInputHandler;
			AssociatedObject.PreviewKeyDown += PreviewKeyDownHandler;
			DataObject.AddPastingHandler(AssociatedObject, PastingHandler);
		}

		/// <summary>
		///     Deattach our behaviour. remove event handlers
		/// </summary>
		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.PreviewTextInput -= PreviewTextInputHandler;
			AssociatedObject.PreviewKeyDown -= PreviewKeyDownHandler;
			DataObject.RemovePastingHandler(AssociatedObject, PastingHandler);
		}

		#region Event handlers [PRIVATE]

		void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
		{
			string text;
			if (AssociatedObject.Text.Length < AssociatedObject.CaretIndex)
				text = AssociatedObject.Text;
			else
			{
				//  Remaining text after removing selected text.
				string remainingTextAfterRemoveSelection;

				text = TreatSelectedText(out remainingTextAfterRemoveSelection)
					? remainingTextAfterRemoveSelection.Insert(AssociatedObject.SelectionStart, e.Text)
					: AssociatedObject.Text.Insert(AssociatedObject.CaretIndex, e.Text);
			}

			e.Handled = !ValidateText(text);
		}

		/// <summary>
		///     PreviewKeyDown event handler
		/// </summary>
		void PreviewKeyDownHandler(object sender, KeyEventArgs e)
		{
			if (string.IsNullOrEmpty(EmptyValue))
				return;

			string text = null;

			// Handle the Backspace key
			if (e.Key == Key.Back)
			{
				if (!TreatSelectedText(out text))
				{
					if (AssociatedObject.SelectionStart > 0)
						text = AssociatedObject.Text.Remove(AssociatedObject.SelectionStart - 1, 1);
				}
			}
			// Handle the Delete key
			else if (e.Key == Key.Delete)
			{
				// If text was selected, delete it
				if (!TreatSelectedText(out text) && AssociatedObject.Text.Length > AssociatedObject.SelectionStart)
				{
					// Otherwise delete next symbol
					text = AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, 1);
				}
			}

			if (text == string.Empty)
			{
				AssociatedObject.Text = EmptyValue;
				if (e.Key == Key.Back)
					AssociatedObject.SelectionStart++;
				e.Handled = true;
			}
		}

		private void PastingHandler(object sender, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(DataFormats.Text))
			{
				string text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));

				if (!ValidateText(text))
					e.CancelCommand();
			}
			else
				e.CancelCommand();
		}

		#endregion Event handlers [PRIVATE]

		#region Auxiliary methods [PRIVATE]

		/// <summary>
		///     Validate certain text by our regular expression and text length conditions
		/// </summary>
		/// <param name="text"> Text for validation </param>
		/// <returns> True - valid, False - invalid </returns>
		private bool ValidateText(string text)
		{
			return (new Regex(RegularExpression, RegexOptions.IgnoreCase)).IsMatch(text) && (MaxLength == 0 || text.Length <= MaxLength);
		}

		/// <summary>
		///     Handle text selection
		/// </summary>
		/// <returns>true if the character was successfully removed; otherwise, false. </returns>
		private bool TreatSelectedText(out string text)
		{
			text = null;
			if (AssociatedObject.SelectionLength > 0)
			{
				text = AssociatedObject.Text.Remove(AssociatedObject.SelectionStart, AssociatedObject.SelectionLength);
				return true;
			}
			return false;
		}

		#endregion Auxiliary methods [PRIVATE]

		#endregion
	}
}