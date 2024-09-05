using System.Collections.Generic;
using AdLib.Contracts.ViewModels;

namespace AdLib.Contracts.Interfaces
{
    public interface IAutomationAction
    {
        string Name { get; }
        string Description { get; }

        // Return ActionPropertyViewModel for property binding
        IEnumerable<ActionPropertyViewModel> GetProperties();

        void Execute();
        void Validate();
        void Configure(IServiceProvider serviceProvider);  // Optional for dependency injection
    }
}
