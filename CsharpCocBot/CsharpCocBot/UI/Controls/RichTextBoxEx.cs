namespace CoC.Bot.UI.Controls
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;

	public class RichTextBoxEx : RichTextBox
	{
		#region Private Members

		private bool _preventDocumentUpdate;
		private bool _preventTextUpdate;

		#endregion

		#region Constructors

		public RichTextBoxEx()
		{
		}

		public RichTextBoxEx(System.Windows.Documents.FlowDocument document) : base(document)
		{

		}

		#endregion

		#region Properties

		#region Text

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(RichTextBoxEx), new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextPropertyChanged, CoerceTextProperty, true, UpdateSourceTrigger.LostFocus));
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RichTextBoxEx)d).UpdateDocumentFromText();
		}

		private static object CoerceTextProperty(DependencyObject d, object value)
		{
			return value ?? "";
		}

		#endregion

		#region TextFormatter

		public static readonly DependencyProperty TextFormatterProperty = DependencyProperty.Register("TextFormatter", typeof(ITextFormatter), typeof(RichTextBoxEx), new FrameworkPropertyMetadata(new RtfFormatter(), OnTextFormatterPropertyChanged));
		public ITextFormatter TextFormatter
		{
			get { return (ITextFormatter)GetValue(TextFormatterProperty); }
			set { SetValue(TextFormatterProperty, value); }
		}

		private static void OnTextFormatterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var richTextBox = d as RichTextBoxEx;
			if (richTextBox != null)
				richTextBox.OnTextFormatterPropertyChanged((ITextFormatter)e.OldValue, (ITextFormatter)e.NewValue);
		}

		protected virtual void OnTextFormatterPropertyChanged(ITextFormatter oldValue, ITextFormatter newValue)
		{
			this.UpdateTextFromDocument();
		}

		#endregion

		#endregion

		#region Methods

		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			base.OnTextChanged(e);
			UpdateTextFromDocument();
		}

		private void UpdateTextFromDocument()
		{
			if (_preventTextUpdate)
				return;

			_preventDocumentUpdate = true;
			Text = TextFormatter.GetText(Document);
			_preventDocumentUpdate = false;
		}

		private void UpdateDocumentFromText()
		{
			if (_preventDocumentUpdate)
				return;

			_preventTextUpdate = true;
			TextFormatter.SetText(Document, Text);
			_preventTextUpdate = false;
		}

		/// <summary>
		/// Clears the content of the RichTextBox.
		/// </summary>
		public void Clear()
		{
			Document.Blocks.Clear();
		}

		public override void BeginInit()
		{
			base.BeginInit();
			// Do not update anything while initializing. See EndInit
			_preventTextUpdate = true;
			_preventDocumentUpdate = true;
		}

		public override void EndInit()
		{
			base.EndInit();
			_preventTextUpdate = false;
			_preventDocumentUpdate = false;
			// Possible conflict here if the user specifies 
			// the document AND the text at the same time 
			// in XAML. Text has priority.
			if (!string.IsNullOrEmpty(Text))
				UpdateDocumentFromText();
			else
				UpdateTextFromDocument();
		}

		#endregion
	}
}