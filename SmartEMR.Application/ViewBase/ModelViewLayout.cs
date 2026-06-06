using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Input;

namespace SmartEMR.Application.ViewBase;

public abstract class ViewLayout : CustomControl, IViewLayout
{
    public static readonly DependencyProperty ViewTitleProperty =
        DependencyProperty.Register("ViewTitle", typeof(string), typeof(ViewLayout), new PropertyMetadata("알림"));

    public string ViewTitle
    {
        get => (string)GetValue(ViewTitleProperty);
        set => SetValue(ViewTitleProperty, value);
    }

    public static readonly DependencyProperty ViewSizeProperty =
        DependencyProperty.Register("ViewSize", typeof(Size), typeof(ViewLayout), new PropertyMetadata(new Size(400, 300)));

    public Size ViewSize
    {
        get => (Size)GetValue(ViewSizeProperty);
        set => SetValue(ViewSizeProperty, value);
    }

    public bool IsPopupView { get; set; } = false;
    public abstract IReadOnlyList<BindGrid> BindGrids { get; }

    public abstract Task OnBindGrid_BindClick(object sender, BindClickEventArgs e);
    public abstract void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e);
    
    public abstract Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request);
    public virtual void RefreshViewData(object? parameter = null) {}
    public virtual bool ClosingFloatPanel() 
    {
        return true;
    }

    public ViewLayout()
    {
        this.PreviewKeyDown += OnPreviewKeyDown_ViewLayout;
        this.Loaded += (s, e) => SmartUI.RegisterView(this);
    }

    public ViewLayout(object parameter) : this() => this.Loaded += (s, e) => RefreshViewData(parameter);

    public void OnPreviewKeyDown_ViewLayout(object sender, KeyEventArgs e)
    {
        var vl = sender as ViewLayout;
        if (vl == null) return;

        if (IsPopupView && e.Key == Key.Escape)
        {
            if (ClosingFloatPanel())
            {
                SmartUI.CloseFloatPanel(this);
            }
        }
    }
}

public abstract partial class ModelViewLayout : ViewLayout, IDisposable
{
    public bool disposed { get; set; }

    protected readonly List<BindGrid> _bindGrids = new();
    public override IReadOnlyList<BindGrid> BindGrids => _bindGrids;

    protected abstract void Initialize();
    protected virtual void SetBindGrid() { }

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
    public override abstract void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e);

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
    // 외부에서 언제든 접근할 수 있도록 래퍼 프로퍼티나 필드로 유지하되, 
    // DataContext와 항상 일치하도록 만듭니다.
    public T vm => this.DataContext as T ?? default!;
    public T Model => this.DataContext as T ?? default!;

    public ModelViewLayout() : base()
    {
        if (typeof(IVIewModel).IsAssignableFrom(typeof(T)))
        {
            this.SetValue(DataContextProperty, Activator.CreateInstance<T>());
        }
        else if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            this.SetValue(DataContextProperty, Activator.CreateInstance<T>());
        }

        this.Loaded += async (s, e) =>
        {
            Initialize();
            SetBindGrid();
        };
    }
}