using SmartEMR.Application.Xpf;
using System.Windows.Controls;

namespace SmartEMR.Application.ViewBase;

public class ModelViewLayout<T> : UserControl, IViewLayout where T : class
{

    private readonly List<BindGrid> _bindGrids = new();
    public IReadOnlyList<BindGrid> BindGrids => _bindGrids;

    public void AddBindGrid(BindGrid bindGrid)
    {
        if (!_bindGrids.Contains(bindGrid))
        {
            _bindGrids.Add(bindGrid);
            bindGrid.BindGrid_BindClickEvent += OnBindGrid_BindClick;
        }
    }

    public void OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        throw new NotImplementedException();
    }
}
