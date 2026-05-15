using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Views.Shared;
using SmartEMR.Application.Xpf;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartEMR.Application.Views;

/// <summary>
/// vLayout.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vLayout : UIWindow
{
    private IViewLayout _mainContent = default!;

    public IViewLayout MainContent
    {
        get => _mainContent;
        set => SetProperty(ref _mainContent, value, nameof(MainContent));
    }

    public vLayout() : base()
    {
        Initialize();

        this.Loaded += (s, e) =>
        {
            SplashScreenManager.CloseAll();
        };
    }

    public vLayout(Type T) : this()
    {
        MainContent = Activator.CreateInstance(T) as IViewLayout ?? default!;
    }

    protected override void Initialize()
    {
        this.ShowTitle = false;

        DevExpress.Xpf.Core.ThemeManager.SetThemeName(this, Theme.Office2019ColorfulFullName);
    }

    private void OnClosing_vLayout(object sender, CancelEventArgs e)
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
    
    private void OnPreviewKeyDown_vLayout(object sender, KeyEventArgs e) 
    {
        if (e.Key == Key.F6)
        {
            var searchView = SmartUI.GetPageView<vSearchView>();

            if (searchView != null)
            {
                searchView.SetFocusToSearch();
            }
        }
    }
}
