using SmartEMR.Application.Services;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SmartEMR.Application.Core;

public enum TargetWindowType
{
    CurrentWindow,
    PreWindow,
    AllWindows
}

public enum NotificationType
{
    Info,
    Success,
    Warning,
    Error
}

public static partial class SmartUI
{
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

public static partial class SmartUI
{
    public static UIManager UIManager => UIManager.Instance;

    public static UIWindow? CurrentWindow
    {
        get
        {
            return UIManager.CurrentWindow;
        }
    }

    public static T? GetPageView<T>() where T : class
    {
        IViewLayout? targetView = null;

        foreach (var vl in UIManager.Views)
        {
            if (vl.GetType() == typeof(T))
            {
                targetView = vl as IViewLayout;
            }
        }

        return targetView as T;
    }


    public static void BeginInvoke(Action action, DispatcherPriority priority)
    {
        var currentWindow = CurrentWindow as Window ?? App.Current.MainWindow;

        if (currentWindow != null)
        {
            currentWindow.Dispatcher?.BeginInvoke(action, priority);
        }
    }
}

public static partial class SmartUI
{
    public static ViewMessenger Messenger = ViewMessenger.Instance;

    public static Task<ViewMessageResponse?> SendMessage(string action, object? parameter = null, TargetViewType viewType = TargetViewType.CurrentView)
    {
        return Messenger.SendMessage(action, parameter, viewType);
    }
    
    public static Task<ViewMessageResponse<T>?> SendMessage<T>(string action, T? parameter = null, TargetViewType viewType = TargetViewType.CurrentView) where T : class
    {
       return Messenger.SendMessage<T>(action, parameter, viewType);
    }
}

public static partial class SmartUI
{
    private static readonly Brush BRUSH_INFO = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00BCD4"));
    private static readonly Brush BURSH_SUCCESS = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#009688"));
    private static readonly Brush BRUSH_WARNING = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800"));
    private static readonly Brush BRUSH_ERROR = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E91E63"));

    private static readonly ImageSource IMAGE_INFO = GlyphImage("/Images/Svg/noti_info.svg");
    private static readonly ImageSource IMAGE_SUCCESS = GlyphImage("/Images/Svg/noti_success.svg");
    private static readonly ImageSource IMAGE_WARNING = GlyphImage("/Images/Svg/noti_warning.svg");
    private static readonly ImageSource IMAGE_ERROR = GlyphImage("/Images/Svg/noti_error.svg");

    private static NotificationService NotificationService => NotificationService.Instance;

    public static void SetNofification(string message, NotificationType type)
    {
        var notiItem = new NotiItem();

        Brush? color = null;
        ImageSource? Image = null;

        switch (type)
        {
            case NotificationType.Info:
                color = BRUSH_INFO;
                Image = IMAGE_INFO;
                break;

            case NotificationType.Success:
                color = BURSH_SUCCESS;
                Image = IMAGE_SUCCESS;
                break;

            case NotificationType.Warning:
                color = BRUSH_WARNING;
                Image = IMAGE_WARNING;
                break;

            case NotificationType.Error:
                color = BRUSH_ERROR;
                Image = IMAGE_ERROR;
                break;
        }

        notiItem.NotiMessage = message;
        notiItem.NotiColor = color;
        notiItem.NotiImage = Image;

        NotificationService.SetNotification(notiItem);
    }
}