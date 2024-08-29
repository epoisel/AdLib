// IConfigurableAction.cs
using AdLib.Common.Interfaces;

namespace AdLib.Automation.Interfaces
{
    public interface IConfigurableAction
    {
        void Configure(IWindowSelectionService windowSelectionService);
    }
}

// IRequiresFilePathAction.cs
namespace AdLib.Automation.Interfaces
{
    public interface IRequiresFilePathAction
    {
        string FilePath { get; set; }
    }
}
