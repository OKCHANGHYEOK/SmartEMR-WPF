using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.Processor;

public class ReceptionBoardProcessor : IQueryResultListProcessor<ReceptionBoard>
{
    public void Process(IEnumerable<ReceptionBoard> items)
    {
        foreach (var mitem in items)
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
                mitem.vRCP_InsuranceType = SmartMVVM.Common.GetCommonCodeName("RCP", "InsuranceType", mitem.RCP_InsuranceType ?? "")?.Substring(0, 1);
                mitem.vRCB_VisitType = SmartMVVM.Common.GetCommonCodeName("RCP", "VisitType", mitem.RCB_VisitType ?? "");
            }
        }
    }
}
