// AdLib.Common.Interfaces/IWindowDialog.cs
using AdLib.Common.Utilities;

namespace AdLib.Common.Interfaces
{
    public interface IWindowDialog
    {
        WindowInfo ShowDialog(List<WindowInfo> windows);
    }
}
