using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net;
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
        private string? _title;


        /*---------------------------------------------------------------------------------*/

        public MainWindowViewModel()
        {
            #region Команды

            LoadTasksAsync();
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecuted);
            MinimizeApplicationCommand = new LambdaCommand(OnMinimizeApplicationCommandExecuted, CanMinimizeApplicationCommandExecuted);
            MaximizeApplicationCommand = new LambdaCommand(OnMaximizeApplicationCommandExecuted, CanMaximizeApplicationCommandExecuted);
            DeleteTaskCommand = new LambdaCommand(async (obj) => await OnDeleteTaskCommandExecuted(obj), CanDeleteTaskCommandExecuted);
            AddTaskCommand = new LambdaCommand(async (obj) => await OnAddTaskCommandExecuted(obj), CanAddTaskCommandExecuted);
            PutTaskCommand = new LambdaCommand(async (obj) => await OnPutTaskCommandExecuted(obj), CanPutTaskCommandExecuted);
            #endregion
        }

        /*---------------------------------------------------------------------------------*/

        #region Properties

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
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

        #region DeleteTaskCommand

        

        #endregion

        #region Requests
        public ICommand AddTaskCommand { get; }
        public bool CanAddTaskCommandExecuted(object p) => true;
        public async Task OnAddTaskCommandExecuted(object p)
        {
            try
            {
                await _taskService.AddTaskAsync();
                await LoadTasksAsync();
            }
            catch
            {
                MessageBox.Show("API не доступен.", "Ошибка!");
            }
        }

        public ICommand DeleteTaskCommand { get; }
        public bool CanDeleteTaskCommandExecuted(object p) => true;
        public async Task OnDeleteTaskCommandExecuted(object p)
        {
            try
            {
                await _taskService.DeleteTaskAsync(SelectedTask);
                await LoadTasksAsync();
            }
            catch
            {
                MessageBox.Show("API не доступен.", "Ошибка!");
            }
        }

        public ICommand PutTaskCommand { get; }
        public bool CanPutTaskCommandExecuted(object p) => true;
        public async Task OnPutTaskCommandExecuted(object p)
        {
            try
            {
                await _taskService.PutTaskAsync(SelectedTask);
                await LoadTasksAsync(); // наверное по другому можно сделать
            }
            catch
            {
                MessageBox.Show("API не доступен.", "Ошибка!");
            }
        }

        #endregion

        #endregion

        /*---------------------------------------------------------------------------------*/

        private async Task LoadTasksAsync()
        {
            var taskList = await _taskService.GetTaskAsync();
            if (taskList != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Tasks = new ObservableCollection<TaskModel>(taskList);
                });
            }
        }
    }
}

    