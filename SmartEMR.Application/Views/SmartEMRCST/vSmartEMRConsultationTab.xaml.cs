using DevExpress.Xpf.Core;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using NotificationType = SmartEMR.Application.Core.NotificationType;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRConsultationTab.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRConsultationTab : ModelViewLayout<ConsultationViewModel>
{
    private Patient SelectedPAT => vm.SelectedPAT; 
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
            case "GetRecentCST":
                await vm.GetRecentCST();
                break;

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

            case "SetSelectedCSTByDate":
                {
                    var parameter = request.MessageParameter as string;
                    if (!string.IsNullOrWhiteSpace(parameter))
                    {
                        if (!await SetSelectedCSTByDate(parameter))
                        {
                            response.IsSuccess = false;
                            return response;
                        }
                    }

                    break;
                }

            case "SetConsultationOrders":
                {
                    var paramItem = request.MessageParameters as IEnumerable<ConsultationOrder>[];
                    if (paramItem is not null)
                    {
                        vm.SetConsultationOrders(paramItem);
                    }

                    break;
                }

            case "UpdateCSTOInfo":
                {
                    var paramItem = request.MessageParameter as Consultation;
                    if (paramItem is not null)
                    {
                       await SmartEMRConulstationTabCSTOInfo.UpdateDataBySelectedCST(paramItem);
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

            case "AddCSTOFromOrderSection":
                {
                    var paramItem = request.MessageParameter as Order;
                    if (paramItem is not null)
                    {
                        AddCSTOFromOrderSection(paramItem);
                    }

                    break;
                }

            case "DeleteCSTOFromOrderSection":
                {
                    var paramItem = request.MessageParameter as Order;
                    if (paramItem is not null)
                    {
                        DeleteCSTOFromOrderSection(paramItem);
                    }

                    break;
                }

            case "RefreshCST":
                await SmartEMRConsultationTabCST.RefreshData();
                break;

            case "ClearPAT":
                ClearData(true);
                break;

            case "ClearSelectedCST":
                ClearData(false, true);
                break;
        }

        response.IsSuccess = true;

        return response;
    }

    // 진료 일자 변경시 로직
    // 선택된 환자가 없으면 날짜 변경
    // 그 외의 경우 판별 로직은 뷰모델을 참조
    private async Task<bool> SetSelectedCSTByDate(string targetDate)
    {
        if (SelectedPAT.PAT_Idx.GetValueOrDefault(0) == 0) return true;

        return await vm.SetSelectedCSTByDate(targetDate);
    }

    public override async Task SetPatientData(Patient item)
    {
        var ret = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = item.PAT_Idx });
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("환자 정보를 불러오지 못했습니다.", NotificationType.Error);
            return;
        }

        SmartMVVM.ModelProperty.SetPatientData(SelectedPAT, ret);

        await PatientViewSummary.SetPatientData(SelectedPAT);
        await PatientHistory.SetPatientData(SelectedPAT);
        await SmartEMRCSTInfo.SetPatientData(SelectedPAT);
    }

    private async void SetSelectedCST(Consultation item)
    {
        await SetPatientData(new Patient { PAT_Idx = item.PAT_Idx });

        Consultation? selectedCST = null;

        if (item.RCP_Idx.GetValueOrDefault(0) == 0)
        {
            selectedCST = await SmartMVVM.DataStore.GetItem<Consultation>(eAPI.Consultation_GetConsultation, new Consultation { PAT_Idx = item.PAT_Idx, CST_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd") });
        }
        else
        {
            selectedCST = item;
        }

        if (selectedCST is null) return;

        await vm.SetSelectedCST(selectedCST);

        SmartUI.SetNofification("진료 선택되었습니다.", NotificationType.Info);
    }

    private void AddCSTO(Order item)
    {
        if (SelectedCST.RCP_Idx.GetValueOrDefault(0) == 0)
        {
            SmartUI.SetNofification("접수(진료) 선택후 처방할 수 있습니다.", NotificationType.Warning);
            return;
        }

        SmartEMRConulstationTabCSTOInfo.AddCSTO(item, SelectedCST.MUR_Idx_DOC.GetValueOrDefault(0));
    }

    private void AddCSTOFromOrderSection(Order paramItem)
    {
        if (SelectedCST.RCP_Idx.GetValueOrDefault(0) == 0)
        {
            SmartUI.SetNofification("접수(진료) 선택후 처방할 수 있습니다.", NotificationType.Warning);
            return;
        }

        if (!vm.CanEnterOrder(paramItem))
        {
            return;
        }

        paramItem.IsSelected = true;

        SmartEMRConulstationTabCSTOInfo.AddCSTO(paramItem, SelectedCST.MUR_Idx_DOC.GetValueOrDefault(0));
    }

    private void DeleteCSTOFromOrderSection(Order paramItem)
    {
        var delItem = vm.GetCSTOItemByDEL(paramItem);
        if (delItem is null) return;

        paramItem.IsSelected = false;

        SmartEMRConulstationTabCSTOInfo.DeleteCSTO(delItem);
    }

    private void ClearData(bool isClearPAT = false, bool isClearCST = false)
    {
        if (isClearPAT)
        {
            PatientViewSummary.ClearData();
            PatientHistory.ClearData();
            SmartEMRCSTInfo.ClearPATData();
        }

        if (isClearCST)
        {
            vm.ClearData();
            SmartEMRConulstationTabCSTOInfo.ClearData();
        } 
    }

    private async void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        switch (element.Tag)
        {
            case "btnReady":
                await vm.SaveDataAsync(targetStatus:ConsultationStatus.RDY);
                break;

            case "btnPending":
                await vm.SaveDataAsync(targetStatus:ConsultationStatus.PND);
                break;

            case "btnContinue":
                await vm.SaveDataAsync(targetStatus:ConsultationStatus.ING);
                break;

            case "btnFinish":
                await vm.SaveDataAsync(targetStatus:ConsultationStatus.END);
                break;

            case "btnCancel":
                await vm.SaveDataAsync(SaveMode.DELETE);
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