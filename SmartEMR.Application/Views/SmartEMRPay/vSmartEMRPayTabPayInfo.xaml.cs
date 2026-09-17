using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRPay;

/// <summary>
/// vSmartEMRPayTabPayInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRPayTabPayInfo : ModelViewLayout<PayInfoViewModel>
{
    public vSmartEMRPayTabPayInfo() {}

    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public async Task UpdatePayInfo(Pay item)
    {
        await vm.UpdatePayInfo(item);
    }
}