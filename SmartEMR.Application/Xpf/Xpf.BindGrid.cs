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

[ObservableObject]
[ContentProperty(nameof(BindItems))]
public partial class BindGrid : StyleGrid, IDisposable
{
    public int ItemSpace
    {
        get => (int)GetValue(ItemSpaceProperty);
        set => SetValue(ItemSpaceProperty, value);
    }

    public static readonly DependencyProperty ItemSpaceProperty =
        DependencyProperty.Register(
            nameof(ItemSpace), 
            typeof(int), 
            typeof(BindGrid), 
            new PropertyMetadata(5));


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

    private void AddElement(BindItem element)
    {
        FrameworkElement? visualChild = new ();

        if (element.BindType == BindType.TextBox || element.BindType == BindType.PasswordBox)
        {
            var styleTextBox = new StyleTextBox()
            {
                DataContext = this.Model,
                BorderThickness = element.BorderThickness,
                CornerRadius = element.CornerRadius
            };

            if (element.BindType == BindType.TextBox)
            {
                styleTextBox.TextBoxType = StyleTextBoxType.Text;
            }
            else if (element.BindType == BindType.PasswordBox)
            {
                styleTextBox.TextBoxType = StyleTextBoxType.Password;
            }

            if (!string.IsNullOrWhiteSpace(element.Placeholder))
            {
                styleTextBox.Placeholder = element.Placeholder;
            }

            visualChild = styleTextBox;
        }

        if (element.BindType == BindType.Button)
        {
            var btn = new Button();

            btn.SetValue(Button.ContentProperty, element.ButtonText);
            btn.SetValue(Button.CornerRadiusProperty, element.CornerRadius);
            btn.SetValue(Button.ForegroundProperty, element.Foreground);
            btn.SetValue(Button.FontWeightProperty, element.FontWeight);

            visualChild = btn;
        }

        visualChild.SetValue(TagProperty, element);
        visualChild.SetValue(MarginProperty, element.Margin == null ? new Thickness(this.ItemSpace) : element.Margin);

        if (!string.IsNullOrWhiteSpace(element.BackGround))
        {
            try
            {
                Brush? bg = (Brush?)SmartMVVM.Common.BrushConverter.ConvertFromString(element.BackGround);

                if (bg != null)
                {
                    visualChild.SetValue(BackgroundProperty, bg);
                }
            } catch (NotSupportedException e)
            {
                MessageBox.Show(e.StackTrace);
            };
        }

        if (!string.IsNullOrWhiteSpace(element.BorderBrush))
        {
            try
            {
                Brush? bg = (Brush?)SmartMVVM.Common.BrushConverter.ConvertFromString(element.BorderBrush);

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

        if (element.Width != null)
        {
            visualChild.SetValue(WidthProperty, element.Width);
        }

        if (element.Height != null)
        {
            visualChild.SetValue(HeightProperty, element.Height);
        }

        if (element.IsBindClickEvent)
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

        if (element.IsBinding == true)
        {
            SmartUI.BeginInvoke(() =>
            {
                visualChild.SetBinding(DataContextProperty, new Binding("Model") { Source = this, Mode = BindingMode.TwoWay });
                BindingExtensions.SetBinding(visualChild, element.FieldName ?? "");

            }, System.Windows.Threading.DispatcherPriority.Background);
        }

        visualChild.LostFocus += OnLostFocus_BindItem;

        AddElement(visualChild, element.Col, element.Row, element.ColSpan, element.RowSpan);
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