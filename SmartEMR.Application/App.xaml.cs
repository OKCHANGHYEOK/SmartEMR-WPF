using System.Configuration;
using System.Data;
using System.Windows;
using DevExpress.Xpf.Core;
using SmartEMR.Application.Views;
using SmartEMR.Application.Windows;
using SmartEMR.Application.Xpf;

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

            bool? isLogin = loginWindow.ShowDialog();

            if (isLogin == true)
            {
                var vlayout = new vLayout();
            }
            else
            {
                Shutdown();
            }
        }
    }

}
