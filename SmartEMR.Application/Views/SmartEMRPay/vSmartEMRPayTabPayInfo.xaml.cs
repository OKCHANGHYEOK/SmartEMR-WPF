using System.Windows;
using System.Globalization;
using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRPay;

/// <summary>
/// vSmartEMRPayTabPayInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRPayTabPayInfo : ModelViewLayout<PayInfoViewModel>
{
    public vSmartEMRPayTabPayInfo() {}

    protected override void Initialize()
    {
    }

    protected override void SetDataGrid()
    {
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
        if (sender is not BindGrid) return;

        var fieldName = e.BindItem.FieldName;
        
        switch (fieldName)
        {
            case "btnRefund":
                if (e.NewValue is int refundPrice)
                {
                    await vm.SetPayItem(PayType.Refund, price:refundPrice);
                }

                break;

            case "btnCutting":
                if (e.NewValue is int cutUnit)
                {
                    await vm.SetPayItem(PayType.Cutting, price:cutUnit);
                }

                break;

            case "btnDiscount":
                if (e.NewValue is int discountPrice)
                {
                    await vm.SetPayItem(PayType.Discount, price:discountPrice);
                }

                break;

            case "btnCash":
                {
                    if (e.NewValue is int price)
                    {
                        await vm.SetPayItem(PayType.Payment, PayMethod.Cash, price);
                    }

                    break;
                }

            case "btnCard":
                {
                    if (e.NewValue is int price)
                    {
                        await vm.SetPayItem(PayType.Payment, PayMethod.Card, price);
                    }

                    break;
                }

            case "btnNaverPay":
                {
                    if (e.NewValue is int price)
                    {
                        await vm.SetPayItem(PayType.Payment, PayMethod.NaverPay, price);
                    }

                    break;
                }
        }
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public async Task UpdatePayInfo(Pay item)
    {
        await vm.UpdatePayInfo(item);
    }
}

public class InsuranceTypeNameToContentConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string typeName) return "";

        return $"{typeName}처방";
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}