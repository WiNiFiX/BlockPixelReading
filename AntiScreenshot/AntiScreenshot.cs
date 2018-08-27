using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AntiScreenshots
{
    public static class AntiScreenshot
    {
        [Flags]
        public enum CompositionAction : uint
        {
            DWM_EC_DISABLECOMPOSITION = 0,
            DWM_EC_ENABLECOMPOSITION = 1
        }

        public enum DisplayAffinity : uint
        {
            None = 0,
            Monitor = 1
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.None, ExactSpelling = false, PreserveSig = false)]
        internal static extern void DwmEnableComposition(CompositionAction uCompositionAction);

        public static bool ProtectAgainstScreenshots(IntPtr windowsHandle)
        {
            try
            {
                if (windowsHandle != IntPtr.Zero)
                {
                    DwmEnableComposition(CompositionAction.DWM_EC_ENABLECOMPOSITION);
                    return User32.SetWindowDisplayAffinity(windowsHandle, DisplayAffinity.Monitor);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            return false;
        }
    }
}