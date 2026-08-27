using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

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

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public override async Task SetPatientData(Patient item)
    {
        await vm.SetPatientData(item);
    }

    public void ClearData()
    {
        vm.ClearData();
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