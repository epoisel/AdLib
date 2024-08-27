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

        public void Execute()
        {
            if (string.IsNullOrEmpty(ApplicationName))
            {
                MessageBox.Show("No application specified to focus.");
                return;
            }

            Process[] processes = Process.GetProcessesByName(ApplicationName);
            if (processes.Length > 0)
            {
                NativeMethods.SetForegroundWindow(processes[0].MainWindowHandle);
            }
            else
            {
                MessageBox.Show($"The application {ApplicationName} is not running.");
            }
        }
    }
}
