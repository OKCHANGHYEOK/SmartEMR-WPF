global using static SmartEMR.Application.Common.Module;
global using IDisposable = SmartEMR.Application.Common.IDisposable;
using System.Windows;
using SmartEMR.Application.Views;
using SmartEMR.Application.Windows;
using DevExpress.Xpf.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Views.Shared;
using SmartEMR.Domain.Entities;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Enums;
using SmartEMR.Infrastructure.Services;

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

        protected override async void OnStartup(StartupEventArgs e)
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
            var IsLogin = false;

#if DEBUG
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
            IsLogin = SmartMVVM.AppSession.GetMemberUser() != null;

            if (IsAppStart)
            {
                if (!IsLogin)
                {
                    var retMUR = await SmartMVVM.DataStore.GetItem<MemberUser>(eAPI.MemberUser_GetMemberUser, new MemberUser { MEM_Idx = 100000, MUR_Idx = this.MUR_Idx });
                    if (retMUR == null) return;

                    var getItem = new MemberUser
                    {
                        MUR_Id = retMUR.MUR_Id,
                        MUR_PassWord = retMUR.MUR_PassWord
                    };

                    var retToken = await AuthenticationService.AuthenticateUserByLogin(getItem);

                    if (retToken == null || !string.IsNullOrWhiteSpace(retToken.FailMessage))
                    {
                        return;
                    }

                    SmartMVVM.AppSession.SetToken(retToken);
                    SmartMVVM.AppSession.SetMemberUser(retToken.User);
                }

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
