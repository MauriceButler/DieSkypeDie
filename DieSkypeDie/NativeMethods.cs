using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DieSkypeDie
{
    static partial class NativeMethods
    {
        internal static int GWL_HWNDPARENT = -8;
        internal static int WINEVENT_OUTOFCONTEXT = 0;

        internal delegate void WinEventProc(IntPtr winEventHookHandle, AccessibleEvents accEvent, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        internal static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool GetTitleBarInfo(IntPtr hwnd, ref TitleBarInfo pti);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr wnd, ref Rect rect);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        internal static extern int SetWindowLong32(IntPtr windowHandle, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWinEventHook(AccessibleEvents eventMin, AccessibleEvents eventMax, IntPtr eventHookAssemblyHandle, WinEventProc eventHookHandle, uint processId, uint threadId, int parameterFlags);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool UnhookWinEvent(IntPtr eventHookHandle);

    }

    

   
}