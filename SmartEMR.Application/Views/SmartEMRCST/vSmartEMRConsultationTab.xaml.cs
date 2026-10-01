using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Views.SmartEMRRCP;
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
                       await SetPatientDataAsync(paramItem);
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

            case "SetSelectedOrders":
                {
                    var paramItem = request.MessageParameter as IQueryable<ConsultationOrder>;
                    if (paramItem is not null)
                    {
                        SmartEMRConsultationTabOrder.SetSelectedOrders(paramItem);
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

            case "UpdatePayInfo":
                {
                    var paramItem = request.MessageParameter as Pay;
                    if (paramItem is not null)
                    {
                        UpdatePayInfo(paramItem);
                    }

                    break;
                }

            case "UpdatePayInfoByCST":
                {
                    var paramItem = request.MessageParameter as Consultation;
                    if (paramItem is not null)
                    {
                        SmartEMRConsultationTabPayInfo.UpdatePriceDataByCST(paramItem);
                    }

                    break;
                }

            case "UpdateCSTByIRC":
                {
                    if (request.MessageParameter is Insurance paramItem)
                    {
                        UpdateCSTByIRC(paramItem);
                    }

                    break;
                }

            case "MoveIRCInfo":
                MoveIRCInfo();
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

            case "DeSelectOrder":
                {
                    var paramItem = request.MessageParameter as Order;
                    if (paramItem is not null)
                    {
                        SmartEMRConsultationTabOrder.DeSelectOrder(paramItem);
                    }

                    break;
                }

            case "RefreshCST":
                await SmartEMRConsultationTabCST.RefreshData();
                break;

            case "ClearPAT":
                if (SmartUI.MsgYesNo("선택된 환자를 초기화하시겠습니까?\n입력중인 진료도 초기화됩니다.") is System.Windows.MessageBoxResult.Yes)
                {
                    ClearData(true, true);
                }

                break;

            case "ClearSelectedCST":
                ClearData(false, true);
                break;

            case "ClearSelectedOrder":
                SmartEMRConsultationTabOrder.ClearData();
                break;
        }

        response.IsSuccess = true;

        return response;
    }

    public override async Task ReceiveRefreshRequest(List<(RefreshPageType type, object? parameter)> requests)
    {
        if (requests.Any(x => x.type == RefreshPageType.CST))
        {
            await SmartEMRConsultationTabCST.RefreshData();

            foreach (var req in requests.Where(x => x.type == RefreshPageType.CST).Reverse())
            {
                if (req.parameter is Consultation consultation)
                {
                    if (consultation.CST_Idx == SelectedCST.CST_Idx)
                    {
                       await vm.RefreshSelectedCST(consultation.CST_Idx.GetValueOrDefault(0));
                    }
                }

                requests.Remove(req);
            }

            SmartUI.SetNotification("진료현황 변경된 이력이 있어 갱신했습니다.", NotificationType.Info);
        }
    }

    // 진료 일자 변경시 로직
    // 선택된 환자가 없으면 날짜 변경
    // 그 외의 경우 판별 로직은 뷰모델을 참조
    private async Task<bool> SetSelectedCSTByDate(string targetDate)
    {
        if (SelectedPAT.PAT_Idx.GetValueOrDefault(0) == 0) return true;

        return await vm.SetSelectedCSTByDate(targetDate);
    }

    public override async Task SetPatientDataAsync(Patient item)
    {
        var ret = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = item.PAT_Idx });
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNotification("환자 정보를 불러오지 못했습니다.", NotificationType.Error);
            return;
        }

        ClearData(true, true);

        SmartMVVM.ModelProperty.SetPatientData(SelectedPAT, ret);

        PatientViewSummary.SetPatientData(SelectedPAT);
        SmartEMRConulstationTabCSTOInfo.SetPatientData(SelectedPAT);
        SmartEMRCSTInfo.SetPatientData(SelectedPAT);

        await PatientHistory.SetPatientDataAsync(SelectedPAT);
    }

    private async void SetSelectedCST(Consultation item)
    {
        ClearData(true, true);

        await SetPatientDataAsync(new Patient { PAT_Idx = item.PAT_Idx });

        Consultation? selectedCST;

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
    }

    private void AddCSTO(Order item)
    {
        SmartEMRConulstationTabCSTOInfo.AddCSTO(item, SelectedCST.MUR_Idx_DOC.GetValueOrDefault(0));
    }

    private void AddCSTOFromOrderSection(Order paramItem)
    {
        var isAdded = SmartEMRConulstationTabCSTOInfo.AddCSTO(paramItem, SelectedCST.MUR_Idx_DOC.GetValueOrDefault(0));
        if (isAdded)
        {
            paramItem.IsSelected = true;
        }
    }

    private void DeleteCSTOFromOrderSection(Order paramItem)
    {
        var delItem = vm.GetCSTOItemByDEL(paramItem);
        if (delItem is null) return;

        paramItem.IsSelected = false;

        SmartEMRConulstationTabCSTOInfo.DeleteCSTO(delItem);
    }

    private void UpdatePayInfo(Pay item)
    {
        SmartEMRConsultationTabPayInfo.UpdatePriceData(item);
        
        vm.UpdatePriceData(item);
    }

    private void UpdateCSTByIRC(Insurance item)
    {
        vm.UpdateInsuranceData(item);

        SmartEMRConulstationTabCSTOInfo.UpdateCSTByIRC(SelectedCST);

        SmartUI.SetNotification("보험 적용되었습니다.", NotificationType.Info);
    }

    private async void MoveIRCInfo()
    {
        if (SelectedCST.CST_Idx.GetValueOrDefault(0) == 0)
        {
            SmartUI.SetNotification("선택된 진료가 없습니다.", NotificationType.Warning);
            return;
        }

        if (SelectedCST.CST_PayStatus != "RDY")
        {
            SmartUI.SetNotification("수납 진행된 진료의 보험은 변경할 수 없습니다.\n수납취소후 다시 시도하세요.", NotificationType.Warning);
            return;
        }

        await SmartUI.NavigateToPage(new vSmartEMRIRCInfo(SelectedCST.IRCItem?.Clone() ?? new Insurance()), ViewMode.POPUP, isPopup: true);
    }

    private async void ClearData(bool isClearPAT = false, bool isClearCST = false)
    {
        if (isClearPAT)
        {
            PatientViewSummary.ClearData();
            PatientHistory.ClearData();
            SmartEMRCSTInfo.ClearPATData();
        }

        if (isClearCST)
        {
            await vm.ClearData();

            SmartEMRCSTInfo.ClearCSTData();
            SmartEMRConulstationTabCSTOInfo.ClearData();
            SmartEMRConsultationTabOrder.ClearData();
            SmartEMRConsultationTabPayInfo.ClearData();
        } 
    }

    private async void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        switch (element.Tag)
        {
            case "btnClear":
                if (SmartUI.MsgYesNo("진료 초기화하시겠습니까? 진료 및 처방 정보 모두 초기화됩니다.") is System.Windows.MessageBoxResult.Yes)
                {
                    ClearData(false, true);
                }

                break;
        }
    }
}