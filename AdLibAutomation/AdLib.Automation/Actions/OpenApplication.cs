// OpenApplicationAction.cs
using System;
using System.Diagnostics;
using System.Windows; // For MessageBox
using AdLib.Automation.Interfaces;
using AdLib.common.Interfaces;
using AdLib.Common.Interfaces;

namespace AdLib.Automation.Actions
{
    public class OpenApplicationAction : IAutomationAction, IConfigurableAction
    {
        public string Name { get; set; } = "Open Application";
        public string ApplicationPath { get; set; }

        public string Description => "Opens a specified application by its file path.";

        public event ActionCompletedHandler OnActionCompleted;

        public void Execute()
        {
            if (Validate())
            {
                try
                {
                    Process.Start(ApplicationPath);
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

        public bool Validate()
        {
            return !string.IsNullOrEmpty(ApplicationPath);
        }

        public void Configure(IWindowSelectionService windowSelectionService)
        {
            // Add configuration logic if necessary
        }

        protected virtual void RaiseOnActionCompleted()
        {
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
