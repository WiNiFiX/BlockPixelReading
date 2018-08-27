//////////////////////////////////////////////////
//                                              //
//   See License.txt for Licensing information  //
//                                              //
//////////////////////////////////////////////////

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AntiScreenshots
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public static class Log
    {
        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;
        private static RichTextBox _rtbLogWindow;
        private static readonly Color _errorColor = Color.Red;
        private static Form _parent;
        private static bool _clearHistory;
        
        private static string lastMessage;

        [ObfuscationAttribute(Exclude = true)]
        public static void Initialize(RichTextBox rtbLogWindow, Form parent, bool clearHistory = true)
        {
            _rtbLogWindow = rtbLogWindow;
            _parent = parent;
            _clearHistory = clearHistory;
        }

        [ObfuscationAttribute(Exclude = true)]
        private static void LogActivityWithoutLineFeedOrTime(string activity, Color c, bool noSound = false)
        {
            _parent.Invoke(
                new Action(() =>
                {
                    InternalWrite(c, activity, true, false);
                    WriteDirectlyToLogFile(activity);
                }));
        }

        [ObfuscationAttribute(Exclude = true)]
        public static void Clear()
        {
            _rtbLogWindow.Clear();
        }

        [ObfuscationAttribute(Exclude = true)]
        public static void DrawHorizontalLine()
        {
            WriteNewLine();
        }

        [ObfuscationAttribute(Exclude = true)]
        public static void WriteNoTime(string activity)
        {
            _parent.Invoke(
                new Action(() =>
                {
                    InternalWrite(Color.Black, activity, true);
                    WriteDirectlyToLogFile(activity);
                }));
        }


        [ObfuscationAttribute(Exclude = true)]
        public static void WriteNoTime(string activity, Color c)
        {
            _parent.Invoke(
                new Action(() =>
                {
                    InternalWrite(c, activity, true);
                    WriteDirectlyToLogFile(activity);
                }));
        }
        
        public static void WriteDirectlyToLogFile(string format, params object[] args)
        {
        }

        public static void Write(string text)
        {
            Write(text, Color.Black);
        }

        public static void Write(string text, Color c)
        {

            if (_parent == null)
            {
                MessageBox.Show("Please ensure you call Log.Initialize()");
                Application.Exit();
            }

            try
            {
                _parent?.Invoke(
                    new Action(() =>
                    {
                        InternalWrite(c, text);
                        WriteDirectlyToLogFile(text);
                    }));
            }
            catch
            {
                // ignored
            }
            lastMessage = text;
        }

        [ObfuscationAttribute(Exclude = true)]
        public static void WriteNewLine()
        {
            _parent.Invoke(
                new Action(() =>
                {
                    InternalWrite(Color.Black, "", true);
                    WriteDirectlyToLogFile("");
                }));
        }

        [ObfuscationAttribute(Exclude = true)]
        public static void Write(Color color, string format, params object[] args)
        {
            _parent.Invoke(
                new Action(() =>
                {
                    InternalWrite(color, string.Format(format, args));
                    WriteDirectlyToLogFile(format, args);
                }));
        }
        
        private static void InternalWrite(Color color, string text, bool noTime = false, bool lineFeed = true)
        {
            try
            {
                var rtb = _rtbLogWindow;

                rtb.SuspendLayout();

                if (rtb.Lines.Length > 2000 && _clearHistory)
                    rtb.Clear();

                if (text.ToLower().Contains("http"))
                {
                    color = Color.Red;
                    text = "[URL Links not permitted in Log Window, please keep these on your settings form.]";
                }

                rtb.SelectionStart = rtb.Text.Length;
                rtb.SelectionLength = 0;

                rtb.SelectionColor = color;
                rtb.AppendText(lineFeed ? $"{text}\r" : $"{text}");

                rtb.ClearUndo();

                rtb.ResumeLayout(false);

                ScrollToBottom(rtb);
            }
            catch
            {
                // ignored
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private static void ScrollToBottom(RichTextBox MyRichTextBox)
        {
            SendMessage(MyRichTextBox.Handle, WM_VSCROLL, (IntPtr) SB_PAGEBOTTOM, IntPtr.Zero);
        }
    }
}