// File: AdLib.Contracts/Interfaces/IActionFactory.cs
using AdLib.Contracts.Interfaces;

namespace AdLib.Contracts.Interfaces
{
    public interface IActionFactory
    {
        IAutomationAction CreateAction(string actionTypeName);  // Create action by its type name
    }
}



