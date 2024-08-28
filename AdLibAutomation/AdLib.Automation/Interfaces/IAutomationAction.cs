using System;

namespace AdLib.Automation.Interfaces
{
    public delegate void ActionCompletedHandler(object sender, EventArgs e);

    public interface IAutomationAction
    {
        string Name { get; }
        void Execute();
        bool Validate();              // Validates if the action is configured correctly
        string Description { get; }   // Provides a description of the action
        event ActionCompletedHandler OnActionCompleted; // Event that triggers when the action is completed
    }
}