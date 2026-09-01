using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// OrderSectionTemplate.xaml에 대한 상호 작용 논리
/// </summary>
public partial class OrderSectionTemplate : CustomControl
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(OrderSectionTemplate), new PropertyMetadata(string.Empty));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly DependencyProperty OrdersProperty =
        DependencyProperty.Register(nameof(Orders), typeof(IEnumerable<Order>), typeof(OrderSectionTemplate), new PropertyMetadata(null));

    public IEnumerable<Order> Orders
    {
        get => (IEnumerable<Order>)GetValue(OrdersProperty);
        set => SetValue(OrdersProperty, value);
    }

    private async void OnClick_Button(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleButton element) return;

        var dataItem = element.DataContext as Order;
        if (dataItem is null) return;

        if (!dataItem.IsSelected)
        {
            await SmartUI.SendMessage("AddCSTOFromOrderSection", dataItem, viewType: TargetViewType.PageView);
        }
        else
        {
            await SmartUI.SendMessage("DeleteCSTOFromOrderSection", dataItem, viewType: TargetViewType.PageView);
        }
    }
}
