using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace AdLib.Common.Utilities
{
    public static class WindowEnumerator
    {
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public static List<WindowInfo> GetOpenWindows()
        {
            List<WindowInfo> windows = new List<WindowInfo>();

            EnumWindows((hWnd, lParam) =>
            {
                if (IsWindowVisible(hWnd))
                {
                    int length = GetWindowTextLength(hWnd);
                    if (length > 0)
                    {
                        StringBuilder builder = new StringBuilder(length);
                        GetWindowText(hWnd, builder, length + 1);
                        string title = builder.ToString();

                        GetWindowThreadProcessId(hWnd, out uint processId);
                        string processName = Process.GetProcessById((int)processId).ProcessName;

                        windows.Add(new WindowInfo { Handle = hWnd, Title = title, ProcessName = processName });
                    }
                }
                return true;
            }, IntPtr.Zero);

            return windows;
        }
    }
}
