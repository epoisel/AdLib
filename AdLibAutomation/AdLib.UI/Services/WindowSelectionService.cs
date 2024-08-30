using System.Collections.Generic;
using AdLib.Common.Interfaces;
using AdLib.Common.Utilities;

namespace AdLib.UI.Services
{
    public class WindowSelectionService : IWindowSelectionService
    {
        private readonly IWindowDialog _windowDialog;

        public WindowSelectionService(IWindowDialog windowDialog)
        {
            _windowDialog = windowDialog;
        }

        public WindowInfo ShowWindowSelectionDialog(List<WindowInfo> windows)
        {
            return _windowDialog.ShowDialog(windows);
        }

        public void SelectWindow()
        {
            // Implement SelectWindow logic here, or provide a stub implementation
        }
    }
}
