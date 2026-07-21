using System.Windows;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRDeskRCBViewModel : BaseViewModel<ReceptionBoard>
{

    private List<ReceptionBoard> _arrRCB = new();

    public List<ReceptionBoard> arrRCB
    {
        get => _arrRCB;
        set => SetProperty(ref _arrRCB, value);
    }

    public override void Initialize()
    {

    }

    protected override ReceptionBoard GetModel(ReceptionBoard item)
    {
        item.MUR_Idx_DOC = 0;

        item.RCP_Status = "";
        item.RCP_InsuranceType = "";
        item.RES_Status = "";

        item.RCB_Subject = "";
        item.RCB_Route = "";
        item.RCB_VisitType = "";
        item.RCB_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd");

        return item;
    }

    public override async Task FetchDataAsync()
    {
        var getRCB = new ReceptionBoard
        {
            MUR_Idx_DOC = Model.MUR_Idx_DOC,

            RCP_Status = Model.RCP_Status,
            RCP_InsuranceType = Model.RCP_InsuranceType,

            RES_Status = Model.RES_Status,

            RCB_Route = Model.RCB_Route,
            RCB_Subject = Model.RCB_Subject,
            RCB_VisitType = Model.RCB_VisitType,
            RCB_YYMMDD = Model.RCB_YYMMDD,

            Keyword = Model.Keyword,
            PageSize = Model.PageSize,
            PageIndex = Model.PageSize,
            SortField = Model.SortField,
            SortDir = Model.SortDir
        };

        var retRCB = await SmartMVVM.DataStore.GetItems<ReceptionBoard>(eAPI.Reception_GetReceptionBoard, getRCB);

        if (retRCB != null && retRCB.Any())
        {
            SmartMVVM.ProcessorProvider.ReceptionBoardProcessor.Process(retRCB);

            arrRCB = retRCB.ToList();
        }
        else
        {
            arrRCB = new List<ReceptionBoard>();
        }
    }

    public async Task CancelRCP(Reception item)
    {
        if (SmartUI.MsgYesNo("접수취소하시겠습니까?") != MessageBoxResult.Yes) return;

        var ret = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_SetReception, new Reception { RCP_Idx = item.RCP_Idx, RCP_IsValid = false });
        if (!SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("접수취소 하는데 실패했습니다. 다시 시도해주세요.", NotificationType.Error);
            return;
        }

        await SmartUI.SendMessage("RefreshDataList");
        SmartUI.SetNofification("접수취소되었습니다.", NotificationType.Success);
    }

    public void SetRCB_YYMMDD(string RCB_YYMMDD)
    {
        Model.RCB_YYMMDD = RCB_YYMMDD;
    }

    [RelayCommand]
    public async Task Search()
    {
        if (string.IsNullOrWhiteSpace(Model.Keyword))
        {
            SmartUI.SetNofification("검색어를 1글자 이상 입력해주세요.", NotificationType.Warning);

            await SmartUI.SendMessage("SetFocusToSearch");

            return;
        }

        await FetchDataAsync();
    }

    [RelayCommand]
    public async Task Reset()
    {
        Model.MUR_Idx_DOC = 0;

        Model.RCP_Status = "";
        Model.RCP_InsuranceType = "";

        Model.RES_Status = "";
        
        Model.RCB_Route = "";
        Model.RCB_VisitType = "";
        Model.RCB_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd");
        Model.Keyword = "";

        await FetchDataAsync();
    }
}
