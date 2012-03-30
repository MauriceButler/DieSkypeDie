using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DieSkypeDie
{
    /// <summary>
    /// Interaction logic for KillButton.xaml
    /// </summary>
    public partial class KillButton : Window
    {
        public KillButton()
        {
            this.InitializeComponent();
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

        private void KillButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            GetSkypeProcess().CloseMainWindow();
        }
    }
}