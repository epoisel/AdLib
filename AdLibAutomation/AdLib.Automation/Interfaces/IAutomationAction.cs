using System;

namespace AdLib.Automation.Interfaces
{
    public delegate void ActionCompletedHandler(object sender, EventArgs e);

    public interface IAutomationAction
    {
        string Name { get; }
        void Execute();
        bool Validate();                // Validates the configuration of the action
        string Description { get; }     // Description of the action
        event ActionCompletedHandler ActionCompleted; // Event for action completion

        // Generic configuration method, can be extended for different needs
        void Configure(IServiceProvider serviceProvider); // Allows configuration of the action before execution
    }
}
