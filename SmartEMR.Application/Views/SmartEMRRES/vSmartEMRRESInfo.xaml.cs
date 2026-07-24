using DevExpress.Xpf.Editors;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRRES;

/// <summary>
/// vSmartEMRDeskPATInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRRESInfo : ModelViewLayout<ReservationInfoViewModel>
{
    private bool _isUpdatedRegNo1 = false;

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
        // 뷰 로드시 Collapsed 인 경우 비주얼트리 탐색에 실패해 수동 등록 처리
        AddBindGrid(RESInfo_PatientView.PatientViewGrid);
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
        if (sender is BindGrid bg)
        {

        }
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
        var bindGrid = sender as BindGrid;
        if (bindGrid is null || e.BindItem is null) return;

        var bindItem = e.BindItem;
        var newValue = e.NewValue?.ToString();

        if (bindGrid == this.BindGrids[0])
        {
            switch (bindItem.FieldName)
            {
                case "PAT_RegisterNum1":
                    if (newValue != null && newValue.Length == 6)
                    {
                        _isUpdatedRegNo1 = true;
                    }
                    else
                    {
                        _isUpdatedRegNo1 = false;
                    }

                    vm.UpdateInputPatientByRegisterNum1(_isUpdatedRegNo1);

                    break;

                case "PAT_RegisterNum2":
                    if (!_isUpdatedRegNo1) return;

                    vm.UpdateInputPatientByRegisterNum2(newValue);

                    break;
            }
        }
        else if (bindGrid == this.BindGrids[1])
        {

        }
        else if (bindGrid == this.BindGrids[2])
        {
            switch (bindItem.FieldName)
            {
                case "RES_ReservationTime":
                    {
                        SetSelectedSlot(newValue);
                        break;
                    }
            }
        }
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse { IsSuccess = false };

        switch (request.MessageAction)
        {
            case "SetSelectedSlot":
                var paramItem = request.MessageParameter as Reservation;
                if (paramItem is null) return null;

                SetSelectedSlot(paramItem.RES_ReservationTime);
                break;

            case "SetPatientSearchResult":
                break;

            case "CloseView":
                SmartUI.CloseView(TargetViewType.CurrentView);
                break;
        }


        return response;
    }

    private void SetSelectedSlot(string? selectedSlot)
    {
        if (vm.Reservations is null || string.IsNullOrWhiteSpace(selectedSlot)) return;

        ReservationSlotListBox.SelectedItem = vm.Reservations.FirstOrDefault(x => x.RES_Time == selectedSlot);
    }

    private void OnEditValueChanging_CheckEdit(object sender, EditValueChangingEventArgs e)
    {
        var element = sender as Xpf.CheckEdit;
        if (element is null) return;

        bool isChecked = (bool)e.NewValue;
        if (isChecked)
        {
            var bFlag = vm.ClearData(true, false);
            if (!bFlag)
            {
                e.IsCancel = true;
            }
        }
    }

    private void OnEditValueChanged_ListBoxEdit(object sender, EditValueChangedEventArgs e)
    {
        var element = sender as ListBoxEdit;
        if (element is null) return;

        var selectedSlot = e.NewValue as ReservationSlot;
        if (selectedSlot is null) return;

        vm.UpdateSelectedSlot(selectedSlot);
    }
}
