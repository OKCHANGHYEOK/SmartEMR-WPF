using MahApps.Metro.Controls.Dialogs;
using SmartEMR.Application.Services;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Core;

public static class SmartUI
{
    public static UIManager UIManager => UIManager.Instance;

    public static bool MsgConfirm(string title, string message)
    {
        var result = DialogService.ShowConfirm(title, message);

        return result;
    }

    //public static IViewLayout? FindParentView(this DependencyObject child)
    //{
    //    DependencyObject parent = VisualTreeHelper.GetParent(child);

    //    while (parent != null)
    //    {
    //        if (parent is IViewLayout typeParent)
    //            return typeParent;

    //        parent = VisualTreeHelper.GetParent(parent);
    //    }

    //    return null;
    //}
}
