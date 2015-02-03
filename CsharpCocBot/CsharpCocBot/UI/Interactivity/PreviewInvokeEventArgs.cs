namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class PreviewInvokeEventArgs : EventArgs
	{
		public PreviewInvokeEventArgs() { }

		public bool Cancelling { get; set; }
	}
}