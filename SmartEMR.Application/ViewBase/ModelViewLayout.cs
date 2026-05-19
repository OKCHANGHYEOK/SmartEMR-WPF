using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewBase;

public abstract class ViewLayout : CustomControl, IViewLayout
{
    public bool IsPopupView { get; set; } = false;
    public abstract IReadOnlyList<BindGrid> BindGrids { get; }

    public abstract Task OnBindGrid_BindClick(object sender, BindClickEventArgs e);
    public abstract Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request);
    
    public ViewLayout()
    {
        // 뷰가 로드될 때 UIManager에게 자신을 등록 (비주얼 트리 탐색 및 BindGrid 등록 위임)
        this.Loaded += (s, e) =>
        {
            SmartUI.RegisterView(this);
        };
    }
}

public abstract partial class ModelViewLayout : ViewLayout, IDisposable
{
    public bool disposed { get; set; }

    protected readonly List<BindGrid> _bindGrids = new();
    public override IReadOnlyList<BindGrid> BindGrids => _bindGrids;

    protected abstract void Initialize();

    // internal로 선언되어 있으므로 동일 어셈블리 내의 UIManager가 리플렉션 없이 호출 가능
    internal void AddBindGrid(BindGrid bindGrid)
    {
        if (!_bindGrids.Contains(bindGrid))
        {
            _bindGrids.Add(bindGrid);
        }
    }

    public virtual void Dispose(bool disposedValue)
    {
        if (!disposedValue || disposed) return;
        disposed = true;
        SmartMVVM.Common.DisposeControl(this);
    }
}


#region "BindGrid"
public abstract partial class ModelViewLayout
{
    public override abstract Task OnBindGrid_BindClick(object sender, BindClickEventArgs e);

    // ViewLayout의 추상 메서드 구현: UIManager가 이벤트를 라우팅해주는 통로
    public async void HandleBindGridClick(object sender, BindClickEventArgs e)
    {
        if (await SmartMVVM.PreventClickFiring(e)) return;

        try
        {
            await OnBindGrid_BindClick(sender, e);
        }
        finally
        {
            SmartMVVM.ReleaseClick();
        }
    }
}
#endregion

#region "Message"
public abstract partial class ModelViewLayout
{
    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        return new ViewMessageResponse();
    }
}
#endregion

public abstract partial class ModelViewLayout<T> : ModelViewLayout where T : class
{
    public T vm = default!;
    public T Model = default!;

    public ModelViewLayout()
    {
        if (typeof(IBaseViewModel).IsAssignableFrom(typeof(T)))
        {
            vm = Activator.CreateInstance<T>();
            this.SetValue(DataContextProperty, vm);
        }
        else if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            Model = Activator.CreateInstance<T>();
            this.SetValue(DataContextProperty, Model);
        }

        Initialize();
    }
}