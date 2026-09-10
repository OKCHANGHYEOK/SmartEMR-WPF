using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Windows;

namespace SmartEMR.Application.ViewModels;

public partial class ConsultationViewModel : BaseViewModel<Consultation>
{
    public Patient SelectedPAT = new();

    [ObservableProperty]
    private List<Consultation>? consultations;

    private Consultation SelectedCST = new();

    private IEnumerable<ConsultationOrder> _consultationOrders = default!;
    private IEnumerable<ConsultationOrder> _deletedCSTOItems = default!;

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        await UpdateConsultationsByRCP();
    }

    protected override Consultation GetModel(Consultation item)
    {
        if (item.CST_Idx.GetValueOrDefault(0) == 0)
        {
            SmartMVVM.ModelProperty.SetDefaultConsultationData(item);
        }

        return item;
    }
    public async Task GetRecentCST()
    {
        SmartUI.SetNofification("기능 구현 중입니다.", NotificationType.Warning);
    }
    public ConsultationOrder? GetCSTOItemByDEL(Order paramItem)
    {
        var targetItem = _consultationOrders.FirstOrDefault(x => x.ORD_Idx == paramItem.ORD_Idx);
        if (targetItem is not null)
        {
            return targetItem;
        }

        return null;
    }

    public async Task SetSelectedCST(Consultation item, bool isUserSelection = true)
    {
        SmartMVVM.ModelProperty.SetConsultationData(Model, item);

        await SetInsuranceData(item);
        await SmartUI.SendMessage("UpdateCSTOInfo", item, viewType: TargetViewType.PageView);

        if (isUserSelection)
        {
            SelectedCST = Model.Clone();
        }
    }

    //  진료 일자 변경시 로직
    /// ===============================
    //  현재 상태   대상날짜   진료 결과
    //  ===============================
    //  진료 없음   있음      해당진료 선택
    //  진료 없음   없음      날짜 변경
    //  진료 있음   있음      물어보고 대상 진료 선택
    //  진료 있음   없음      접수일 이전이면 변경 거부
    //  진료 있음   없음      접수일 이후면 현재 진료 날짜 변경
    public async Task<bool> SetSelectedCSTByDate(string targetDate)
    {
        // 변경하려는 일자가 기존에 선택된 진료일자와 같은 경우
        if (targetDate == SelectedCST.CST_YYMMDD)
        {
            await SetSelectedCST(SelectedCST);
            return true;
        }

        Consultation? retCST = await SmartMVVM.Common.GetConsultationByDate(SelectedPAT.PAT_Idx.GetValueOrDefault(0), targetDate);

        if (Model.CST_Idx.GetValueOrDefault(0) > 0 && retCST is null)
        {
            if (string.Compare(Model.NOW_RECEPTION_YYMMDD, targetDate) > 0)
            {
                SmartUI.SetNofification("해당 날짜에 진료 기록이 없으므로 접수일 이전으로 날짜 변경할 수 없습니다.", NotificationType.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        if (retCST != null && SmartUI.MsgYesNo("해당 날짜에 진료 기록이 있습니다" + "\n" + "해당 진료로 변경하시겠습니까?") is MessageBoxResult.Yes)
        {
            await SetSelectedCST(retCST, isUserSelection:false);
            return true;
        }

        return true;
    }

    public void SetConsultationOrders(IEnumerable<ConsultationOrder>[] items)
    {
        if (items.Length < 2) return;

        _consultationOrders = items[0];
        _deletedCSTOItems = items[1];
    }

    public void UpdatePriceData(Pay item)
    {
        Model.CST_InsuredPrice = item.PAY_InsuredPrice;
        Model.CST_NonInsuredPrice = item.PAY_NonInsuredPrice;
        Model.CST_OwnPatientPrice = item.PAY_OwnPatientPrice;
        Model.CST_TotalPrice = item.PAY_TotalPrice;
    }

    [RelayCommand]
    public async Task UpdateConsultationsByRCP()
    {
        var item = new Consultation
        {
            PAT_Idx = Model.PAT_Idx,
            MUR_Idx_DOC = Model.MUR_Idx_DOC,

            CST_InsuranceType = Model.CST_InsuranceType,
            CST_Status = Model.CST_Status,
            CST_PayStatus = Model.CST_PayStatus,
            CST_Subject = Model.CST_Subject,
            CST_YYMMDD = SmartMVVM.Common.GetYYMMDDByDateString(Model.CST_YYMMDD),

            Keyword = Model.Keyword,
            SortField = Model.SortField,
            SortDir = Model.SortDir,
            PageSize = Model.PageSize,
            PageIndex = Model.PageIndex
        };

        var ret = await SmartMVVM.DataStore.GetItems<Consultation>(eAPI.Consultation_GetConsultationByRCP, item);
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("진료현황을 불러오지 못했습니다.", NotificationType.Error);
            return;
        }

        DisplayDataMappers.ConsultationDisplayDataMapper.Map(ret);

        Consultations = ret.ToList();
    }

    [RelayCommand]
    public async Task SetConsultation(ConsultationStatus targetStatus)
    {
        if (Model.RCP_Idx.GetValueOrDefault(0) == 0)
        {
            SmartUI.SetNofification("선택된 진료(접수)가 없습니다.", NotificationType.Warning);
            return;
        }

        SetConsultationStatus(targetStatus);

        var item = SmartMVVM.ModelProperty.GetConsultationDataForSave(Model, _consultationOrders.Concat(_deletedCSTOItems));
        var ret = await SmartMVVM.DataStore.GetItem<Consultation>(eAPI.Consultation_SetConsultationByCST, item);

        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("진료 저장에 실패했습니다.", NotificationType.Error);
            return;
        }

        await SetSelectedCST(ret);
        await NotifyCompletedTaskAsync(SaveMode.SAVE);
    }

    [RelayCommand]
    public async Task CancelConsultation()
    {
        if (SmartUI.MsgYesNo("진료 및 처방 기록이 모두 삭제됩니다.\n 취소하시겠습니까?") is MessageBoxResult.No) return;

        await SmartMVVM.DataStore.GetItem<Consultation>(eAPI.Consultation_CancelConsultation, new Consultation { CST_Idx = Model.CST_Idx, CST_IsValid = false });

        if (!SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("진료취소하지 못했습니다.", NotificationType.Error);
            return;
        }

        await NotifyCompletedTaskAsync(SaveMode.DELETE);
    }

    public bool CanEnterOrder(Order item)
    {
        if (OrderMaster.ORDER_ASSESSMENTS.Contains(item.ORD_SugaCode))
        {
            if (_consultationOrders.FirstOrDefault(x => OrderMaster.ORDER_ASSESSMENTS.Contains(x.CSTO_SugaCode)) is not null)
            {
                SmartUI.SetNofification("진찰료는 중복 처방할 수 없습니다.", NotificationType.Warning);
                return false;
            }
        }

        return true;
    }

    private async Task SetInsuranceData(Consultation item)
    {
        Insurance? IRCItem = null;

        if (item.IRC_Idx > 0)
        {
            var retIRC = await SmartMVVM.DataStore.GetItem<Insurance>(eAPI.Insurance_GetInsurance, new Insurance { IRC_Idx = item.IRC_Idx });
            if (retIRC != null)
            {
                IRCItem = retIRC;
            }
        }
        else
        {
            IRCItem = new Insurance { IRC_Type = item.RCP_Idx > 0 ? item.CST_InsuranceType : "NON" };
        }

        Model.IRCItem = IRCItem;
    }


    private void SetConsultationStatus(ConsultationStatus targetStatus)
    {
        var CST_Status = targetStatus switch
        {
            ConsultationStatus.RDY => "RDY",
            ConsultationStatus.PND => "PND",
            ConsultationStatus.ING => "ING",
            ConsultationStatus.END => "END",
            _ => throw new ArgumentOutOfRangeException(nameof(targetStatus))
        };

        Model.CST_Status = CST_Status;
    }

    protected override async Task NotifyCompletedTaskAsync(SaveMode operation)
    {
        await SmartUI.SendMessage("RefreshCST");

        if (operation == SaveMode.DELETE)
        {
            await SmartUI.SendMessage("ClearSelectedCST");
        }

        SmartUI.SetNofification($"진료{(operation == SaveMode.SAVE ? "저장" : "취소")} 되었습니다.", NotificationType.Success);
    }

    [RelayCommand]
    public async Task ClearData(bool isClearFilter = false)
    {
        SmartMVVM.ModelProperty.ClearCSTData(Model, isClearFilter);

        if (isClearFilter)
        {
            await UpdateConsultationsByRCP();
        }
    }
}
