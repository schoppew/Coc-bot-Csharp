namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
	public sealed class DefaultTriggerAttribute : Attribute
	{
		private object[] parameters;
		private Type targetType;
		private Type triggerType;

		public DefaultTriggerAttribute(Type targetType, Type triggerType, params object[] parameters)
		{
			if (!typeof(TriggerBase).IsAssignableFrom(triggerType))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage, new object[] { triggerType.Name }));
			}
			this.targetType = targetType;
			this.triggerType = triggerType;
			this.parameters = parameters;
		}

		public TriggerBase Instantiate()
		{
			object obj2 = null;
			try
			{
				obj2 = Activator.CreateInstance(TriggerType, parameters);
			}
			catch
			{
			}
			return (TriggerBase)obj2;
		}

		public IEnumerable Parameters
		{
			get { return parameters; }
		}

		public Type TargetType
		{
			get { return targetType; }
		}

		public Type TriggerType
		{
			get { return triggerType; }
		}
	}
}