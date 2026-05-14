using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Views;

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

    // 내부 저장소는 공통 베이스 클래스 핸들러로 관리
    private readonly List<(ViewLayout View, Func<ViewMessageRequest, Task<ViewMessageResponse>> Handler)> _subscribers = new();

    public void Register(ViewLayout view, Func<ViewMessageRequest, Task<ViewMessageResponse>> handler)
    {
        _subscribers.Add((view, handler));
    }

    public async Task<ViewMessageResponse?> SendMessage(string action, object? parameter = null, TargetViewType viewType = TargetViewType.CurrentView)
    {
        var request = new ViewMessageRequest { MessageAction = action, MessageParameter = parameter };
        ViewLayout? targetView = GetTargetView(viewType); // 로직 분리 추천

        if (targetView == null) return null;

        var sub = _subscribers.FirstOrDefault(s => s.View == targetView);
        return sub.Handler != null ? await sub.Handler(request) : null;
    }

    public async Task<ViewMessageResponse<T>?> SendMessage<T>(string action, object? parameter = null, TargetViewType viewType = TargetViewType.CurrentView) where T : class
    {
        // 일반 SendMessage를 먼저 호출
        var response = await SendMessage(action, parameter, viewType);

        if (response == null) return null;

        // 결과를 제네릭 응답 객체로 래핑하여 반환
        return new ViewMessageResponse<T>
        {
            MessageAction = response.MessageAction,
            IsSuccess = response.IsSuccess,
            Item = response.Item as T
        };
    }

    // TargetViewType에 따른 타겟 추출 로직 (UIManager 활용)
    private ViewLayout? GetTargetView(TargetViewType viewType)
    {
        return viewType switch
        {
            TargetViewType.CurrentView => SmartUI.UIManager.CurrentView as ViewLayout,
            TargetViewType.RootView => (SmartUI.UIManager.CurrentWindow as vLayout)?.MainContent as ViewLayout,
            _ => null
        };
    }
}

public class ViewMessageRequest()
{
    public string? MessageAction { get; set; } 
    public object? MessageParameter { get; set; }
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