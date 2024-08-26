using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdLib.Common.Plugins
{
    public class TestPlugin : IAutomationPlugin
    {
        public string Name => "Test Plugin";

        public void Initialize()
        {
            Console.WriteLine("Test Plugin Initialized");
        }

        public void Execute()
        {
            Console.WriteLine("Test Plugin Executing");
        }

        public void Cleanup()
        {
            Console.WriteLine("Test Plugin Cleanup");
        }
    }
}
