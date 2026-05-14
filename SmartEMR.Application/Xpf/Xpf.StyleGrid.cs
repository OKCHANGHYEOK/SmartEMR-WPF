using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SmartEMR.Application.Xpf;

// [ContentProperty]를 사용해 XAML에서 추가되는 자식이 어디로 갈지 지정합니다.
[ContentProperty(nameof(Children))]
public partial class StyleGrid : ContentControl
{

    #region "Dependency Properties"

    // LayoutDefinitions (이전 코드에 있던 것 유지)
    public string LayoutDefinitions
    {
        get => (string)GetValue(LayoutDefinitionsProperty);
        set => SetValue(LayoutDefinitionsProperty, value);
    }

    public static readonly DependencyProperty LayoutDefinitionsProperty = DependencyProperty.Register(
        nameof(LayoutDefinitions),
        typeof(string),
        typeof(StyleGrid),
        new PropertyMetadata("1,1", OnLayoutDefinitionsChanged));

    // ColumnSizeDefinitions
    public string ColumnSizeDefinitions
    {
        get => (string)GetValue(ColumnSizeDefintionsProperty);
        set => SetValue(ColumnSizeDefintionsProperty, value);
    }

    public static readonly DependencyProperty ColumnSizeDefintionsProperty = DependencyProperty.Register(
        nameof(ColumnSizeDefinitions),
        typeof(string),
        typeof(StyleGrid),
        new PropertyMetadata("", OnColumnSizeDefinitionsChanged));

    // RowSizeDefinitions
    public string RowSizeDefinitions
    {
        get => (string)GetValue(RowSizeDefintionsProperty);
        set => SetValue(RowSizeDefintionsProperty, value);
    }

    public static readonly DependencyProperty RowSizeDefintionsProperty = DependencyProperty.Register(
        nameof(RowSizeDefinitions),
        typeof(string),
        typeof(StyleGrid),
        new PropertyMetadata("", OnRowSizeDefinitionsChanged));

    public bool ShowGridLines
    {
        get => LayoutRoot.ShowGridLines;
        set => LayoutRoot.ShowGridLines = value;
    }

    #endregion

    public Grid LayoutRoot { get; } = new Grid();

    // 외부에서 접근하는 Children 속성을 LayoutRoot의 Children으로 연결(Shadowing)합니다.
    public UIElementCollection Children => LayoutRoot.Children;

    public StyleGrid()
    {
        this.Content = LayoutRoot;

        LayoutRoot.SetBinding(Grid.BackgroundProperty, new System.Windows.Data.Binding(nameof(Background)) { Source = this });
    }

    public void AddElement(UIElement element, int col, int row, int colspan, int rowspan)
    {
        Grid.SetColumn(element, col);
        Grid.SetRow(element, row);
        Grid.SetColumnSpan(element, colspan);
        Grid.SetRowSpan(element, rowspan);

        LayoutRoot.Children.Add(element);
    }

    public void SetLayout(int col, int row)
    {
        LayoutRoot.ColumnDefinitions.Clear();
        LayoutRoot.RowDefinitions.Clear();

        for (int i = 0; i < col; i++)
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition());

        for (int i = 0; i < row; i++)
            LayoutRoot.RowDefinitions.Add(new RowDefinition());
    }

    public void SetColumnWidth(int index, GridLength width)
    {
        if (index >= 0 && index < LayoutRoot.ColumnDefinitions.Count)
        {
            LayoutRoot.ColumnDefinitions[index].Width = width;
        }
    }

    public void SetRowHeight(int index, GridLength height)
    {
        if (index >= 0 && index < LayoutRoot.RowDefinitions.Count)
        {
            LayoutRoot.RowDefinitions[index].Height = height;
        }
    }

    private static void OnLayoutDefinitionsChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        if (element is StyleGrid styleGrid == false) return;

        var definitions = (string)e.NewValue;
        var arrDefinitions = definitions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        if (arrDefinitions == null || arrDefinitions.Length < 2)
        {
            return;
        }

        var col = arrDefinitions[0];
        var row = arrDefinitions[1];

        if (Int32.TryParse(col, out int iCol) && Int32.TryParse(row, out int iRow))
        {
            styleGrid.SetLayout(iCol, iRow);
        }
    }

    private static void OnColumnSizeDefinitionsChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        if (element is StyleGrid styleGrid == false) return;

        var definitions = (string)e.NewValue;
        var arrDefinitions = definitions.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (arrDefinitions == null || arrDefinitions.Length < 1) return;

        var converter = new GridLengthConverter();

        for (int i = 0; i < arrDefinitions.Length; i++)
        {
            if (converter.ConvertFromString(arrDefinitions[i]) is GridLength gl)
            {
                styleGrid.SetColumnWidth(i, gl);
            }
        }
    }

    private static void OnRowSizeDefinitionsChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        if (element is StyleGrid styleGrid == false) return;

        var definitions = (string)e.NewValue;
        var arrDefinitions = definitions.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (arrDefinitions == null || arrDefinitions.Length < 1) return;

        var converter = new GridLengthConverter();

        for (int i = 0; i < arrDefinitions.Length; i++)
        {
            if (converter.ConvertFromString(arrDefinitions[i]) is GridLength gl)
            {
                styleGrid.SetRowHeight(i, gl);
            }
        }
    }
}