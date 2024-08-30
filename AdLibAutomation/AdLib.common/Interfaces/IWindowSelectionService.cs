using AdLib.Common.Utilities;

namespace AdLib.Common.Interfaces
{
    public interface IWindowSelectionService
    {
        WindowInfo ShowWindowSelectionDialog(List<WindowInfo> windows);
        void SelectWindow(); // Ensure this method is defined
    }
}
