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
}
