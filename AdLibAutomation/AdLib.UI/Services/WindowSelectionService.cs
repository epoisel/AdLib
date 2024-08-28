using System.Collections.Generic;
using AdLib.Common.Utilities;
using AdLib.Common.Interfaces;


namespace AdLib.UI.Services
{
    public class WindowSelectionService : IWindowSelectionService
    {
        public WindowInfo ShowWindowSelectionDialog(List<WindowInfo> windows)
        {
            var windowSelectionDialog = new WindowSelectionDialog(windows);

            if (windowSelectionDialog.ShowDialog() == true)
            {
                return windowSelectionDialog.SelectedWindow;
            }

            return null; // No selection made or dialog was canceled
        }
    }
}
