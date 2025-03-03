using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToDoWPF.Infrastructure.Commands;
using ToDoWPF.Infrastructure.Services;
using ToDoWPF.Models;
using ToDoWPF.ViewModels.Base;

namespace ToDoWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly TaskService _taskService = new TaskService();
        private ObservableCollection<TaskModel> _tasks;
        public TaskModel _selectedTask;
        private string? _Title;

        /*---------------------------------------------------------------------------------*/

        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecuted);
            MinimizeApplicationCommand = new LambdaCommand(OnMinimizeApplicationCommandExecuted, CanMinimizeApplicationCommandExecuted);
            MaximizeApplicationCommand = new LambdaCommand(OnMaximizeApplicationCommandExecuted, CanMaximizeApplicationCommandExecuted);
            GetTasksCommand = new LambdaCommand(async (obj) => await OnGetTasksCommandExecuted(obj), CanGetTasksCommandExecuted);
            #endregion
        }

        /*---------------------------------------------------------------------------------*/

        #region Properties

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        public ObservableCollection<TaskModel> Tasks
        {
            get => _tasks;
            set => Set(ref _tasks, value);
        }

        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set => Set(ref _selectedTask, value);
        }

        #endregion

        /*---------------------------------------------------------------------------------*/

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

        #region Requests
        public ICommand GetTasksCommand { get; }
        public bool CanGetTasksCommandExecuted(object p) => true;
        public async Task OnGetTasksCommandExecuted(object p)
        {
            var listTasks = await _taskService.GetTaskAsync();
            if (listTasks != null)
            {
                Tasks = new ObservableCollection<TaskModel>(listTasks);
            }
        }
        #endregion
        #endregion

        /*---------------------------------------------------------------------------------*/

        
    }
}
    