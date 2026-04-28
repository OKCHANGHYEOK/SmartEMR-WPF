using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using System.Windows;

namespace SmartEMR.Application.Xpf;

[ObservableObject]
public abstract partial class UIWindow : ThemedWindow
{

    [ObservableProperty] private string m_ContentTitle = "SmartEMR";
    [ObservableProperty] private Size m_ContentSize = new Size(600, 800);

    protected abstract void Initialize();

    public UIWindow()
    {
        SetUIWindow();
        Initialize();

        SmartUI.UIManager.RegisterWindow(this);
    }

    private void SetUIWindow()
    {
        DevExpress.Xpf.Core.ThemeManager.SetThemeName(this, Theme.Office2019ColorfulFullName);

        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }
}
