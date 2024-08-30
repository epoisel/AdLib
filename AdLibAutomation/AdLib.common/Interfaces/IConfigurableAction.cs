// IConfigurableAction.cs
using AdLib.Common.Interfaces;

namespace AdLib.common.Interfaces
{
    public interface IConfigurableAction
    {
        void Configure(IWindowSelectionService windowSelectionService);
    }
}

// IRequiresFilePathAction.cs
namespace AdLib.common.Interfaces
{
    public interface IRequiresFilePathAction
    {
        string FilePath { get; set; }
    }
}
