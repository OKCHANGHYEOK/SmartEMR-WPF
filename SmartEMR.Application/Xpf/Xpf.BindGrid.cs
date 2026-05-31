using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
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

    public static readonly DependencyProperty ItemSpaceProperty =
        DependencyProperty.Register(
            nameof(ItemSpace),
            typeof(int),
            typeof(BindGrid),
            new PropertyMetadata(5));

    public int ItemSpace
    {
        get => (int)GetValue(ItemSpaceProperty);
        set => SetValue(ItemSpaceProperty, value);
    }

    public static readonly DependencyProperty BindStyleProperty =
        DependencyProperty.Register(
            nameof(BindStyle),
            typeof(BindStyle),
            typeof(BindGrid),
            new PropertyMetadata(BindStyle.None));

    public BindStyle BindStyle
    {
        get => (BindStyle)GetValue(BindStyleProperty);
        set => SetValue(BindStyleProperty, value);
    }

    public double HeaderWidth { get; set; } = 60;

    private object? m_Model;
    public object? Model
    {
        get => m_Model;
        set
        {
            if (m_Model != value)
            {
                m_Model = value;
                OnPropertyChanged(nameof(Model));
            }
        }
    }

    // 1. UIElementCollection 대신 ObservableCollection<BindItem> 사용
    public ObservableCollection<BindItem> BindItems { get; }
        = new ObservableCollection<BindItem>();

    public bool disposed { get; set; }

    public delegate void BindClickEventHandler(object sender, BindClickEventArgs e);
    public event BindClickEventHandler? BindGrid_BindClickEvent;
    
    public BindGrid() : base()
    {
        this.BindItems.CollectionChanged += OnBindItemsChanged;
        this.DataContextChanged += (s, e) => UpdateModel();
    }

    private void UpdateModel()
    {
        if (this.DataContext == null) return;

        if (this.DataContext is IVIewModel vm)
        {
            this.Model = vm.Model;
        }
    }

    private void AddElement(BindItem bindItem, bool isBottomLine = true)
    {
        FrameworkElement? visualChild = new ();

        if (bindItem.Content != null)
        {
            visualChild = bindItem.Content as FrameworkElement;
        }
        else
        {
            if (bindItem.BindType == BindType.TextBox || bindItem.BindType == BindType.PasswordBox)
            {
                var styleTextBox = new StyleTextBox()
                {
                    DataContext = this.Model,
                    BorderBrush = (this.BindStyle == BindStyle.DataCell ? Brushes.LightGray : Brushes.Transparent),
                    BorderThickness = bindItem.BorderThickness,
                    CornerRadius = bindItem.CornerRadius
                };

                if (bindItem.BindType == BindType.TextBox)
                {
                    styleTextBox.TextBoxType = StyleTextBoxType.Text;
                }
                else if (bindItem.BindType == BindType.PasswordBox)
                {
                    styleTextBox.TextBoxType = StyleTextBoxType.Password;
                }

                if (!string.IsNullOrWhiteSpace(bindItem.Placeholder))
                {
                    styleTextBox.Placeholder = bindItem.Placeholder;
                }

                visualChild = styleTextBox;
            }

            if (bindItem.BindType == BindType.Button)
            {
                var btn = new Button();

                btn.SetValue(Button.ContentProperty, bindItem.ButtonText);
                btn.SetValue(Button.CornerRadiusProperty, bindItem.CornerRadius);
                btn.SetValue(Button.ForegroundProperty, bindItem.Foreground);
                btn.SetValue(Button.FontWeightProperty, bindItem.FontWeight);
                btn.SetValue(Button.IsExpandingWhenClickProperty, bindItem.IsExpandingWhenClick);

                visualChild = btn;
            }

            if (bindItem.BindType == BindType.Image)
            {
                var ImageBorder = new Border
                {
                    BorderBrush = Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                };

                var image = new Image
                {
                    Width = bindItem.Width,
                    Height = bindItem.Height,
                    Stretch = Stretch.UniformToFill,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                ImageBorder.Child = image;

                visualChild = ImageBorder;

                isBottomLine = false;
            }

            visualChild.SetValue(TagProperty, bindItem);
            visualChild.SetValue(MarginProperty, bindItem.Margin == null ? new Thickness(this.ItemSpace) : bindItem.Margin);

            if (!string.IsNullOrWhiteSpace(bindItem.BackGround))
            {
                try
                {
                    Brush? bg = (Brush?)SmartMVVM.Common.BrushConverter.ConvertFromString(bindItem.BackGround);

                    if (bg != null)
                    {
                        visualChild.SetValue(BackgroundProperty, bg);
                    }
                }
                catch (NotSupportedException e)
                {
                    MessageBox.Show(e.StackTrace);
                }
                ;
            }

            if (!string.IsNullOrWhiteSpace(bindItem.BorderBrush))
            {
                try
                {
                    Brush? bg = (Brush?)SmartMVVM.Common.BrushConverter.ConvertFromString(bindItem.BorderBrush);

                    if (bg != null)
                    {
                        visualChild.SetValue(Control.BorderBrushProperty, bg);
                    }
                }
                catch (NotSupportedException e)
                {
                    MessageBox.Show(e.StackTrace);
                }
                ;
            }

            if (bindItem.Width > 0)
            {
                visualChild.SetValue(WidthProperty, bindItem.Width);
            }
            if (bindItem.Height > 0)
            {
                visualChild.SetValue(HeightProperty, bindItem.Height);
            }

            if (bindItem.IsBindClickEvent)
            {
                if (visualChild is Button btn)
                {
                    btn.Click += OnBindClick;
                }
                else
                {
                    visualChild.MouseLeftButtonDown += OnBindClick;
                }
            }

            if (bindItem.IsBinding == true)
            {
                BindingVisualChild(visualChild, bindItem);
            }

            var elementToAdd = visualChild;

            if (this.BindStyle == BindStyle.DataCell)
            {
                var grid = new StyleGrid();
                var lblHeader = new Label
                {
                    Background = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                    FontSize = bindItem.HeaderFontSize,
                    FontWeight = bindItem.HeaderFontWeight,
                    Foreground = bindItem.HeaderForeground,
                    Content = bindItem.Header,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(2, 0, 0, 0),
                    Visibility = (bindItem.IsHeader ? Visibility.Visible : Visibility.Collapsed)
                };

                if (bindItem.HeaderWidth is double hwidth)
                {
                    lblHeader.Width = hwidth;
                }
                else
                {
                    lblHeader.Width = this.HeaderWidth;
                }

                grid.SetLayout(2, 2);
                grid.LayoutRoot.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Auto);
                grid.LayoutRoot.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                grid.LayoutRoot.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                grid.LayoutRoot.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
                grid.AddElement(lblHeader, 0, 0);
                grid.AddElement(visualChild, 1, 0);

                if (isBottomLine)
                {
                    grid.AddElement(new Border { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(0, 0, 0, 1) }, 0, 1, 2);
                }

                elementToAdd = grid;
            }

            AddElement(elementToAdd, bindItem.Col, bindItem.Row, bindItem.ColSpan, bindItem.RowSpan);
        }

        visualChild?.LostFocus += OnLostFocus_BindItem;
    }

    private void BindingVisualChild(FrameworkElement visualChild, BindItem bindItem)
    {
        SmartUI.BeginInvoke(() =>
        {
            if (visualChild is StyleTextBox textBox)
            {
                textBox.SetBinding(DataContextProperty, new Binding("Model") { Source = this, Mode = BindingMode.TwoWay });
            }
            else if (visualChild is Image image)
            {

            }

            BindingExtensions.SetBinding(visualChild, bindItem.FieldName ?? "");
        });
    }

    private void OnBindItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            if (e.NewItems == null) return;

            foreach (BindItem item in e.NewItems)
            {
                this.AddElement(item);
            }
        }
    }

    private void OnLostFocus_BindItem(object sender,  RoutedEventArgs e)
    {
        var element = sender as FrameworkElement;

        if (element != null && element.ToolTip != null)
        {
            var toolTip = element.ToolTip as ToolTip;

            if (toolTip != null)
            {
                toolTip.IsOpen = false;
            }
        }
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
                if (fe is Xpf.Button btn)
                {
                    btn.Click -= OnBindClick;
                }
                else
                {
                    fe.MouseLeftButtonDown -= OnBindClick;
                }

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