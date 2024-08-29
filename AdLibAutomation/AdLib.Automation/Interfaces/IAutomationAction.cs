using AdLib.Common.Interfaces;
using System;

namespace AdLib.Automation.Interfaces
{
    public delegate void ActionCompletedHandler(object sender, EventArgs e);

    public interface IAutomationAction
    {
        string Name { get; }
        void Execute();
        bool Validate();              // Ensures the action is correctly configured
        string Description { get; }   // Provides a description of the action
        event ActionCompletedHandler OnActionCompleted; // Triggers when the action is completed
        void Configure(IWindowSelectionService windowSelectionService); // Allows configuration of the action before execution
    }
}