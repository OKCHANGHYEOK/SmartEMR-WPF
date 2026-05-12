using DevExpress.Dialogs.Core.ViewModel;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.ViewBase;

public abstract class ModelViewLayout<T> : CustomControl, IViewLayout, IDisposable
                                             where T : class
{
    public T vm = default!;
    public T Model = default!;

    private readonly List<BindGrid> _bindGrids = new();
    public IReadOnlyList<BindGrid> BindGrids => _bindGrids;

    public bool disposed { get; set; }

    protected abstract void Initialize();

    public ModelViewLayout()
    {
        Initialize();

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

    public abstract Task OnBindGrid_BindClick(object sender, BindClickEventArgs e);

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
