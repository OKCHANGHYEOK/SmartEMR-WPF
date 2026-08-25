using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
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
        vm.SetOrderType(OrderType.NON);
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public async Task UpdateOrders()
    {
        await vm.FetchDataAsync();
    }

    private async void OnSelectionChanged_TabControl(object sender, DevExpress.Xpf.Core.TabControlSelectionChangedEventArgs e)
    {
        if (sender is not DXTabControl tabControl) return;

        var selectedItem = e.NewSelectedItem as DXTabItem;
        if (selectedItem is null) return;

        switch (selectedItem.Tag)
        {
            case "PRC":
                await SmartEMRConsultationTabOrderPRC.UpdateOrders();
                break;

            case "TRT":
                await SmartEMRConsultationTabOrderTRT.UpdateOrders();
                break;

            case "EXM":
                await SmartEMRConsultationTabOrderEXM.UpdateOrders();
                break;

            case "DOC":
                await SmartEMRConsultationTabOrderDOC.UpdateOrders();
                break;

            case "MED":
                await SmartEMRConsultationTabOrderMED.UpdateOrders();
                break;
        }
    }
}