// File: AdLib.Automation/Actions/OpenApplication.cs

using System;
using System.Diagnostics;
using System.Windows; // For MessageBox
using AdLib.Automation.Interfaces;

namespace AdLib.Automation.Actions
{
    public class OpenApplicationAction : IAutomationAction
    {
        public string Name { get; set; } = "Open Application";
        public string ApplicationPath { get; set; }

        // Implement the Description property
        public string Description => "Opens a specified application by its file path.";

        // Implement the OnActionCompleted event
        public event ActionCompletedHandler OnActionCompleted;

        // Implement the Validate method to check if ApplicationPath is set
        public bool Validate()
        {
            return !string.IsNullOrEmpty(ApplicationPath);
        }

        // Implement the Execute method to perform the application opening
        public void Execute()
        {
            if (Validate())
            {
                try
                {
                    Process.Start(ApplicationPath);

                    // Raise the OnActionCompleted event after action execution
                    RaiseOnActionCompleted();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open application: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Application path is not set.");
            }
        }

        // Method to raise the OnActionCompleted event
        protected virtual void RaiseOnActionCompleted()
        {
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
