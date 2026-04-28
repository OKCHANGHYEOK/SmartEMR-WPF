using System.Windows;
using SmartEMR.Application.Views;
using SmartEMR.Application.Windows;
using DevExpress.Xpf.Core;

namespace SmartEMR.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private int? MUR_Idx { get; set; } = 100000;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        
            LoginWindow loginWindow = new LoginWindow();

            if (loginWindow.ShowDialog() == true)
            {
                DXSplashScreen.Show<vSmartEMRWaitingView>();

                var mainWindow = new vLayout();

                this.MainWindow = mainWindow;

                DXSplashScreen.Close();
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }

}
