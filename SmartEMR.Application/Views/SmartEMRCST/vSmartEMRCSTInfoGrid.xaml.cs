using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using NotificationType = SmartEMR.Application.Core.NotificationType;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRCSTInfoGrid.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRCSTInfoGrid : CustomControl
{
    private Patient SelectedPatient { get; set; } = new();
    private Consultation SelectedCST { get; set; } = new();

    public async Task SetPatientData(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(SelectedPatient, item);
    }

    public async Task UpdateDataBySelectedCST(Consultation item)
    {
        Consultation? currentCST = new();
        
        if (item.RCP_Idx.GetValueOrDefault(0) == 0)
        {
            currentCST = await SmartMVVM.DataStore.GetItem<Consultation>(eAPI.Consultation_GetConsultation, new Consultation { PAT_Idx = item.PAT_Idx, CST_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd") });
        }
        else
        {
            currentCST = item;
        }

        if (currentCST is not null)
        {
            SmartMVVM.ModelProperty.SetConsultationData(SelectedCST, currentCST);
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
