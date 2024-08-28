using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using AdLib.Automation.Interfaces;
using AdLib.Automation.Actions;
using AdLib.Common.Interfaces;
using AdLib.Common.Utilities;
using AdLib.UI.Services;

namespace AdLib.UI
{
    public partial class AutomationBuilder : Window
    {
        private List<Type> availableActions = new List<Type>();
        private ObservableCollection<IAutomationAction> automationActions = new ObservableCollection<IAutomationAction>();
        private IAutomationAction selectedAction;
        private readonly IWindowSelectionService _windowSelectionService;

        public AutomationBuilder()
        {
            InitializeComponent();
            _windowSelectionService = new WindowSelectionService(); // Instance of the selection service
            LoadAvailableActions(); // Dynamically load and create buttons for available actions
        }

        // Show Window Selection Dialog when configuring InputTextAction
        private void ConfigureInputTextAction(InputTextAction inputTextAction)
        {
            var windows = WindowEnumerator.GetOpenWindows(); // Get the list of open windows
            var selectionDialog = new WindowSelectionDialog(windows); // Create a new instance of the dialog
            if (selectionDialog.ShowDialog() == true) // Show the dialog and check if OK was clicked
            {
                inputTextAction.SelectedWindow = selectionDialog.SelectedWindow; // Set the selected window
            }
        }

        // Load all available actions and create buttons in the toolbox
        private void LoadAvailableActions()
        {
            availableActions = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IAutomationAction).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToList();

            DisplayActions(availableActions);
        }

        // Display actions in the toolbox based on a given list
        private void DisplayActions(IEnumerable<Type> actions)
        {
            ActionButtonsPanel.Children.Clear(); // Clear previous buttons

            foreach (var type in actions)
            {
                if (Activator.CreateInstance(type) is IAutomationAction actionInstance)
                {
                    // Configure actions dynamically but only call dialog when user interacts
                    var button = new Button
                    {
                        Content = actionInstance.Name,
                        Margin = new Thickness(5),
                        Tag = type
                    };
                    button.Click += ActionButton_Click;
                    ActionButtonsPanel.Children.Add(button);
                }
            }
        }

        // Event handler for toolbox button click to add action to workspace
        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Type actionType)
            {
                if (Activator.CreateInstance(actionType) is IAutomationAction action)
                {
                    // Inject the window selection service if needed
                    if (action is InputTextAction inputTextAction)
                    {
                        inputTextAction.SetWindowSelectionService(_windowSelectionService);
                    }

                    AddActionToWorkspace(action);
                }
            }
        }

        // Method to add action to the workspace dynamically
        private void AddActionToWorkspace(IAutomationAction action)
        {
            var actionBlock = new TextBlock
            {
                Text = action.Name,
                Background = Brushes.LightBlue,
                Width = 120,
                Height = 30,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            actionBlock.Tag = action;
            actionBlock.MouseMove += Action_MouseMove;
            actionBlock.MouseLeftButtonUp += (s, e) => ShowProperties(action);

            Canvas.SetLeft(actionBlock, 50); // Example position
            Canvas.SetTop(actionBlock, 50);  // Example position
            WorkspaceCanvas.Children.Add(actionBlock);
        }

        // Implementing drag-and-drop functionality
        private void Action_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var textBlock = sender as TextBlock;
                if (textBlock != null)
                {
                    DragDrop.DoDragDrop(textBlock, new DataObject(typeof(TextBlock), textBlock), DragDropEffects.Move);
                }
            }
        }

        // Handles the drop event on the workspace to position action blocks
        private void WorkspaceCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TextBlock)))
            {
                var actionBlock = e.Data.GetData(typeof(TextBlock)) as TextBlock;
                var position = e.GetPosition(WorkspaceCanvas);
                Canvas.SetLeft(actionBlock, position.X);
                Canvas.SetTop(actionBlock, position.Y);

                if (!WorkspaceCanvas.Children.Contains(actionBlock))
                {
                    WorkspaceCanvas.Children.Add(actionBlock);
                }
            }
        }

        // Handles the drag-over event to show that dropping is allowed
        private void WorkspaceCanvas_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        // Show properties of selected action dynamically
        // Show properties of selected action dynamically
        // Show properties of selected action dynamically
        private void ShowProperties(IAutomationAction action)
        {
            selectedAction = action;
            PropertiesPanelContent.Children.Clear();

            var properties = action.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    TextBlock label = new TextBlock { Text = $"{property.Name}:", Margin = new Thickness(5) };

                    if (property.Name == "SelectedWindow" && action is InputTextAction inputTextAction)
                    {
                        TextBox textBox = new TextBox
                        {
                            Text = inputTextAction.SelectedWindow?.Title ?? "Click to select a window...",
                            Margin = new Thickness(5),
                            Width = 200,
                            IsReadOnly = true // Make the textbox read-only
                        };

                        // Attach the event handler to open the WindowSelectionDialog
                        textBox.GotFocus += (s, e) =>
                        {
                            ConfigureInputTextAction(inputTextAction); // Open the dialog on focus
                            textBox.Text = inputTextAction.SelectedWindow?.Title ?? "No window selected";
                        };

                        PropertiesPanelContent.Children.Add(label);
                        PropertiesPanelContent.Children.Add(textBox);
                    }
                    else
                    {
                        TextBox textBox = new TextBox
                        {
                            Text = property.GetValue(action)?.ToString(),
                            Margin = new Thickness(5),
                            Width = 200
                        };

                        textBox.TextChanged += (s, e) =>
                        {
                            try
                            {
                                var value = Convert.ChangeType(textBox.Text, property.PropertyType);
                                property.SetValue(action, value);

                                if (property.Name == "Name")
                                {
                                    UpdateActionName(action); // Update the action name in the UI
                                    UpdateExecutionPanel(); // Ensure the execution list reflects the new name
                                }
                                else if (property.Name == "ApplicationPath" && action is OpenApplicationAction openAppAction)
                                {
                                    UpdateExePathDisplay(openAppAction); // Update the display for selected EXE path
                                }
                            }
                            catch { }
                        };

                        PropertiesPanelContent.Children.Add(label);
                        PropertiesPanelContent.Children.Add(textBox);
                    }
                }

                // Special handling for ApplicationPath property
                if (property.Name == "ApplicationPath" && action is OpenApplicationAction)
                {
                    Button browseButton = new Button
                    {
                        Content = "Browse...",
                        Margin = new Thickness(5)
                    };
                    browseButton.Click += (s, e) =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog
                        {
                            Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*"
                        };
                        if (openFileDialog.ShowDialog() == true)
                        {
                            property.SetValue(action, openFileDialog.FileName);
                            UpdateExePathDisplay(action as OpenApplicationAction);
                        }
                    };

                    PropertiesPanelContent.Children.Add(browseButton);
                }
            }

            AddToWorkflowButton.Visibility = Visibility.Visible;
        }

        private void AddToWorkflowButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAction != null && !automationActions.Contains(selectedAction))
            {
                automationActions.Add(selectedAction);
                UpdateExecutionPanel(); // Refresh the execution panel to reflect the new action
            }
        }

        // Update the text displayed in the action block
        private void UpdateActionName(IAutomationAction action)
        {
            foreach (var item in WorkspaceCanvas.Children)
            {
                if (item is TextBlock textBlock && textBlock.Tag == action)
                {
                    textBlock.Text = action.Name;
                }
            }
        }

        // Update the Execution Panel to show current workflow order
        private void UpdateExecutionPanel()
        {
            ExecutionListBox.ItemsSource = null;
            ExecutionListBox.ItemsSource = automationActions;
        }

        // Method to visually display the selected EXE path in the properties panel
        private void UpdateExePathDisplay(OpenApplicationAction action)
        {
            foreach (var item in PropertiesPanelContent.Children)
            {
                if (item is TextBox textBox && textBox.Text == action.ApplicationPath)
                {
                    textBox.Text = action.ApplicationPath;
                    break;
                }
            }
        }

        // Remove action from workflow
        private void RemoveActionFromWorkflow(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string actionName)
            {
                var actionToRemove = automationActions.FirstOrDefault(a => a.Name == actionName);
                if (actionToRemove != null)
                {
                    automationActions.Remove(actionToRemove);
                    UpdateExecutionPanel();
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAction = ExecutionListBox.SelectedItem as IAutomationAction;
            if (selectedAction != null)
            {
                automationActions.Remove(selectedAction);
                UpdateExecutionPanel(); // Refresh the list after removing an action
            }
        }

        // Search box text changed event handler
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = SearchTextBox.Text.ToLower();
            var filteredActions = availableActions.Where(type =>
                type.Name.ToLower().Contains(searchQuery) ||
                (Activator.CreateInstance(type) as IAutomationAction)?.Name.ToLower().Contains(searchQuery) == true);
            DisplayActions(filteredActions);
        }

        // Execute the workflow
          private void RunWorkflow(object sender, RoutedEventArgs e)
        {
            foreach (var action in automationActions)
            {
                try
                {
                    ExecutionListBox.SelectedItem = action;
                    // Show window selection dialog if needed
                    if (action is InputTextAction inputTextAction)
                    {
                        ConfigureInputTextAction(inputTextAction);
                    }
                    action.Execute();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error executing {action.Name}: {ex.Message}");
                }
            }
        }
    }
}
