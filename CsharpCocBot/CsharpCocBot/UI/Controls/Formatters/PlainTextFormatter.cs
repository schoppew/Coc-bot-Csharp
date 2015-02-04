namespace CoC.Bot.UI.Controls
{
	using System.Windows.Documents;
	using System.Windows.Media;

	/// <summary>
	/// Formats the RichTextBox text as plain text
	/// </summary>
	public class PlainTextFormatter : ITextFormatter
	{
		public string GetText(FlowDocument document)
		{
			return new TextRange(document.ContentStart, document.ContentEnd).Text;
		}

		public void SetText(FlowDocument document, string text)
		{
			new TextRange(document.ContentStart, document.ContentEnd).Text = text;

			//TextRange tr = new TextRange(document.ContentEnd, document.ContentEnd);
			//tr.Text = text;
			////tr.ApplyPropertyValue(TextElement.ForegroundProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue));
			//tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
		}
	}
}