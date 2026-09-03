using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using NotificationType = SmartEMR.Application.Core.NotificationType;

namespace SmartEMR.Application.Views.Patients;

/// <summary>
/// vPatientHistory.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vPatientHistory : ModelViewLayout<PatientHistoryViewModel>
{
    public vPatientHistory() { }

    protected override void Initialize()
    {
    }

    protected override void SetDataGrid()
    {
        
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public override async void OnDataGrid_DataItemChanged(object? sender, DataItemChangedEventArgs e)
    {
        if (sender is not DataGrid dataGrid) return;

        if (dataGrid.IsDoubleClicked)
        {
            switch (dataGrid.Tag)
            {
                case "CST":
                    var dataItem = e.DataItem as Consultation;
                    if (dataItem is not null)
                    {
                        await SmartUI.SendMessage("SetSelectedCST", dataItem, viewType:TargetViewType.PageView);

                        SmartUI.SetNofification("선택된 진료가 적용되었습니다.", NotificationType.Info);
                    }
                    break;
            }
        }
    }

    public override async Task SetPatientData(Patient item)
    {
        if (item.PAT_Idx != vm.Model.PAT_Idx)
        {
            ClearData();
        }

        await vm.SetPatientData(item);
    }

    public void ClearData()
    {
        TabControl.SelectedIndex = 0;

        vm.ClearData();
    }

    private void OnLoaded_DataGrid(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not DataGrid dataGrid) return;

        if (!this.DataGrids.Contains(dataGrid))
        {
            AddDataGrid(dataGrid);
        }
    }

    private async void OnTabControl_SelectionChanged(object sender, TabControlSelectionChangedEventArgs e)
    {
        var element = sender as DXTabControl;
        if (element is null) return;

        var selectedItem = element.SelectedItem as DXTabItem;
        if (selectedItem is null) return;

        var targetHistoryType = selectedItem.Tag.ToString();
        if (string.IsNullOrWhiteSpace(targetHistoryType)) return;

        SmartUI.BeginInvoke(async () =>
        {
            await vm.UpdateHistoryBySelection(targetHistoryType);
        }, System.Windows.Threading.DispatcherPriority.Background);
    }
}