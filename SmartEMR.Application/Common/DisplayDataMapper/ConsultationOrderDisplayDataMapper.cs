using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.DisplayDataMapper;

public class ConsultationOrderDisplayDataMapper : IDisplayDataMapper<ConsultationOrder>
{
    private List<MemberUser> docters = SmartMVVM.Master.GetMemberUsers("DOC");

    public void Map(IEnumerable<ConsultationOrder> items)
    {
        foreach (var item in items)
        {
            item.MUR_Name_DOC = item.MUR_Idx_DOC.GetValueOrDefault(0) > 0 ? docters.FirstOrDefault(x => x.MUR_Idx == item.MUR_Idx_DOC)?.MUR_Name : "-";
            item.vORDC_Cd = SmartMVVM.Master.Query<Order>("ORDC_Cd").FirstOrDefault(x => x.ORDC_Cd == item.ORDC_Cd)?.vORDC_Cd;
            item.vCSTO_InsuranceType = SmartMVVM.Common.GetCommonCodeName("ORD", "InsuranceType", item.CSTO_InsuranceType ?? "")?[..1];

            if (item.CSTO_InsuranceType == "INS")
            {
                item.CSTO_InsuranceTypeName = "급여";
            }
            else
            {
                item.CSTO_InsuranceTypeName = "비급여";
            }
        }
    }
}
