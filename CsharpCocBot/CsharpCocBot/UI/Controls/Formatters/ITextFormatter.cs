namespace CoC.Bot.UI.Controls
{
	using System.Windows.Documents;

	public interface ITextFormatter
	{
		string GetText(FlowDocument document);

		void SetText(FlowDocument document, string text);
	}
}