using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRCSTInfoGrid.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRCSTInfoGrid : CustomControl
{
    private async void OnClick_Button(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton sbtn) return;

        await SmartUI.SendMessage("MoveIRCInfo", viewType: TargetViewType.PageView);
    }
}
