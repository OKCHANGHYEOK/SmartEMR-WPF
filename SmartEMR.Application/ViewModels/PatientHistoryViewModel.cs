using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public enum ePatientHistoryType
{
    RES,
    RCP,
    CST,
    CSTO,
    PAY
}

public class PatientHistoryViewModel : PatientViewModel
{
    private List<Reservation> arrRES { get; set; } = new();
    private List<Reception> arrRCP { get; set; } = new();
    private List<Consultation> arrCST { get; set; } = new();
    private List<ConsultationOrder> arrCSTO { get; set; } = new();
    private List<Pay> arrPAY { get; set; } = new();

    public override async Task FetchDataAsync(object parameter)
    {
        if (parameter is not ePatientHistoryType targetHistoryType) return;

        switch (targetHistoryType)
        {
            case ePatientHistoryType.RES:
                await FetchRESHistoryAsync();
                break;

            case ePatientHistoryType.RCP:
                await FetchRCPHistoryAsync();
                break;

            case ePatientHistoryType.CST:
                await FetchCSTHistoryAsync();
                break;

            case ePatientHistoryType.CSTO:
                await FetchCSTOHistoryAsync();
                break;

            case ePatientHistoryType.PAY:
                await FetchPAYHistoryAsync();
                break;
        }
    }

    public async Task UpdateHistoryBySelection(string targetHistoryType)
    {
        var bFlag = Enum.TryParse<ePatientHistoryType>(targetHistoryType, out var result);
        if (!bFlag) return;

        await FetchDataAsync(result);
    }

    private async Task FetchRESHistoryAsync()
    {
        SmartUI.SetNofification("기능 구현중입니다.", NotificationType.Info);
        return;

        //var ret = await SmartMVVM.DataStore.GetItems<Reservation>(eAPI.Reservation_GetReservation, new Reservation { PAT_Idx = Model.PAT_Idx });
        //if (ret == null || !SmartMVVM.DataStore.retIsSuccess)
        //{

        //}
    }

    private async Task FetchRCPHistoryAsync()
    {
        var ret = await SmartMVVM.DataStore.GetItems<Reception>(eAPI.Reception_GetReception, new Reception { PAT_Idx = Model.PAT_Idx });
        if (ret == null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("접수이력을 불러오는 데 실패했습니다.", NotificationType.Error);
            return;
        }

        arrRCP = [.. ret];
    }

    private async Task FetchCSTHistoryAsync()
    {
        SmartUI.SetNofification("기능 구현중입니다.", NotificationType.Info);
        return;
    }

    private async Task FetchCSTOHistoryAsync()
    {
        SmartUI.SetNofification("기능 구현중입니다.", NotificationType.Info);
        return;
    }

    private async Task FetchPAYHistoryAsync()
    {
        SmartUI.SetNofification("기능 구현중입니다.", NotificationType.Info);
        return;
    }
}
