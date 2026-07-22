using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.SmartEMRRES;

/// <summary>
/// vSmartEMRDeskPATInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRRESInfo : ModelViewLayout<ReservationViewModel>
{
    protected override void Initialize()
    {
        this.ViewTitle = "예약" + (vm.Model.RES_Idx.GetValueOrDefault(0) == 0 ? "등록" : "수정"); 
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
        if (sender is BindGrid bg)
        {

        }
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }
}
