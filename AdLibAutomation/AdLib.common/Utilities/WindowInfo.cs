// File: AdLib.Automation/Utilities/WindowInfo.cs

using System; // Required for IntPtr type

namespace AdLib.Common.Utilities
{
    public class WindowInfo
    {
        public IntPtr Handle { get; set; } // Handle to the window
        public string Title { get; set; } // Title of the window
        public string ProcessName { get; set; } // Name of the process owning the window

        public override string ToString()
        {
            return $"{Title} - ({ProcessName})";
        }
    }
}
