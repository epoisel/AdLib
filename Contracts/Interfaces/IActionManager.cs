// File: AdLib.Contracts/Interfaces/IActionManager.cs
using System.Collections.Generic;

namespace AdLib.Contracts.Interfaces
{
    public interface IActionManager
    {
        void RegisterAction(IAutomationAction action);
        IAutomationAction GetAction(string actionTypeName);  // Retrieve action by type name
        List<IAutomationAction> GetAvailableActions();
    }
}


