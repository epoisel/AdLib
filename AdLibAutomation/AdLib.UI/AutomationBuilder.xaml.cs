using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AdLib.UI
{
    public partial class AutomationBuilder : Window
    {
        private List<IAutomationAction> automationActions = new List<IAutomationAction>();

        public AutomationBuilder()
        {
            InitializeComponent();
        }

        // Event handler for adding an Open Application action
        private void AddOpenApplicationAction(object sender, RoutedEventArgs e)
        {
            var openAppAction = new OpenApplicationAction();
            var actionBlock = new TextBlock
            {
                Text = openAppAction.Name,
                Background = Brushes.LightBlue,
                Width = 120,
                Height = 30,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            actionBlock.Tag = openAppAction;
            automationActions.Add(openAppAction); // Add action to the list

            Canvas.SetLeft(actionBlock, 50); // Example position
            Canvas.SetTop(actionBlock, 50);  // Example position
            WorkspaceCanvas.Children.Add(actionBlock);

            actionBlock.MouseMove += Action_MouseMove;
            actionBlock.MouseLeftButtonUp += (s, e) => ShowProperties(openAppAction);

            UpdateExecutionPanel();
        }

        // Event handler for adding an Input Text action
        private void AddInputTextAction(object sender, RoutedEventArgs e)
        {
            var inputTextAction = new InputTextAction();
            var actionBlock = new TextBlock
            {
                Text = inputTextAction.Name,
                Background = Brushes.LightGreen,
                Width = 120,
                Height = 30,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            actionBlock.Tag = inputTextAction;
            automationActions.Add(inputTextAction); // Add action to the list

            Canvas.SetLeft(actionBlock, 50); // Example position
            Canvas.SetTop(actionBlock, 100); // Example position
            WorkspaceCanvas.Children.Add(actionBlock);

            actionBlock.MouseMove += Action_MouseMove;
            actionBlock.MouseLeftButtonUp += (s, e) => ShowProperties(inputTextAction);

            UpdateExecutionPanel();
        }

        // Event handler for adding a Click Button action
        private void AddClickButtonAction(object sender, RoutedEventArgs e)
        {
            var clickButtonAction = new ClickButtonAction();
            var actionBlock = new TextBlock
            {
                Text = clickButtonAction.Name,
                Background = Brushes.Orange,
                Width = 120,
                Height = 30,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            actionBlock.Tag = clickButtonAction;
            automationActions.Add(clickButtonAction); // Add action to the list

            Canvas.SetLeft(actionBlock, 50); // Example position
            Canvas.SetTop(actionBlock, 150); // Example position
            WorkspaceCanvas.Children.Add(actionBlock);

            actionBlock.MouseMove += Action_MouseMove;
            actionBlock.MouseLeftButtonUp += (s, e) => ShowProperties(clickButtonAction);

            UpdateExecutionPanel();
        }

        // Update the Execution Panel ListBox
        private void UpdateExecutionPanel()
        {
            ExecutionListBox.Items.Clear();
            foreach (var action in automationActions)
            {
                ExecutionListBox.Items.Add(action.Name);
            }
        }

        // Method to show properties for a selected action
        private void ShowProperties(IAutomationAction action)
        {
            // Assuming you have a StackPanel named propertiesPanel in XAML to hold dynamic content
            StackPanel propertiesPanel = (StackPanel)this.FindName("PropertiesPanelContent");
            propertiesPanel.Children.Clear(); // Clear previous properties

            if (action is OpenApplicationAction openAppAction)
            {
                TextBlock pathLabel = new TextBlock
                {
                    Text = "Application Path:",
                    Margin = new Thickness(5)
                };
                TextBox pathTextBox = new TextBox
                {
                    Text = openAppAction.ApplicationPath,
                    Margin = new Thickness(5),
                    Width = 200
                };
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
                        pathTextBox.Text = openFileDialog.FileName;
                        openAppAction.ApplicationPath = openFileDialog.FileName;
                    }
                };

                pathTextBox.TextChanged += (s, e) => openAppAction.ApplicationPath = pathTextBox.Text;

                propertiesPanel.Children.Add(pathLabel);
                propertiesPanel.Children.Add(pathTextBox);
                propertiesPanel.Children.Add(browseButton);
            }
             else if (action is InputTextAction inputTextAction)
                    {
                        TextBlock inputLabel = new TextBlock
                        {
                            Text = "Text to Input:",
                            Margin = new Thickness(5)
                        };
                        TextBox inputTextBox = new TextBox
                        {
                            Text = inputTextAction.TextToInput,
                            Margin = new Thickness(5),
                            Width = 200
                        };

                        inputTextBox.TextChanged += (s, e) => inputTextAction.TextToInput = inputTextBox.Text;

                        propertiesPanel.Children.Add(inputLabel);
                        propertiesPanel.Children.Add(inputTextBox);
                    }
            // Add similar handling for other action types if needed
        }

        // Run the workflow by executing all actions in sequence
        private void RunWorkflow(object sender, RoutedEventArgs e)
        {
            foreach (var action in automationActions)
            {
                try
                {
                    ExecutionListBox.SelectedItem = action.Name; // Highlight the current action
                    action.Execute();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error executing {action.Name}: {ex.Message}");
                }
            }
        }

        // Method to enable dragging of actions on the workspace canvas
        private void Action_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var textBlock = sender as TextBlock;
                if (textBlock != null)
                {
                    var data = new DataObject(typeof(TextBlock), textBlock);
                    DragDrop.DoDragDrop(textBlock, data, DragDropEffects.Move);
                }
            }
        }

        // Handles the drop event on the workspace
        private void WorkspaceCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TextBlock)))
            {
                var action = e.Data.GetData(typeof(TextBlock)) as TextBlock;
                var position = e.GetPosition(WorkspaceCanvas);
                Canvas.SetLeft(action, position.X);
                Canvas.SetTop(action, position.Y);

                if (!WorkspaceCanvas.Children.Contains(action))
                {
                    WorkspaceCanvas.Children.Add(action);
                }
            }
        }

        // Handles the drag-over event to show that dropping is allowed
        private void WorkspaceCanvas_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }
    }

    // Example of base interface for actions
    public interface IAutomationAction
    {
        string Name { get; set; }
        void Execute();
    }

    // Example action class for opening an application
    public class OpenApplicationAction : IAutomationAction
    {
        public string Name { get; set; } = "Open Application";
        public string ApplicationPath { get; set; }

        public void Execute()
        {
            if (!string.IsNullOrEmpty(ApplicationPath))
            {
                System.Diagnostics.Process.Start(ApplicationPath);
            }
            else
            {
                MessageBox.Show("Application path is not set.");
            }
        }
    }

    // Example action class for inputting text
    public class InputTextAction : IAutomationAction
    {
        public string Name { get; set; } = "Input Text";
        public string TextToInput { get; set; }

        public void Execute()
        {
            // Simulate text input (placeholder for real implementation)
            MessageBox.Show($"Inputting text: {TextToInput}");
        }
    }

    // Example action class for clicking a button
    public class ClickButtonAction : IAutomationAction
    {
        public string Name { get; set; } = "Click Button";

        public void Execute()
        {
            // Simulate button click (placeholder for real implementation)
            MessageBox.Show("Button clicked!");
        }
    }
}