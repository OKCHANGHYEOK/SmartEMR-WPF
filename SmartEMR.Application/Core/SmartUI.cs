using MahApps.Metro.Controls.Dialogs;
using SmartEMR.Application.Services;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Core;

public static class SmartUI
{
    public static UIManager UIManager => UIManager.Instance;

    public static bool MsgConfirm(string title, string message)
    {
        var result = DialogService.ShowConfirm(title, message);

        return result;
    }
}
