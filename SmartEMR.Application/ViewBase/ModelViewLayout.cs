using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.ViewBase;

public abstract class ViewLayout : CustomControl, IViewLayout
{
    public bool IsPopupView { get; set; } = false;

    public abstract IReadOnlyList<BindGrid> BindGrids { get; }

    public abstract Task OnBindGrid_BindClick(object sender, BindClickEventArgs e);
    public abstract Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request);

    public ViewLayout()
    {
        SmartUI.Messenger.Register(this, this.ReceiveMessage);
    }
}

public abstract partial class ModelViewLayout<T> : ViewLayout, IDisposable where T : class
{
    public T vm = default!;
    public T Model = default!;

    private readonly List<BindGrid> _bindGrids = new();
    public override IReadOnlyList<BindGrid> BindGrids => _bindGrids;

    public bool disposed { get; set; }

    protected abstract void Initialize();

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

        this.Loaded += (s, e) => 
        {
            RegisterElement();

            SmartUI.UIManager.RegisterView(this);
        };
    }

    private void RegisterElement()
    {
        if (this.Content is DependencyObject obj)
        {
            FindAndRegisterBindGrids(obj);
        }
    }

    private void FindAndRegisterBindGrids(DependencyObject parent)
    {
        int childCount = VisualTreeHelper.GetChildrenCount(parent);

        for (int i = 0; i < childCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);

            if (child is BindGrid bindGrid)
            {
                if (!_bindGrids.Contains(bindGrid))
                {
                    bindGrid.BindGrid_BindClickEvent += OnBindClick_ModelViewLayout;
                    _bindGrids.Add(bindGrid);
                }
            }

            FindAndRegisterBindGrids(child);
        }
    }

    public void Dispose(bool disposedValue)
    {
        if (!disposedValue || disposed) return;

        disposed = true;

        SmartMVVM.Common.DisposeControl(this);
    }
}

#region "BindGrid"
public abstract partial class ModelViewLayout<T>
{
    public override abstract Task OnBindGrid_BindClick(object sender, BindClickEventArgs e);

    private async void OnBindClick_ModelViewLayout(object sender, BindClickEventArgs e)
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

public abstract partial class ModelViewLayout<T>
{
    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse();

        return response;
    }
}

#endregion 
