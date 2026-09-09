using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRCSTInfoGrid.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRCSTInfoGrid : CustomControl
{
    private Patient SelectedPatient { get; set; } = new();
    private Consultation SelectedCST
    {
        get
        {
            var vm = this.DataContext as ConsultationViewModel;
            if (vm != null)
            {
                return vm.Model;
            }

            return default!;
        }
    }

    public vSmartEMRCSTInfoGrid() : base()
    {
    }

    public async Task SetPatientData(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(SelectedPatient, item);
    }

    public void ClearPATData()
    {
        SmartMVVM.ModelProperty.ClearPATData(SelectedPatient);
    }

    public void ClearCSTData()
    {
        SmartMVVM.ModelProperty.SetDefaultConsultationData(SelectedCST);

        RichTextEdit.ClearDocument();
    }

    private async void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        switch (element.Tag)
        {
            case "btnGetRecentCST":
                await SmartUI.SendMessage("GetRecentCST", viewType: TargetViewType.PageView);
                break;

            case "btnMoveIRCInfo":
                await SmartUI.SendMessage("MoveIRCInfo", viewType: TargetViewType.PageView);
                break;

            case "btnClear":
                if (SmartUI.MsgYesNo("진료 정보를 초기화하시겠습니까?") is System.Windows.MessageBoxResult.Yes)
                {
                    ClearCSTData();
                }

                break;
        }
    }

    private async void OnEditValueChanging_DateEdit(object sender, DevExpress.Xpf.Editors.EditValueChangingEventArgs e)
    {
        if (sender is not DateEdit element) return;
        if (e.NewValue is not DateTime targetDate) return;

        var targetYYMMDD = targetDate.ToString("yyyy-MM-dd");

        switch (element.Tag)
        {
            case "CST_YYMMDD":
                var response = await SmartUI.SendMessage("SetSelectedCSTByDate", targetYYMMDD, viewType:TargetViewType.PageView);
                if (response is not null && !response.IsSuccess)
                {
                    e.Handled = true;
                    e.IsCancel = true;
                }
                
                break;
        }
    }
}
