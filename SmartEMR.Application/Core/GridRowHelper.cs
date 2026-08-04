using DevExpress.Xpf.Grid;
using System.Windows;

namespace SmartEMR.Application.Core;

public class GridRowHelper
{
    public static GridColumn GetColumn(DependencyObject source, TableView view)
    {
        return view.CalcHitInfo(source).Column;
    }

    public static object? GetRowData(DependencyObject source, TableView view, GridControl gridControl)
    {
        var hitInfo = view.CalcHitInfo(source);
        if (hitInfo == null || !hitInfo.InRowCell) return null;

        return gridControl.GetRow(hitInfo.RowHandle);
    }
}
