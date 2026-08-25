using SmartEMR.Application.Common.DisplayDataMapper;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.DiplsayDataMapper;

public class OrderDisplayDataMapper : IDisplayDataMapper<Order>
{
    private OrderType OrderType;

    private string[] ORDER_MED_IV_INFUSIONS = new string[]
    {
        OrderMaster.ORDER_MED_IV_INFUSION_UNDER_100,
        OrderMaster.ORDER_MED_IV_INFUSION_100_TO_500,
        OrderMaster.ORDER_MED_IV_INFUSION_501_TO_1000
    };

    public void Map(IEnumerable<Order> items) 
    {
        foreach (var item in items)
        {
            if (OrderType == OrderType.NON)
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

                if (item.ORDC_Cd == "TRT")
                {
                    item.ORD_Name = GetDisplayOrderNameByTRT(item.ORD_Name);
                }

                if (item.ORDC_Cd == "MED")
                {
                    item.ORD_Name = GetDisplayOrderNameByMED(item.ORD_Name, item.ORD_SugaCode);
                }
            }

            item.vORD_InsuranceType = item.ORD_InsuranceType == "INS" ? "급" : "비";
        }
    }

    public void Map(IEnumerable<Order> items, OrderType viewType)
    {
        OrderType = viewType;

        Map(items);
    }

    private string GetDisplayOrderNameByTRT(string? ORD_Name)
    {
        if (string.IsNullOrWhiteSpace(ORD_Name)) return string.Empty;

        int index = ORD_Name.IndexOf("처치");

        string result = index >= 0
            ? ORD_Name[..(index + "처치".Length)]
            : ORD_Name;

        return result;
    }

    private string GetDisplayOrderNameByMED(string? ORD_Name, string? ORD_SugaCode)
    {
        if (string.IsNullOrWhiteSpace(ORD_Name)) return string.Empty;

        int index = -1;
        string result = "";

        if (ORDER_MED_IV_INFUSIONS.Contains(ORD_SugaCode))
        {
            index = ORD_Name.IndexOf("[");
            result = index >= 0 ? ORD_Name[..(index)] : ORD_Name;
        }
        else
        {
            index = ORD_Name.IndexOf("주사");
            result = index >= 0 ? ORD_Name[..(index + "주사".Length)] : ORD_Name;
        }

        return result;
    }
}

