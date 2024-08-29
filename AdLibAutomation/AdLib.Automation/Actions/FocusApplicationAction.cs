// FocusApplicationAction.cs
using System;
using System.Diagnostics;
using System.Windows;
using AdLib.Automation.Interfaces;
using AdLib.Common.Interfaces;

namespace AdLib.Automation.Actions
{
    public class FocusApplicationAction : IAutomationAction, IConfigurableAction
    {
        private readonly IProcessService _processService;

        public string Name { get; set; } = "Focus Application";
        public string ApplicationName { get; set; }

        public string Description => "Brings the specified application window to the foreground.";

        public event ActionCompletedHandler OnActionCompleted;

        public FocusApplicationAction(IProcessService processService)
        {
            _processService = processService;
        }

        public void Execute()
        {
            if (Validate())
            {
                try
                {
                    Process[] processes = _processService.GetProcessesByName(ApplicationName);
                    if (processes.Length > 0)
                    {
                        _processService.SetForegroundWindow(processes[0].MainWindowHandle);
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

        public bool Validate()
        {
            return !string.IsNullOrEmpty(ApplicationName);
        }

        public void Configure(IWindowSelectionService windowSelectionService)
        {
            // Configuration logic if necessary
        }

        protected virtual void RaiseOnActionCompleted()
        {
            OnActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
