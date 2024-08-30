// File: AdLib.common/Services/ProcessService.cs

using System;
using System.Diagnostics;
using AdLib.Common.Interfaces;
//using AdLib.common.Utilities;

namespace AdLib.Common.Services
{
    public class ProcessService : IProcessService
    {
        public Process[] GetProcessesByName(string name)
        {
            return Process.GetProcessesByName(name);
        }

        public void SetForegroundWindow(IntPtr hWnd)
        {
            NativeMethods.SetForegroundWindow(hWnd); // Using the existing NativeMethods utility
        }

        public void CloseProcess(Process process)
        {
            process.CloseMainWindow();
            process.Close();
        }
    }
}
