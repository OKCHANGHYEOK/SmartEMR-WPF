using SmartEMR.Application.Common;
using SmartEMR.Application.Resources;
using SmartEMR.Application.Services;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Views.Shared;
using SmartEMR.Application.Xpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
    // 페이지 이동 연속호출 방지를 위한 세마포어
    private static readonly SemaphoreSlim _navigationLock = new SemaphoreSlim(1, 1);

    private static DialogService _dialogService = new();

    public static void RegisterView(ViewLayout vl)
    {
        Messenger.Register(vl, vl.ReceiveMessage);
        UIManager.RegisterView(vl);
    }

    public static MessageBoxResult MsgConfirm(string msg)
    {
        return _dialogService.MsgConfirm(msg);
    }

    public static MessageBoxResult MsgYesNo(string msg)
    {
        return _dialogService.MsgYesNo(msg);
    }

    public static MessageBoxResult MsgYesNo(List<Run> arrRun)
    {
        return _dialogService.MsgYesNo(arrRun);
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

    public static async Task NavigateToPage(ViewLayout targetView, object? parameter = null, bool isPopup = false)
    {
        // 락을 즉시 획득할 수 있는지 확인 및 이미 실행중이면 함수 종료
        if (!_navigationLock.Wait(0))
        {
            SetNofification("페이지 로딩중입니다. 잠시 기다려주세요.", NotificationType.Info);
            return;
        }

        Mouse.OverrideCursor = Cursors.Wait;

        try
        {
            // 팝업일 때 화면 표시 로직
            if (isPopup)
            {
                await CreatePopupElement(targetView);
                return;
            }

            // 메인 레이아웃 준비
            var vlayout = CurrentWindow?.Content as vLayout;
            if (vlayout == null)
                return;

            // 이미 해당 만들어진 해당 페이지가 존재하는지 체크
            var vl = UIManager.Views.FirstOrDefault(x => x.GetType() == targetView.GetType());
            if (vl != null)
            {
                targetView = vl;
            }

            BeginInvoke(() =>
            {
                vlayout.MainContent = targetView;
            }, DispatcherPriority.ApplicationIdle);
        }
        finally
        {
            Mouse.OverrideCursor = null;
            _navigationLock.Release();
        }
    }

    public static async Task RefreshCurrentPage()
    {
        var currentView = CurrentPageView;
        if (currentView == null) return;

        var vlayout = CurrentWindow?.Content as vLayout;
        if (vlayout == null) return;

        vlayout.SetIndicatorVisibility(true);

        Mouse.OverrideCursor = Cursors.Wait;

        try
        {
            UIManager.RemoveViewLayout(currentView);
            await Task.Delay(400);

            if (Activator.CreateInstance(currentView.GetType()) is ViewLayout view)
            {
                vlayout.MainContent = view;
            }
        }
        finally 
        {
            vlayout.SetIndicatorVisibility(false);

            Mouse.OverrideCursor = null;
        }
    }

    public static void CloseView(TargetViewType viewType)
    {
        var targetView = UIManager.GetTargetView(viewType);
        if (targetView == null) return;

        UIManager.RemoveViewLayout(targetView);
    }

    public static void CloseFloatPanel(FloatPanel floatPanel)
    {
        if (floatPanel.Content is IViewLayout vl && !vl.ClosingFloatPanel())
        {
            return;
        }

        UIManager.RemoveFloatPanel(floatPanel);
    }

    private static async Task CreatePopupElement(ViewLayout vl)
    {
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

    public static ViewLayout? CurrentView => UIManager.CurrentView;
    public static ViewLayout? CurrentPageView => UIManager.CurrentPageView;
    public static ViewLayout? RootView => UIManager.RootView;
    
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
                color = SmartBrush.SMART_BRUSH_INFO;
                Image = SmartImage.SMART_IMAGE_INFO;
                break;

            case NotificationType.Success:
                color = SmartBrush.SMART_BRUSH_SUCCESS;
                Image = SmartImage.SMART_IMAGE_SUCCESS;
                break;

            case NotificationType.Warning:
                color = SmartBrush.SMART_BRUSH_WARNING;
                Image = SmartImage.SMART_IMAGE_WARNING;
                break;

            case NotificationType.Error:
                color = SmartBrush.SMART_BRUSH_ERROR;
                Image = SmartImage.SMART_IMAGE_ERROR;
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