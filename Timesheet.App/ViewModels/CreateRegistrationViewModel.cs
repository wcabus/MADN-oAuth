using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TTask = System.Threading.Tasks.Task;
using Timesheet.App.Services;
using Timesheet.Domain;
using Timesheet.App.Messages;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.UI.Popups;
using System.Linq;
using GalaSoft.MvvmLight.Views;

namespace Timesheet.App.ViewModels
{
    public sealed class CreateRegistrationViewModel : ViewModelBase
    {
        private readonly INavigationService _navService;
        private readonly ApiService _apiService;

        private ICommand _saveCommand;
        private ICommand _deleteCommand;

        private Project _selectedProject;
        private Task _selectedTask;
        private DateTimeOffset _date = DateTimeOffset.Now;
        private TimeSpan _start;
        private TimeSpan _end;
        private string _remarks = "";
        private bool _initialized;
        
        public CreateRegistrationViewModel(INavigationService navService, ApiService apiService)
        {
            _navService = navService;
            _apiService = apiService;

            Projects = new ObservableCollection<Project>();
            Tasks = new ObservableCollection<Task>();

            RegisterMessaging();
            CreateCommands();
        }

        public ObservableCollection<Project> Projects { get; }
        public ObservableCollection<Task> Tasks { get; }

        public Project SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                if (_selectedProject == value)
                {
                    return;
                }

                _selectedProject = value;
                RaisePropertyChanged();

                Tasks.Clear();
                SelectedTask = null;
                if (_selectedProject != null)
                {
                    MessengerInstance.Send(new LoadTasksMessage(_selectedProject));
                }
            }
        }

        public Task SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                if (_selectedTask == value)
                {
                    return;
                }

                _selectedTask = value;
                RaisePropertyChanged();
            }
        }

        public DateTimeOffset Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (_date == value)
                {
                    return;
                }

                _date = value;
                RaisePropertyChanged();
            }
        }

        public TimeSpan Start
        {
            get
            {
                return _start;
            }
            set
            {
                if (_start == value)
                {
                    return;
                }

                _start = value;
                RaisePropertyChanged();
            }
        }

        public TimeSpan End
        {
            get
            {
                return _end;
            }
            set
            {
                if (_end == value)
                {
                    return;
                }

                _end = value;
                RaisePropertyChanged();
            }
        }

        public string Remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                if (string.Equals(_remarks, value))
                {
                    return;
                }

                _remarks = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SaveCommand => _saveCommand;
        public ICommand DeleteCommand => _deleteCommand;

        private void RegisterMessaging()
        {
            MessengerInstance.Register<InitializeRegistrationViewModelMessage>(this, async msg => await InitializeAsync());
            MessengerInstance.Register<LoadTasksMessage>(this, async msg => await LoadTasksAsync(msg.Project));
        }

        private void CreateCommands()
        {
            _saveCommand = new RelayCommand(async() => await SaveAsync());
            _deleteCommand = new RelayCommand(Reset);
        }

        private async TTask LoadTasksAsync(Project project)
        {
            Tasks.Clear();

            IEnumerable<Task> tasks = null;
            try
            {
                tasks = await _apiService.GetListAsync<Task>($"projects/{project.Id}/tasks");
            }
            catch (AccessTokenExpiredException)
            {
                try
                {
                    await _apiService.RefreshAccessTokenAsync();
                    tasks = await _apiService.GetListAsync<Task>($"projects/{project.Id}/tasks");
                }
                catch
                {
                    _navService.GoBack();
                }
            }

            foreach (var task in tasks.OrderBy(t => t.Name))
            {
                Tasks.Add(task);
            }
        }

        private async TTask SaveAsync()
        {
            var registration = new
            {
                TaskId = _selectedTask.Id,
                Start = _date.Date.Add(_start),
                End = _date.Date.Add(_end),
                Remarks = _remarks
            };
            
            try
            {
                await _apiService.CreateRegistrationAsync(App.EmployeeId, registration);
                Reset();

                var dlg = new MessageDialog("Saved!", "Create Registration");
                await dlg.ShowAsync();
            }
            catch (Exception ex)
            {
                var dlg = new MessageDialog($"An error occurred while creating the registration: {ex.Message}", "Create Registration");
                await dlg.ShowAsync();
            }
        }

        private void Reset()
        {
            SelectedProject = null;
            SelectedTask = null;
            Date = DateTimeOffset.UtcNow;
            Start = TimeSpan.FromHours(8);
            End = TimeSpan.FromHours(12);
            Remarks = "";
        }

        private async TTask InitializeAsync()
        {
            if (_initialized)
            {
                return;
            }

            // Load the projects
            Projects.Clear();
            IEnumerable<Project> projects;

            try
            {
                projects = await _apiService.GetListAsync<Project>("projects");
            }
            catch (AccessTokenExpiredException)
            {
                await _apiService.RefreshAccessTokenAsync();

                try
                {
                    projects = await _apiService.GetListAsync<Project>("projects");
                }
                catch (AccessTokenExpiredException)
                {
                    ApiService.RemoveTokenFromVault();
                    _navService.GoBack();
                    return;
                }
            }

            foreach (var project in projects.OrderBy(p => p.Name))
            {
                Projects.Add(project);
            }

            _initialized = true;

            // Set defaults
            Reset();
        }
    }
}
