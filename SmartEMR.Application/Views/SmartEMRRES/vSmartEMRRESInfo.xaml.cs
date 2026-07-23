using System.Windows;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.SmartEMRRES;

/// <summary>
/// vSmartEMRDeskPATInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRRESInfo : ModelViewLayout<ReservationInfoViewModel>
{
    protected override void Initialize()
    {
        this.ViewTitle = "예약" + (vm.Model.RES_Idx.GetValueOrDefault(0) == 0 ? "등록" : "수정");

        if (vm.SelectedPatient.PAT_Idx.GetValueOrDefault(0) > 0)
        {
            chkIsNewPAT.IsEnabled = false;
        }
    }

    protected override void SetBindGrid()
    {
        var stbRES_Memo = this.BindGrids[1].GetBindItem<StyleTextBox>("RES_Memo");
        if (stbRES_Memo is not null)
        {
            stbRES_Memo.AcceptsReturn = true;
        }
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
        if (sender is BindGrid bg)
        {

        }
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse { IsSuccess = false };

        switch (request.MessageAction) 
        { 
            case "SetPatientSearchResult":
                break;
        }


        return response;
    }

    private void OnEditValueChanged_CheckEdit(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
    {
        var element = sender as CheckEdit;
        if (element is null) return;

        bool isChecked = (bool)e.NewValue;
        if (isChecked && SmartUI.MsgYesNo("신환예약 등록으로 변경하시겠습니까? 환자정보가 초기화됩니다.") is MessageBoxResult.Yes)
        {
            vm.ClearData(true, false);
        }
    }
}
