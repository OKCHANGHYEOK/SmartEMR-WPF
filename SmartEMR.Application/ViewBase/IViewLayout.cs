using SmartEMR.Application.Core;

namespace SmartEMR.Application.ViewBase;

public interface IViewLayout
{
    public abstract Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request);
    public virtual void RefreshViewData(object? parameter = null) { }
    public virtual bool ClosingFloatPanel()
    {
        return true;
    }
}
