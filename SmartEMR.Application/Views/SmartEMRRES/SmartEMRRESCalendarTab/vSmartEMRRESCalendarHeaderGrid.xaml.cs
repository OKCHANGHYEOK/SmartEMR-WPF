using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using System.Windows.Controls;

namespace SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;

/// <summary>
/// vSmartEMRRESCalendarHeaderGrid.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRRESCalendarHeaderGrid : CustomControl
{
    private void OnMouseLeftButtonDown_RadioButton(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var element = sender as RadioButton;
        if (element is null) return;

        if (element.Tag.ToString() == "Month")
        {
            SmartUI.SetNofification("기능 구현중입니다.", NotificationType.Info);

            element.IsChecked = false;
            e.Handled = true;
        }
    }

    private async void OnEditValueChanged_DateEdit(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
    {
        var element = sender as DateEdit;
        if (element is null) return;

        await SmartUI.SendMessage("UpdateCalendar", viewType: TargetViewType.PageView);
    }
}
