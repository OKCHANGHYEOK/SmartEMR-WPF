using SmartEMR.Application.Common.DisplayDataMapper;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.DiplsayDataMapper;

public class OrderDisplayDataMapper : IDisplayDataMapper<Order>
{
    private OrderViewType orderViewType;

    public void Map(IEnumerable<Order> items) 
    {
        foreach (var item in items)
        {
            if (orderViewType == OrderViewType.NON)
            {
                if (item.ORDC_Cd == "ASM")
                {
                    item.ORD_Name = item.ORD_Name?[0..5];
                
                    if (item.ORD_SugaCode == OrderMaster.ORDER_CLINIC_ASM_FIR || item.ORD_SugaCode == OrderMaster.ORDER_CLINIC_ASM_REP)
                    {
                        item.ORD_IsView = SmartMVVM.AppSession.Member?.MEM_BizType == "CLN";
                    }
                    else if (item.ORD_SugaCode == OrderMaster.ORDER_HOSPITAL_ASM_FIR || item.ORD_SugaCode == OrderMaster.ORDER_HOSPITAL_ASM_REP)
                    {
                        item.ORD_IsView = SmartMVVM.AppSession.Member?.MEM_BizType == "HOS";
                    }
                }
            }
        }
    }

    public void Map(IEnumerable<Order> items, OrderViewType viewType)
    {
        orderViewType = viewType;

        Map(items);
    }
}

