using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.SmartEMRDesk;

/// <summary>
/// vSmartEMRDeskPAY.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRDeskPAY : ModelViewLayout<PayViewModel>
{
    public vSmartEMRDeskPAY() { }

    protected override void Initialize()
    {
    }

    protected override void SetBindGrid()
    {
        var cmbPAY_Status = this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAY_Status");
        if (cmbPAY_Status != null)
        {
            cmbPAY_Status.ItemsSource = vm.arrPAY_Status;
        }

        var txtSearch = this.BindGrids[0].GetBindItem<SearchEdit>("Keyword");
        if (txtSearch != null)
        {
            txtSearch.MinHeight = 23;
        }
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }
}