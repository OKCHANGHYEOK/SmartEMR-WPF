using System.Globalization;
using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows.Data;

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

    protected override void SetBindGrid()
    {
        if (this.BindGrids[1].GetBindItem<StyleTextBox>("PAY_DiscountPrice") is StyleTextBox stbPAY_DiscountPrice)
        {
            stbPAY_DiscountPrice.SetBinding(StyleTextBox.MaxLengthProperty, new Binding("PAY_RemainPrice") { Source = vm.Model, Converter = new PriceToMaxLengthConverter() });
        }

        if (this.BindGrids[1].GetBindItem<StyleTextBox>("PAY_PriceForPay") is StyleTextBox stbPAY_PriceForPay)
        {
            stbPAY_PriceForPay.SetBinding(StyleTextBox.MaxLengthProperty, new Binding("PAY_RemainPrice") { Source = vm.Model, Converter = new PriceToMaxLengthConverter() });
        }
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
                await vm.SetPayItem(PayType.Refund);
                break;

            case "btnCutting":
                await vm.SetPayItem(PayType.Cutting);
                break;

            case "btnDiscount":
                await vm.SetPayItem(PayType.Discount);
                break;

            case "btnSetAllPrice":
                vm.SetAllPrice();
                break;

            case "btnCash":
                await vm.SetPayItem(PayType.Payment, PayMethod.Cash);
                break;

            case "btnCard":
                await vm.SetPayItem(PayType.Payment, PayMethod.Card);
                break;

            case "btnNaverPay":
                await vm.SetPayItem(PayType.Payment, PayMethod.NaverPay);
                break;
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

public class PriceToMaxLengthConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not decimal price) return 0;

        return price.ToString().Length;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}