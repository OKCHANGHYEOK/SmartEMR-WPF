using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Views;
using SmartEMR.Application.Xpf;
using System.Windows;

namespace SmartEMR.Application.Core;

public class UIManager
{
    private static readonly Lazy<UIManager> _instance = new(() => new UIManager());
    public static UIManager Instance => _instance.Value;

    private readonly List<UIWindow> _activeWindows = new();
    private readonly List<ViewLayout> _activeViews = new();

    private UIManager() {}

    public IReadOnlyList<ViewLayout> Views => _activeViews;

    public UIWindow? CurrentWindow
    {
        get
        {
            var window = _activeWindows.LastOrDefault(w => w.IsActive || w.IsFocused);

            if (window == null) window = _activeWindows.LastOrDefault();

            return window;
        }
    }

    public ViewLayout? CurrentView
    {
        get
        {
            if (_activeViews.Any(v => v.IsPopupView))
            {
                return _activeViews.LastOrDefault(v => v.IsPopupView);
            }
            else
            {
                var windows = _activeWindows.OfType<vLayout>();

                if (!windows.Any())
                {
                    return _activeViews.FirstOrDefault();
                }

                return windows.FirstOrDefault()?.MainContent as ViewLayout;
            }
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

    public void RegisterView(ViewLayout view)
    {
        if (!_activeViews.Contains(view))
        {
            _activeViews.Add(view);

            if (view is FrameworkElement fe)
            {
                fe.Unloaded += (s, e) =>
                {
                    _activeViews.Remove(view);

                    if (view is IDisposable disp)
                    {
                        disp.Dispose(true);
                    } 
                };
            }
        }
    }

    public void CloseWindow(TargetWindowType targetWindowType) 
    {
        switch (targetWindowType)
        {
            case TargetWindowType.CurrentWindow:

                if (CurrentWindow != null)
                {
                    CurrentWindow.DialogResult= true;
                    CurrentWindow.Close();
                }
                else
                {
                    FrameworkElement? pageView = CurrentView as FrameworkElement;

                    if (pageView != null)
                    {
                        var targetWindow = Window.GetWindow(pageView);
                        
                        if (targetWindow != null)
                        {
                            targetWindow.DialogResult = true;
                            targetWindow.Close();
                        }
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
