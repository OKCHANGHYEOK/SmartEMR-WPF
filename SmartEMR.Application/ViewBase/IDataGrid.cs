using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.ViewBase;
internal interface IDataGrid
{
    IReadOnlyList<DataGrid> DataGrids { get; }

    void OnDataGrid_DataItemChanged(object? sender, DataItemChangedEventArgs e);
}
