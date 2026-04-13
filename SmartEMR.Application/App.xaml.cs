using System.Configuration;
using System.Data;
using System.Windows;
using SmartEMR.Application.Windows;

namespace SmartEMR.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        
            LoginWindow loginWindow = new LoginWindow();

            bool? isResult = loginWindow.ShowDialog();

            if (isResult == true)
            {

            }
            else
            {
                Shutdown();
            }
        }
    }

}
