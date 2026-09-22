using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Views.Patients;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRPay;

/// <summary>
/// vSmartEMRPayTabPAY.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRPayTabPAY : ModelViewLayout<PayViewModel>
{
    public vSmartEMRPayTabPAY() { }

    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
        if (sender is not BindGrid) return;

        var fieldName = e.BindItem.FieldName;

        switch (fieldName)
        {
            case "btnToday":
                vm.SetToday();
                break;
        }
    }

    public override async void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
        if (sender is not BindGrid) return;

        var fieldName = e.BindItem.FieldName;

        switch (fieldName)
        {
            case "PAY_YYMMDD" or "CST_Status" or "PAY_Status":
                await vm.FetchDataAsync();
                break;
        }
    }

    public override async void OnDataGrid_DataItemChanged(object? sender, DataItemChangedEventArgs e)
    {
        if (sender is not DataGrid dataGrid) return;

        var dataItem = e.DataItem as Pay;
        if (dataItem is null) return;

        if (dataGrid.IsDoubleClicked)
        {
            await SmartUI.SendMessage("SetSelectedPAY", dataItem, viewType:TargetViewType.PageView);
        }
        else
        {
            switch (e.Column.FieldName)
            {
                case "PAT_Name":
                    await SmartUI.NavigateToPage(new vPatientInfo(new Patient { PAT_Idx = dataItem.PAT_Idx }), isPopup:true);
                    break;
            }
        }
    }

    public async Task RefreshData()
    {
        await vm.FetchDataAsync();
    }
}