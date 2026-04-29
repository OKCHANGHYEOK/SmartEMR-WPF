using SmartEMR.Application.ViewBase;
using System.Windows;

namespace SmartEMR.Application.Xpf;

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
}
