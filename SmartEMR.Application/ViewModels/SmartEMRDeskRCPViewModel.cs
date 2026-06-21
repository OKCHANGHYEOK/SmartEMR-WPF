using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class SmartEMRDeskRCPViewModel : ReceptionViewModel
{
    public IQueryable<MemberUser>? arrMUR_DOC { get; set; }
    public IQueryable<object>? arrRCP_Status { get; set; }
    public IQueryable<object>? arrRCP_Route { get; set; }
    public IQueryable<object>? arrRCP_VisitType { get; set; }
    public IQueryable<object>? arrIRC_Type { get; set; }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        arrMUR_DOC = SmartMVVM.Master.GetMemberUsers("DOC", true, "담당의구분");
        arrRCP_Status = SmartMVVM.Common.GetChartCommonCode("RCP", "Status", "", true, "상태구분");
        arrRCP_Route = SmartMVVM.Common.GetChartCommonCode("RCP", "Route", "", true, "방문구분");
        arrRCP_VisitType = SmartMVVM.Common.GetChartCommonCode("RCP", "VisitType", "", true, "초재진구분");
        arrIRC_Type = SmartMVVM.Common.GetChartCommonCode("RCP", "InsuranceType", "", true, "보험구분");
    }

    public void ClearData()
    {
        Model.MUR_Idx_DOC = 0;
        Model.RCP_Status = "";
        Model.RCP_Route = "";
        Model.RCP_VisitType = "";
        Model.IRC_Type = "";
    }
}
