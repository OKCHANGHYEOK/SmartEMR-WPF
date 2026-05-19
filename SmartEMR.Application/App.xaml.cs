global using static SmartEMR.Application.Common.Module;
global using IDisposable = SmartEMR.Application.Common.IDisposable;
using System.Windows;
using SmartEMR.Application.Views;
using SmartEMR.Application.Windows;
using DevExpress.Xpf.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Views.Shared;

namespace SmartEMR.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private static readonly string AppName = "Global\\SmartEMR_Application_Unique_Mutex_Key_2026";
        private static Mutex? _mutex;

        private int? MUR_Idx { get; set; } = 100000;

        protected override void OnStartup(StartupEventArgs e)
        {
            _mutex = new Mutex(true, AppName, out bool isNewInstance);

            if (!isNewInstance)
            {
                MessageBox.Show("SmartEMR이 이미 실행중입니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);

                _mutex.Dispose();
                Shutdown();
                return;
            }

            base.OnStartup(e);

            var IsAppStart = true;

#if !DEBUG
            try
            {
                LoginWindow loginWindow = new LoginWindow();

                IsAppStart = loginWindow.ShowDialog() ?? false;
            }
            catch (Exception ex)
            {
                IsAppStart = false;

                MessageBox.Show("예기치 않은 오류가 발생했습니다. 프로그램을 종료합니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);

                Logger.WriteLog(ex);
            }

#endif
            if (IsAppStart)
            {
                AppStart();
            }
            else
            {
                Shutdown();
            }
        }

        private void AppStart()
        {
            var manager = SplashScreenManager.CreateThemed();

            manager.Show();

            this.MainWindow = new SmartEMRWindow();
            this.MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();   
            }

            base.OnExit(e);
        }
    }

}
