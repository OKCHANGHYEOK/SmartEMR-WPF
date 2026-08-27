using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Windows;

namespace SmartEMR.Application.Views.Patients;

/// <summary>
/// vSmartEMRDeskPATInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vPatientViewSummary : ModelViewLayout<PatientViewModel>
{
    private Patient PATItem => vm.Model;

    protected override void Initialize()
    {
       
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e) {}

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public override async Task SetPatientData(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(PATItem, item);
    }

    public void ClearData()
    {
        vm.ClearData();
    }

    private void OnClick_ImageButton(object sender, System.Windows.RoutedEventArgs e)
    {
        var element = sender as ImageButton;
        if (element == null) return;

        switch (element.Name)
        {
            case "btnCopyAddress":
                Clipboard.SetText(PATItem.PAT_Address1 ?? "");

                MessageBox.Show("주소가 복사되었습니다.");

                break;
        }
    }

    private async void OnClick_Button(object sender, RoutedEventArgs e)
    {
        var element = sender as Button;
        if (element == null) return;

        switch (element.Name)
        {
            case "btnClear":
                await SmartUI.SendMessage("ClearPAT", viewType:TargetViewType.PageView);
                break;

            case "btnMovePAT":
                await SmartUI.NavigateToPage(new vPatientInfo(new Patient { PAT_Idx = PATItem.PAT_Idx }) ,isPopup:true);
                break;
        }
    }
}
