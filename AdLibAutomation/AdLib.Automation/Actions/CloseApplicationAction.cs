// CloseApplicationAction.cs
using System;
using System.Diagnostics;
using System.Windows; // For MessageBox
using AdLib.Automation.Interfaces;
using AdLib.common.Interfaces;
using AdLib.Common.Interfaces;

namespace AdLib.Automation.Actions
{
    public class CloseApplicationAction : IAutomationAction, IConfigurableAction
    {
        public string Name { get; set; } = "Close Application";
        public string ProcessName { get; set; }

        public string Description => "Closes a specified application by its process name.";

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

        public bool Validate()
        {
            return !string.IsNullOrEmpty(ProcessName);
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
