using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.ViewBase;

public interface IViewLayout
{
    IReadOnlyList<BindGrid> BindGrids { get; }

    Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e);
}
