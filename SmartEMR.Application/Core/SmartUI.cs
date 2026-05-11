using SmartEMR.Application.Services;
using SmartEMR.Application.Xpf;
using System.Windows;
using System.Windows.Controls;

namespace SmartEMR.Application.Core;

public enum TargetWindowType
{
    CurrentWindow,
    PreWindow,
    AllWindows
}

public static class SmartUI
{
    public static UIManager UIManager => UIManager.Instance;

    public static UIWindow? CurrentWindow
    {
        get
        {
            if (UIManager.CurrentWindow != null)
            {
                return UIManager.CurrentWindow;
            }
            else
            {
                return null;
            }
        }
    }

    public static bool MsgConfirm(string title, string message)
    {
        var result = DialogService.ShowConfirm(title, message);

        return result;
    }
    
    public static void ShowRequiredMessage(FrameworkElement element, string message)
    {
        var tooltip = new ToolTip()
        {
            Content = message,
            StaysOpen = false,
            PlacementTarget = element,
            Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom,
            Margin = new Thickness(5)
        };

        element.ToolTip = tooltip;

        tooltip.IsOpen = true;
    }
}
