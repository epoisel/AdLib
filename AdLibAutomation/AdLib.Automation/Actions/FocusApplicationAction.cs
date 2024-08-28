using System;
using System.Diagnostics;
using System.Windows;
using AdLib.Automation.Utilities;
using AdLib.Automation.Interfaces;

namespace AdLib.Automation.Actions
{
    public class FocusApplicationAction : IAutomationAction
    {
        public string Name { get; set; } = "Focus Application";
        public string ApplicationName { get; set; }

        // Implement the Description property
        public string Description => "Brings the specified application window to the foreground.";

        // Implement the OnActionCompleted event
        public event ActionCompletedHandler OnActionCompleted;

        public void Execute()
        {
            if (Validate())
            {
                try
                {
                    Process[] processes = Process.GetProcessesByName(ApplicationName);
                    if (processes.Length > 0)
                    {
                        NativeMethods.SetForegroundWindow(processes[0].MainWindowHandle);

                        // Raise the OnActionCompleted event after action execution
                        RaiseOnActionCompleted();
                    }
                    else
                    {
                        MessageBox.Show($"The application {ApplicationName} is not running.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to focus on application: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Application name is not set.");
            }
        }

        // Implement the Validate method to check if ApplicationName is set
        public bool Validate()
        {
            return !string.IsNullOrEmpty(ApplicationName);
        }

        // Method to raise the OnActionCompleted event
        protected virtual void RaiseOnActionCompleted()
        {
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
