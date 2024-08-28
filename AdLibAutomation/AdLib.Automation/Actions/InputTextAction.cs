using System;
using System.Windows;
using AdLib.Automation.Interfaces;
using AdLib.Automation.Utilities;
using AdLib.Common.Interfaces; // Using the interface from AdLib.Common
using AdLib.Common.Utilities; // Using the shared utilities
using InputSimulatorStandard; // Assuming InputSimulator is being used for text input simulation

namespace AdLib.Automation.Actions
{
    public class InputTextAction : IAutomationAction
    {
        public string Name { get; set; } = "Input Text";
        public string TextToInput { get; set; }
        private IWindowSelectionService _windowSelectionService; // Make this non-readonly to allow setting after construction
        public WindowInfo SelectedWindow { get; set; }

        public string Description => "Inputs text into the active field of a selected window.";

        public event ActionCompletedHandler OnActionCompleted;

        // Default constructor
        public InputTextAction() { }

        // Constructor to inject the window selection service
        public InputTextAction(IWindowSelectionService windowSelectionService)
        {
            _windowSelectionService = windowSelectionService;
        }

        // Method to set the WindowSelectionService if default constructor is used
        public void SetWindowSelectionService(IWindowSelectionService windowSelectionService)
        {
            _windowSelectionService = windowSelectionService;
        }

        public void Execute()
        {
            if (_windowSelectionService == null)
            {
                MessageBox.Show("WindowSelectionService is not set.");
                return;
            }

            if (SelectedWindow == null)
            {
                var windows = WindowEnumerator.GetOpenWindows();
                SelectedWindow = _windowSelectionService.ShowWindowSelectionDialog(windows);
            }

            if (Validate())
            {
                try
                {
                    // Focus the selected window
                    NativeMethods.SetForegroundWindow(SelectedWindow.Handle);

                    // Send the text to the active element using InputSimulator
                    var simulator = new InputSimulator();
                    simulator.Keyboard.TextEntry(TextToInput);

                    // Raise the OnActionCompleted event
                    RaiseOnActionCompleted();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to input text: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No window selected or text to input is not set.");
            }
        }

        public bool Validate()
        {
            return SelectedWindow != null && !string.IsNullOrEmpty(TextToInput);
        }

        protected virtual void RaiseOnActionCompleted()
        {
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}

