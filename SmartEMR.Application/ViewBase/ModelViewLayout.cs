using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using System.Windows.Controls;

namespace SmartEMR.Application.ViewBase;

public abstract class ModelViewLayout<TVM> : UserControl, IViewLayout 
                                             where TVM : class, IVIewModel
{
    public TVM vm = default!;

    private readonly List<BindGrid> _bindGrids = new();
    public IReadOnlyList<BindGrid> BindGrids => _bindGrids;

    public ModelViewLayout()
    {
        Initialize();

        this.Loaded += (s, e) => 
        {
            vm = (TVM)this.DataContext;

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
