using SmartEMR.Application.ViewBase;

namespace SmartEMR.Application.Core;

public enum TargetViewType
{
    CurrentView = 0,        // 메세지를 전송한 뷰
    PageView = 1,           // 메세지를 전송한 뷰가 포함된 페이지(부모뷰)에 해당하는 뷰
    PreFloatView= 2,        // 팝업인 경우 해당 팝업의 이전 팝업
    RootView = 3            // vLayout 의 메인컨텐츠
}

public class ViewMessenger
{
    private static readonly Lazy<ViewMessenger> _instance = new(() => new ViewMessenger());

    public static ViewMessenger Instance => _instance.Value;

    private readonly List<(ViewLayout View, Func<ViewMessageRequest, Task<ViewMessageResponse>> Handler)> _subscribers = new();

    public void Register(ViewLayout view, Func<ViewMessageRequest, Task<ViewMessageResponse>> handler)
    {
        _subscribers.Add((view, handler));
    }

    public async Task<ViewMessageResponse?> SendMessage(string action, object? parmeter = null, TargetViewType viewType = TargetViewType.CurrentView)
    {
        if (string.IsNullOrWhiteSpace(action)) return null;

        var request = new ViewMessageRequest
        {
            MessageAction = action,
            MessageParameter = parmeter,
        };

        ViewLayout? targetView = null;

        switch (viewType)
        {
            case TargetViewType.CurrentView:
                break;

            case TargetViewType.PageView:
                break;

            case TargetViewType.PreFloatView:
                break;

            case TargetViewType.RootView:
                break;
        }

        if (targetView == null) return null;

        var sub = _subscribers.FirstOrDefault(s => s.View == targetView);

        if (sub.Handler != null)
        {
            return await sub.Handler(request);
        }

        return null;
    }
}

public class ViewMessageRequest()
{
    public string? MessageAction { get; set; } 
    public object? MessageParameter { get; set; }
}

public class ViewMessageResponse()
{
    public string? MessageAction { get; set; }
    public object? Item { get; set; }
    public object[]? Items { get; set; }
    public bool? IsSuccess { get; set; }
}