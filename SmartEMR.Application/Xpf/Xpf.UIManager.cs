namespace SmartEMR.Application.Xpf;

public class UIManager
{
    private static readonly Lazy<UIManager> _instance = new(() => new UIManager());
    public static UIManager Instance => _instance.Value;

    private readonly List<UIWindow> _activeWindows = new ();

    private UIManager() {}

    public void RegisterWindow(UIWindow window)
    {
        if (!_activeWindows.Contains(window))
        {
            _activeWindows.Add(window);
            window.Closed += (s, e) => _activeWindows.Remove(window);
        }
    }

    public UIWindow? CurrentWindow
    {
        get
        {
            return _activeWindows.FirstOrDefault(x => x.IsActive) ?? _activeWindows.LastOrDefault();
        }
    }
}
