// File: AdLib.Automation/Utilities/NativeMethods.cs

using System;
using System.Runtime.InteropServices;

namespace AdLib.Automation.Utilities
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
