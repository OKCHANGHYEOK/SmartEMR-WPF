global using static SmartEMR.Application.Common.Module;
global using IDisposable = SmartEMR.Application.Common.IDisposable;
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

#if DEBUG
            AppStart();
#else
            try
            {
                if (loginWindow.ShowDialog() == true)
                {
                    AppStart();
                }
                else
                {
                    Shutdown();
                }
            }
            catch (Exception ex) 
            { 
                Shutdown();
            }
#endif

        }

        private void AppStart()
        {
            var manager = SplashScreenManager.CreateThemed();

            manager.Show();

            this.MainWindow = new vLayout(typeof(vSmartEMRDeskTab));
            this.MainWindow.Show();
        }
    }

}
