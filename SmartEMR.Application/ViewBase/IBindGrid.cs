using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.ViewBase;

internal interface IBindGrid
{
    IReadOnlyList<BindGrid> BindGrids { get; }

    Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e);
    void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e);
}
