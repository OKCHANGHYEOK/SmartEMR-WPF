using System.Windows;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRRCPInfoViewModel : ReceptionViewModel
{
    public override void Initialize()
    {
        arrMUR_DOC = SmartMVVM.Master.GetMemberUsers("DOC", true, "의사선택");
        arrMUR_STF = SmartMVVM.Master.GetMemberUsers("STF", true, "직원선택");
        arrRCP_Subject = SmartMVVM.Common.GetCommonCode("RCP","Subject");
        arrRCP_VisitType = SmartMVVM.Common.GetCommonCode("RCP", "VisitType");
        arrRCP_Route = SmartMVVM.Common.GetCommonCode("RCP", "Route");
        arrRCP_InsuranceType = SmartMVVM.Common.GetCommonCode("RCP", "InsuranceType");
    }

    protected override Reception GetModel(Reception item)
    {
        item.RCP_ReceiptDate = DateTime.Now.ToString("yyyy-MM-dd");
        item.RCP_ReceiptTime = DateTime.Now.ToString("HH:mm");
        item.MUR_Idx_DOC = 0;;
        item.MUR_Idx_STF = 0;

        return item;
    }

    [RelayCommand]
    public async Task SetReception(string operation)
    {
        if (operation == "DELETE")
        {
            if (SmartUI.MsgYesNo("접수 삭제하시겠습니까?") != MessageBoxResult.Yes) return;

            await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_SetReception, new Reception { RCP_Idx = Model.RCP_Idx, RCP_IsValid = false });

            if (SmartMVVM.DataStore.retIsSuccess == false)
            {
                SmartUI.SetNofification("접수삭제하지 못했습니다.", NotificationType.Error);
                return;
            }

            SmartUI.SetNofification("삭제되었습니다.", NotificationType.Success);

            await SmartUI.SendMessage("ClearReception", viewType:TargetViewType.PageView);
            await SmartUI.SendMessage("RefreshReception", viewType: TargetViewType.PageView);

            return;
        }

        // 접수 등록시 오늘 날짜의 기존 접수 체크
        if (Model.RCP_Idx.GetValueOrDefault(0) == 0)
        {
            var isExisitsRCP = await SmartMVVM.Common.ExisitsReception(Model.PAT_Idx.GetValueOrDefault(0), DateTime.Now.ToString("yyyy-MM-dd"));
            if (isExisitsRCP && SmartUI.MsgYesNo("오늘 날짜의 접수가 존재합니다. 접수 진행하시겠습니까?") != MessageBoxResult.Yes)
            {
                return;
            }
        }

        var msg = Model.RCP_Idx.GetValueOrDefault(0) == 0 ? "등록" : "수정";

        var RCPItem = SmartMVVM.ModelProperty.GetReceptionDataForSave(Model);
        if (RCPItem.RCP_InsuranceType != "NOR")
        {
            var retIRC = await SmartUI.SendMessage<Insurance>("GetIRCItem", viewType:TargetViewType.PageView);
            if (retIRC == null)
            {
                SmartUI.SetNofification("보험정보가 올바르지 않습니다. 확인후 다시 시도해주세요.", NotificationType.Error);
                return;
            }

            RCPItem.IRCItem = retIRC.Item;
        }

        var retRCP = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_SetReception, RCPItem);

        if (retRCP == null || SmartMVVM.DataStore.retIsSuccess == false)
        {
            SmartUI.SetNofification($"접수{msg}하지 못했습니다.", NotificationType.Error);
            return;
        }

        SmartUI.SetNofification($"접수{msg}되었습니다.", NotificationType.Success);

        await SmartUI.SendMessage("SetReception", retRCP);
        await SmartUI.SendMessage("RefreshReception", viewType:TargetViewType.PageView);
    }
}
