using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Views.Shared;

namespace SmartEMR.Application.Core;

public class ViewMessenger
{
    private static readonly Lazy<ViewMessenger> _instance = new(() => new ViewMessenger());

    public static ViewMessenger Instance => _instance.Value;

    // 내부 저장소는 공통 베이스 클래스 핸들러로 관리
    private readonly List<(ViewLayout View, Func<ViewMessageRequest, Task<ViewMessageResponse?>> Handler)> _subscribers = new();

    public void Register(ViewLayout view, Func<ViewMessageRequest, Task<ViewMessageResponse?>> handler)
    {
        var sub = _subscribers.FirstOrDefault(v => v.View == view);
        if (sub.View is null)
        {
            _subscribers.Add((view, handler));
        }
    }

    public void UnRegister(ViewLayout view)
    {
        var sub = _subscribers.FirstOrDefault(v => v.View == view);
        if (sub.View != null)
        {
            _subscribers.Remove(sub);
        }
    }

    public async Task<ViewMessageResponse?> SendMessage(string action, object? parameter = null, object[]? parameters = null, TargetViewType viewType = TargetViewType.CurrentView)
    {
        var request = new ViewMessageRequest { MessageAction = action, MessageParameter = parameter, MessageParameters = parameters };
        ViewLayout? targetView = SmartUI.UIManager.GetTargetView(viewType);

        var sub = _subscribers.FirstOrDefault(s => s.View == targetView);

        return sub.Handler != null ? await sub.Handler(request) : null;
    }

    public async Task<ViewMessageResponse<T>?> SendMessage<T>(string action, object? parameter = null, object[]? parameters = null, TargetViewType viewType = TargetViewType.CurrentView, object? sender= null) where T : class
    {
        // 일반 SendMessage를 먼저 호출
        var response = await SendMessage(action, parameter, parameters, viewType);

        if (response == null) return null;

        // 결과를 제네릭 응답 객체로 래핑하여 반환
        return new ViewMessageResponse<T>
        {
            MessageAction = response.MessageAction,
            IsSuccess = response.IsSuccess,
            Item = response.Item as T
        };
    }

    /// <summary>
    /// vSearchView 에 대한 메시지 송신
    /// </summary>
    /// <param name="viewType"></param>
    /// <returns></returns>
    public async Task<ViewMessageResponse?> SendMessageToSearchView(string action, object? parameter = null)
    {
        var response = new ViewMessageResponse() { IsSuccess = true};
        var vSearchView = SmartUI.GetViewLayout<vSearchView>();
        if (vSearchView == null) return null;

        await vSearchView.ReceiveMessage(new ViewMessageRequest { MessageAction = action, MessageParameter = parameter});

        return response;
    }
}

public class ViewMessageRequest
{
    public string? MessageAction { get; set; } 
    public object? MessageParameter { get; set; }
    public object[]? MessageParameters { get; set; }
}

public class ViewMessageResponse
{
    public string? MessageAction { get; set; }
    public object? Item { get; set; }
    public List<object>? Items { get; set; }
    public bool? IsSuccess { get; set; }
}

public class ViewMessageResponse<T>() : ViewMessageResponse where T : class
{
    public new T? Item;
    public new List<T>? Items;
}