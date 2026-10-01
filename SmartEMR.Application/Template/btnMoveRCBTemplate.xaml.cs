using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.Views;
using SmartEMR.Application.Views.SmartEMRCST;
using SmartEMR.Application.Views.SmartEMRRES;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Template;

/// <summary>
/// btnMoveRCBTemplate.xaml에 대한 상호 작용 논리
/// </summary>
public partial class btnMoveRCBTemplate : GridTemplate
{
    public btnMoveRCBTemplate() { }

    public override void Initialize()
    {
    }

    private async void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        var dataItem = element.DataContext as ReceptionBoard;
        if (dataItem is null) return;

        if (dataItem.RCB_Type == "RES")
        {
            await SmartUI.NavigateToPage(new vSmartEMRRESInfo(new Reservation { RES_Idx = dataItem.RES_Idx, PAT_Idx = dataItem.PAT_Idx }), isPopup:true);
        }
        else if (dataItem.RCB_Type == "RCP")
        {
            await SmartUI.NavigateToPage(new vSmartEMRDeskTab(new Reception { RCP_Idx = dataItem.RCP_Idx, PAT_Idx = dataItem.PAT_Idx }), isReuse:false);
        }
    }
}