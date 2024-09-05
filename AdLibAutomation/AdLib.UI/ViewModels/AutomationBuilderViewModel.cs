using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AdLib.Contracts.Interfaces;
using AdLib.Contracts.ViewModels;
using CommunityToolkit.Mvvm.Input;

namespace AdLib.UI.ViewModels
{
    public class AutomationBuilderViewModel : BaseViewModel
    {
        private readonly IActionManager _actionManager;
        public ObservableCollection<IAutomationAction> AvailableActions { get; private set; }
        public ObservableCollection<IAutomationAction> AutomationActions { get; private set; }
        public ObservableCollection<ActionPropertyViewModel> SelectedActionProperties { get; private set; }

        private IAutomationAction _selectedAction;
        public IAutomationAction SelectedAction
        {
            get => _selectedAction;
            set
            {
                if (_selectedAction != value)
                {
                    _selectedAction = value;
                    OnPropertyChanged(nameof(SelectedAction));
                    LoadActionProperties();
                }
            }
        }

        public ICommand SelectActionCommand { get; private set; }
        public ICommand AddToWorkflowCommand { get; private set; }
        public ICommand RunWorkflowCommand { get; private set; }

        public AutomationBuilderViewModel(IActionManager actionManager)
        {
            _actionManager = actionManager ?? throw new ArgumentNullException(nameof(actionManager));

            AvailableActions = new ObservableCollection<IAutomationAction>();
            AutomationActions = new ObservableCollection<IAutomationAction>();
            SelectedActionProperties = new ObservableCollection<ActionPropertyViewModel>();

            LoadActions();
            InitializeCommands();
        }

        private void LoadActions()
        {
            var availableActions = _actionManager.GetAvailableActions();
            foreach (var action in availableActions)
            {
                AvailableActions.Add(action);
            }
        }

        private void LoadActionProperties()
        {
            SelectedActionProperties.Clear();
            if (_selectedAction != null)
            {
                var properties = _selectedAction.GetProperties();
                foreach (var property in properties)
                {
                    SelectedActionProperties.Add(property);
                }
            }
        }

        private void InitializeCommands()
        {
            SelectActionCommand = new RelayCommand<IAutomationAction>(action =>
            {
                SelectedAction = action;
            });

            AddToWorkflowCommand = new RelayCommand(AddToWorkflow);
            RunWorkflowCommand = new RelayCommand(RunWorkflow, CanRunWorkflow);
        }

        private void AddToWorkflow()
        {
            if (SelectedAction != null)
            {
                AutomationActions.Add(SelectedAction);
                Console.WriteLine($"Action {SelectedAction.Name} added to workflow.");
            }
        }

        private bool CanRunWorkflow() => AutomationActions.Count > 0;

        private void RunWorkflow()
        {
            foreach (var action in AutomationActions)
            {
                try
                {
                    action.Validate();
                    action.Execute();
                    Console.WriteLine($"{action.Name} executed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing {action.Name}: {ex.Message}");
                }
            }
        }
    }
}
