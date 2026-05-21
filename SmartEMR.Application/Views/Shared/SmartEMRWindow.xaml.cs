using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Views.Shared;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace SmartEMR.Application.Views;

/// <summary>
/// SmartEMRWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class SmartEMRWindow : UIWindow
{
    private SmartEMRWindowModel Model { get; set; } = new();

    public SmartEMRWindow()
    {
    }

    protected override void Initialize()
    {
        this.Content = new vLayout(typeof(vSmartEMRDeskTab));

        //this.ShowTitle = false;
        this.DataContext = Model;

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
