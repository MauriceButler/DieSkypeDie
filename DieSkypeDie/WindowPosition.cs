namespace DieSkypeDie
{
    using System;
    using System.Linq;

    public class WindowPosition
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Width { get; set; }

        public WindowPosition()
        {
        }

        internal WindowPosition(NativeMethods.TitleBarInfo pti, IntPtr wnd)
        {

            var rect = new NativeMethods.Rect();
            NativeMethods.GetWindowRect(wnd, ref rect);

            this.Width = rect.right - rect.left;
            this.Left = pti.rcTitleBar.left;
            this.Top = pti.rcTitleBar.top;
            this.Right = pti.rcTitleBar.right;
            this.Bottom = pti.rcTitleBar.bottom;
        }
    }
}