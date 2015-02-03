namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	internal sealed class NameResolvedEventArgs : EventArgs
	{
		private object newObject;
		private object oldObject;

		public NameResolvedEventArgs(object oldObject, object newObject)
		{
			this.oldObject = oldObject;
			this.newObject = newObject;
		}

		public object NewObject
		{
			get { return newObject; }
		}

		public object OldObject
		{
			get { return oldObject; }
		}
	}
}