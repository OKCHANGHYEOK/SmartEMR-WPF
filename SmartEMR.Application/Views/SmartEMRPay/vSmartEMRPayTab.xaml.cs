using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.Views.SmartEMRPay;

/// <summary>
/// vSmartEMRPayTab.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRPayTab : ModelViewLayout<PayViewModel>
{
    public vSmartEMRPayTab() { }

    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse<Pay> { IsSuccess = false };

        switch (request.MessageAction)
        {
            case "SetSelectedPAY":
                if (request.MessageParameter is Pay item)
                {
                    await SetSelectedPAY(item);
                }

                break;
        }

        response.IsSuccess = true;

        return response;
    }

    private async Task SetSelectedPAY(Pay item)
    {
        var retPAT = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = item.PAT_Idx });
        if (retPAT is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("한자정보가 유효하지 않습니다.", NotificationType.Error);
            return;
        }

        PatientViewSummary.SetPatientData(retPAT);
        await PatientHistory.SetPatientDataAsync(retPAT);

        await SmartEMRPayTabPayInfo.UpdatePayInfo(item);
    }
}