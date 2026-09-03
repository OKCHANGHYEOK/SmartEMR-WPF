using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Common;
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

public partial class PatientHistoryViewModel : PatientViewModel
{
    [ObservableProperty]
    private List<Reservation> reservationItems;
    [ObservableProperty]
    private List<Reception> receptionItems;
    [ObservableProperty]
    private List<Consultation> consultationItems;
    [ObservableProperty]
    private List<ConsultationOrder> consultationOrderItems;
    [ObservableProperty]
    private List<Pay> payItems;

    public PatientHistoryViewModel()
    {
        ReservationItems = new();
        ReceptionItems = new();
        ConsultationItems = new();
        ConsultationOrderItems = new();
        PayItems = new();
    }

    public override async Task FetchDataAsync(object parameter)
    {
        if (Model.PAT_Idx.GetValueOrDefault(0) == 0) return;
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

    public async Task SetPatientData(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(Model, item);

        await FetchDataAsync(ePatientHistoryType.RES);
    }

    public async Task UpdateHistoryBySelection(string targetHistoryType)
    {
        var bFlag = Enum.TryParse<ePatientHistoryType>(targetHistoryType, out var result);
        if (!bFlag) return;

        await FetchDataAsync(result);
    }

    public override void ClearData()
    {
        base.ClearData();

        ReservationItems = new();
        ReceptionItems = new();
        ConsultationItems = new();
        ConsultationOrderItems = new();
        PayItems = new();
    }

    private async Task FetchRESHistoryAsync()
    {
        var ret = await SmartMVVM.DataStore.GetItems<Reservation>(eAPI.Reservation_GetReservation, new Reservation { PAT_Idx = Model.PAT_Idx });
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("예약이력을 불러오는데 실패했습니다.", NotificationType.Error);
            return;
        }

        DisplayDataMappers.ReservationDisplayDataMapper.Map(ret);

        ReservationItems = ret.ToList();
    }

    private async Task FetchRCPHistoryAsync()
    {
        var ret = await SmartMVVM.DataStore.GetItems<Reception>(eAPI.Reception_GetReception, new Reception { PAT_Idx = Model.PAT_Idx });
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("접수이력을 불러오는데 실패했습니다.", NotificationType.Error);
            return;
        }

        DisplayDataMappers.ReceptionDisplayDataMapper.Map(ret);

        ReceptionItems = ret.ToList();
    }

    private async Task FetchCSTHistoryAsync()
    {
        var ret = await SmartMVVM.DataStore.GetItems<Consultation>(eAPI.Consultation_GetConsultation, new Consultation { PAT_Idx = Model.PAT_Idx });
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("진료이력을 불러오는데 실패했습니다.", NotificationType.Error);
            return;
        }

        DisplayDataMappers.ConsultationDisplayDataMapper.Map(ret);

        ConsultationItems = ret.ToList();
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
