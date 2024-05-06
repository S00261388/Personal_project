using System;
using System.Windows;

namespace MailBoxd
{
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            App application = new App();
            application.InitializeComponent();
            application.Run(new MainWindow());  // Cette ligne ouvre une autre instance de MainWindow.
        }
    }
}