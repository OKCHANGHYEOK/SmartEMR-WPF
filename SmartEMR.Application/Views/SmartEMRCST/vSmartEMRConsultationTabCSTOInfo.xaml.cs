using SmartEMR.Application.Core;
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

    protected override async void Initialize() {}

    protected override void SetDataGrid()
    {
        var dataGrid = this.DataGrids[0];
        if (dataGrid is not null)
        {
            dataGrid.DataGrid_CellValueChanged += OnDataGrid_CellValueChanged;
        }
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

    private void OnDataGrid_CellValueChanged(object? sender, DataGridCellValueChangedEventArgs e)
    {
        if (sender is not DataGrid dataGrid) return;

        var dataItem = e.DataItem as ConsultationOrder;
        if (dataItem is null) return;

        var fieldName = e.Column.FieldName;

        switch (fieldName)
        {
            case "CSTO_Day" or "CSTO_Count":
                vm.UpdateCSTOData(dataItem);
                break;
        }
    }
}