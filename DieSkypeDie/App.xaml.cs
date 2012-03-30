using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace DieSkypeDie
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal System.Windows.Forms.NotifyIcon notify;

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs"/> that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            if (this.notify != null)
            {
                this.notify.Dispose();
            }

            base.OnExit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.InitializeNotifyIcon();
        }

        /// <summary>
        /// Initializes the notify icon.
        /// </summary>
        private void InitializeNotifyIcon()
        {
            this.notify = new System.Windows.Forms.NotifyIcon();
            this.notify.Text = "Die Skype Die";
            this.notify.Icon = DieSkypeDie.Properties.Resources.DSD;
            this.notify.Visible = true;
            this.notify.ContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[]
            {
                new System.Windows.Forms.MenuItem(string.Format("{0} Kill Button", "Add"),(s, e) => ((MainWindow)this.MainWindow).ToggleButton(s, new RoutedEventArgs())),
                new System.Windows.Forms.MenuItem("Close", (s, e) => this.Shutdown())
            });
        }
    }
}
