using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// File: AdLib.Automation/Interfaces/IAutomationAction.cs

namespace AdLib.Automation.Interfaces
{
    public interface IAutomationAction
    {
        string Name { get; set; }
        void Execute();
    }
}

