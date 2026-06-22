using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class SmartEMRDeskRCPViewModel : ReceptionViewModel
{
    public override void Initialize()
    {
        arrMUR_DOC = SmartMVVM.Master.GetMemberUsers("DOC", true, "담당의구분");
        arrRCP_Status = SmartMVVM.Common.GetChartCommonCode("RCP", "Status", "", true, "상태구분");
        arrRCP_Route = SmartMVVM.Common.GetChartCommonCode("RCP", "Route", "", true, "방문구분");
        arrRCP_VisitType = SmartMVVM.Common.GetChartCommonCode("RCP", "VisitType", "", true, "초재진구분");
        arrRCP_InsuranceType = SmartMVVM.Common.GetChartCommonCode("RCP", "InsuranceType", "", true, "보험구분");
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
    }

    protected override Reception GetModel(Reception item)
    {
        item.MUR_Idx_DOC = 0;
        item.RCP_Status = "";
        item.RCP_Route = "";
        item.RCP_VisitType = "";
        item.IRC_Type = "";
        item.RCP_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd");

        return item;
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
