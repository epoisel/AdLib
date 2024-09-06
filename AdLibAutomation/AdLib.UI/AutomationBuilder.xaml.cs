using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using AdLib.UI.ViewModels;
using AdLib.Contracts.ViewModels;
using System.Diagnostics;
using System.Windows.Controls;

namespace AdLib.UI
{
    public partial class AutomationBuilder : Window
    {
        private readonly AutomationBuilderViewModel _viewModel;

        public AutomationBuilder(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Console.WriteLine("AutomationBuilder constructor called.");

            // Inject and set the ViewModel
            _viewModel = serviceProvider.GetRequiredService<AutomationBuilderViewModel>();
            DataContext = _viewModel;

            Console.WriteLine("DataContext set to AutomationBuilderViewModel.");
        }


        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dataContext = (ActionPropertyViewModel)e.NewValue;
            Debug.WriteLine($"Button DataContext: {dataContext}");
        }
    }
}









//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Input;
//using System.Windows.Media;
//using Microsoft.Win32;
//using AdLib.Automation.Interfaces;
//using AdLib.Common.Interfaces;
//using AdLib.Common.Utilities;
//using AdLib.UI.Services;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using AdLib.Automation;
//using AdLib.common.Interfaces;


//namespace AdLib.UI
//{
//    public partial class AutomationBuilder : Window
//    {
//        private readonly IServiceProvider _serviceProvider;
//        private readonly IWindowSelectionService _windowSelectionService;
//        private readonly ILogger<AutomationBuilder> _logger;

//        // Declare availableActions as a class-level field
//        private List<Type> availableActions = new List<Type>();

//        private ObservableCollection<IAutomationAction> automationActions = new ObservableCollection<IAutomationAction>();
//        private IAutomationAction selectedAction;

//        // Expose automationActions as a public property for binding
//        public ObservableCollection<IAutomationAction> AutomationActions => automationActions;

//        // Parameterless constructor for XAML support
//        public AutomationBuilder()
//        {
//            InitializeComponent();
//        }

//        // Main constructor with dependency injection
//        public AutomationBuilder(IServiceProvider serviceProvider, IWindowSelectionService windowSelectionService, ILogger<AutomationBuilder> logger)
//            : this() // Call the parameterless constructor
//        {
//            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
//            _windowSelectionService = windowSelectionService ?? throw new ArgumentNullException(nameof(windowSelectionService));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger was not provided via dependency injection.");

//            // Set the DataContext to this instance for data binding in XAML
//            this.DataContext = this;

//            _logger.LogInformation("AutomationBuilder initialized with services.");
//            LoadAvailableActions(); // Dynamically load and create buttons for available actions
//        }

//        public static class WindowFactory
//        {
//            public static AutomationBuilder CreateAutomationBuilder(IServiceProvider serviceProvider)
//            {
//                try
//                {
//                    var windowSelectionService = serviceProvider.GetRequiredService<IWindowSelectionService>();
//                    var logger = serviceProvider.GetRequiredService<ILogger<AutomationBuilder>>();

//                    if (logger == null)
//                    {
//                        Console.WriteLine("Logger not found.");
//                        throw new ArgumentNullException(nameof(logger), "Logger is null. Check DI setup.");
//                    }

//                    return new AutomationBuilder(serviceProvider, windowSelectionService, logger);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error creating AutomationBuilder: {ex.Message}");
//                    throw new InvalidOperationException("Failed to create AutomationBuilder instance due to missing services.", ex);
//                }
//            }

//        }





//        // Load all available actions and create buttons in the toolbox
//        private void LoadAvailableActions()
//        {
//            _logger.LogDebug("Loading available actions...");
//            availableActions = AutomationActionFactory.GetAvailableActions();

//            if (availableActions == null || !availableActions.Any())
//            {
//                _logger.LogWarning("No available actions found.");
//                MessageBox.Show("No available actions found.");
//                return;
//            }

//            _logger.LogInformation($"{availableActions.Count} available actions loaded.");
//            DisplayActions(availableActions);
//        }



//        private void DisplayActions(IEnumerable<Type> actions)
//        {
//            ActionButtonsPanel.Children.Clear();
//            _logger.LogDebug("Displaying actions...");

//            foreach (var type in actions)
//            {
//                _logger.LogDebug($"Attempting to create instance for action: {type.Name}");
//                var actionInstance = _serviceProvider.GetService(type) as IAutomationAction;
//                if (actionInstance != null)
//                {
//                    var button = new Button
//                    {
//                        Content = actionInstance.Name,
//                        Margin = new Thickness(5),
//                        Tag = type
//                    };

//                    button.Click += ActionButton_Click;
//                    ActionButtonsPanel.Children.Add(button);
//                    _logger.LogInformation($"Button created for action: {actionInstance.Name}");
//                }
//                else
//                {
//                    _logger.LogError($"Failed to create an instance of {type.Name}.");
//                    MessageBox.Show($"Failed to create an instance of {type.Name}.");
//                }
//            }
//        }



//        // Event handler for toolbox button click to add action to workspace
//        private void ActionButton_Click(object sender, RoutedEventArgs e)
//        {
//            if (sender is Button button && button.Tag is Type actionType)
//            {
//                var action = AutomationActionFactory.CreateAction(actionType, _serviceProvider);
//                if (action != null)
//                {
//                    ConfigureAction(action); // Generic configuration
//                    AddActionToWorkspace(action);
//                }
//                else
//                {
//                    _logger.LogError($"Failed to create action from type: {actionType.Name}");
//                }
//            }
//        }

//        // Method to add action to the workspace dynamically
//        private void AddActionToWorkspace(IAutomationAction action)
//        {
//            var actionBlock = new TextBlock
//            {
//                Text = action.Name,
//                Background = Brushes.LightBlue,
//                Width = 120,
//                Height = 30,
//                TextAlignment = TextAlignment.Center,
//                Margin = new Thickness(5),
//                VerticalAlignment = VerticalAlignment.Center,
//                HorizontalAlignment = HorizontalAlignment.Center
//            };

//            actionBlock.Tag = action;
//            actionBlock.MouseMove += Action_MouseMove;
//            actionBlock.MouseLeftButtonUp += (s, e) => ShowProperties(action);

//            Canvas.SetLeft(actionBlock, 50); // Example position
//            Canvas.SetTop(actionBlock, 50);  // Example position
//            WorkspaceCanvas.Children.Add(actionBlock);
//            _logger.LogInformation($"Action {action.Name} added to workspace.");
//        }

//        // Implementing drag-and-drop functionality
//        private void Action_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (e.LeftButton == MouseButtonState.Pressed)
//            {
//                if (sender is TextBlock textBlock)
//                {
//                    DragDrop.DoDragDrop(textBlock, new DataObject(typeof(TextBlock), textBlock), DragDropEffects.Move);
//                    _logger.LogInformation("Drag-and-drop operation started.");
//                }
//            }
//        }

//        // Handles the drop event on the workspace to position action blocks
//        private void WorkspaceCanvas_Drop(object sender, DragEventArgs e)
//        {
//            if (e.Data.GetDataPresent(typeof(TextBlock)))
//            {
//                var actionBlock = e.Data.GetData(typeof(TextBlock)) as TextBlock;
//                var position = e.GetPosition(WorkspaceCanvas);
//                Canvas.SetLeft(actionBlock, position.X);
//                Canvas.SetTop(actionBlock, position.Y);

//                if (!WorkspaceCanvas.Children.Contains(actionBlock))
//                {
//                    WorkspaceCanvas.Children.Add(actionBlock);
//                }
//                _logger.LogInformation($"Action block dropped at position ({position.X}, {position.Y}).");
//            }
//        }

//        // Handles the drag-over event to show that dropping is allowed
//        private void WorkspaceCanvas_DragOver(object sender, DragEventArgs e)
//        {
//            e.Effects = DragDropEffects.Move;
//        }

//        // Show properties of selected action dynamically
//        private void ShowProperties(IAutomationAction action)
//        {
//            selectedAction = action;
//            PropertiesPanelContent.Children.Clear();

//            var properties = action.GetType().GetProperties();
//            foreach (var property in properties)
//            {
//                if (property.CanRead && property.CanWrite)
//                {
//                    TextBlock label = new TextBlock { Text = $"{property.Name}:", Margin = new Thickness(5) };
//                    TextBox textBox = new TextBox
//                    {
//                        Text = property.GetValue(action)?.ToString(),
//                        Margin = new Thickness(5),
//                        Width = 200
//                    };

//                    textBox.TextChanged += (s, e) =>
//                    {
//                        try
//                        {
//                            var value = Convert.ChangeType(textBox.Text, property.PropertyType);
//                            property.SetValue(action, value);

//                            if (property.Name == "Name")
//                            {
//                                UpdateActionName(action); // Update the action name in the UI
//                                UpdateExecutionPanel(); // Ensure the execution list reflects the new name
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            _logger.LogError($"Error updating property {property.Name} for action {action.Name}: {ex.Message}");
//                        }
//                    };

//                    PropertiesPanelContent.Children.Add(label);
//                    PropertiesPanelContent.Children.Add(textBox);

//                    if (property.Name == "ApplicationPath" && action is IRequiresFilePathAction)
//                    {
//                        Button browseButton = new Button
//                        {
//                            Content = "Browse...",
//                            Margin = new Thickness(5)
//                        };
//                        browseButton.Click += (s, e) =>
//                        {
//                            OpenFileDialog openFileDialog = new OpenFileDialog
//                            {
//                                Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*"
//                            };
//                            if (openFileDialog.ShowDialog() == true)
//                            {
//                                property.SetValue(action, openFileDialog.FileName);
//                                UpdateExePathDisplay(action as IRequiresFilePathAction);
//                                _logger.LogInformation($"File path set to: {openFileDialog.FileName}");
//                            }
//                        };

//                        PropertiesPanelContent.Children.Add(browseButton);
//                    }
//                }
//            }

//            AddToWorkflowButton.Visibility = Visibility.Visible;
//            _logger.LogInformation($"Properties displayed for action {action.Name}.");
//        }

//        private void AddToWorkflowButton_Click(object sender, RoutedEventArgs e)
//        {
//            if (selectedAction != null && !automationActions.Contains(selectedAction))
//            {
//                automationActions.Add(selectedAction);
//                UpdateExecutionPanel(); // Refresh the execution panel to reflect the new action
//                _logger.LogInformation($"Action {selectedAction.Name} added to workflow.");
//            }
//        }

//        // Generic configuration method for any action
//        private void ConfigureAction(IAutomationAction action)
//        {
//            // Use reflection or interfaces to configure specific types of actions
//            if (action is IConfigurableAction configurableAction)
//            {
//                configurableAction.Configure(_windowSelectionService); // Pass the required window selection service
//                _logger.LogInformation($"Action {action.Name} configured with window selection service.");
//            }
//        }

//        private void UpdateActionName(IAutomationAction action)
//        {
//            foreach (var item in WorkspaceCanvas.Children)
//            {
//                if (item is TextBlock textBlock && textBlock.Tag == action)
//                {
//                    textBlock.Text = action.Name;
//                }
//            }
//            _logger.LogInformation($"Action name updated to {action.Name}.");
//        }

//        private void UpdateExecutionPanel()
//        {
//            ExecutionListBox.ItemsSource = null;
//            ExecutionListBox.ItemsSource = automationActions;
//            _logger.LogInformation("Execution panel updated.");
//        }

//        private void UpdateExePathDisplay(IRequiresFilePathAction action)
//        {
//            foreach (var item in PropertiesPanelContent.Children)
//            {
//                if (item is TextBox textBox && textBox.Text == action.FilePath)
//                {
//                    textBox.Text = action.FilePath;
//                    break;
//                }
//            }
//            _logger.LogInformation("Executable path display updated.");
//        }

//        private void RemoveButton_Click(object sender, RoutedEventArgs e)
//        {
//            var selectedAction = ExecutionListBox.SelectedItem as IAutomationAction;
//            if (selectedAction != null)
//            {
//                automationActions.Remove(selectedAction);
//                UpdateExecutionPanel(); // Refresh the list after removing an action
//                _logger.LogInformation($"Action {selectedAction.Name} removed from workflow.");
//            }
//        }

//        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
//        {
//            string searchQuery = SearchTextBox.Text.ToLower();
//            var filteredActions = availableActions.Where(type =>
//                type.Name.ToLower().Contains(searchQuery) ||
//                (AutomationActionFactory.CreateAction(type, _serviceProvider)?.Name.ToLower().Contains(searchQuery) == true));
//            DisplayActions(filteredActions);
//            _logger.LogInformation($"Actions filtered by search query: {searchQuery}");
//        }

//        private void RunWorkflow(object sender, RoutedEventArgs e)
//        {
//            foreach (var action in automationActions)
//            {
//                try
//                {
//                    ExecutionListBox.SelectedItem = action;
//                    ConfigureAction(action); // Configure action generically if needed
//                    action.Execute();
//                    _logger.LogInformation($"Action {action.Name} executed.");
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Error executing {action.Name}: {ex.Message}");
//                    _logger.LogError($"Error executing {action.Name}: {ex.Message}");
//                }
//            }
//        }
//    }
//}
