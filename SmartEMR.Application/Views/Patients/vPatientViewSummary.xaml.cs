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

    public override void SetPatientData(Patient item)
    {
        vm.SetPatientData(item);
    }

    public void ClearData()
    {
        vm.ClearData();
    }

    private async void OnClick_Button(object sender, RoutedEventArgs e)
    {
        if (sender is not System.Windows.Controls.Button element) return;

        switch (element.Name)
        {
            case "btnClear":
                await SmartUI.SendMessage("ClearPAT", viewType:TargetViewType.PageView);
                break;

            case "btnMovePAT":
                await SmartUI.NavigateToPage(new vPatientInfo(new Patient { PAT_Idx = PATItem.PAT_Idx }) ,isPopup:true);
                break;

            case "btnCopyAddress":
                if (!string.IsNullOrWhiteSpace(PATItem.PAT_Address1))
                {
                    Clipboard.SetText(PATItem.PAT_Address1);
                    SmartUI.SetNotification("주소가 복사되었습니다.", NotificationType.Info);
                }
                else
                {
                    SmartUI.SetNotification("입력된 주소가 없습니다.", NotificationType.Warning);
                }

                break;
        }
    }
}
