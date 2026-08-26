using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Input;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRConsultationTabOrderORD.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRConsultationTabOrderORD : ModelViewLayout<OrderViewModel>
{
    public static readonly DependencyProperty OrderTypeProperty =
        DependencyProperty.Register(nameof(OrderType), typeof(OrderType), typeof(vSmartEMRConsultationTabOrderORD), new PropertyMetadata(OrderType.NON, OnOrderTypeChanged));

    private static void OnOrderTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is vSmartEMRConsultationTabOrderORD view)
        {
            view.SetOrderType((OrderType)e.NewValue);
        }
    }

    public OrderType OrderType
    {
        get => (OrderType)GetValue(OrderTypeProperty);
        set => SetValue(OrderTypeProperty, value);
    }

    public vSmartEMRConsultationTabOrderORD() { }

    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override async void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
        if (sender is not BindGrid) return;

        var bindItem = e.BindItem;
        switch (bindItem.FieldName)
        {
            case "ORD_InsuranceType":
                await vm.FetchDataAsync();
                break;

            case "keyword":
                if (string.IsNullOrWhiteSpace(e.NewValue?.ToString()))
                {
                    await vm.FetchDataAsync();
                }

                break;
        }
    }

    public override async void OnDataGrid_DataItemChanged(object? sender, DataItemChangedEventArgs e)
    {
        if (sender is not DataGrid) return;
        if (e.DataItem is not Order dataItem) return;

        var fieldName = e.Column.FieldName;
        switch (fieldName)
        {
            case "btnAddCORD":
                await SmartUI.SendMessage("AddCORD", dataItem, TargetViewType.PageView);
                break;
        }
    }

    public override async void OnDataGrid_PageIndexChanged(object? sender, PageIndexChangedEventArgs e)
    {
        if (sender is not DataGrid) return;
        if (e.PageIndex < 0) return;

        await vm.LoadPageAsync(e.PageIndex);
    }

    private void SetOrderType(OrderType type)
    {
        vm.SetOrderType(type);
    }

    public async Task UpdateOrders()
    {
        await vm.FetchDataAsync();
    }
}