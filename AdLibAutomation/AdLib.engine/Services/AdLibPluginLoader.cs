using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AdLib.Common.Plugins; // Reference to the common interface namespace

namespace AdLib.Engine.Services
{
    public class AdLibPluginLoader
    {
        private readonly string _pluginDirectory;
        private readonly List<IAutomationPlugin> _loadedPlugins;

        public AdLibPluginLoader(string pluginDirectory)
        {
            _pluginDirectory = pluginDirectory;
            _loadedPlugins = new List<IAutomationPlugin>();
        }

        public IEnumerable<IAutomationPlugin> LoadPlugins()
        {
            if (!Directory.Exists(_pluginDirectory))
            {
                Console.WriteLine($"Plugin directory {_pluginDirectory} does not exist.");
                return Enumerable.Empty<IAutomationPlugin>();
            }

            var pluginFiles = Directory.GetFiles(_pluginDirectory, "*.dll");

            foreach (var pluginFile in pluginFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(pluginFile);
                    var pluginTypes = assembly.GetTypes()
                        .Where(t => typeof(IAutomationPlugin).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in pluginTypes)
                    {
                        var plugin = (IAutomationPlugin)Activator.CreateInstance(type);
                        plugin.Initialize();
                        _loadedPlugins.Add(plugin);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load plugin from {pluginFile}: {ex.Message}");
                }
            }

            return _loadedPlugins;
        }

        public void ExecuteAllPlugins()
        {
            foreach (var plugin in _loadedPlugins)
            {
                try
                {
                    plugin.Execute();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing plugin {plugin.Name}: {ex.Message}");
                }
            }
        }

        public void CleanupAllPlugins()
        {
            foreach (var plugin in _loadedPlugins)
            {
                try
                {
                    plugin.Cleanup();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during cleanup of plugin {plugin.Name}: {ex.Message}");
                }
            }
        }
    }
}
