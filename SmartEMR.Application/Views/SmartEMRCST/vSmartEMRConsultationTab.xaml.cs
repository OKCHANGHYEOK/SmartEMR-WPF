using DevExpress.Xpf.Core;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using NotificationType = SmartEMR.Application.Core.NotificationType;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRConsultationTab.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRConsultationTab : ModelViewLayout<ConsultationViewModel>
{
    private Consultation SelectedCST => vm.Model;

    public vSmartEMRConsultationTab() { }

    protected override void Initialize()
    {
    }

    public override async Task InitializeViewData()
    {
        await SmartEMRConsultationTabOrder.UpdateOrders();
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse { IsSuccess = false };

        switch (request.MessageAction)
        {
            case "SetSelectedPatient":
                {
                    var paramItem = request.MessageParameter as Patient;
                    if (paramItem is not null)
                    {
                       await SetPatientData(paramItem);
                    }

                    break;
                }

            case "SetSelectedCST":
                {
                    var paramItem = request.MessageParameter as Consultation;
                    if (paramItem is not null)
                    {
                        SetSelectedCST(paramItem);
                    }

                    break;
                }

            case "MoveIRCInfo":
                if (SelectedCST.RCP_Idx.GetValueOrDefault(0) == 0)
                {
                    SmartUI.SetNofification("선택된 접수가 없습니다.", NotificationType.Warning);
                    return null;
                }

                // 보험 수정 페이지 이동
                //await SmartUI.NavigateToPage();

                break;

            case "AddCSTO":
                {
                    var paramItem = request.MessageParameter as Order;
                    if (paramItem is not null)
                    {
                        AddCSTO(paramItem);
                    }

                    break;
                }

            case "ClearPAT":
                ClearData();
                break;
        }

        response.IsSuccess = true;

        return response;
    }

    public override async Task SetPatientData(Patient item)
    {
        await PatientViewSummary.SetPatientData(item);
        await PatientHistory.SetPatientData(item);
        await SmartEMRCSTInfo.SetPatientData(item);
    }

    private async void SetSelectedCST(Consultation item)
    {
        await SetPatientData(new Patient { PAT_Idx = item.PAT_Idx });

        vm.SetSelectedCST(item);

        await SmartEMRCSTInfo.UpdateDataBySelectedCST(item);
        await SmartEMRConulstationTabCSTOInfo.UpdateDataBySelectedCST(item);
    }

    private void AddCSTO(Order item)
    {
        if (SelectedCST.RCP_Idx.GetValueOrDefault(0) == 0)
        {
            SmartUI.SetNofification("접수(진료) 선택후 처방할 수 있습니다.", NotificationType.Warning);
            return;
        }

        SmartEMRConulstationTabCSTOInfo.AddCSTO(item);
    }

    private void ClearData()
    {
        PatientViewSummary.ClearData();
        PatientHistory.ClearData();
        SmartEMRCSTInfo.ClearDataByPAT();
    }

    private async Task OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        switch (element.Tag)
        {
            case "btnReady":
                await vm.SaveDataAsync(ConsultationStatus.RDY);
                break;

            case "btnPending":
                await vm.SaveDataAsync(ConsultationStatus.PND);
                break;

            case "btnContinue":
                await vm.SaveDataAsync(ConsultationStatus.ING);
                break;

            case "btnFinish":
                await vm.SaveDataAsync(ConsultationStatus.END);
                break;

            case "btnCancel":
                await vm.SaveDataAsync(CST_IsValid:false);
                break;

            case "btnClear":
                vm.ClearData();
                break;
        }
    }
}