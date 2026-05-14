global using static SmartEMR.Application.Common.Module;
global using IDisposable = SmartEMR.Application.Common.IDisposable;
using System.Windows;
using SmartEMR.Application.Views;
using SmartEMR.Application.Windows;
using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;

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

            try
            {
                if (loginWindow.ShowDialog() == true)
                {
                    var manager = SplashScreenManager.CreateThemed();

                    manager.Show();

                    this.MainWindow = new vLayout(typeof(vSmartEMRDeskTab));
                    this.MainWindow.Show();
                }
                else
                {
                    Shutdown();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("예기치 않은 오류가 발생했습니다. 프로그램을 종료합니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);

                Logger.WriteLog(ex);

                Shutdown();
            }
        }
    }

}
