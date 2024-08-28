using System.Collections.Generic;
using AdLib.Common.Utilities;


namespace AdLib.Common.Interfaces
{
    public interface IWindowSelectionService
    {
        WindowInfo ShowWindowSelectionDialog(List<WindowInfo> windows);
    }
}
