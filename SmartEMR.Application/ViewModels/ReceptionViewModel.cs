using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Windows;

namespace SmartEMR.Application.ViewModels;

public partial class ReceptionViewModel : BaseViewModel<Reception>
{
    public ReceptionViewModel() { }
    public ReceptionViewModel(Reception item) : base(item) { }

    public Patient PATItem { get; set; } = new();
    public Reception RCPItem { get; set; } = new();
    public Insurance IRCItem { get; set; } = new();

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        if (Model.RCP_Idx.GetValueOrDefault(0) > 0)
        {
            var retPAT = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = Model.PAT_Idx });
            if (retPAT == null || !SmartMVVM.DataStore.retIsSuccess)
            {
                SmartUI.SetNofification("삭제됐거나 존재하지 않는 환자입니다.", NotificationType.Error);
                return;
            }

            var retRCP = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_GetReception, new Reception { RCP_Idx = Model.RCP_Idx });
            if (retRCP == null || !SmartMVVM.DataStore.retIsSuccess)
            {
                SmartUI.SetNofification("삭제됐거나 존재하지 않는 접수입니다.", NotificationType.Error);
                return;
            }

            var retIRC = SmartMVVM.ModelProperty.GetInsuranceDataFromRCP(retRCP);

            SmartMVVM.ModelProperty.SetPatientData(PATItem, retPAT);
            SmartMVVM.ModelProperty.SetReceptionData(RCPItem, retRCP);
            SmartMVVM.ModelProperty.SetInsuranceData(IRCItem, retIRC);
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
    public async Task SetReception(SaveMode operation)
    {
       await SaveDataAsync(operation);
    }

    public async Task SaveDataAsync(SaveMode operation)
    {
        bool isNew = RCPItem.RCP_Idx.GetValueOrDefault(0) == 0;
        string actionName = operation switch
        {
            SaveMode.SAVE => isNew ? "등록" : "수정",
            SaveMode.DELETE => "취소",
            _ => ""
        };

        if (operation == SaveMode.DELETE)
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
            var retRCP = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_SetReception, setRCP);

            if (retRCP == null || SmartMVVM.DataStore.retIsSuccess == false)
            {
                SmartUI.SetNofification($"접수{actionName}하지 못했습니다.", NotificationType.Error);
                return;
            }

            SmartMVVM.ModelProperty.SetReceptionData(RCPItem, retRCP);

            if (retRCP.IRCItem is not null)
            {
                SmartMVVM.ModelProperty.SetInsuranceData(IRCItem, retRCP.IRCItem);
            }
        }

        await NotifyCompletedTaskAsync(operation);

        SmartUI.SetNofification($"접수{actionName}되었습니다.", NotificationType.Success);
    }

    private async Task<bool> DeleteDataAsync()
    {
        if (SmartUI.MsgYesNo("접수취소 하시겠습니까?") != MessageBoxResult.Yes) return false;

        await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_CancelReception, new Reception { RCP_Idx = RCPItem.RCP_Idx, RES_Idx = RCPItem.RES_Idx, RCP_IsValid = false });

        if (SmartMVVM.DataStore.retIsSuccess == false)
        {
            SmartUI.SetNofification("접수취소하지 못했습니다.", NotificationType.Error);
            return false;
        }

        return true;
    }

    protected override async Task NotifyCompletedTaskAsync(SaveMode operation)
    {
        await SmartUI.SendMessage("CloseView");
        await SmartUI.SendMessage("RefreshRCB", viewType: TargetViewType.PageView);

        if (operation == SaveMode.SAVE)
        {
            await SmartUI.SendMessage("UpdateRCPInfo", RCPItem, viewType: TargetViewType.PageView);
        }
        else
        {
            await SmartUI.SendMessage("ClearRCP", RCPItem, viewType: TargetViewType.PageView);
        }
    }

    public void ClearData()
    {
        SmartMVVM.ModelProperty.ClearRCPData(RCPItem, false);
        SmartMVVM.ModelProperty.ClearIRCData(IRCItem, true);
    }
}
