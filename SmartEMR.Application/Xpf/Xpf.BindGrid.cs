using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Xpf.Editors;
using MahApps.Metro.Controls;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
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

    #endregion

    public event EventHandler<BindClickEventArgs>? BindGrid_BindClickEvent;

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

    public bool IsPreventBindGridEvent { get; set; } = false;

    public bool disposed { get; set; }

    public BindGrid()
    {
        _bindItems?.Clear();
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
            finalElement = WrapWithDataCell(item, contentElement, item.IsBottomLine);
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

        var firstItem = arrList[0];

        var layoutPanel = new StyleGrid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

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

        if (this.BindStyle == BindStyle.DataCell)
        {
            elementToAdd = WrapWithDataCell(firstItem, layoutPanel);
        }

        this.AddElement(elementToAdd, firstItem.Col, firstItem.Row, firstItem.ColSpan, firstItem.RowSpan);
    }

    /// <summary>
    /// [공통 래퍼] 컨트롤(또는 서브패널)을 헤더 라벨 및 하단 라인이 있는 DataCell 구조로 묶어줍니다.
    /// </summary>
    private StyleGrid WrapWithDataCell(BindItem headerInfo, FrameworkElement contentElement, bool isBottomLine = true)
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

        if (isBottomLine)
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
            case BindType.Label:
                visualChild = new Xpf.Label
                {
                    Content = bindItem.TextValue,
                    Foreground = bindItem.Foreground,
                    FontSize = bindItem.FontSize,
                    FontWeight = bindItem.FontWeight
                };

                break;

            case BindType.TextBox or BindType.PasswordBox:
                visualChild = new StyleTextBox
                {
                    FontSize = bindItem.FontSize,
                    Foreground = bindItem.Foreground,
                    FontWeight = bindItem.FontWeight,
                    BorderBrush = (this.BindStyle == BindStyle.DataCell ? Brushes.LightGray : Brushes.Transparent),
                    BorderThickness = bindItem.BorderThickness,
                    CornerRadius = bindItem.CornerRadius,
                    TextBoxType = bindItem.BindType == BindType.TextBox ? StyleTextBoxType.Text : StyleTextBoxType.Password,
                    ContentAlignment = bindItem.ContentAlignment,
                    Placeholder = bindItem.Placeholder,
                    MaxLength = bindItem.MaxLength,
                    IsNumericOnly = bindItem.IsNumericOnly,
                    IsReadOnly = bindItem.IsReadOnly
                };

                break;

            case BindType.Button:  
                visualChild = new Button
                {
                    Content = bindItem.TextValue,
                    FontSize = bindItem.FontSize,
                    Foreground = bindItem.Foreground,
                    FontWeight = bindItem.FontWeight,
                    IsExpandingWhenClick = bindItem.IsExpandingWhenClick
                };

                break;

            case BindType.ComboBox:
                visualChild = new ComboBoxEdit()
                {
                    ItemsSource = bindItem.ItemsSource,
                    DisplayMember = bindItem.DisplayMember,
                    ValueMember = bindItem.ValueMember,
                    CornerRadius = bindItem.CornerRadius,
                    BorderThickness = bindItem.BorderThickness
                };

                break;

            case BindType.CheckBox:
                visualChild = new CheckEdit()
                {
                    Content = bindItem.TextValue,
                    FontSize = bindItem.FontSize,
                    FontWeight = bindItem.FontWeight,
                    Foreground = bindItem.Foreground
                };

                break;

            case BindType.Image:
                var image = new Image { Width = bindItem.Width, Height = bindItem.Height, Stretch = Stretch.UniformToFill, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                
                visualChild = new Border
                {
                    BorderBrush = Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    Child = image
                };

                break;

            case BindType.DateEdit:
                visualChild = bindItem.DateEditType switch
                {
                    DateEditType.Date => new DateEdit { },
                    DateEditType.Time => new DateEdit 
                    {
                        Mask = "tt hh:mm",
                        MaskUseAsDisplayFormat=true,
                        MaskType = DevExpress.Xpf.Editors.MaskType.DateTime,
                        StyleSettings = new DateEditTimePickerStyleSettings()
                    },
                    _ => default!
                };

                break;
        }

        if (visualChild == null) return null;

        // 공통 속성 설정 및 이벤트 바인딩
        visualChild.Name = bindItem.FieldName;
        visualChild.Tag = bindItem;
        visualChild.Width = bindItem.Width > 0 ? bindItem.Width : visualChild.Width;
        visualChild.Height = bindItem.Height > 0 ? bindItem.Height : visualChild.Height;
        visualChild.HorizontalAlignment = bindItem.HAlignment;
        visualChild.VerticalAlignment = bindItem.VAlignment;
        visualChild.Margin = (Thickness)((bindItem.Margin == null) ? new Thickness(this.ItemSpace) : bindItem.Margin);
        visualChild.ToolTip = bindItem.ToolTip;
        visualChild.IsEnabled = bindItem.IsEnabled;

        if (!string.IsNullOrWhiteSpace(bindItem.BackGround) && SmartMVVM.Common.BrushConverter.ConvertFromString(bindItem.BackGround) is Brush bg)
            visualChild.SetValue(BackgroundProperty, bg);

        if (!string.IsNullOrWhiteSpace(bindItem.BorderBrush) && SmartMVVM.Common.BrushConverter.ConvertFromString(bindItem.BorderBrush) is Brush borderBrush)
            visualChild.SetValue(Control.BorderBrushProperty, borderBrush);

        if (bindItem.IsBindClickEvent)
        {
            AddBindClickEvent(visualChild, bindItem);
        }

        if (bindItem.IsBinding == true)
        {
            SmartUI.BeginInvoke(() =>
            {
                BindingExtensions.SetBinding(visualChild, bindItem);

                RegisterBindItemChangedEvents(visualChild, bindItem);

            }, System.Windows.Threading.DispatcherPriority.Background);
        }

        visualChild.LostFocus += OnLostFocus_BindItem;

        return visualChild;
    }

    private void AddBindClickEvent(FrameworkElement element, BindItem bindItem)
    {
        if (element is Button button)
        {
            button.Click += OnBindClick;
        }
        else if (element is BaseEdit baseEdit)
        {
            baseEdit.EditValueChanging += OnBindClick;
        }
        else element.MouseLeftButtonDown += OnBindClick;
    }

    private void OnBindClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.Tag is BindItem element)
        {
            object? newValue = null;

            if (sender is BaseEdit && e is EditValueChangingEventArgs evcArgs)
            {
                newValue = evcArgs.NewValue;
            }

            var args = new BindClickEventArgs(e.RoutedEvent, this, element, newValue);
            BindGrid_BindClickEvent?.Invoke(this, args);
        }
    }

    private void OnLostFocus_BindItem(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.Tag is BindItem bindItem)
        {
            var value = fe is StyleTextBox stb ? stb.Text : null;
        }
    }

    public T? GetBindItem<T>(string fieldName) where T : FrameworkElement
    {
        foreach (FrameworkElement element in LayoutRoot.Children)
        {
            var bindItem = element.Tag as BindItem;

            if (bindItem != null && bindItem.FieldName == fieldName)
            {
                return element as T;   
            }
            else
            {
                var targetItem = element.FindChild<T>(fieldName);

                if (targetItem != null)
                {
                    return targetItem;
                }
            }
        }

        return default!;
    }

    public void Dispose(bool disposedValue)
    {
        if (!disposedValue || disposed) return;

        ClearTranckedProperties();

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

// BindItemChangedEvent
public partial class BindGrid
{

    public event EventHandler<BindItemChangedEventArgs>? BindGrid_BindItemChangedEvent;
    private readonly List<(DependencyObject element, DependencyPropertyDescriptor descriptor, EventHandler handler)> _trackedProperties = new();

    public void RegisterBindItemChangedEvents(FrameworkElement element, BindItem bindItem)
    {
        DependencyProperty? dp = element switch
        {
            _ when element is StyleTextBox => StyleTextBox.TextProperty,
            _ when element is Xpf.TextBox => Xpf.TextBox.TextProperty,
            _ when element is Xpf.Image => Xpf.Image.SourceProperty,
            _ when element is CheckEdit => CheckEdit.EditValueProperty,
            _ when element is ComboBoxEdit => ComboBoxEdit.EditValueProperty,
            _ => null
        };

        if (dp == null)
            return;

        var descriptor = DependencyPropertyDescriptor.FromProperty(dp, element.GetType());
        if (descriptor != null)
        {
            EventHandler handler = (s, e) =>
            {
                if (this.IsPreventBindGridEvent) return;

                var args = new BindItemChangedEventArgs(bindItem, element, dp, descriptor.GetValue(element));
                BindGrid_BindItemChangedEvent?.Invoke(this, args);
            };

            descriptor.AddValueChanged(element, handler);
            _trackedProperties.Add((element, descriptor, handler));
        }
    }

    private void ClearTranckedProperties()
    {
        foreach (var item in _trackedProperties)
        {
            item.descriptor.RemoveValueChanged(item.element, item.handler);
        }
        _trackedProperties.Clear();
    }
}

public class BindClickEventArgs : RoutedEventArgs
{
    public BindItem BindItem { get; }
    public object? NewValue { get; }

    public BindClickEventArgs(RoutedEvent routedEvent, object source, BindItem item, object? newValue = null) 
        : base(routedEvent, source)
    {
        BindItem = item;
        NewValue = newValue;
    }
}

public class BindItemChangedEventArgs : EventArgs
{
    public BindItem BindItem { get; }            // 메타데이터 정보
    public FrameworkElement UIElement { get; }     // 실제 렌더링된 TextBox 등의 UI객체
    public DependencyProperty Property { get; }    // 변경된 속성 (TextProperty 등)
    public object? NewValue { get; }               // 바뀐 새로운 값

    public BindItemChangedEventArgs(BindItem bindItem, FrameworkElement uiElement, DependencyProperty property, object? newValue)
    {
        BindItem = bindItem;
        UIElement = uiElement;
        Property = property;
        NewValue = newValue;
    }
}

public class BindItemCollection : ObservableCollection<object>
{
}