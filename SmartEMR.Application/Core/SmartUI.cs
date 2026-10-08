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

public enum TargetResource
{
    Generic,
    DataGridCell,
    Calendar
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

    public static MessageBoxResult MsgYesNo(List<Inline> inlines)
    {
        return _dialogService.MsgYesNo(inlines);
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

// 뷰 이동, 표시제어 등 관련 로직
public static partial class SmartUI
{
    public static async Task NavigateToPage(ViewLayout targetView, object? parameter = null, bool isPopup = false, bool isReuse = true)
    {
        // 락을 즉시 획득할 수 있는지 확인 및 이미 실행중이면 함수 종료
        if (!_navigationLock.Wait(0))
        {
            SetNotification("페이지 로딩중입니다. 잠시 기다려주세요.", NotificationType.Info);
            return;
        }

        Mouse.OverrideCursor = Cursors.Wait;

        var vlayout = CurrentWindow?.Content as vLayout;
        if (vlayout is null) return;

        try
        {
            var vl = UIManager.Views.FirstOrDefault(x => x.GetType() == targetView.GetType());

            if (vl is not null)
            {
                if (isReuse)
                {
                    targetView = vl;
                }
                else
                {
                    UIManager.RemoveViewLayout(vl);
                }
            }

            if (targetView is ModelViewLayout mvLayout)
            {
                vlayout.SetIndicatorVisibility(true);

                await Task.Delay(500);
                await mvLayout.InitializeViewData();
            }

            if (parameter is not null)
            {
                targetView.SetViewData(parameter);
            }

            if (isPopup)
            {
                UIManager.ShowPopup(targetView);
            }
            else
            {
                vlayout.MainContent = targetView;

                Messenger.ExecuteRefresh(targetView);
            }

            vlayout.SetIndicatorVisibility(false);
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
        if (currentView is null) return;

        var vlayout = CurrentWindow?.Content as vLayout;
        if (vlayout is null) return;

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

    public static void CloseView(TargetViewType viewType = TargetViewType.CurrentView)
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

        UIManager.ClosePopup(floatPanel);
    }
}

public static partial class SmartUI
{
    public static UIManager UIManager => UIManager.Instance;
    public static PopupManager PopupManager => UIManager.PopupManager;

    public static Window? CurrentWindow
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

    public static async Task RefreshSummaryBoard()
    {
        var targetView = UIManager.Views.FirstOrDefault(x => x is vSmartEMRSummaryBoard);

        if (targetView is not null && targetView is vSmartEMRSummaryBoard summaryBoard)
        {
            await summaryBoard.RefreshData();
        }
    }
}

public static partial class SmartUI
{
    public static ViewMessenger Messenger = ViewMessenger.Instance;

    /// <summary>
    /// 뷰간 메시지 전송, 반환타입 X
    /// </summary>
    /// <param name="action"></param>
    /// <param name="parameter"></param>
    /// <param name="parameters"></param>
    /// <param name="viewType"></param>
    /// <returns></returns>
    public static Task<ViewMessageResponse?> SendMessage(string action, object? parameter = null, object[]? parameters = null, TargetViewType viewType = TargetViewType.CurrentView)
    {
        return Messenger.SendMessage(action, parameter, parameters, viewType);
    }
    
    /// <summary>
    /// 뷰간 메시지 전송, 반환타입 O
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    /// <param name="parameter"></param>
    /// <param name="parameters"></param>
    /// <param name="viewType"></param>
    /// <returns></returns>
    public static Task<ViewMessageResponse<T>?> SendMessage<T>(string action, object? parameter = null, object[]? parameters = null, TargetViewType viewType = TargetViewType.CurrentView) where T : class
    {
       return Messenger.SendMessage<T>(action, parameter, parameters, viewType);
    }
    
    /// <summary>
    /// vSearchView 로 보내야하는 메시지가 있을 때 사용
    /// </summary>
    /// <param name="action"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static Task<ViewMessageResponse?> SendMessageToSearchView(string action, object? parameter = null)
    {
        return Messenger.SendMessageToSearchView(action, parameter);
    }

    /// <summary>
    /// 페이지 이동시 내역을 갱신해야하는 경우 사용
    /// </summary>
    /// <param name="type"></param>
    public static void AddRefreshRequest(RefreshPageType type)
    {
        Messenger.AddRefreshRequest(type);
    }

    /// <summary>
    /// 페이지 이동시 내역 갱신 및 해당 페이지에서 보고 있는(선택된) 데이터도 갱신해야하는 경우 사용
    /// </summary>
    /// <param name="type"></param>
    /// <param name="parameter"></param>
    public static void AddRefreshRequest(RefreshPageType type, object? parameter = null)
    {
        Messenger.AddRefreshRequest(type, parameter);
    }
}

public static partial class SmartUI
{
    private static NotificationService NotificationService => NotificationService.Instance;

    public static void SetNotification(string message, NotificationType type)
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

// 그 외 유틸 함수
public partial class SmartUI 
{
    public static T? FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        DependencyObject parentObject = VisualTreeHelper.GetParent(child);

        if (parentObject == null) return null;

        return parentObject is T parent ? parent : FindParent<T>(parentObject);
    }
}
