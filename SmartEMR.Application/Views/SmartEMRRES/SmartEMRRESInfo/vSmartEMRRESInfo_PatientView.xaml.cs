using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using System.Windows;

namespace SmartEMR.Application.Views.SmartEMRRES;

/// <summary>
/// vSmartEMRRESInfo_PatientView.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRRESInfo_PatientView : CustomControl
{
    public vSmartEMRRESInfo_PatientView()
    {
        var stbRES_Memo = PatientViewGrid.GetBindItem<StyleTextBox>("RES_Memo");
        if (stbRES_Memo is not null)
        {
            stbRES_Memo.AcceptsReturn = true;
        }

        var cmbRES_ReservationTime = PatientViewGrid.GetBindItem<Xpf.ComboBoxEdit>("RES_ReservationTime");
        if (cmbRES_ReservationTime is not null)
        {
            cmbRES_ReservationTime.HorizontalContentAlignment = HorizontalAlignment.Center;
        }
    }

    private async void OnClick_Button(object sender, RoutedEventArgs e)
    {
        var btn = sender as Xpf.Button;
        if (btn is null) return;

        await SmartUI.SendMessage("MovePatientInfo");
    }
}
