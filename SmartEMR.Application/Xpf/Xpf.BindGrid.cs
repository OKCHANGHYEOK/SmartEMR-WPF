using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public enum BindStyle
{
    None,
    DataCell
}

[ObservableObject]
[ContentProperty(nameof(BindItems))]
public partial class BindGrid : StyleGrid, IDisposable
{
    #region Dependency Properties

    public static readonly DependencyProperty ItemSpaceProperty =
        DependencyProperty.Register(nameof(ItemSpace), typeof(int), typeof(BindGrid), new PropertyMetadata(5));

    public int ItemSpace
    {
        get => (int)GetValue(ItemSpaceProperty);
        set => SetValue(ItemSpaceProperty, value);
    }

    public static readonly DependencyProperty BindStyleProperty =
        DependencyProperty.Register(nameof(BindStyle), typeof(BindStyle), typeof(BindGrid), new PropertyMetadata(BindStyle.None));

    public BindStyle BindStyle
    {
        get => (BindStyle)GetValue(BindStyleProperty);
        set => SetValue(BindStyleProperty, value);
    }

    public static readonly DependencyProperty HeaderWidthProperty =
        DependencyProperty.Register(nameof(HeaderWidth), typeof(double), typeof(BindGrid), new PropertyMetadata(80.0));

    public double HeaderWidth
    {
        get => (double)GetValue(HeaderWidthProperty);
        set => SetValue(HeaderWidthProperty, value);
    }

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register(nameof(Model), typeof(object), typeof(BindGrid), new PropertyMetadata(null, OnModelChanged));

    public object Model
    {
        get => GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BindGrid bindGrid) bindGrid.UpdateModel();
    }

    #endregion

    public event Action<BindItem, BindClickEventArgs>? BindGrid_BindClickEvent;

    private BindItemCollection? _bindItems;
    public BindItemCollection BindItems
    {
        get
        {
            if (_bindItems == null)
            {
                _bindItems = new BindItemCollection();
                _bindItems.CollectionChanged += OnBindItemsChanged;
            }
            return _bindItems;
        }
    }

    public bool disposed { get; set; }

    public BindGrid() : base()
    {
        this.DataContextChanged += (s, e) => UpdateModel();
    }

    private void OnBindItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
        {
            foreach (var item in e.NewItems)
            {
                if (item is BindItem element)
                {
                    AddSingleBindItem(element);
                }
                else if (item is IEnumerable arrElements)
                {
                    AddArrayBindItems(arrElements);
                }
            }
        }
    }

    /// <summary>
    /// 1. 단일 BindItem 추가 처리
    /// </summary>
    private void AddSingleBindItem(BindItem item)
    {
        // 순수 컨트롤 생성
        FrameworkElement? contentElement = CreateVisualElement(item);
        if (contentElement == null) return;

        FrameworkElement finalElement = contentElement;

        // DataCell 스타일인 경우 복합 레이아웃으로 감싸기
        if (this.BindStyle == BindStyle.DataCell)
        {
            finalElement = WrapWithDataCell(item, contentElement);
        }

        // 부모 Grid(StyleGrid)에 최종 배치
        this.AddElement(finalElement, item.Col, item.Row, item.ColSpan, item.RowSpan);
    }

    /// <summary>
    /// 2. 배열(x:Array) 형태의 BindItem들 추가 처리
    /// </summary>
    private void AddArrayBindItems(IEnumerable bindItems)
    {
        var arrList = new List<BindItem>();
        foreach (var obj in bindItems)
        {
            if (obj is BindItem bItem) arrList.Add(bItem);
        }

        if (arrList.Count == 0) return;

        // 기준이 되는 첫 번째 아이템 정보
        var firstItem = arrList[0];

        // 내부 서브 패널 생성 및 아이템들 나열
        var layoutPanel = new StyleGrid();
        layoutPanel.SetLayout(arrList.Count, 1);

        int col = 0;
        foreach (var item in arrList)
        {
            FrameworkElement? element = CreateVisualElement(item);
            if (element != null)
            {
                layoutPanel.AddElement(element, col, 0);
                layoutPanel.LayoutRoot.ColumnDefinitions[col].Width = new GridLength(1, item.Width > 0 ? GridUnitType.Auto : GridUnitType.Star);
                col++;
            }
        }

        FrameworkElement elementToAdd = layoutPanel;
        elementToAdd.Margin = new Thickness(this.ItemSpace);

        // DataCell 스타일인 경우 배열 패널 전체를 하나의 헤더로 감싸기
        if (this.BindStyle == BindStyle.DataCell)
        {
            elementToAdd = WrapWithDataCell(firstItem, layoutPanel);
        }

        // 부모 Grid에 최종 배치 (위치는 첫 번째 아이템의 설정을 따름)
        this.AddElement(elementToAdd, firstItem.Col, firstItem.Row, firstItem.ColSpan, firstItem.RowSpan);
    }

    /// <summary>
    /// [공통 래퍼] 컨트롤(또는 서브패널)을 헤더 라벨 및 하단 라인이 있는 DataCell 구조로 묶어줍니다.
    /// </summary>
    private StyleGrid WrapWithDataCell(BindItem headerInfo, FrameworkElement contentElement)
    {
        var cellGrid = new StyleGrid();
        cellGrid.SetLayout(2, 2);
        cellGrid.LayoutRoot.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Auto);
        cellGrid.LayoutRoot.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
        cellGrid.LayoutRoot.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
        cellGrid.LayoutRoot.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);

        var lblHeader = new Label
        {
            Width = headerInfo.HeaderWidth is double hWidth ? hWidth : this.HeaderWidth,
            Background = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
            FontSize = headerInfo.HeaderFontSize,
            FontWeight = headerInfo.HeaderFontWeight,
            Foreground = headerInfo.HeaderForeground,
            Content = headerInfo.Header,
            VerticalContentAlignment = VerticalAlignment.Center,
            Margin = new Thickness(2, 0, 0, 0),
            Visibility = headerInfo.IsHeader ? Visibility.Visible : Visibility.Collapsed
        };

        cellGrid.AddElement(lblHeader, 0, 0);
        cellGrid.AddElement(contentElement, 1, 0);
        cellGrid.AddElement(new Border { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(0, 0, 0, 1) }, 0, 1, 2);

        return cellGrid;
    }

    /// <summary>
    /// [팩토리] BindItem 설정을 기반으로 순수한 UI 컨트롤만 생성합니다.
    /// </summary>
    private FrameworkElement? CreateVisualElement(BindItem bindItem)
    {
        if (bindItem.Content != null)
        {
            return bindItem.Content as FrameworkElement;
        }

        FrameworkElement? visualChild = null;

        switch (bindItem.BindType)
        {
            case BindType.TextBox or BindType.PasswordBox:
                var styleTextBox = new StyleTextBox()
                {
                    DataContext = this.Model,
                    BorderBrush = (this.BindStyle == BindStyle.DataCell ? Brushes.LightGray : Brushes.Transparent),
                    BorderThickness = bindItem.BorderThickness,
                    CornerRadius = bindItem.CornerRadius,
                    TextBoxType = bindItem.BindType == BindType.TextBox ? StyleTextBoxType.Text : StyleTextBoxType.Password
                };

                if (!string.IsNullOrWhiteSpace(bindItem.Placeholder)) styleTextBox.Placeholder = bindItem.Placeholder;
                visualChild = styleTextBox;
                break;

            case BindType.Button:
                var btn = new Button();
                btn.SetValue(Button.ContentProperty, bindItem.ButtonText);
                btn.SetValue(Button.CornerRadiusProperty, bindItem.CornerRadius);
                btn.SetValue(Button.ForegroundProperty, bindItem.Foreground);
                btn.SetValue(Button.FontWeightProperty, bindItem.FontWeight);
                btn.SetValue(Button.IsExpandingWhenClickProperty, bindItem.IsExpandingWhenClick);
                visualChild = btn;
                break;

            case BindType.Image:
                var imageBorder = new Border { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(1) };
                var image = new Image { Width = bindItem.Width, Height = bindItem.Height, Stretch = Stretch.UniformToFill, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                imageBorder.Child = image;
                visualChild = imageBorder;
                break;
        }

        if (visualChild == null) return null;

        // 공통 속성 설정 및 이벤트 바인딩
        visualChild.SetValue(TagProperty, bindItem);
        visualChild.SetValue(MarginProperty, bindItem.Margin == null ? new Thickness(this.ItemSpace) : bindItem.Margin);

        if (!string.IsNullOrWhiteSpace(bindItem.BackGround) && SmartMVVM.Common.BrushConverter.ConvertFromString(bindItem.BackGround) is Brush bg)
            visualChild.SetValue(BackgroundProperty, bg);

        if (!string.IsNullOrWhiteSpace(bindItem.BorderBrush) && SmartMVVM.Common.BrushConverter.ConvertFromString(bindItem.BorderBrush) is Brush borderBrush)
            visualChild.SetValue(Control.BorderBrushProperty, borderBrush);

        if (bindItem.Width > 0) visualChild.SetValue(WidthProperty, bindItem.Width);
        if (bindItem.Height > 0) visualChild.SetValue(HeightProperty, bindItem.Height);

        if (bindItem.IsBindClickEvent)
        {
            if (visualChild is Button button) button.Click += OnBindClick;
            else visualChild.MouseLeftButtonDown += OnBindClick;
        }

        if (bindItem.IsBinding == true) BindingVisualChild(visualChild, bindItem);

        visualChild.LostFocus += OnLostFocus_BindItem;

        return visualChild;
    }

    private void BindingVisualChild(FrameworkElement visualChild, BindItem bindItem)
    {
        DependencyProperty? targetProperty = null;

        if (visualChild is StyleTextBox) targetProperty = StyleTextBox.TextProperty;
        else if (visualChild is Image img) targetProperty = Image.SourceProperty;

        if (targetProperty != null && this.Model != null && !string.IsNullOrWhiteSpace(bindItem.FieldName))
        {
            Binding binding = new Binding(bindItem.FieldName)
            {
                Source = this.Model,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            visualChild.SetBinding(targetProperty, binding);
        }
    }

    private void OnLostFocus_BindItem(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.Tag is BindItem bindItem && this.Model != null)
        {
            var value = fe is StyleTextBox stb ? stb.Text : null;
            // 필요 시 비즈니스 로직 추가
        }
    }

    public void UpdateModel()
    {
        if (this.Model == null) return;
        // 필요시 데이터 컨텍스트 전파 로직 구현
    }

    public FrameworkElement GetBindItem<T>(string fieldName) where T : FrameworkElement
    {
        foreach (var item in LayoutRoot.Children)
        {
            var element = item as T;

            if (element != null && element.Tag is BindItem bindItem && bindItem.FieldName == fieldName)
            {
                return element;
            }
        }

        return default!;
    }

    private void OnBindClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.Tag is BindItem element)
        {
            var args = new BindClickEventArgs(e.RoutedEvent, this, element);
            BindGrid_BindClickEvent?.Invoke(element, args);
        }
    }

    public void Dispose(bool disposedValue)
    {
        if (!disposedValue || disposed) return;

        foreach (var child in LayoutRoot.Children)
        {
            if (child is FrameworkElement fe)
            {
                if (fe is Button btn) btn.Click -= OnBindClick;
                else fe.MouseLeftButtonDown -= OnBindClick;

                fe.LostFocus -= OnLostFocus_BindItem;
            }
        }

        LayoutRoot.Children.Clear();
        disposed = disposedValue;
    }
}

public class BindClickEventArgs : RoutedEventArgs
{
    public BindItem bindItem { get; }
    public BindClickEventArgs(RoutedEvent routedEvent, object source, BindItem item) : base(routedEvent, source)
    {
        bindItem = item;
    }
}

public class BindItemCollection : ObservableCollection<object>
{
}