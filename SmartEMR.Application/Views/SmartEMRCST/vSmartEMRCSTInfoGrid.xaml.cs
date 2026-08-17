using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRCSTInfoGrid.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRCSTInfoGrid : CustomControl
{
    private Patient SelectedPatient { get; set; } = new();
    private Consultation SelectedCST { get; set; } = new();

    public async Task UpdateDataByPAT(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(SelectedPatient, item);

        var ret = await SmartMVVM.DataStore.GetItem<Consultation>(eAPI.Consultation_GetConsultation, new Consultation { PAT_Idx = item.PAT_Idx, CST_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd") });
        if (ret is not null)
        {
            SmartMVVM.ModelProperty.SetConsultationData(SelectedCST, ret);
        }
    }

    public void ClearDataByPAT()
    {
        if (SelectedPatient.PAT_Idx.GetValueOrDefault(0) > 0)
        {
            SmartMVVM.ModelProperty.ClearPATData(SelectedPatient);
            SmartMVVM.ModelProperty.ClearCSTData(SelectedCST);
        }
    }

    private async void OnClick_Button(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton sbtn) return;

        await SmartUI.SendMessage("MoveIRCInfo", viewType: TargetViewType.PageView);
    }
}
