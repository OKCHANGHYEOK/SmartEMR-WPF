using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.Shared;

/// <summary>
/// vSmartEMRSummaryBoard.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRSummaryBoard : ModelViewLayout<SmartEMRSummaryBoardViewModel>
{
    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    protected override void SetDataGrid()
    {
        if (this.DataGrids[0] is DataGrid dataGrid)
        {
            dataGrid.GridControl.CustomRowFilter += OnCustomRowFilter_GridControl;
        }
    }

    public async Task RefreshData()
    {
        await vm.FetchDataAsync();
    }


    private void OnCustomRowFilter_GridControl(object sender, RowFilterEventArgs e)
    {
        var element = sender as GridControl;
        if (element is null) return;

        var dataItem = element.GetRow(e.ListSourceRowIndex) as ReceptionBoard;
        if (dataItem is null) return;

        if (!dataItem.IsVisible.GetValueOrDefault(false))
        {
            e.Visible = false;
            e.Handled = true;
        }
    }
}