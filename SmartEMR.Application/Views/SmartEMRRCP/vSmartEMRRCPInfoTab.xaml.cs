using SmartEMR.Application.Common.Converter.Base;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Diagnostics;
using System.Globalization;
using System.Windows;

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

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse { IsSuccess = false };

        switch (request.MessageAction)
        {
            case "SetRCPItem":
                {
                    var paramItem = request.MessageParameter as Reception;
                    if (paramItem != null)
                    {
                        vm.SetRCPItem(paramItem);
                    }

                    break;
                }

            case "SetIRCItem":
                {
                    var paramItem = request.MessageParameter as Insurance;
                    if (paramItem != null)
                    {
                        vm.SetIRCItem(paramItem);
                    }

                    break;
                }

            case "SetInsuranceType":
                {
                    var paramItem = request.MessageParameter?.ToString();
                    if (paramItem == null) return null;

                    SmartEMRIRCInfo.SetInsuranceType(paramItem);
                    break;
                }

            case "CloseView":
                SmartUI.CloseView();
                break;
        }

        return response;
    }

    private void OnClick_Button(object sender, System.Windows.RoutedEventArgs e)
    {
        var btn = sender as Button;
        if (btn == null) return;

        switch (btn.Name)
        {
            case "btnClear":
                if (SmartUI.MsgYesNo("초기화하시겠습니까?") is not MessageBoxResult.Yes)
                {
                    vm.ClearData();
                }

                break;
        }
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