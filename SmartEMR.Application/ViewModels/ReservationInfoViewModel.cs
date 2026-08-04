using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.Views.SmartEMRRES;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class ReservationInfoViewModel : ReservationViewModel
{
    public Patient SelectedPatient { get; set; } = new();
    public Patient InputPatient { get; set; } = new();

    [ObservableProperty]
    private ReservationSlot? selectedSlot;
    [ObservableProperty]
    private List<Patient> patients = default!;
    [ObservableProperty]
    private List<ReservationSlot> reservations = default!;

    [ObservableProperty]
    private bool isNewPatient = true;
    [ObservableProperty]
    private bool canChangingIsNewPatient = false;

    public ReservationInfoViewModel() { }
    public ReservationInfoViewModel(Reservation item) : base(item) { }

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

            IsNewPatient = false;
        }
        else
        {
            SmartMVVM.ModelProperty.SetDefaultPatientData(InputPatient);
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
        if (item.RES_Idx.GetValueOrDefault(0) == 0)
        {
            item.MUR_Idx_DOC = 0;
            item.MUR_Idx_STF = 0;
            item.RES_Route = "DSK";
            item.RES_Subject = "GNR";
            item.RES_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd");
            item.RES_ReservationDate = DateTime.Now.ToString("yyyy-MM-dd");
            item.RES_ReservationTime = SmartMVVM.Common.GetRoundUpTimeByInterval(DateTime.Now, SmartMVVM.AppSession.ReservationTimeInterval);
        }

        item.PageSize = 10;

        return item;
    }

    public void SetSelectedPatient(Patient patient)
    {
        SmartMVVM.ModelProperty.SetPatientData(SelectedPatient, patient);

        CanChangingIsNewPatient = true;
    }

    public void SetSelectedSlot(string? reservationTime = "")
    {
        if (Reservations is null) return;

        Func<ReservationSlot, bool>? selectFunc = null;

        // 초기 설정 또는 예약날짜 변경시(시간이 전달되지 않은 경우)
        if (string.IsNullOrWhiteSpace(reservationTime))
        {
            // 오늘 날짜로 변경된 경우
            if (SmartMVVM.Common.IsToday(Model.RES_YYMMDD))
            {
                if (Model.RES_Idx.GetValueOrDefault(0) > 0) // 예약 수정으로 들어온 경우 해당 예약의 시간으로 설정
                {
                    selectFunc = (x) =>
                    {
                        return x.RESItem is not null && x.RESItem.RES_Idx == Model.RES_Idx;
                    };
                }             
                else // 예약 등록으로 들어온 경우 현재 기준으로 가장 가까운 시간으로 설정
                {
                    selectFunc = (x) =>
                    {
                        return x.RES_Time == SmartMVVM.Common.GetRoundUpTimeByInterval(DateTime.Now, SmartMVVM.AppSession.ReservationTimeInterval);
                    };
                }

            }
            // 오늘 날짜가 아닌 경우 첫 번째 시각으로 설정
            else
            {
                selectFunc = (x) =>
                {
                    return true;
                };
            }
        }
        // 예약시간 변경시
        else
        {
            selectFunc = (x) =>
            {
                return x.RES_Time == reservationTime;
            };
        }

        SelectedSlot = Reservations.FirstOrDefault(selectFunc);
    }

    public async Task UpdateReservations(string? RES_YYMMDD = "")
    {
        Reservations = SmartMVVM.Common.GetReservationSlots();

        Model.RES_YYMMDD = string.IsNullOrWhiteSpace(RES_YYMMDD) ? Model.RES_YYMMDD : DateTime.Parse(RES_YYMMDD).ToString("yyyy-MM-dd");

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

        SetSelectedSlot();
    }

    public void UpdateReservationTime(string? RES_Time)
    {
        if (string.IsNullOrWhiteSpace(RES_Time) || Model.RES_ReservationTime == RES_Time) return;

        Model.RES_ReservationTime = RES_Time;
    }

    public void UpdateInputPatientByRegisterNum1(bool isUpdatedRegNo1)
    {   
        if (!isUpdatedRegNo1)
        {
            InputPatient.PAT_Sex = "N";
            InputPatient.PAT_BirthDate = "";
        }
    }

    public void UpdateInputPatientByRegisterNum2(string? PAT_RegisterNum2)
    {
        if (!string.IsNullOrWhiteSpace(PAT_RegisterNum2))
        {
            var firstChar = PAT_RegisterNum2[0];
            var century = firstChar switch
            {
                '1' or '2' => "19",
                '3' or '4' => "20",
                _ => ""
            };

            var gender = firstChar switch
            {
                '1' or '3' => "M",
                '2' or '4' => "F",
                _ => ""
            };

            if (!string.IsNullOrWhiteSpace(century))
            {
                InputPatient.PAT_BirthDate = century + InputPatient.PAT_RegisterNum1;
            }

            if (!string.IsNullOrWhiteSpace(gender))
            {
                InputPatient.PAT_Sex = gender;
            }
        }
        else
        {
            InputPatient.PAT_BirthDate = "";
            InputPatient.PAT_Sex = "N";
        }
    }

    public bool ClearData(bool isClearPAT, bool isClearRES)
    {
        if (isClearPAT)
        {  
            if (SelectedPatient.PAT_Idx.GetValueOrDefault(0) == 0) return false;

            var messages = new List<Inline>();
            messages.Add(new Run("신환예약 등록으로 변경하시겠습니까?"));
            messages.Add(new LineBreak());
            messages.Add(new Run("(선택된 환자정보는 초기화됩니다.)") { FontSize = 14, Foreground = Brushes.DimGray });

            if (SmartUI.MsgYesNo(messages) is MessageBoxResult.No) return false;

            SmartMVVM.ModelProperty.ClearPATData(SelectedPatient);

            CanChangingIsNewPatient = false;
        }

        if (isClearRES)
        {
            SmartMVVM.ModelProperty.ClearRESData(Model);
        }

        return false;
    }

    [RelayCommand]
    public async Task Search()
    {
        if (string.IsNullOrWhiteSpace(Model.Keyword))
        {
            SmartUI.SetNofification("검색어를 1글자 이상 입력해주세요", NotificationType.Warning);
            return;
        }

        Patient getPAT = new Patient
        {
            Keyword = Model.Keyword,
            PageSize = Model.PageSize
        };

        var retPAT = await SmartMVVM.DataStore.GetItems<Patient>(eAPI.Patient_GetPatient, getPAT);
        if (retPAT is null || !retPAT.Any() || !SmartMVVM.DataStore.retIsSuccess) 
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

    [RelayCommand]
    public async Task SetReservation(SaveMode operation)
    {
        bool isNew = Model.RES_Idx.GetValueOrDefault(0) == 0;
        string actionName = operation switch
        {
            SaveMode.SAVE => isNew ? "등록" : "수정",
            SaveMode.DELETE => "삭제",
            _ => ""
        };

        if (operation == SaveMode.DELETE)
        {
            if (!await DeleteReservationAsync()) return;
        }
        else
        {
            if (IsNewPatient) // 신환예약인 경우 환자정보 유효성 체크
            {
                if (!ValidatePatientData()) return;
            }
            else if (SelectedPatient.PAT_Idx.GetValueOrDefault(0) == 0)  // 기존환자 예약인 경우 선택된 환자 있는지 체크
            {
                SmartUI.SetNofification("선택된 환자가 없습니다. 신환예약 또는 환자선택후 다시 시도해주세요.", NotificationType.Warning);
                return;
            }

            Reservation? item = null;

            if (IsNewPatient)
            {
                item = SmartMVVM.ModelProperty.GetReservationDataForSave(Model, InputPatient);
            }
            else
            {
                item = SmartMVVM.ModelProperty.GetReservationDataForSave(Model, SelectedPatient);
            }

            var retRES = await SmartMVVM.DataStore.GetItem<Reservation>(eAPI.Reservation_SetReservation, item);
        
            if (retRES is null || !SmartMVVM.DataStore.retIsSuccess)
            {
                SmartUI.SetNofification("예약 저장에 실패했습니다.", NotificationType.Error);
                return;
            }
        }

        await NotifyCompletedTaskAsync(operation);

        SmartUI.SetNofification($"예약{actionName}되었습니다.", NotificationType.Success);
    }

    protected override async Task NotifyCompletedTaskAsync(SaveMode operation)
    {
        await SmartUI.SendMessage("CloseView");
        await SmartUI.SendMessage("RefreshRCB", viewType: TargetViewType.PageView);
    }

    private bool ValidatePatientData()
    {
        List<string[]> missingFields = new List<string[]>();
        List<string[]> uncorrectFields = new List<string[]>();

        if (string.IsNullOrWhiteSpace(InputPatient.PAT_Name))
        {
            missingFields.Add(["PAT_Name", "성명"]);
        }

        if (string.IsNullOrWhiteSpace(InputPatient.PAT_RegisterNum1))
        {
            missingFields.Add(["PAT_RegisterNum1", "주민번호앞자리"]);
        }

        if (string.IsNullOrWhiteSpace(InputPatient.PAT_RegisterNum2))
        {
            missingFields.Add(["PAT_RegisterNum2", "주민번호뒷자리"]);
        }

        if (missingFields.Any())
        {
            var message = "아래 항목들을 입력해주세요.\n- ";
            message += string.Join(", ", missingFields.Select(field => field[1]));

            SmartUI.SetNofification(message, NotificationType.Warning);

            TextFocusBehavior.SetFocusByName(missingFields[0][0]);

            return false;
        }

        return true;
    }

    private async Task<bool> DeleteReservationAsync()
    {
        if (SmartUI.MsgYesNo("예약 삭제하시겠습니까?") is not System.Windows.MessageBoxResult.Yes) return false;

        await SmartMVVM.DataStore.GetItem<Reservation>(eAPI.Reservation_GetReservation, new Reservation { RES_Idx = Model.RES_Idx, RES_IsValid = false });

        if (!SmartMVVM.DataStore.retIsSuccess) return false;

        return true;
    }
}
