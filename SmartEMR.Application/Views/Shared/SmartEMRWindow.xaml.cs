using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.Views.Shared;
using SmartEMR.Application.Xpf;
using System.ComponentModel;
using System.Windows;

namespace SmartEMR.Application.Views;

/// <summary>
/// SmartEMRWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class SmartEMRWindow : UIWindow
{
    public SmartEMRWindow()
    {
    }

    protected override void Initialize()
    {
        this.ShowTitle = false;
        this.Content = new vLayout(typeof(vSmartEMRDeskTab));

        DevExpress.Xpf.Core.ThemeManager.SetThemeName(this, Theme.Office2019ColorfulFullName);

        this.Loaded += (s, e) =>
        {
            SplashScreenManager.CloseAll();
        };
    }

    private void OnClosing_SmartEMRWindow(object sender, CancelEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("프로그램을 종료하시겠습니까?", "종료 확인", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            App.Current.Shutdown();
        }
        else
        {
            e.Cancel = true;
        }
    }

}
