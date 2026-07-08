using SmartEMR.Application.Common.Converter;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Diagnostics;
using System.Globalization;

namespace SmartEMR.Application.Views.SmartEMRRCP;

/// <summary>
/// vSmartEMRRCPInfoTab.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRRCPInfoTab : ModelViewLayout<ReceptionViewModel>
{
    public vSmartEMRRCPInfoTab() { }
    public vSmartEMRRCPInfoTab(Reception item) : base(item) { }

    protected override void Initialize()
    {
        SmartEMRRCPInfo.SetData(vm.Model);
        SmartEMRIRCInfo.SetData(vm.IRCItem);
    }

    public override async Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }
}

public class RCP_IdxToContentConverter : BaseConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            var RCP_Idx = System.Convert.ToInt32(value);

            return "접수" + (RCP_Idx == 0 ? "등록" : "수정");
        }
        catch (InvalidCastException e)
        {
            Debug.WriteLine(e.StackTrace);
        }

        return default!;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}