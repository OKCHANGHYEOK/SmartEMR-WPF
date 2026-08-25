using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

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
                        SetPatientData(paramItem);
                    }

                    break;
                }

            case "MoveIRCInfo":
                if (SelectedCST.CST_Idx.GetValueOrDefault(0) == 0)
                {
                    SmartUI.SetNofification("선택된 진료가 없습니다.", NotificationType.Warning);
                    return null;
                }

                // 보험 수정 페이지 이동
                //await SmartUI.NavigateToPage();

                break;

            case "AddCORD":
                {
                    var paramItem = request.MessageParameter as Order;
                    if (paramItem is not null)
                    {
                        AddCORD(paramItem);
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

    public override async void SetPatientData(Patient item)
    {
        PatientViewSummary.SetPatientData(item);
        PatientHistory.SetPatientData(item);
        
        await SmartEMRCSTInfo.UpdateDataByPAT(item);
    }

    private void AddCORD(Order item)
    {
        if (SelectedCST.CST_Idx.GetValueOrDefault(0) == 0)
        {
            SmartUI.SetNofification("진료 선택후 처방할 수 있습니다.", NotificationType.Warning);
            return;
        }
    }

    private void ClearData()
    {
        PatientViewSummary.ClearData();
        PatientHistory.ClearData();
        SmartEMRCSTInfo.ClearDataByPAT();
    }
}