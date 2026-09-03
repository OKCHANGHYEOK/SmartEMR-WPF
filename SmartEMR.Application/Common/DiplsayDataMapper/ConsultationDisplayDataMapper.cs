using SmartEMR.Application.Common.Converter.etc;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.DisplayDataMapper;

public class ConsultationDisplayDataMapper : IDisplayDataMapper<Consultation>
{
    public void Map(IEnumerable<Consultation> items)
    {
        foreach (var item in items)
        {
            item.MUR_Name_DOC = item.MUR_Idx_DOC.GetValueOrDefault(0) == 0 ? "미정" : item.MUR_Name_DOC;
            item.IRC_Type = item.CST_InsuranceType;
            item.vIRC_Type = SmartMVVM.Common.GetCommonCodeName("CST", "InsuranceType", item.CST_InsuranceType ?? "")?[..1];
            item.vPAT_Info = (item.PAT_Sex == "M" ? "남" : "여") + "/" + item.PAT_Age + "세";
            item.vCST_Status = SmartMVVM.Common.GetCommonCodeName("CST", "Status", item.CST_Status ?? "")?[..2];
            item.vCST_PayStatus = SmartMVVM.Common.GetCommonCodeName("CST", "PayStatus", item.CST_PayStatus ?? "")?[..2];
            item.vCST_SubjectName = item.CST_Subject == "ETC" ? item.CST_SubjectName : SmartMVVM.Common.GetCommonCodeName("CST", "Subject", item.CST_Subject ?? "");
        
            if (!string.IsNullOrWhiteSpace(item.CST_Opinion))
            {
                item.vCST_Opinion = RtfConverter.ConvertRtfToPlainText(item.CST_Opinion);
            }
        }
    }
}
