using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Views.Shared;
using SmartEMR.Application.Xpf;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartEMR.Application.Core;

public enum TargetViewType
{
    CurrentView = 0,        // 현재 포커스중인 뷰
    PageView = 1,           // 현재 보고 있는 페이지에 해당하는 뷰
    PreFloatView = 2,       // 팝업인 경우 해당 팝업의 이전 팝업
    RootView = 3            // vLayout
}


public partial class UIManager
{
    private static readonly Lazy<UIManager> _instance = new(() => new UIManager());
    public static UIManager Instance => _instance.Value;


    private UIManager() {}
}

// Windows 관련 UI 관리
public partial class UIManager 
{
    private readonly List<UIWindow> _activeWindows = new();

    public UIWindow? CurrentWindow
    {
        get
        {
            var window = _activeWindows.LastOrDefault(w => w.IsActive || w.IsFocused);

            if (window == null) window = _activeWindows.LastOrDefault();

            return window;
        }
    }

    public void RegisterWindow(UIWindow window)
    {
        if (!_activeWindows.Contains(window))
        {
            _activeWindows.Add(window);
            window.Closed += (s, e) => _activeWindows.Remove(window);
        }
    }

    public void CloseWindow(TargetWindowType targetWindowType)
    {
        switch (targetWindowType)
        {
            case TargetWindowType.CurrentWindow:

                if (CurrentWindow != null)
                {
                    CurrentWindow.DialogResult = true;
                    CurrentWindow.Close();
                }
                else
                {
                    var window = App.Current.MainWindow;

                    if (window != null)
                    {
                        window.DialogResult = true;
                        window.Close();
                    }
                }

                break;

            case TargetWindowType.PreWindow:

                var preWindow = _activeWindows.Where(w => w != CurrentWindow).LastOrDefault();
                if (preWindow != null)
                {
                    preWindow.DialogResult = true;
                    preWindow.Close();
                }

                break;

            case TargetWindowType.AllWindows:

                foreach (var window in _activeWindows.ToList())
                {
                    window.DialogResult = true;
                    window.Close();
                }

                break;
        }
    }
}

// View 관련 UI 관리
public partial class UIManager
{
    private readonly List<ViewLayout> _activeViews = new();
    public IReadOnlyList<ViewLayout> Views => _activeViews;

    public PopupManager PopupManager { get; } = new();

    public ViewLayout? RootView
    {
        get
        {
            return CurrentWindow?.Content as ViewLayout;
        }
    }

    public ViewLayout? CurrentView
    {
        get
        {
            if (PopupManager.HasPopup)
            {
                return _activeViews.LastOrDefault(v => v.IsPopupView);
            }

            var windows = _activeWindows.OfType<UIWindow>();
            if (windows.Any())
            {
                return GetCurrentView();
            }

            return windows.FirstOrDefault()?.Content as ViewLayout;
        }
    }

    public ViewLayout? CurrentPageView
    {
        get
        {
            if (_activeViews.Any(v => v.IsPopupView))
            {
                return _activeViews.FirstOrDefault(v => v.IsPopupView);
            }

            return (RootView as vLayout)?.MainContent as ViewLayout;
        }
    }

    public void ShowPopup(ViewLayout targetView)
    {
        PopupManager.Show(targetView);
    }

    public void ClosePopup(FloatPanel floatPanel)
    {
        var vl = floatPanel.Content as ViewLayout;
        if (vl is not null)
        {
           var isRemoved = UnRegisterView(vl);
           if (isRemoved)
            {
                PopupManager.Close(floatPanel);
            }
        }
    }

    public void RegisterView(ViewLayout view)
    {
        if (_activeViews.Contains(view)) return;

        _activeViews.Add(view);

        var mv = view as ModelViewLayout;
        if (mv == null) return;

        FindAndRegisterElements(mv);
    }

    // TargetViewType에 따른 타겟 추출 로직 (UIManager 활용)
    public ViewLayout? GetTargetView(TargetViewType viewType)
    {
        return viewType switch
        {
            TargetViewType.CurrentView => CurrentView,
            TargetViewType.PageView => CurrentPageView,
            TargetViewType.RootView => RootView,
            _ => null
        };
    }

    public void RemoveViewLayout(ViewLayout view)
    {
        bool isRemoved = UnRegisterView(view);
        if (isRemoved)
        {
            if (view.Parent is FloatPanel floatPanel)
            {
                PopupManager.Close(floatPanel);
            }
        }
    }

    private static ViewLayout? GetCurrentView()
    {
        try
        {
            var focusedElement = FocusManager.GetFocusedElement(SmartUI.CurrentWindow) as DependencyObject;

            if (focusedElement != null)
            {
                DependencyObject parent = focusedElement;

                while (true)
                {
                    if (parent is ViewLayout view)
                    {
                        return view;
                    }

                    parent = VisualTreeHelper.GetParent(parent);
                }
            }
        }
        catch 
        {
        }

        return default!;
    }

    private bool UnRegisterView(ViewLayout view)
    {
        bool isRemoved = _activeViews.Remove(view);
        if (isRemoved)
        {
            view.Dispose(true); // IDisposable 명시적 캐스팅 대신 직접 호출
        }

        return isRemoved;
    }

    private void FindAndRegisterElements(ModelViewLayout view, DependencyObject? parent = null)
    {
        parent ??= view as DependencyObject;

        int childCount = VisualTreeHelper.GetChildrenCount(parent);

        for (int i = 0; i < childCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);
            if (child is ModelViewLayout) continue;

            if (child is BindGrid bindGrid)
            {
                view.AddBindGrid(bindGrid);
            }
            else if (child is DataGrid dataGrid)
            {
                view.AddDataGrid(dataGrid);
            }

            // 재귀 탐색
            FindAndRegisterElements(view, child);
        }
    }
}