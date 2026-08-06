using System.Runtime.InteropServices;

namespace SmartEMR.Application.Core;

public class MousePointHelper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Win32Point
    {
        public int X;
        public int Y;
    };

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetCursorPos(ref Win32Point pt);
}
