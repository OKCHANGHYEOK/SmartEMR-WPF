using SmartEMR.Application.Common.DisplayDataMapper;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.DiplsayDataMapper;

public class ConsultationOrderDisplayDataMapper : IDisplayDataMapper<ConsultationOrder>
{
    public void Map(IEnumerable<ConsultationOrder> items)
    {
        foreach (var item in items)
        {
            item.vORDC_Cd = SmartMVVM.Master.Query<Order>("ORDC_Cd").FirstOrDefault(x => x.ORDC_Cd == item.ORDC_Cd)?.vORDC_Cd;
            item.vCSTO_InsuranceType = SmartMVVM.Common.GetCommonCodeName("ORD", "InsuranceType", item.CSTO_InsuranceType ?? "")?[..1];
        }
    }
}
