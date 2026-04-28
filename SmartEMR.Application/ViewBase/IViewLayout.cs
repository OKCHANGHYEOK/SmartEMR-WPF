using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.ViewBase;

public interface IViewLayout
{
    IReadOnlyList<BindGrid> BindGrids { get; }

    void AddBindGrid(BindGrid bindGrid);
    void OnBindGrid_BindClick(object sender, BindClickEventArgs e);
}
