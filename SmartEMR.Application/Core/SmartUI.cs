using SmartEMR.Application.Resources;
using SmartEMR.Application.Services;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Views.Shared;
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
    public static void RegisterView(ViewLayout vl)
    {
        Messenger.Register(vl, vl.ReceiveMessage);
        UIManager.RegisterView(vl);
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

    public static async void NavigateToPage<T>(object? parameter = null, bool isPopup = false) where T : class
    {
        // 뷰가 아닌 타입을 호출하는 경우 종료
        if (typeof(T).IsAssignableFrom(typeof(IViewLayout))) return;

        // 메인 레이아웃 준비
        var vlayout = CurrentWindow?.Content as vLayout;
        if (vlayout == null)
            return;

        // 팝업일 때 화면 표시 로직
        if (isPopup)
        {
            var floatPanel = new FloatPanel();
            var popup = Activator.CreateInstance<T>();
            var popupElement = popup as UIElement;

            if (popupElement != null)
            {
                floatPanel.Content = popupElement;
            }

            BeginInvoke(() =>
            {
                UIManager.AddFloatPanel(floatPanel);
            });    

            return;
        }

        // 이미 생성된 페이지인지 확인
        IViewLayout? targetView = null;

        var vl = GetViewLayout<T>();

        // 처음 이동하는 페이지인 경우 생성해줌
        if (vl == null)
        {
            vl = (T?)Activator.CreateInstance(typeof(T), parameter);
        }

        if (vl is IViewLayout)
        {
            targetView = vl as IViewLayout;
        }

        if (targetView == null) return;

        vlayout.MainContent = targetView;
    }

    public static void CloseFloatPanel(FloatPanel panel)
    {
        if (panel == null) return;

        BeginInvoke(() =>
        {
            UIManager.RemoveFloatPanel(panel);
        });
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

    public static ViewLayout? CurrentView
    {
        get
        {
            return UIManager.CurrentView;
        }
    }

    public static ViewLayout? CurrentPageView
    {
        get
        {
            return (RootView as vLayout)?.MainContent as ViewLayout;
        }
    }

    public static ViewLayout? RootView
    {
        get
        {
            return UIManager.CurrentWindow?.Content as ViewLayout;
        }
    }
    
    public static T? GetViewLayout<T>() where T : class
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


    public static void BeginInvoke(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
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
    
    public static Task<ViewMessageResponse<T>?> SendMessage<T>(string action, object? parameter = null, TargetViewType viewType = TargetViewType.CurrentView) where T : class
    {
       return Messenger.SendMessage<T>(action, parameter, viewType);
    }
    
    public static Task<ViewMessageResponse?> SendMessageToSearchView(string action, object? parameter = null)
    {
        return Messenger.SendMessageToSearchView(action, parameter);
    }
}

public static partial class SmartUI
{
    private static NotificationService NotificationService => NotificationService.Instance;

    public static void SetNofification(string message, NotificationType type)
    {
        var notiItem = new NotiItem();

        Brush? color = null;
        ImageSource? Image = null;

        switch (type)
        {
            case NotificationType.Info:
                color = SmartBrush.BRUSH_INFO;
                Image = SmartImage.IMAGE_INFO;
                break;

            case NotificationType.Success:
                color = SmartBrush.BRUSH_SUCCESS;
                Image = SmartImage.IMAGE_SUCCESS;
                break;

            case NotificationType.Warning:
                color = SmartBrush.BRUSH_WARNING;
                Image = SmartImage.IMAGE_WARNING;
                break;

            case NotificationType.Error:
                color = SmartBrush.BRUSH_ERROR;
                Image = SmartImage.IMAGE_ERROR;
                break;
        }

        notiItem.NotiMessage = message;
        notiItem.NotiColor = color;
        notiItem.NotiImage = Image;

        NotificationService.SetNotification(notiItem);
    }

    public static void CloseNotification(NotiItem notiItem)
    {
        NotificationService.CloseNotification(notiItem);
    }
}