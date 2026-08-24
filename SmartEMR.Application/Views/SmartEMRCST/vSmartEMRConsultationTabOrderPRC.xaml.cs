using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRConsultationTabOrderPRC.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRConsultationTabOrderPRC : ModelViewLayout<OrderViewModel>
{
    public vSmartEMRConsultationTabOrderPRC() { }

    protected override void Initialize()
    {
        vm.SetOrderType(OrderType.PRC);
    }

    public async Task UpdateOrders()
    {
        SmartUI.BeginInvoke(async () =>
        {
            await vm.FetchDataAsync();
        }, System.Windows.Threading.DispatcherPriority.Background);
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }
}