using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdLib.Common.Plugins
{
    public interface IAutomationPlugin
    {
        string Name { get; }
        void Initialize();
        void Execute();
        void Cleanup();
    }
}

