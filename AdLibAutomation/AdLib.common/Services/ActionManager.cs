// File: AdLib.Common/Services/ActionManager.cs
using System;
using System.Collections.Generic;

namespace AdLib.Common.Services
{
    public class ActionManager : IActionManager
    {
        public List<Type> GetAvailableActions()
        {
            // Return a list of available action types
            // Example: return new List<Type> { typeof(SomeAction), typeof(AnotherAction) };
            return new List<Type>(); // Populate this with your actual action types
        }
    }
}
