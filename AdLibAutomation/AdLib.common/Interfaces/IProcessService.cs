// File: AdLib.common/Interfaces/IProcessService.cs

using System.Diagnostics;

namespace AdLib.Common.Interfaces
{
    public interface IProcessService
    {
        Process[] GetProcessesByName(string name);
        void SetForegroundWindow(IntPtr hWnd);
        void CloseProcess(Process process);
    }
}
