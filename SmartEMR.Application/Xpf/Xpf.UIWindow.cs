using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace SmartEMR.Application.Xpf;

[ObservableObject]
public abstract partial class UIWindow : Window
{
    [ObservableProperty] private string m_ContentTitle = "SmartEMR";
    [ObservableProperty] private Size m_ContentSize = new Size(600, 800);

    public UIWindow()
    {
        SetUIWindow();
        Initialize();
    }

    private void SetUIWindow()
    {
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    protected abstract void Initialize();
}
