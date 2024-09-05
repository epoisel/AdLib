using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdLib.Contracts.Interfaces;

namespace AdLib.Common.Factories
{
    public class ActionFactory : IActionFactory
    {
        // Since we're using plugins, we don't manually register actions here
        // This factory can be simplified to create instances dynamically as needed

        public IAutomationAction CreateAction(string actionTypeName)
        {
            // Use reflection to find the type based on its name and create it dynamically
            var actionType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(t => t.Name == actionTypeName && typeof(IAutomationAction).IsAssignableFrom(t));

            if (actionType != null)
            {
                return (IAutomationAction)Activator.CreateInstance(actionType);
            }

            Console.WriteLine($"Action type {actionTypeName} is not recognized.");
            throw new ArgumentException($"Action type {actionTypeName} is not recognized.");
        }
    }
}
