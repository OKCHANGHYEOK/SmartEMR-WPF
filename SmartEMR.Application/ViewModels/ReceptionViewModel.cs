using CommunityToolkit.Mvvm.Input;
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

    public List<MemberUser> arrMUR_DOC { get; set; } = default!;
    public List<MemberUser> arrMUR_STF { get; set; } = default!;

    public List<CommonCode> arrRCP_Status { get; set; } = default!;
    public List<CommonCode> arrRCP_Subject { get; set; } = default!;
    public List<CommonCode> arrRCP_VisitType { get; set; } = default!;
    public List<CommonCode> arrRCP_Route { get; set; } = default!;
    public List<CommonCode> arrRCP_InsuranceType { get; set; } = default!;

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

            var IRCItem = SmartMVVM.ModelProperty.GetInsuranceDataFromRCP(retRCP);

            SmartMVVM.ModelProperty.SetPatientData(PATItem, retPAT);
            SmartMVVM.ModelProperty.SetReceptionData(Model, retRCP);
            SmartMVVM.ModelProperty.SetInsuranceData(this.IRCItem, IRCItem);
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
    public async Task SetReception(string operation)
    {
        SaveData(operation);
    }

    public async void SaveData(string operation)
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
