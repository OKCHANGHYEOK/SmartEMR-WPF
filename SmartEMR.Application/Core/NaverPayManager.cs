using SmartEMR.Application.Views.SmartEMRPay;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Core;

public class NaverPayManager
{
    static NaverPayManager()
    {
    }

    public static async Task Open(Pay item)
    {
        var nPayInfo = new vSmartEMRNaverPayInfo(item)
        {
            Owner = SmartUI.CurrentWindow
        };

        nPayInfo.ShowDialog();
    }
}
