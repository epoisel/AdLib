using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using AdLib.Contracts.Interfaces;

namespace AdLib.Engine.Services
{
    public class AdLibPluginLoader
    {
        private readonly string _pluginDirectory;
        private readonly List<IAutomationAction> _loadedActions;

        public AdLibPluginLoader(string pluginDirectory)
        {
            _pluginDirectory = pluginDirectory;
            _loadedActions = new List<IAutomationAction>();
        }

        public IEnumerable<IAutomationAction> LoadActions()
        {
            if (!Directory.Exists(_pluginDirectory))
            {
                Debug.WriteLine($"Plugin directory {_pluginDirectory} does not exist.");
                return Enumerable.Empty<IAutomationAction>();
            }

            Console.WriteLine($"Plugin directory {_pluginDirectory} exists.");

            var pluginFiles = Directory.GetFiles(_pluginDirectory, "*.dll");
            if (pluginFiles.Length == 0)
            {
                Debug.WriteLine("No plugin DLLs found in the directory.");
                return Enumerable.Empty<IAutomationAction>();
            }

            Console.WriteLine($"Found {pluginFiles.Length} plugin files in the directory.");

            foreach (var pluginFile in pluginFiles)
            {
                Debug.WriteLine($"Attempting to load plugin file: {pluginFile}");
                try
                {
                    var assembly = Assembly.LoadFrom(pluginFile);

                    var actionTypes = assembly.GetTypes()
                        .Where(t => typeof(IAutomationAction).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in actionTypes)
                    {
                        var action = (IAutomationAction)Activator.CreateInstance(type);
                        _loadedActions.Add(action);
                        Debug.WriteLine($"Loaded action from plugin: {action.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to load actions from {pluginFile}: {ex.Message}");
                }
            }

            return _loadedActions;
        }

    }
}
