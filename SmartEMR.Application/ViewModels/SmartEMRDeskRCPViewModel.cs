using System.Windows;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public class SmartEMRDeskRCBViewModel : BaseViewModel<ReceptionBoard>
{

    private List<ReceptionBoard> _arrRCB = new();

    public List<ReceptionBoard> arrRCB
    {
        get => _arrRCB;
        set => SetProperty(ref _arrRCB, value);
    }

    public List<MemberUser> arrMUR_DOC { get; set; } = default!;

    public List<CommonCode> arrRCP_Status { get; set; } = default!;
    public List<CommonCode> arrRCP_InsuranceType { get; set; } = default!;

    public List<CommonCode> arrRES_Status { get; set; } = default!;

    public List<CommonCode> arrRCB_Route { get; set; } = default!;
    public List<CommonCode> arrRCB_VisitType { get; set; } = default!;
    public List<CommonCode> arrRCB_Subject { get; set; } = default!;

    public override void Initialize()
    {
        arrMUR_DOC = SmartMVVM.Master.GetMemberUsers("DOC", true, "담당의구분");

        arrRCP_Status = SmartMVVM.Common.GetCommonCode("RCP", "Status", "", true, "접수상태");
        arrRCP_InsuranceType = SmartMVVM.Common.GetCommonCode("RCP", "InsuranceType", "", true, "보험구분");

        arrRES_Status = SmartMVVM.Common.GetCommonCode("RES", "Status", "", true, "예약상태");

        arrRCB_Route = SmartMVVM.Common.GetCommonCode("RCB", "Route", "", true, "방문구분");
        arrRCB_VisitType = SmartMVVM.Common.GetCommonCode("RCB", "VisitType", "", true, "초재진구분");
        arrRCB_Subject = SmartMVVM.Common.GetCommonCode("RCB", "Subject", "", true, "과목구분");
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
            foreach (var mitem in retRCB)
            {
                mitem.MUR_Name_DOC = mitem.MUR_Name_DOC ?? "-";
                mitem.vPAT_Sex = mitem.PAT_Sex == "M" ? "남" : "여";
                mitem.vPAT_Info = $"{mitem.vPAT_Sex}/{mitem.PAT_Age.GetValueOrDefault(0)}";
                mitem.vRCB_Type = mitem.RCB_Type == "RES" ? "예약" : mitem.RCB_Type == "RCP" ? "접수" : "";
                mitem.vRCB_Subject = SmartMVVM.Common.GetCommonCodeName("RCB", "Subject", mitem.RCB_Subject ?? "");

                if (mitem.RCB_Type == "RES")
                {
                    mitem.vRES_Status = SmartMVVM.Common.GetCommonCodeName("RES", "Status", mitem.RES_Status ?? "")?.Substring(2);
                    mitem.vRCP_Status = "-";
                }
                else if (mitem.RCB_Type == "RCP")
                {
                    mitem.vRES_Status = "-";
                    mitem.vRCP_Status = SmartMVVM.Common.GetCommonCodeName("RCP", "Status", mitem.RCP_Status ?? "")?.Substring(2);
                    mitem.vRCP_InsuranceType = SmartMVVM.Common.GetCommonCodeName("RCP", "InsuranceType", mitem.RCP_InsuranceType ?? "")?.Substring(0,1);
                    mitem.vRCB_VisitType = SmartMVVM.Common.GetCommonCodeName("RCP", "VisitType", mitem.RCB_VisitType ?? "");
                }
            }

            arrRCB = retRCB.ToList();
        }
        else
        {
            arrRCB = new List<ReceptionBoard>();
        }
    }

    public async Task SearchData()
    {
        if (string.IsNullOrWhiteSpace(Model.Keyword))
        {
            SmartUI.SetNofification("검색어를 1글자 이상 입력해주세요.", NotificationType.Warning);

            await SmartUI.SendMessage("SetFocusToSearch");

            return;
        }

        await FetchDataAsync();
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

    public async void ClearData()
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
