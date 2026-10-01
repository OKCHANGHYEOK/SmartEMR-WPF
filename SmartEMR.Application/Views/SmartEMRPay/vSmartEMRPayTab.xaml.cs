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

    public vSmartEMRPayTab(Pay item) : base(item) { }

    protected override void Initialize()
    {
    }

    public override async void SetViewData(object? parameter = null)
    {
        if (parameter is Pay item)
        {
            await SetSelectedPAY(item);
        }
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

            case "RefreshPAY":
                await SmartEMRPayTabPAY.RefreshData();
                break;

            case "ClearPAT":
                ClearData();
                break;
        }

        response.IsSuccess = true;

        return response;
    }

    public override async Task ReceiveRefreshRequest(List<(RefreshPageType type, object? parameter)> requests)
    {
        if (requests.Any(x => x.type == RefreshPageType.PAY))
        {
            await SmartEMRPayTabPAY.RefreshData();

            foreach (var req in requests.Where(x => x.type == RefreshPageType.PAY))
            {
                if (req.parameter is Pay item && item.PAY_Idx == SmartEMRPayTabPayInfo.SelectedPAY.PAY_Idx)
                {
                    await SmartEMRPayTabPayInfo.RefreshSelectedPAY(item);
                }
            }

            SmartUI.SetNotification("수납 데이터 변경된 이력이 있어 갱신했습니다.", NotificationType.Info);
        }
    }

    private async Task SetSelectedPAY(Pay item)
    {
        var retPAT = await vm.GetPatient(item.PAT_Idx.GetValueOrDefault(0));
        if (retPAT is not null)
        {
            PatientViewSummary.SetPatientData(retPAT);
            await PatientHistory.SetPatientDataAsync(retPAT);
        }

        await SmartEMRPayTabPayInfo.UpdatePayInfo(item);
    }

    private void ClearData()
    {
        PatientViewSummary.ClearData();
        PatientHistory.ClearData();

        SmartEMRPayTabPayInfo.ClearData();
    }
}