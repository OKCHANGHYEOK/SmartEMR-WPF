using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.DisplayDataMapper;

public class ReceptionDisplayDataMapper : IDisplayDataMapper<Reception>
{
    public void Map(IEnumerable<Reception> items)
    {
        foreach (Reception item in items)
        {
            item.IRC_Type = item.RCP_InsuranceType;
            item.vIRC_Type = SmartMVVM.Common.GetCommonCodeName("RCP", "InsuranceType", item.IRC_Type ?? "")?[0..1];
            item.vRCP_Status = SmartMVVM.Common.GetCommonCodeName("RCP", "Status", item.RCP_Status ?? "")?[2..];
            item.vRCP_Route = SmartMVVM.Common.GetCommonCodeName("RCP", "Route", item.RCP_Route ?? "");
            item.vRCP_SubjectName = item.RCP_Subject == "ETC" ? item.RCP_SubjectName : SmartMVVM.Common.GetCommonCodeName("RCP", "Subject", item.RCP_Subject ?? "");
        }
    }
}
