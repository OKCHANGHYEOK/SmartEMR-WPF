using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

[ObservableObject]
public abstract partial class UIWindow : Window, IViewLayout
{
    [ObservableProperty] private string m_ContentTitle = "SmartEMR";
    [ObservableProperty] private Size m_ContentSize = new Size(600, 800);

    private readonly List<BindGrid> _bindGrids = new();

    public IReadOnlyList<BindGrid> BindGrids => _bindGrids;

    protected abstract void Initialize();

    public UIWindow()
    {
        SetUIWindow();
        Initialize();
    }

    private void SetUIWindow()
    {
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    public void AddBindGrid(BindGrid bindGrid)
    {
        if (!_bindGrids.Contains(bindGrid)) 
        {
            _bindGrids.Add(bindGrid);
            bindGrid.BindGrid_BindClickEvent += OnBindGrid_BindClick;
        }
    }

    public abstract void OnBindGrid_BindClick(object sender, BindClickEventArgs e);
}
