using System.Windows;
using AdLib.Automation.Interfaces;

namespace AdLib.Automation.Actions
{
    public class ClickButtonAction : IAutomationAction
    {
        public string Name { get; set; } = "Click Button";

        public void Execute()
        {
            MessageBox.Show("Button clicked!");
        }
    }
}