using DevExpress.XtraRichEdit.API.Layout;
using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using System.Windows;
using System.Windows.Controls;

namespace SmartEMR.Application.ViewBase;

public abstract class ModelViewLayout<TVM> : UserControl, IViewLayout, IDisposable
                                             where TVM : class
{
    public TVM vm = default!;

    private readonly List<BindGrid> _bindGrids = new();
    public IReadOnlyList<BindGrid> BindGrids => _bindGrids;

    public bool disposed { get; set; }

    public ModelViewLayout()
    {
        Initialize();

        vm = Activator.CreateInstance<TVM>();
        
        this.SetValue(DataContextProperty, vm);
        this.GetType().GetMethod("InitializeComponent")?.Invoke(this, null);

        this.Loaded += (s, e) => 
        {
            SmartUI.UIManager.RegisterView(this);
        };
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

    protected abstract void Initialize();

    public void Dispose(bool disposedValue)
    {
        if (!disposedValue || disposed) return;

        disposed = true;

        SmartMVVM.Common.DisposeControl(this);
    }
}
