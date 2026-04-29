using DevExpress.Charts.Model;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views;

/// <summary>
/// vSmartEMRDeskTab.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRDeskTab : ModelViewLayout<DeskViewModel>
{
    public vSmartEMRDeskTab()
    {
        InitializeComponent();
    }

    protected override void Initialize()
    {
        throw new NotImplementedException();
    }

    public override void OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        throw new NotImplementedException();
    }
}
