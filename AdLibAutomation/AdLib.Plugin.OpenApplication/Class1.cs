using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdLib.Contracts.Interfaces;
using Microsoft.Win32;
using AdLib.Contracts.ViewModels;

namespace AdLib.Plugin.OpenApplication
{
    public class OpenApplicationAction : IAutomationAction
    {
        private string _applicationPath;
        private string _actionName = "Open Application";

        public string Name => _actionName;
        public string Description => "Opens a specified application.";

        public void Execute()
        {
            if (string.IsNullOrEmpty(_applicationPath))
            {
                throw new InvalidOperationException("Application path is not set.");
            }

            Process.Start(_applicationPath);
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(_applicationPath))
            {
                throw new InvalidOperationException("Application path is required.");
            }
        }

        // Returns a list of properties for UI binding
        public IEnumerable<ActionPropertyViewModel> GetProperties()
        {
            return new List<ActionPropertyViewModel>
            {
                new ActionPropertyViewModel
                {
                    PropertyName = "Action Name",
                    PropertyType = typeof(string).FullName,
                    PropertyValue = _actionName,
                    SetProperty = value => _actionName = (string)value // Allows the UI to modify the action name
                },
                new ActionPropertyViewModel
                {
                    PropertyName = "Application Path",
                    PropertyType = typeof(string).FullName,
                    PropertyValue = _applicationPath,
                    SetProperty = value => _applicationPath = (string)value, // Allows the UI to modify the app path
                    BrowseAction = BrowseForExecutable // Allows file browsing for an executable path
                }
            };
        }

        private void BrowseForExecutable()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Executable files (*.exe)|*.exe"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _applicationPath = openFileDialog.FileName;
            }
        }

        public void Configure(IServiceProvider serviceProvider)
        {
            // Optional: Use serviceProvider to resolve dependencies
        }
    }
}
