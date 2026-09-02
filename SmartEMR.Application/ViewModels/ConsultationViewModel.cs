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
    [ObservableProperty]
    private List<Consultation>? consultations;

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

    public void SetSelectedCST(Consultation item)
    {
        SmartMVVM.ModelProperty.SetConsultationData(Model, item);
    }

    public void SetConsultationOrders(IEnumerable<ConsultationOrder>[] items)
    {
        if (items.Length < 2) return;

        _consultationOrders = items[0];
        _deletedCSTOItems = items[1];
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

    public async Task GetRecentCST()
    {
        SmartUI.SetNofification("기능 구현 중입니다.", NotificationType.Warning);
    }

    public bool CanEnterOrder(Order item)
    {
        if (OrderMaster.ORDER_ASSESSMENTS.Contains(item.ORD_SugaCode))
        {
            ConsultationOrder? ASMItem = _consultationOrders.FirstOrDefault(x => OrderMaster.ORDER_ASSESSMENTS.Contains(x.CSTO_SugaCode));
            if (ASMItem is not null)
            {
                SmartUI.SetNofification("진찰료는 중복 처방할 수 없습니다.", NotificationType.Warning);
                return false;
            }
        }

        return true;
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
            CST_YYMMDD = Model.CST_YYMMDD,

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

    public async Task SaveDataAsync(SaveMode saveMode = SaveMode.SAVE, ConsultationStatus targetStatus = ConsultationStatus.RDY)
    {
        string actionName = saveMode switch
        {
            SaveMode.SAVE => "저장",
            SaveMode.DELETE => "취소",
            _ => ""
        };

        var isSuccess = false;

        if (saveMode == SaveMode.SAVE)
        {
            isSuccess = await SetConsultation(targetStatus);
        }
        else
        {
            isSuccess = await DeleteConsultation();
        }

        if (!isSuccess) return;

        await NotifyCompletedTaskAsync(saveMode);

        SmartUI.SetNofification($"진료{actionName} 되었습니다.", NotificationType.Success);
    }

    private async Task<bool> SetConsultation(ConsultationStatus targetStatus)
    {
        if (Model.RCP_Idx.GetValueOrDefault(0) == 0)
        {
            SmartUI.SetNofification("선택된 진료(접수)가 없습니다.", NotificationType.Warning);
            return false;
        }

        SetConsultationStatus(targetStatus);

        var item = SmartMVVM.ModelProperty.GetConsultationDataForSave(Model, _consultationOrders.Concat(_deletedCSTOItems));
        var ret = await SmartMVVM.DataStore.GetItem<Consultation>(eAPI.Consultation_SetConsultationByCST, item);

        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("진료 저장에 실패했습니다.", NotificationType.Error);
            return false;
        }

        SmartMVVM.ModelProperty.SetConsultationData(Model, ret);

        return true;
    }

    private async Task<bool> DeleteConsultation()
    {
        if (SmartUI.MsgYesNo("진료취소하시겠습니까? 진료 및 처방 기록 모두 삭제됩니다.") is MessageBoxResult.No) return false;

        await SmartMVVM.DataStore.GetItem<Consultation>(eAPI.Consultation_SetConsultation, new Consultation { CST_Idx = Model.CST_Idx, CST_IsValid = false });

        if (!SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("진료취소하지 못했습니다.", NotificationType.Error);
            return false;
        }

        return true;
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

    [RelayCommand]
    public void ClearData(bool isClearFilter = false)
    {
        SmartMVVM.ModelProperty.ClearCSTData(Model, isClearFilter);
    }
}
