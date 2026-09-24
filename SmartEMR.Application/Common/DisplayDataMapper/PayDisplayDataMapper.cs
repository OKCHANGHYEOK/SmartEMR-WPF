using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.DisplayDataMapper;

public class PayDisplayDataMapper : IDisplayDataMapper<Pay>
{
    public void Map(IEnumerable<Pay> items)
    {
        foreach (var item in items)
        {
            item.vCST_Status = SmartMVVM.Common.GetCommonCodeName("CST", "Status", item.CST_Status ?? "")?[2..];
            item.vPAY_Status = item.PAY_Status switch
            {
                "PAR" => SmartMVVM.Common.GetCommonCodeName("PAY", "Status", item.PAY_Status ?? "")?[..2],
                _ => SmartMVVM.Common.GetCommonCodeName("PAY", "Status", item.PAY_Status ?? "")?[2..]
            };

            item.vPAT_Info = (item.PAT_Sex == "M" ? "남" : "여") + "/" + item.PAT_Age;
        }
    }
}
