using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Application.Views.SmartEMRRES;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class ReservationInfoViewModel : ReservationViewModel
{
    public Patient SelectedPatient { get; set; } = new();

    [ObservableProperty]
    private List<Patient> patients = default!;
    [ObservableProperty]
    private List<ReservationSlot>? reservations = null;

    public override async Task InitializeAsync()
    {
        if (Model.PAT_Idx.GetValueOrDefault(0) > 0)
        {
            var retPAT = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = Model.PAT_Idx });
            if (retPAT is null || !SmartMVVM.DataStore.retIsSuccess)
            {
                SmartUI.SetNofification("환자 조회에 실패했습니다.", NotificationType.Error);
                return;
            }

            SmartMVVM.ModelProperty.SetPatientData(SelectedPatient, retPAT);
        }

        if (Model.RES_Idx.GetValueOrDefault(0) > 0) 
        {
            var retRES = await SmartMVVM.DataStore.GetItem<Reservation>(eAPI.Reservation_GetReservation, new Reservation { RES_Idx = Model.RES_Idx });
            if (retRES is null || !SmartMVVM.DataStore.retIsSuccess)
            {
                SmartUI.SetNofification("예약 조회에 실패했습니다.", NotificationType.Error);
                return;
            }

            SmartMVVM.ModelProperty.SetReservationData(Model, retRES);
        }

        await UpdateReservations();
    }

    protected override Reservation GetModel(Reservation item)
    {
        item.MUR_Idx_DOC = 0;
        item.MUR_Idx_STF = 0;
        item.RES_Route = "DSK";
        item.RES_Subject = "GNR";
        item.RES_ReservationDate = DateTime.Now.ToString("yyyy-MM-dd");
        item.RES_ReservationTime = SmartMVVM.Common.GetRoundUpTimeByInterval(DateTime.Now, SmartMVVM.AppSession.ReservationTimeInterval);

        item.PageSize = 10;

        return item;
    }

    public async Task UpdateReservations()
    {
        if (Reservations is null)
        {
           Reservations = SmartMVVM.Common.GetReservationSlots();
        }

        if (Reservations is null) return;

        var ret = await SmartMVVM.DataStore.GetItems<Reservation>(eAPI.Reservation_GetReservation, new Reservation { RES_YYMMDD = Model.RES_YYMMDD });
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("예약현황 조회에 실패했습니다.", NotificationType.Error);
            return;
        }
        
        foreach (var slot in Reservations)
        {
            slot.RESItem = ret.FirstOrDefault(x => x.RES_ReservationTime == slot.RES_Time);
            slot.IsReserved = slot.RESItem is not null;
        }

        await SmartUI.SendMessage("SetSelectedSlot", new Reservation { RES_ReservationTime = Model.RES_ReservationTime });
    }

    public void UpdateSelectedSlot(ReservationSlot selectedSlot)
    {
        if (Reservations is null) return;

        foreach (var slot in Reservations)
        {
            slot.IsSelected = (slot == selectedSlot);
        }

        Model.RES_ReservationTime = selectedSlot.RES_Time;
    }

    public void ClearData(bool isClearPAT, bool isClearRES)
    {
        if (isClearPAT)
        {
            SmartMVVM.ModelProperty.ClearPATData(SelectedPatient);
        }

        if (isClearRES)
        {
            SmartMVVM.ModelProperty.ClearRESData(Model);
        }
    }

    [RelayCommand]
    public async Task Search()
    {
        Patient getPAT = new Patient
        {
            Keyword = Model.Keyword,
            PageSize = Model.PageSize
        };

        var retPAT = await SmartMVVM.DataStore.GetItems<Patient>(eAPI.Patient_GetPatient, getPAT);
        if (retPAT is null || !SmartMVVM.DataStore.retIsSuccess) 
        {
            SmartUI.SetNofification("검색된 환자가 없습니다.", NotificationType.Warning);
            return;
        }

        Patients = retPAT.ToList();

        await SmartUI.SendMessage("SetPatientSearchResult");
    }

    [RelayCommand]
    public async Task Reset()
    {
        Model.Keyword = "";
    }
}
