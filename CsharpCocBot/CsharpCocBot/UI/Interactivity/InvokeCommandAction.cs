namespace CoC.Bot.UI.Interactivity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Input;

	public sealed class InvokeCommandAction : TriggerAction<DependencyObject>
	{
		private string commandName;
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommandAction), null);
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandAction), null);

		protected override void Invoke(object parameter)
		{
			if (AssociatedObject != null)
			{
				ICommand command = ResolveCommand();
				if ((command != null) && command.CanExecute(CommandParameter))
				{
					command.Execute(CommandParameter);
				}
			}
		}

		private ICommand ResolveCommand()
		{
			ICommand command = null;
			if (Command != null)
			{
				return Command;
			}
			if (AssociatedObject != null)
			{
				foreach (PropertyInfo info in AssociatedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
				{
					if (typeof(ICommand).IsAssignableFrom(info.PropertyType) && string.Equals(info.Name, CommandName, StringComparison.Ordinal))
					{
						command = (ICommand)info.GetValue(AssociatedObject, null);
					}
				}
			}
			return command;
		}

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(CommandProperty);
			}
			set
			{
				base.SetValue(CommandProperty, value);
			}
		}

		public string CommandName
		{
			get
			{
				base.ReadPreamble();
				return commandName;
			}
			set
			{
				if (CommandName != value)
				{
					base.WritePreamble();
					commandName = value;
					base.WritePostscript();
				}
			}
		}

		public object CommandParameter
		{
			get
			{
				return base.GetValue(CommandParameterProperty);
			}
			set
			{
				base.SetValue(CommandParameterProperty, value);
			}
		}
	}
}