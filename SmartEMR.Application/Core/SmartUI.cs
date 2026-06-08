using SmartEMR.Application.Common;
using SmartEMR.Application.Resources;
using SmartEMR.Application.Services;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Views.Shared;
using SmartEMR.Application.Xpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    private static bool IsBusy = false;

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
        if (!typeof(IViewLayout).IsAssignableFrom(typeof(T))) return;

        if (IsBusy)
        {
            SetNofification("페이지 로딩중입니다. 잠시 기다려주세요.", NotificationType.Info);
            return;
        }

        IsBusy = true;
        Mouse.OverrideCursor = Cursors.Wait;

        try
        {
            // 메인 레이아웃 준비
            var vlayout = CurrentWindow?.Content as vLayout;
            if (vlayout == null)
                return;

            // 팝업일 때 화면 표시 로직
            if (isPopup)
            {
                CreatePopupElement<T>();
                return;
            }

            var vl = GetViewLayout<T>();

            // 처음 이동하는 페이지인 경우 생성해줌
            if (vl == null)
            {
                vl = (T?)Activator.CreateInstance(typeof(T), parameter);
            }

            if (vl is ViewLayout targetView == false)
                return;

            await InitializeViewData(targetView);

            BeginInvoke(() =>
            {
                vlayout.MainContent = targetView;
            }, DispatcherPriority.ApplicationIdle);
        }
        finally
        {
            IsBusy = false;
            Mouse.OverrideCursor = null;
        }
    }

    public static void CloseFloatPanel(FloatPanel floatPanel)
    {
        if (floatPanel.Content is ViewLayout vl && !vl.ClosingFloatPanel())
        {
            return;
        }

        UIManager.RemoveFloatPanel(floatPanel);
    }

    private static async Task InitializeViewData(ViewLayout vl)
    {
        if (vl.DataContext == null || vl.DataContext is BaseViewModel vm == false)
            return;

        var method = vm.GetType().GetMethod("InitializeAsync");

        if (method != null)
        {
            var task = method.Invoke(vm, null) as Task;
            if (task != null)
            {
                await task;
            }
        }
    }

    private static async void CreatePopupElement<T>() where T : class
    {
        var vl = Activator.CreateInstance<T>() as ViewLayout;
        if (vl == null) return;

        await InitializeViewData(vl);

        UIManager.AddFloatPanel(new FloatPanel { Content = vl });

        BeginInvoke(() =>
        {
            TextFocusBehavior.SetFocusToFirstTextElement(vl);
        }, DispatcherPriority.Background);
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

    public static async Task InvokeAsnyc(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
    {
        var currentWindow = CurrentWindow as Window ?? App.Current.MainWindow;

        if (currentWindow != null)
        {
            currentWindow.Dispatcher?.InvokeAsync(action, priority);
        }
    }

    public static void ReturnFocusTovLayout()
    {
        var vlayout = RootView as vLayout;
        if (vlayout != null)
        {
            vlayout.Focus();
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