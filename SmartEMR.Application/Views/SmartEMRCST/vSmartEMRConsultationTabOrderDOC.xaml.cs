using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRConsultationTabOrderDOC.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRConsultationTabOrderDOC : ModelViewLayout<OrderViewModel>
{
    public vSmartEMRConsultationTabOrderDOC() { }

    protected override void Initialize()
    {
        vm.SetOrderType(OrderType.DOC);
    }

    public async Task UpdateOrders()
    {
        await vm.FetchDataAsync();
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public override async void OnDataGrid_DataItemChanged(object? sender, DataItemChangedEventArgs e)
    {
        if (sender is not DataGrid dataGrid) return;
        if (e.DataItem is not Order dataItem) return;

        var fieldName = e.Column.FieldName;
        switch (fieldName)
        {
            case "btnAddCORD":
                await SmartUI.SendMessage("AddCORD", dataItem, TargetViewType.PageView);
                break;
        }
    }
}