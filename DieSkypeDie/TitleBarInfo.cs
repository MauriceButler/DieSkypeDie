using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DieSkypeDie
{
    static partial class NativeMethods
    {
        internal struct TitleBarInfo
        {
            public uint cbSize;
            public Rect rcTitleBar;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public AccessibleStates[] rgstate;
        }
    }
}