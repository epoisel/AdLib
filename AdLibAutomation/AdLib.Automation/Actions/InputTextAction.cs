using System.Windows;
using AdLib.Automation.Interfaces;

namespace AdLib.Automation.Actions
{
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
}
