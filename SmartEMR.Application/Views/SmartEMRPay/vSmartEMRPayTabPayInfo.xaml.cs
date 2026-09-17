using DevExpress.Xpf.Grid;
using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Globalization;

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

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
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