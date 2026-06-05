using SmartEMR.Application.Common;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartEMR.Application.Core;

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

    private readonly ObservableCollection<FloatPanel> _activePopups = new();
    public  ObservableCollection<FloatPanel> Popups => _activePopups;

    public ViewLayout? CurrentView
    {
        get
        {
            if (_activeViews.Any(v => v.IsPopupView))
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

    public void RegisterView(ViewLayout view)
    {
        if (_activeViews.Contains(view)) return;

        _activeViews.Add(view);

        var mv = view as ModelViewLayout;
        if (mv == null) return;

        FindAndRegisterBindGrids(mv);

        if (view is FrameworkElement fe)
        {
            fe.Unloaded += (s, e) => UnRegisterView(mv);
        }
    }

    public void AddFloatPanel(FloatPanel panel)
    {
        if (!_activePopups.Contains(panel))
        {
            _activePopups.Add(panel);
        }
    }

    public void RemoveFloatPanel(FloatPanel panel)
    {
        if (_activePopups.Contains(panel))
        {
            _activePopups.Remove(panel);
        }
    }

    private static ViewLayout? GetCurrentView()
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

        return SmartUI.CurrentPageView;
    }

    private void UnRegisterView(ModelViewLayout view)
    {
        _activeViews.Remove(view);

        foreach (var bindGrid in view.BindGrids)
        {
            bindGrid.BindGrid_BindClickEvent -= view.HandleBindGridClick;
        }

        view.Dispose(true); // IDisposable 명시적 캐스팅 대신 직접 호출
    }

    private void FindAndRegisterBindGrids(ModelViewLayout view, DependencyObject? parent = null)
    {
        parent ??= view as DependencyObject;

        int childCount = VisualTreeHelper.GetChildrenCount(parent);

        for (int i = 0; i < childCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);

            if (child is BindGrid bindGrid)
            {
                if (!view.BindGrids.Contains(bindGrid))
                {
                    bindGrid.BindGrid_BindClickEvent += view.HandleBindGridClick;
                    bindGrid.BindGrid_BindItemChangedEvent += view.OnBindGrid_BindItemChanged;

                    // [개선] 리플렉션 없이 비제네릭 부모 클래스의 internal 메서드를 바로 호출
                    view.AddBindGrid(bindGrid);
                }
            }

            // 재귀 탐색
            FindAndRegisterBindGrids(view, child);
        }
    }
}