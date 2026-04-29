using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using System.Collections.ObjectModel; 
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

[ContentProperty(nameof(BindItems))]
public partial class BindGrid : StyleGrid
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

    private object? Model = null;

    // 1. UIElementCollection 대신 ObservableCollection<BindItem> 사용
    public ObservableCollection<BindItem> BindItems { get; }
        = new ObservableCollection<BindItem>();


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

        visualChild.SetValue(DataContextProperty, this.Model);
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
                btn.Click += (s, e) =>
                {
                    var args = new BindClickEventArgs(e.RoutedEvent, this, element);
                    BindGrid_BindClickEvent?.Invoke(element, args);
                };
            }
            else
            {
                visualChild.MouseLeftButtonDown += (s, e) =>
                {
                    var args = new BindClickEventArgs(e.RoutedEvent, this, element);
                    BindGrid_BindClickEvent?.Invoke(element, args);
                };
            }
        }

        if (element.IsBinding == true)
        {
            BindingExtensions.SetBinding(visualChild, element.FieldName ?? "");
        }

        AddElement(visualChild, element.Col, element.Row);
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

    protected override void OnVisualParentChanged(DependencyObject oldParent)
    {
        base.OnVisualParentChanged(oldParent);

        this.Loaded += (s, e) =>
        {
            var viewLayout = SmartUI.UIManager.CurrentPageView;

            if (viewLayout != null)
                viewLayout.AddBindGrid(this);
        };
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