namespace CoC.Bot.UI.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// The AboutCommand.
    /// </summary>
    public class AboutCommand : ICommand
    {
        private bool IsExecuted = false;

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            // do your checking here to see if the command can execute
            if (IsExecuted)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            IsExecuted = true;
            var aboutWindow = new Views.About();
            aboutWindow.Owner = App.Current.MainWindow;
            aboutWindow.ShowDialog();
            IsExecuted = false;
        }
    }
}