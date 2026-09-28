using SmartEMR.Application.Schemas;
using SmartEMR.Application.Views.SmartEMRPay;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Core.NaverPay;

public class NaverPayManager
{
    public static async Task<NaverPayResponse?> RequestPayment(Pay item)
    {
        var nPayInfo = new vSmartEMRNaverPayInfo(item)
        {
            Owner = SmartUI.CurrentWindow
        };

        var result = nPayInfo.ShowDialog();
        if (nPayInfo.response != null && nPayInfo.response.IsSuccess)
        {
            return nPayInfo.response;
        }

        return null;
    }

    public static string CreateMerchantPayKey(int? PAY_Idx)
    {
        if (PAY_Idx.HasValue)
        {
            return $"PAY_{PAY_Idx}";
        }

        return $"PAY_{DateTime.Now:yyyyMMddHHmmssfff}";
    }
}


