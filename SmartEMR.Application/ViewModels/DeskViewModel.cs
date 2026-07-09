using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Windows;

namespace SmartEMR.Application.ViewModels;

public class DeskViewModel : BaseViewModel<Reception>
{
    public Reception RCPItem { get; set; } = new();
    public Insurance IRCItem { get; set; } = new();

    public override void Initialize()
    {
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

    public async void SetReception(string operation)
    {
        string actionName = "";

        try
        {
            if (operation == "DELETE")
            {
                if (SmartUI.MsgYesNo("접수취소 하시겠습니까?") != MessageBoxResult.Yes) return;

                await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_SetReception, new Reception { RCP_Idx = RCPItem.RCP_Idx, RCP_IsValid = false });

                if (SmartMVVM.DataStore.retIsSuccess == false)
                {
                    SmartUI.SetNofification("접수취소하지 못했습니다.", NotificationType.Error);
                    return;
                }

                actionName = "취소";

                await SmartUI.SendMessage("ClearReception", new Reception { RCP_Idx = Model.RCP_Idx }, viewType: TargetViewType.PageView);
                await SmartUI.SendMessage("RefreshReception", viewType: TargetViewType.PageView);

                return;
            }

            actionName = RCPItem.RCP_Idx.GetValueOrDefault(0) == 0 ? "등록" : "수정";

            // 접수 등록시 오늘 날짜의 기존 접수 체크
            if (RCPItem.RCP_Idx.GetValueOrDefault(0) == 0)
            {
                var IsExistsTodayRCP = await SmartMVVM.Common.ExisitsReception(RCPItem.PAT_Idx.GetValueOrDefault(0), DateTime.Now.ToString("yyyy-MM-dd"));
                if (IsExistsTodayRCP && SmartUI.MsgYesNo("오늘 날짜의 접수가 존재합니다. 접수 진행하시겠습니까?") != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            var setRCP = SmartMVVM.ModelProperty.GetReceptionDataForSave(RCPItem);
            setRCP.IRCItem = IRCItem;

            var retRCP = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_SetReception, setRCP);

            if (retRCP == null || SmartMVVM.DataStore.retIsSuccess == false)
            {
                SmartUI.SetNofification($"접수{actionName}하지 못했습니다.", NotificationType.Error);
                return;
            }

            await SmartUI.SendMessage("RefreshRCP", viewType: TargetViewType.PageView);

            SmartMVVM.ModelProperty.SetReceptionData(RCPItem, retRCP);
            
            if (retRCP.IRCItem != null)
            {
                SmartMVVM.ModelProperty.SetInsuranceData(IRCItem, retRCP.IRCItem);
            }
        }
        finally
        {
            if (!string.IsNullOrWhiteSpace(actionName))
            {
                SmartUI.SetNofification($"접수{actionName}되었습니다.", NotificationType.Success);

                await SmartUI.SendMessage("CloseView");
            }
        }
    }
}
