using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdLib.Contracts.Interfaces;
using AdLib.Engine.Services;  // For plugin loading

namespace AdLib.Common.Services
{
    public class ActionManager : IActionManager
    {
        private readonly Dictionary<Type, IAutomationAction> _registeredActions;
        private readonly AdLibPluginLoader _pluginLoader;

        // Constructor takes the plugin directory as input
        public ActionManager(string pluginDirectory)
        {
            Debug.WriteLine("ActionManager initialized.");

            _registeredActions = new Dictionary<Type, IAutomationAction>();

            // Initialize the plugin loader to dynamically load plugin actions
            _pluginLoader = new AdLibPluginLoader(pluginDirectory);

            // Register plugin actions
            RegisterPluginActions();
        }

        private void RegisterPluginActions()
        {
            var pluginActions = _pluginLoader.LoadActions();
            foreach (var action in pluginActions)
            {
                RegisterAction(action);
                Debug.WriteLine($"Action {action.Name} loaded and registered.");
            }
        }

        public void RegisterAction(IAutomationAction action)
        {
            var type = action.GetType();
            if (!_registeredActions.ContainsKey(type))
            {
                _registeredActions[type] = action;
                Debug.WriteLine($"Action registered: {action.Name}");
            }
            else
            {
                Debug.WriteLine($"Action already registered: {action.Name}");
            }
        }

        public IAutomationAction GetAction(string actionTypeName)
        {
            // Retrieve the action by its name
            var action = _registeredActions.Values.FirstOrDefault(a => a.GetType().Name == actionTypeName);
            if (action != null)
            {
                return action;
            }

            throw new KeyNotFoundException($"Action of type {actionTypeName} is not registered.");
        }

        public List<IAutomationAction> GetAvailableActions()
        {
            return new List<IAutomationAction>(_registeredActions.Values);
        }
    }
}
