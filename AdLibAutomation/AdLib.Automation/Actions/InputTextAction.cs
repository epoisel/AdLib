// InputTextAction.cs
using System;
using System.Windows;
using AdLib.Automation.Interfaces;
using AdLib.common.Interfaces;
using AdLib.common.Utilities;
using AdLib.Common.Interfaces; // For the window selection service
using AdLib.Common.Utilities;  // Assuming NativeMethods is defined here
using InputSimulatorStandard;  // Assuming InputSimulator is being used for text input simulation

namespace AdLib.Automation.Actions
{
    public class InputTextAction : IAutomationAction, IConfigurableAction
    {
        public string Name { get; set; } = "Input Text";
        public string TextToInput { get; set; }
        public WindowInfo SelectedWindow { get; set; }
        private IWindowSelectionService _windowSelectionService;

        public string Description => "Inputs text into the active field of a selected window.";

        public event ActionCompletedHandler OnActionCompleted;

        public InputTextAction(IWindowSelectionService windowSelectionService)
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
                    NativeMethods.SetForegroundWindow(SelectedWindow.Handle);
                    var simulator = new InputSimulator();
                    simulator.Keyboard.TextEntry(TextToInput);
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

        public void Configure(IWindowSelectionService windowSelectionService)
        {
            _windowSelectionService = windowSelectionService;
        }

        protected virtual void RaiseOnActionCompleted()
        {
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
