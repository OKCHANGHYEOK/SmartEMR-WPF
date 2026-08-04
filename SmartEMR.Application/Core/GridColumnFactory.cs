using DevExpress.Utils;
using DevExpress.Xpf.Grid;
using SmartEMR.Application.Resources;
using SmartEMR.Application.Xpf;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SmartEMR.Application.Core;

public class GridColumnFactory
{
    public static GridColumn Create(ColumnItem item)
    {
        StyleGridColumn element = new StyleGridColumn();

        if (item.ColumnStyle != null)
        {
            SetHorizontalAlignment(item);
        }

        element.FieldName = item.FieldName;
        element.Header = item.Header;
        element.Width = item.ColumnWidth > 0 ? new GridColumnWidth(item.ColumnWidth, GridColumnUnitType.Pixel) : new GridColumnWidth(1, GridColumnUnitType.Star);
        element.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
        element.CellTemplate = GetCellTemplate(item);
        element.ColumnItem = item;
        element.AllowSorting = item.AllowSorting ? DefaultBoolean.True : DefaultBoolean.False;

        return element;
    }

    private static void SetHorizontalAlignment(ColumnItem item)
    {
        item.HorizontalAlignment = item.ColumnStyle switch
        {
            ColumnStyle.Name => HorizontalAlignment.Left,
            ColumnStyle.Code => HorizontalAlignment.Center,
            ColumnStyle.YYMMDD => HorizontalAlignment.Center,
            ColumnStyle.Sum => HorizontalAlignment.Right,
            _ => HorizontalAlignment.Left
        };
    }

    private static DataTemplate? GetCellTemplate(ColumnItem item)
    {
        var template = new DataTemplate();

        if (item.CellTemplateType != null)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(item.CellTemplateType))
            {
                Debug.WriteLine($"{item.CellTemplateType.Name}은 FrameworkElement를 상속해야 합니다.");
                return null;
            }

            template = CreateTemplate(item.CellTemplateType);
        }
        else
        {
            string resourceKey = item.ColumnType switch
            {
                ColumnType.Label => "GridColumnLabelTemplate",
                ColumnType.TextBox => "GridColumnTextBoxTemplate",
                ColumnType.CheckBox => "GridColumnCheckBoxTemplate",
                ColumnType.TextLink => "GridColumnTextLinkTemplate",
                _ => "GridColumnLabelTemplate" // 기본값
            };

            template = SmartResourceDictionary.GetStaticResource<DataTemplate>(TargetResource.DataGridCell, resourceKey);
        }

        return template;
    }

    private static DataTemplate CreateTemplate(Type templateType)
    {
        var presenter = new FrameworkElementFactory(typeof(ContentPresenter));
        var innerTemplate = new DataTemplate();
        innerTemplate.VisualTree = new FrameworkElementFactory(templateType);

        presenter.SetBinding(ContentPresenter.ContentProperty, new Binding("RowData.Row"));
        presenter.SetValue(ContentPresenter.ContentTemplateProperty, innerTemplate);

        return new DataTemplate { VisualTree = presenter };
    }
}
