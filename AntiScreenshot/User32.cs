using System;
using System.Runtime.InteropServices;

namespace AntiScreenshots
{
    public static class User32
    {
        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        internal static extern bool SetWindowDisplayAffinity(IntPtr hwnd, AntiScreenshot.DisplayAffinity affinity);

        internal delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);
    }
}
