using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Input;

namespace SmartEMR.Application.ViewBase;

public abstract partial class ViewLayout : CustomControl, IViewLayout, IBindGrid, IDataGrid
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


    public ViewLayout()
    {
        this.PreviewKeyDown += OnPreviewKeyDown_ViewLayout;
        this.Loaded += (s, e) => SmartUI.RegisterView(this);
    }

    public ViewLayout(object parameter) : this()
    {
        this.Loaded += (s, e) => ((IViewLayout)this).RefreshViewData(parameter);
    }

    public abstract Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request);

    private void OnPreviewKeyDown_ViewLayout(object sender, KeyEventArgs e)
    {
        var vl = sender as ViewLayout;
        if (vl == null) return;
    }
}

// BindGrid
public abstract partial class ViewLayout
{
    public abstract IReadOnlyList<BindGrid> BindGrids { get; }

    public abstract Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e);
    public abstract void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e);
}

// DataGrid
public abstract partial class ViewLayout
{
    public abstract IReadOnlyList<DataGrid> DataGrids { get; }

    public abstract void OnDataGrid_DataItemChanged(object? sender, DataItemChangedEventArgs e);
    public abstract void OnDataGrid_ContextMenuItemClicked(object? sender, ContextMenuItemClickedEventArgs e);
}

public abstract partial class ModelViewLayout : ViewLayout, IDisposable
{
    public bool disposed { get; set; }

    protected abstract void Initialize();
    protected virtual void SetBindGrid() { }
    protected virtual void SetDataGrid() { }

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
    protected readonly List<BindGrid> _bindGrids = new();
    public override IReadOnlyList<BindGrid> BindGrids => _bindGrids;

    public override abstract Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e);
    public override abstract void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e);

    // ViewLayout의 추상 메서드 구현: UIManager가 이벤트를 라우팅해주는 통로
    public async void HandleBindGridClick(object? sender, BindClickEventArgs e)
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

    internal void AddBindGrid(BindGrid bindGrid)
    {
        if (!_bindGrids.Contains(bindGrid))
        {
            bindGrid.BindGrid_BindClickEvent += this.HandleBindGridClick;
            bindGrid.BindGrid_BindItemChangedEvent += this.OnBindGrid_BindItemChanged;

            _bindGrids.Add(bindGrid);
        }
    }
}
#endregion

#region "DataGrid"

public abstract partial class ModelViewLayout
{
    protected readonly List<DataGrid> _dataGrids = new();
    public override IReadOnlyList<DataGrid> DataGrids => _dataGrids;

    public override void OnDataGrid_DataItemChanged(object? sender, DataItemChangedEventArgs e) { }
    public override void OnDataGrid_ContextMenuItemClicked(object? sender, ContextMenuItemClickedEventArgs e) { }

    internal void AddDataGrid(DataGrid dataGrid)
    {
        if (!_dataGrids.Contains(dataGrid))
        {
            dataGrid.DataGrid_DataItemChangedEvent += this.OnDataGrid_DataItemChanged;
            dataGrid.DataGrid_ContextMenuItemClickedEvent += this.OnDataGrid_ContextMenuItemClicked;

            _dataGrids.Add(dataGrid);
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
        SetDataContext(null);
        this.Loaded += OnViewLoaded;
    }

    public ModelViewLayout(object item) : base()
    {
        SetDataContext(item);
        this.Loaded += OnViewLoaded;
    }

    private void SetDataContext(object? item)
    {
        if (typeof(IViewModel).IsAssignableFrom(typeof(T)))
        {
            if (item != null)
            {
                this.DataContext = (T)Activator.CreateInstance(typeof(T), item)!;
            }
            else
            {
                this.DataContext = Activator.CreateInstance<T>();
            }
        }
        else if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            this.DataContext = item as T ?? Activator.CreateInstance<T>();
        }
    }

    private async void OnViewLoaded(object sender, RoutedEventArgs e)
    {
        this.Loaded -= OnViewLoaded;

        if (vm is BaseViewModel bvm)
        {
            bvm.Initialize();

            await bvm.InitializeAsync();
        }

        Initialize();
        SetBindGrid();
        SetDataGrid();
    }

    public virtual void SetPatientData(Patient item) { }
}