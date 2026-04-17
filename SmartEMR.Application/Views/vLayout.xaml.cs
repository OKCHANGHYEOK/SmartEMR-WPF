using SmartEMR.Application.Xpf;
using System.Windows;

namespace SmartEMR.Application.Views;

/// <summary>
/// vLayout.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vLayout : UIWindow
{
    public vLayout() : base()
    {
        InitializeComponent();
    }

    public override void OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected override void Initialize()
    {

    }
}
