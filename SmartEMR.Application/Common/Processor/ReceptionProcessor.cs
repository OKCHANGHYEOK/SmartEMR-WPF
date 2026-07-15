using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.Processor;

public class ReceptionProcessor : IQueryResultListProcessor<Reception>
{
    public void Process(IEnumerable<Reception> items)
    {
        foreach (Reception item in items)
        {
            item.vRCP_Status = SmartMVVM.Common.GetCommonCodeName("RCP", "Status", item.RCP_Status ?? "")?[2..];
            item.vRCP_Route = SmartMVVM.Common.GetCommonCodeName("RCP", "Route", item.RCP_Route ?? "");
            item.vRCP_InsuranceType = SmartMVVM.Common.GetCommonCodeName("RCP", "InsuranceType", item.RCP_InsuranceType ?? "")?[0..1];
            item.RCP_SubjectName = item.RCP_Subject == "ETC" ? item.RCP_SubjectName : SmartMVVM.Common.GetCommonCodeName("RCP", "Subject", item.RCP_Subject ?? "");
        }
    }
}
