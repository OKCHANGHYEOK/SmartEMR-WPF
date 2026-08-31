using DevExpress.Xpf.Core;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Collections.ObjectModel;
using NotificationType = SmartEMR.Application.Core.NotificationType;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRConsultationTab.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRConsultationTab : ModelViewLayout<ConsultationViewModel>
{
    private Consultation SelectedCST => vm.Model;

    private ObservableCollection<ConsultationOrder> ConsultationOrders => SmartEMRConulstationTabCSTOInfo.ConsultationOrders;

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
                ClearData(true);
                break;

            case "ClearCSTInfo":
                SmartEMRCSTInfo.ClearData(true);
                break;
        }

        response.IsSuccess = true;

        return response;
    }

    public override async Task SetPatientData(Patient item)
    {
        var ret = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = item.PAT_Idx });
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("환자 정보를 불러오지 못했습니다.", NotificationType.Error);
            return;
        }

        await PatientViewSummary.SetPatientData(ret);
        await PatientHistory.SetPatientData(ret);
        await SmartEMRCSTInfo.SetPatientData(ret);
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

    private void ClearData(bool isClearPAT = false, bool isClearCST = false)
    {
        if (isClearPAT)
        {
            PatientViewSummary.ClearData();
            PatientHistory.ClearData();
        }

        if (isClearCST)
        {
            SmartEMRCSTInfo.ClearData();
            SmartEMRConulstationTabCSTOInfo.ClearData();
        } 
    }

    private async void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
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
                if (SmartUI.MsgYesNo("진료 초기화하시겠습니까? 진료 및 처방 정보 모두 초기화됩니다.") is System.Windows.MessageBoxResult.Yes)
                {
                    ClearData(false, true);
                }

                break;
        }
    }
}