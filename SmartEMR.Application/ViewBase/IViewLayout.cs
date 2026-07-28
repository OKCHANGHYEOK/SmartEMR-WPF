using SmartEMR.Application.Core;

namespace SmartEMR.Application.ViewBase;

public interface IViewLayout
{
    public Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request);
    public void SetViewData(object? parameter = null);
    public bool ClosingFloatPanel();
}
