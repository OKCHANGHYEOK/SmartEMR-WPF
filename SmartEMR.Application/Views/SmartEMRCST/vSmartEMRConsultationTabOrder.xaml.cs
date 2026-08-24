using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRConsultationTabOrder.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRConsultationTabOrder : ModelViewLayout<OrderViewModel>
{
    public vSmartEMRConsultationTabOrder() { }

    protected override void Initialize()
    {
    }

    public override async Task InitializeViewData()
    {
        vm.SetOrderType(OrderType.NON);

        await vm.FetchDataAsync();
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }
}