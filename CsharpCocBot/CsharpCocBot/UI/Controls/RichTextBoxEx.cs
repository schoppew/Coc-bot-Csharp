namespace CoC.Bot.UI.Controls
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Documents;

	public class RichTextBoxEx : RichTextBox
	{
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(RichTextBoxEx), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDocumentChanged)));

		public new FlowDocument Document
		{
			get
			{
				return (FlowDocument)GetValue(DocumentProperty);
			}

			set
			{
				SetValue(DocumentProperty, value);
			}
		}

		public static void OnDocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			RichTextBox rtb = (RichTextBox)obj;
			rtb.Document = (FlowDocument)args.NewValue;
		}
	}
}