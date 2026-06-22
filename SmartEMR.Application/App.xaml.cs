global using static SmartEMR.Application.Common.Module;
global using IDisposable = SmartEMR.Application.Common.IDisposable;
using System.Windows;
using SmartEMR.Application.Views;
using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.Windows;

namespace SmartEMR.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private static readonly string AppName = "Global\\SmartEMR_Application_Unique_Mutex_Key_2026";
        private static Mutex? _mutex;

        private const int MUR_Idx = 100000;

        protected override async void OnStartup(StartupEventArgs e)
        {
            _mutex = new Mutex(true, AppName, out bool isNewInstance);

            if (!isNewInstance)
            {
                MessageBox.Show("SmartEMR이 이미 실행중입니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);

                _mutex.Dispose();
                _mutex = null;

                Shutdown();

                return;
            }
            
            base.OnStartup(e);

            DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = Theme.VS2017LightName;

            var isLogin = false;

            try
            {
                isLogin = SetAuthenticateUser();
            }
            catch (Exception ex)
            {
                MessageBox.Show("프로그램 초기화 중 예기치 않은 오류가 발생했습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.WriteLog(ex);
            }

            if (!isLogin)
            {
                Shutdown();
                return;
            }

            await InitializeAppData();

            var manager = SplashScreenManager.CreateThemed();

            manager.Show();

            this.MainWindow = new SmartEMRWindow();
            this.MainWindow.Show();
        }

        private bool SetAuthenticateUser()
        {
#if DEBUG
            var ret = Task.Run(async () => await SmartMVVM.SetUserByMUR_Idx(MUR_Idx)).GetAwaiter().GetResult();

            if (ret == null || !string.IsNullOrWhiteSpace(ret.Message))
            {
                return false;
            }

            return true;

#else
            var loginWindow = new LoginWindow();
            
            return loginWindow.ShowDialog() ?? false;
#endif 
        }

        private async Task InitializeAppData()
        {
            await SmartMVVM.Master.Initialize();
            await SmartMVVM.Common.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Mutex가 정상적으로 생성되어 유지 중인 경우에만 해제
            if (_mutex != null)
            {
                try
                {
                    _mutex.ReleaseMutex();
                }
                catch (ApplicationException)
                {
                    // 이미 해제되었거나 소유권이 없는 경우 예외 방지
                }
                _mutex.Dispose();
            }

            base.OnExit(e);
        }
    }
}
