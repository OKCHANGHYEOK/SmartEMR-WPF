using DevExpress.Charts.Model;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using System.Windows;

namespace SmartEMR.Application.Views;

/// <summary>
/// vLayout.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vLayout : UIWindow
{
    private IViewLayout _mainContent;

    public IViewLayout MainContent
    {
        get => _mainContent;
        set => SetProperty(ref _mainContent, value, nameof(MainContent));
    }

    public vLayout() : base()
    {
        InitializeComponent();
        Initialize();
    }

    protected override void Initialize()
    {
        MainContent = new vSmartEMRDeskTab() as IViewLayout ?? new ModelViewLayout<Chart>();
    }
}
