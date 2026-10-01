using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;
using System.Windows;

namespace SmartEMR.Application.ViewModels;

public partial class ReceptionViewModel : BaseViewModel<Reception>
{
    public Patient PATItem { get; set; } = new();
    public Reception RCPItem { get; set; } = new();
    public Insurance IRCItem { get; set; } = new();

    protected readonly IPatientService _patientService;
    protected readonly IReceptionService _receptionService;

    public ReceptionViewModel(IPatientService patientService, IReceptionService receptionService)
    {
        _patientService = patientService;
        _receptionService = receptionService;
    }

    public ReceptionViewModel(IPatientService patientService, IReceptionService receptionService, Reception item) : base(item) 
    {
        _patientService = patientService;
        _receptionService = receptionService;
    }

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        if (Model.RCP_Idx.GetValueOrDefault(0) > 0)
        {
            var retPAT = await _patientService.GetPatient(new Patient { PAT_Idx = Model.PAT_Idx });
            if (retPAT.Item is null || retPAT.IsSuccess)
            {
                SmartUI.SetNotification(retPAT.Message ?? "", NotificationType.Error);
                return;
            }

            var retRCP = await _receptionService.GetReception(new Reception { RCP_Idx = Model.RCP_Idx });
            if (retRCP.Item is null || !retRCP.IsSuccess)
            {
                SmartUI.SetNotification(retRCP.Message ?? "", NotificationType.Error);
                return;
            }

            SmartMVVM.ModelProperty.SetPatientData(PATItem, retPAT.Item);
            SmartMVVM.ModelProperty.SetReceptionData(RCPItem, retRCP.Item);
            SmartMVVM.ModelProperty.SetInsuranceData(IRCItem, SmartMVVM.ModelProperty.GetInsuranceDataFromRCP(retRCP.Item));
        }
    }

    protected override Reception GetModel(Reception item)
    {
        return item;
    }

    public void SetRCPItem(Reception paramItem)
    {
        RCPItem = paramItem;
    }

    public void SetIRCItem(Insurance paramItem)
    {
        IRCItem = paramItem;
    }

    [RelayCommand]
    public async Task SetReception(SaveMode saveMode)
    {
       await SaveDataAsync(saveMode);
    }

    public async Task SaveDataAsync(SaveMode saveMode)
    {
        bool isNew = RCPItem.RCP_Idx.GetValueOrDefault(0) == 0;
        string actionName = saveMode switch
        {
            SaveMode.SAVE => isNew ? "등록" : "수정",
            SaveMode.DELETE => "취소",
            _ => ""
        };

        if (saveMode == SaveMode.DELETE)
        {
            if (!await DeleteDataAsync()) return;
        }
        else
        {
            // 접수 등록시 오늘 날짜의 기존 접수 체크
            if (isNew)
            {
                var existsTodayRCP = await SmartMVVM.Common.ExisitsReception(RCPItem.PAT_Idx.GetValueOrDefault(0), DateTime.Now.ToString("yyyy-MM-dd"));
                if (existsTodayRCP && SmartUI.MsgYesNo("오늘 날짜의 접수가 존재합니다. 접수 진행하시겠습니까?") != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            var setRCP = SmartMVVM.ModelProperty.GetReceptionDataForSave(RCPItem, IRCItem);
            var retRCP = await _receptionService.SetReception(setRCP);

            if (retRCP.Item is null || !retRCP.IsSuccess)
            {
                SmartUI.SetNotification($"접수{actionName}하지 못했습니다.", NotificationType.Error);
                return;
            }

            var reception = retRCP.Item;

            SmartMVVM.ModelProperty.SetReceptionData(RCPItem, reception);

            if (reception.IRCItem is not null)
            {
                SmartMVVM.ModelProperty.SetInsuranceData(IRCItem, reception.IRCItem);
            }
        }

        await NotifyCompletedTaskAsync(saveMode);

        SmartUI.SetNotification($"접수{actionName}되었습니다.", NotificationType.Success);
    }

    private async Task<bool> DeleteDataAsync()
    {
        if (SmartUI.MsgYesNo("접수취소 하시겠습니까?") != MessageBoxResult.Yes) return false;

        var ret = await _receptionService.CancelReception(new Reception { RCP_Idx = RCPItem.RCP_Idx, RES_Idx = RCPItem.RES_Idx, RCP_IsValid = false });
        if (!ret.IsSuccess || !string.IsNullOrWhiteSpace(ret.Message))
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return false;
        }

        return true;
    }

    protected override async Task NotifyCompletedTaskAsync(SaveMode saveMode)
    {
        await SmartUI.SendMessage("CloseView");
        await SmartUI.SendMessage("RefreshRCB", viewType: TargetViewType.PageView);

        if (saveMode == SaveMode.SAVE)
        {
            await SmartUI.SendMessage("UpdateRCPInfo", RCPItem, viewType: TargetViewType.PageView);
        }
        else
        {
            await SmartUI.SendMessage("ClearRCP", RCPItem, viewType: TargetViewType.PageView);
        }

        await SmartUI.RefreshSummaryBoard();

        SmartUI.AddRefreshRequest(RefreshPageType.CST);
    }

    public void ClearData()
    {
        SmartMVVM.ModelProperty.ClearRCPData(RCPItem, false);
        SmartMVVM.ModelProperty.ClearIRCData(IRCItem, true);
    }
}
