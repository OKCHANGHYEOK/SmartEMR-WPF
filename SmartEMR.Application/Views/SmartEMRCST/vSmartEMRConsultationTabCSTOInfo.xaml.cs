using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Collections.ObjectModel;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRConsultationTabCSTOInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRConsultationTabCSTOInfo : ModelViewLayout<ConsultationOrderViewModel>
{
    public ObservableCollection<ConsultationOrder> ConsultationOrders => vm.ConsultationOrderItems;

    public vSmartEMRConsultationTabCSTOInfo() {}

    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public override void OnDataGrid_DataItemChanged(object? sender, DataItemChangedEventArgs e)
    {
        if (sender is not DataGrid) return;

        var dataItem = e.DataItem as ConsultationOrder;
        if (dataItem is null) return;

        switch (e.Column.FieldName)
        {
            case "btnDelete":
                DeleteCSTO(dataItem);
                break;
        }
    }

    public async Task UpdateDataBySelectedCST(Consultation item)
    {
       await vm.UpdateDataBySelectedCST(item);
    }

    public void ClearData()
    {
        vm.ClearData();
    }

    public void AddCSTO(Order item)
    {
        vm.AddCSTO(item);
    }

    private void DeleteCSTO(ConsultationOrder item)
    {
        vm.DeleteCSTO(item);
    }
}