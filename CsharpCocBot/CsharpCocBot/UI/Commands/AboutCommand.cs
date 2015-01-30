namespace CoC.Bot.UI.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    public class AboutCommand : ICommand
    {
        private bool IsExecuted = false;

        public bool CanExecute(object parameter)
        {
            // do your checking here to see if the command can execute
            if (IsExecuted)
                return false;
            else
                return true;
        }

        public event EventHandler CanExecuteChanged;

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