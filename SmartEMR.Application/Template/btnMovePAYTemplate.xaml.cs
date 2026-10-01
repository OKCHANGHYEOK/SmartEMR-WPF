using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.Views.SmartEMRPay;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Template;

/// <summary>
/// btnMovePAYTemplate.xaml에 대한 상호 작용 논리
/// </summary>
public partial class btnMovePAYTemplate : GridTemplate
{
    public btnMovePAYTemplate() { }

    public override void Initialize()
    {
    }

    private async void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        var dataItem = element.DataContext as Pay;
        if (dataItem is null) return;

        await SmartUI.NavigateToPage(new vSmartEMRPayTab(), parameter: dataItem, isReuse:false);
    }
}