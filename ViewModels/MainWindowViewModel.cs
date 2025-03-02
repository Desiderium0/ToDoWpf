using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToDoWPF.Infrastructure.Commands;
using ToDoWPF.ViewModels.Base;

namespace ToDoWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Properties
        private string? _Title;

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private List<string> _Tasks;

        public List<string> Tasks
        {
            get => _Tasks;
            set => Set(ref _Tasks, value);
        }
        #endregion

        #region Commands
        #region CloseApplicationCommandExecuted
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecuted(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region MinimizeApplicationCommand
        public ICommand MinimizeApplicationCommand { get; }
        private bool CanMinimizeApplicationCommandExecuted(object p) => true;
        private void OnMinimizeApplicationCommandExecuted(object p)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        #endregion

        #region MaximizeApplicationCommand
        public ICommand MaximizeApplicationCommand { get; }
        public WindowState CurrentWindowState 
        {
            get => Application.Current.MainWindow.WindowState;
        }
        private bool CanMaximizeApplicationCommandExecuted(object p) => true;
        private void OnMaximizeApplicationCommandExecuted(object p)
        {
            if (CurrentWindowState == WindowState.Normal)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                Application.Current.MainWindow.WindowState = WindowState.Normal;
        }
        #endregion
        #endregion

        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecuted);
            MinimizeApplicationCommand = new LambdaCommand(OnMinimizeApplicationCommandExecuted, CanMinimizeApplicationCommandExecuted);
            MaximizeApplicationCommand = new LambdaCommand(OnMaximizeApplicationCommandExecuted, CanMaximizeApplicationCommandExecuted);

            #endregion
        }
    }
}
    