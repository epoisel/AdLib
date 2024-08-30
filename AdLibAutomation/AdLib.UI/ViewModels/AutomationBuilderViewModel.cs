using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using AdLib.Common.Services;
using AdLib.Automation.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace AdLib.UI.ViewModels
{
    public class AutomationBuilderViewModel : BaseViewModel
    {
        private readonly ILogger<AutomationBuilderViewModel> _logger;
        private readonly IActionManager _actionManager;

        public ObservableCollection<IAutomationAction> AutomationActions { get; private set; }
        public ICommand AddToWorkflowCommand { get; private set; }
        public ICommand RunWorkflowCommand { get; private set; }

        // Property to hold the search text
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public AutomationBuilderViewModel(ILogger<AutomationBuilderViewModel> logger, IActionManager actionManager)
        {
            _logger = logger;
            _actionManager = actionManager;

            AutomationActions = new ObservableCollection<IAutomationAction>();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddToWorkflowCommand = new RelayCommand<object>(AddToWorkflow, CanAddToWorkflow);
            RunWorkflowCommand = new RelayCommand<object>(RunWorkflow, CanRunWorkflow);
        }

        private bool CanAddToWorkflow(object parameter) => true;

        private void AddToWorkflow(object parameter)
        {
            // For now, this can just log a message
            _logger.LogInformation("AddToWorkflowCommand executed.");
        }

        private bool CanRunWorkflow(object parameter) => AutomationActions.Count > 0;

        private void RunWorkflow(object parameter)
        {
            // For now, this can just log a message
            _logger.LogInformation("RunWorkflowCommand executed.");
        }
    }
}
