// File: AdLib.Automation/Actions/OpenApplication.cs

using System;
using System.Diagnostics;
using System.Windows;
using AdLib.Automation.Interfaces;

namespace AdLib.Automation.Actions
{
    public class OpenApplicationAction : IAutomationAction
    {
        public string Name { get; set; } = "Open Application";
        public string ApplicationPath { get; set; }

        public void Execute()
        {
            if (!string.IsNullOrEmpty(ApplicationPath))
            {
                try
                {
                    Process.Start(ApplicationPath);
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
    }
}
