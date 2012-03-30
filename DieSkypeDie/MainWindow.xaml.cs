using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace DieSkypeDie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void Execute();
        private IntPtr skypeWnd = new IntPtr(0);
        private KillButton killButton;
        IntPtr hook;
        WindowPosition position;

        public MainWindow()
        {
            this.InitializeComponent();
            var app = App.Current as App;
            var process = GetSkypeProcess();
            if (process != null)
            {
                ToggleButton(app.notify.ContextMenu.MenuItems[0], new RoutedEventArgs());
            }
        }

        private static Process GetSkypeProcess()
        {
            Process skype = null;

            var skypeProcess = Process.GetProcessesByName("Skype");
            if (skypeProcess.Length > 0)
            {
                skype = skypeProcess[0];
            }

            return skype;
        }

        public void ToggleButton(object sender, RoutedEventArgs e)
        {
            var menutItem = sender as MenuItem;
            if (menutItem != null)
            {
                if (menutItem.Text.StartsWith("Add"))
                {
                    var process = GetSkypeProcess();
                    this.skypeWnd = NativeMethods.FindWindowByCaption(IntPtr.Zero, process.MainWindowTitle);

                    if (!this.skypeWnd.Equals(IntPtr.Zero))
                    {
                        this.GetWindowPosition();
                        this.SetControl();
                        this.AddPositionalEvents();

                        menutItem.Text = menutItem.Text.Replace("Add", "Remove");
                    }
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.DestroyHelper));
                    menutItem.Text = menutItem.Text.Replace("Remove", "Add");
                }
            }
        }

        void GetWindowPosition()
        {
            NativeMethods.TitleBarInfo pti = new NativeMethods.TitleBarInfo();
            pti.cbSize = (uint)Marshal.SizeOf(pti);

            this.position = NativeMethods.GetTitleBarInfo(this.skypeWnd, ref pti) ? (new WindowPosition(pti, this.skypeWnd)) : (new WindowPosition());
        }

        void SetControl()
        {
            if (this.killButton == null)
            {
                this.killButton = new KillButton();
                this.killButton.Show();
            }

            this.killButton.Left = this.position.Left + (this.position.Width / 2);
            this.killButton.Top = this.position.Top -5;

            NativeMethods.SetWindowLong32(NativeMethods.FindWindowByCaption(IntPtr.Zero, this.killButton.Title), NativeMethods.GWL_HWNDPARENT, this.skypeWnd.ToInt32());
        }

        private void AddPositionalEvents()
        {
            Dictionary<AccessibleEvents, NativeMethods.WinEventProc> events = this.InitializeWinEventToHandlerMap();

            NativeMethods.WinEventProc eventHandler =
                new NativeMethods.WinEventProc(events[AccessibleEvents.LocationChange].Invoke);

            //Tell the garbage collector not to move the callback.
            GCHandle.Alloc(eventHandler);

            this.hook = NativeMethods.SetWinEventHook(AccessibleEvents.LocationChange,
                AccessibleEvents.LocationChange, IntPtr.Zero, eventHandler,
                0, 0, NativeMethods.WINEVENT_OUTOFCONTEXT);

            eventHandler = new NativeMethods.WinEventProc(events[AccessibleEvents.Destroy].Invoke);

            //Tell the garbage collector not to move the callback.
            GCHandle.Alloc(eventHandler);

            this.hook = NativeMethods.SetWinEventHook(AccessibleEvents.Destroy,
                AccessibleEvents.LocationChange, IntPtr.Zero, eventHandler,
                0, 0, NativeMethods.WINEVENT_OUTOFCONTEXT);
        }

        private Dictionary<AccessibleEvents, NativeMethods.WinEventProc> InitializeWinEventToHandlerMap()
        {
            Dictionary<AccessibleEvents, NativeMethods.WinEventProc> dictionary = new Dictionary<AccessibleEvents, NativeMethods.WinEventProc>();
            dictionary.Add(AccessibleEvents.LocationChange, new NativeMethods.WinEventProc(this.LocationChangedCallback));
            dictionary.Add(AccessibleEvents.Destroy, new NativeMethods.WinEventProc(this.DestroyCallback));

            return dictionary;
        }

        private void DestroyCallback(IntPtr winEventHookHandle, AccessibleEvents accEvent, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds)
        {
            if (accEvent == AccessibleEvents.Destroy && windowHandle.ToInt32() == this.skypeWnd.ToInt32())
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.DestroyHelper));
            }
        }

        private void DestroyHelper(object state)
        {
            Execute ex = delegate()
            {
                NativeMethods.UnhookWinEvent(this.hook);
                if (this.killButton != null)
                {
                    this.killButton.Close();

                    this.killButton = null;
                }
            };
            this.Dispatcher.Invoke(ex, null);
        }

        private void LocationChangedCallback(IntPtr winEventHookHandle, AccessibleEvents accEvent, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds)
        {
            if (accEvent == AccessibleEvents.LocationChange && windowHandle.ToInt32() == this.skypeWnd.ToInt32())
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.LocationChangedHelper));
            }
        }

        private void LocationChangedHelper(object state)
        {
            Execute ex = delegate()
            {
                this.GetWindowPosition();
                this.SetControl();
            };
            this.Dispatcher.Invoke(ex, null);
        }
    }
}