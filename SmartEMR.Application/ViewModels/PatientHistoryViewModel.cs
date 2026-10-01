using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class PatientHistoryViewModel : PatientViewModel
{
    [ObservableProperty]
    private List<Reservation> reservationItems = new();
    [ObservableProperty]
    private List<Reception> receptionItems = new();
    [ObservableProperty]
    private List<Consultation> consultationItems = new();
    [ObservableProperty]
    private List<ConsultationOrder> consultationOrderItems = new();
    [ObservableProperty]
    private List<Pay> payItems = new();

    private readonly IReservationService _reservationService;
    private readonly IReceptionService _receptionService;
    private readonly IConsultationService _consultationService;
    private readonly IConsultationOrderService _consultationOrderService;
    private readonly IPayService _payService;

    public PatientHistoryViewModel(IPatientService patientService,
                                   IReceptionService receptionService,
                                   IReservationService reservationService,
                                   IConsultationService consultationService,
                                   IConsultationOrderService consultationOrderService,
                                   IPayService payService) : base(patientService)
    {
        _reservationService = reservationService;
        _receptionService = receptionService;
        _consultationService = consultationService;
        _consultationOrderService = consultationOrderService;
        _payService = payService;
    }

    public PatientHistoryViewModel(IPatientService patientServie,
                                   IReceptionService receptionService,
                                   IReservationService reservationService,
                                   IConsultationService consultationService,
                                   IConsultationOrderService consultationOrderService,
                                   IPayService payService, 
                                   Patient item) : base(patientServie, item)
    {
        _reservationService = reservationService;
        _receptionService = receptionService;
        _consultationService = consultationService;
        _consultationOrderService = consultationOrderService;
        _payService = payService;
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

    public async Task SetPatientDataAsync(Patient item)
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
        var ret = await _reservationService.GetReservations(new Reservation { PAT_Idx = Model.PAT_Idx });
        if (ret.Items is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        ReservationItems = [.. ret.Items];
    }

    private async Task FetchRCPHistoryAsync()
    {
        var ret = await _receptionService.GetReceptions(new Reception { PAT_Idx = Model.PAT_Idx });
        if (ret.Items is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        ReceptionItems = [.. ret.Items];
    }

    private async Task FetchCSTHistoryAsync()
    {
        var ret = await _consultationService.GetConsultations(new Consultation { PAT_Idx = Model.PAT_Idx });
        if (ret.Items is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        ConsultationItems = [.. ret.Items];
    }

    private async Task FetchCSTOHistoryAsync()
    {
        var ret = await _consultationOrderService.GetConsultationOrders(new ConsultationOrder { PAT_Idx = Model.PAT_Idx });
        if (ret.Items is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        ConsultationOrderItems = [.. ret.Items];
    }

    private async Task FetchPAYHistoryAsync()
    {
        var ret = await _payService.GetPays(new Pay { PAT_Idx = Model.PAT_Idx });
        if (ret.Items is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        PayItems = [.. ret.Items];
    }
}
