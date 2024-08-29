// ClickButtonAction.cs
using System;
using System.Windows; // For MessageBox (if needed)
using AdLib.Automation.Interfaces;
using AdLib.Common.Interfaces; // Assuming the click service is defined here

namespace AdLib.Automation.Actions
{
    public class ClickButtonAction : IAutomationAction, IConfigurableAction
    {
        private readonly IButtonClickService _buttonClickService;

        public string Name { get; set; } = "Click Button";
        public string ButtonIdentifier { get; set; } // Identifier for the button (could be an ID, name, etc.)

        public string Description => "Simulates a click on a specified button in the user interface.";

        public event ActionCompletedHandler OnActionCompleted;

        public ClickButtonAction(IButtonClickService buttonClickService)
        {
            _buttonClickService = buttonClickService ?? throw new ArgumentNullException(nameof(buttonClickService));
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(ButtonIdentifier);
        }

        public void Execute()
        {
            if (Validate())
            {
                try
                {
                    _buttonClickService.ClickButton(ButtonIdentifier);
                    RaiseOnActionCompleted();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to click button: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Button identifier is not set or is invalid.");
            }
        }

        // Implement the Configure method to meet the interface contract
        public void Configure(IWindowSelectionService windowSelectionService)
        {
            // Configuration logic if required
        }

        protected virtual void RaiseOnActionCompleted()
        {
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
