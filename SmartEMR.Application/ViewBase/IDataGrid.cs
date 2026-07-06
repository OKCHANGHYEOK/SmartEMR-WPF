using SmartEMR.Application.Xpf;
using System.Windows;

namespace SmartEMR.Application.ViewBase;
internal interface IDataGrid
{
    IReadOnlyList<DataGrid> DataGrids { get; }

    void OnDataGrid_DataItemChanged(object? sender, DataItemChangedEventArgs e);
    void OnDataGrid_PopupMenuOpening(object? sender, PopupMenuOpeningEventArgs e); 
    void OnDataGridPopupMenu_PopupMenuItemClicked(object? sender, PopupMenuItemClickEventArgs e);
}
