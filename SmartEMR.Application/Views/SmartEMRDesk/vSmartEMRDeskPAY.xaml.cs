using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.SmartEMRDesk;

/// <summary>
/// vSmartEMRDeskPAY.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRDeskPAY : ModelViewLayout<PayViewModel>
{
    public vSmartEMRDeskPAY() { }

    protected override void Initialize()
    {
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }
}