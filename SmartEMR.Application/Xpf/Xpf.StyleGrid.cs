using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SmartEMR.Application.Xpf;

// [ContentProperty]를 사용해 XAML에서 추가되는 자식이 어디로 갈지 지정합니다.
[ContentProperty(nameof(Children))]
public class StyleGrid : Grid
{

    #region "Dependency Properties"
    public static readonly DependencyProperty LayoutDefinitionsProperty = DependencyProperty.Register(
        nameof(LayoutDefinitions),
        typeof(string),
        typeof(StyleGrid),
        new PropertyMetadata("1,1", OnLayoutDefinitionsChanged));
    #endregion
        
    public string LayoutDefinitions
    {
        get => (string)GetValue(LayoutDefinitionsProperty);
        set => SetValue(LayoutDefinitionsProperty, value);
    }

    private static void OnLayoutDefinitionsChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        if (element is StyleGrid styleGrid)
        {
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
    }

    public Grid LayoutRoot { get; } = new Grid();

    public StyleGrid()
    {
        base.Children.Add(LayoutRoot);
    
    }

    // 외부에서 접근하는 Children 속성을 LayoutRoot의 Children으로 연결(Shadowing)합니다.
    public new UIElementCollection Children => LayoutRoot.Children;

    public void SetLayout(int col, int row)
    {
        LayoutRoot.ColumnDefinitions.Clear();
        LayoutRoot.RowDefinitions.Clear();

        for (int i = 0; i < col; i++)
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition());

        for (int i = 0; i < row; i++)
            LayoutRoot.RowDefinitions.Add(new RowDefinition());
    }

    // 이제 이 메서드는 내부 전용이나 보조 용도로만 써도 됩니다.
    public void AddElement(UIElement element, int col, int row)
    {
        Grid.SetColumn(element, col);
        Grid.SetRow(element, row);
        LayoutRoot.Children.Add(element);
    }
}