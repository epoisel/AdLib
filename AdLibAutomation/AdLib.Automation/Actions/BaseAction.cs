using System;
using AdLib.Contracts.Interfaces;

namespace AdLib.Automation.Actions
{
    public abstract class BaseAction : IAutomationAction
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public event EventHandler ActionCompleted;

        // Constructor to initialize common properties
        protected BaseAction(string name, string description)
        {
            Name = name;
            Description = description;
        }

        // Common logic for all actions
        public abstract void Execute();

        public virtual void Configure(IServiceProvider serviceProvider)
        {
            // Default implementation can be overridden by derived classes
        }

        public virtual void Validate()
        {
            // Default validation logic, can be overridden
            if (string.IsNullOrEmpty(Name))
                throw new InvalidOperationException("Action name is not set.");
        }

        protected virtual void OnActionCompleted(EventArgs e)
        {
            ActionCompleted?.Invoke(this, e);
        }

        // Method to return action-specific properties for dynamic properties panel
        public abstract object GetConfiguration();
    }
}
