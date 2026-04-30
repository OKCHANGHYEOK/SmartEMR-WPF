using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows.Controls;

namespace SmartEMR.Application.ViewBase;

public abstract class ModelViewLayout<TVM> : UserControl, IViewLayout 
                                             where TVM : class, IVIewModel
{
    public TVM vm = default!;
    public object Model = default!;

    private readonly List<BindGrid> _bindGrids = new();
    public IReadOnlyList<BindGrid> BindGrids => _bindGrids;

    public ModelViewLayout()
    {
        Initialize();

        vm = Activator.CreateInstance<TVM>();
        
        Model = vm.Model!;

        this.SetValue(DataContextProperty, vm);

        this.GetType().GetMethod("InitializeComponent")?.Invoke(this, null);

        this.Loaded += (s, e) => 
        {
            SmartUI.UIManager.RegisterView(this);
        };
    }

    public void AddBindGrid(BindGrid bindGrid)
    {
        if (!_bindGrids.Contains(bindGrid))
        {
            _bindGrids.Add(bindGrid);
            bindGrid.BindGrid_BindClickEvent += OnBindGrid_BindClick;
        }
    }

    protected abstract void Initialize();

    public abstract void OnBindGrid_BindClick(object sender, BindClickEventArgs e);
}
