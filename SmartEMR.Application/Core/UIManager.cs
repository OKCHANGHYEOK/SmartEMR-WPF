using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using System.Windows;

namespace SmartEMR.Application.Core;

public class UIManager
{
    private static readonly Lazy<UIManager> _instance = new(() => new UIManager());
    public static UIManager Instance => _instance.Value;

    private readonly List<UIWindow> _activeWindows = new();
    private readonly List<IViewLayout> _activeViews = new();

    private UIManager() {}

    public UIWindow? CurrentWindow
    {
        get
        {
            return _activeWindows.FirstOrDefault(x => x.IsActive) ?? _activeWindows.LastOrDefault();
        }
    }

    public IViewLayout? CurrentPageView
    {
        get
        {
            return  _activeViews.LastOrDefault();
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

    public void RegisterView(IViewLayout view)
    {
        if (!_activeViews.Contains(view))
        {
            _activeViews.Add(view);

            if (view is FrameworkElement fe)
            {
                fe.Unloaded += (s, e) => _activeViews.Remove(view);
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
                    FrameworkElement? pageView = CurrentPageView as FrameworkElement;

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
