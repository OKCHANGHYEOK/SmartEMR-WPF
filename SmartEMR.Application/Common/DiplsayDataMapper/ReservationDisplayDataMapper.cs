using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.DisplayDataMapper;

public class ReservationDisplayDataMapper : IDisplayDataMapper<Reservation>
{
    public void Map(IEnumerable<Reservation> items)
    {
        foreach (var item in items)
        {
            item.vRES_Status = SmartMVVM.Common.GetCommonCodeName("RES", "Status", item.RES_Status ?? "")?.Substring(0,2);
            item.vRES_Route = SmartMVVM.Common.GetCommonCodeName("RES", "Route", item.RES_Route ?? "");
            item.vRES_SubjectName = item.RES_Subject == "ETC" ? item.RES_SubjectName : SmartMVVM.Common.GetCommonCodeName("RES", "Subject", item.RES_Subject ?? "");
        }
    }
}
