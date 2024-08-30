// File: AdLib.Common/Interfaces/IActionManager.cs
using System;
using System.Collections.Generic;

namespace AdLib.common.Interfaces
{
    public interface IActionManager
    {
        List<Type> GetAvailableActions();
    }
}
