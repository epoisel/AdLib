// File: AdLib.Automation/Utilities/NativeMethods.cs

using System;
using System.Runtime.InteropServices;

namespace AdLib.common.Utilities
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(nint hWnd);
    }
}
