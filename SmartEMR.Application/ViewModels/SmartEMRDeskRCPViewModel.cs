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
    public List<ChartCommonCode> arrRCB_Status { get; set; } = default!;
    public List<ChartCommonCode> arrRCB_Route { get; set; } = default!;
    public List<ChartCommonCode> arrRCB_VisitType { get; set; } = default!;
    public List<ChartCommonCode> arrRCB_InsuranceType { get; set; } = default!;

    public override void Initialize()
    {
        arrMUR_DOC = SmartMVVM.Master.GetMemberUsers("DOC", true, "담당의구분");
        arrRCB_Status = SmartMVVM.Common.GetChartCommonCode("RCB", "Status", "", true, "상태구분");
        arrRCB_Route = SmartMVVM.Common.GetChartCommonCode("RCB", "Route", "", true, "방문구분");
        arrRCB_VisitType = SmartMVVM.Common.GetChartCommonCode("RCB", "VisitType", "", true, "초재진구분");
        arrRCB_InsuranceType = SmartMVVM.Common.GetChartCommonCode("RCB", "InsuranceType", "", true, "보험구분");
    }

    protected override ReceptionBoard GetModel(ReceptionBoard item)
    {
        item.MUR_Idx_DOC = 0;
        item.RCB_Status = "";
        item.RCB_Route = "";
        item.RCB_VisitType = "";
        item.RCB_InsuranceType = "";
        item.RCB_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd");

        return item;
    }

    public override async Task FetchDataAsync()
    {
        var getRCB = new ReceptionBoard
        {
            MUR_Idx_DOC = Model.MUR_Idx_DOC,

            RCB_Status = Model.RCB_Status,
            RCB_Route = Model.RCB_Route,
            RCB_VisitType = Model.RCB_VisitType,
            RCB_InsuranceType = Model.RCB_InsuranceType,
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
                mitem.vPAT_Sex = mitem.PAT_Sex == "M" ? "남" : "여";
                mitem.vPAT_Info = $"{mitem.vPAT_Sex}/{mitem.PAT_Age.GetValueOrDefault(0)}";
                mitem.vRCB_Status = arrRCB_Status.FirstOrDefault(x => x.CCC_Cd == mitem.RCB_Status)?.CCC_Name;
                mitem.vRCB_InsuranceType = arrRCB_InsuranceType.FirstOrDefault(x => x.CCC_Cd == mitem.RCB_InsuranceType)?.CCC_Name;
            }

            arrRCB = retRCB.ToList();
        }
        else
        {
            arrRCB = new List<ReceptionBoard>();
        }
    }

    public void SetToDay()
    {
        Model.RCB_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd");
    }

    public void ClearData()
    {
        Model.MUR_Idx_DOC = 0;
        Model.RCB_Status = "";
        Model.RCB_Route = "";
        Model.RCB_VisitType = "";
        Model.RCB_InsuranceType = "";
    }
}
