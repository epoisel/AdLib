using System;
using System.Diagnostics;
using System.Windows;
using AdLib.Automation.Interfaces;

namespace AdLib.Automation.Actions
{
    public class CloseApplicationAction : IAutomationAction
    {
        public string Name { get; set; } = "Close Application";
        public string ProcessName { get; set; }

        // Implement the Description property
        public string Description => "Closes a specified application by its process name.";

        // Implement the OnActionCompleted event
        public event ActionCompletedHandler OnActionCompleted;

        public void Execute()
        {
            if (Validate())
            {
                try
                {
                    var processes = Process.GetProcessesByName(ProcessName);
                    foreach (var process in processes)
                    {
                        process.CloseMainWindow();
                        process.Close();
                    }

                    // Raise the OnActionCompleted event after action execution
                    RaiseOnActionCompleted();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to close application: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Process name is not set.");
            }
        }

        // Implement the Validate method to check if ProcessName is set
        public bool Validate()
        {
            return !string.IsNullOrEmpty(ProcessName);
        }

        // Method to raise the OnActionCompleted event
        protected virtual void RaiseOnActionCompleted()
        {
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
