using DevExpress.Xpf.Editors;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SmartEMR.Application.Views.SmartEMRRES;

/// <summary>
/// vSmartEMRDeskPATInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRRESInfo : ModelViewLayout<ReservationInfoViewModel>
{
    private bool _isUpdatedRegNo1 = false;
    private bool _isCloseView = true;

    public static readonly DependencyProperty IsPopupOpenProperty =
        DependencyProperty.Register(nameof(IsPopupOpen), typeof(bool), typeof(vSmartEMRRESInfo), new PropertyMetadata(false));

    public bool IsPopupOpen
    {
        get => (bool)GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }

    public vSmartEMRRESInfo() { }
    public vSmartEMRRESInfo(Reservation item) : base(item) { }

    protected override void Initialize()
    {
        this.ViewTitle = "예약" + (vm.Model.RES_Idx.GetValueOrDefault(0) == 0 ? "등록" : "수정");
    }

    protected override void SetViewLayout()
    {
        if (vm.SelectedPatient.PAT_Idx.GetValueOrDefault(0) > 0)
        {
            chkIsNewPAT.IsEnabled = false;
        }
        else
        {
            SearchPanel.IsEnabled = false;
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

    public override bool ClosingFloatPanel()
    {
        if (!_isCloseView)
        {
            _isCloseView = true;
            return false;
        }

        return true;
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse { IsSuccess = false };

        switch (request.MessageAction)
        {
            case "SetSelectedSlot":
                {
                    var paramItem = request.MessageParameter as Reservation;
                    if (paramItem is null) return null;

                    SetSelectedSlot(paramItem.RES_ReservationTime);
                    break;
                }

            case "SetPatientSearchResult":
                PatientPopup.IsOpen = true;
                break;

            case "UpdatePatientData":
                {
                    var paramItem = request.MessageParameter as Patient;
                    if (paramItem is null) return null;

                    vm.SetSelectedPatient(paramItem);
                    break;
                }

            case "MovePatientInfo":
                await SmartUI.NavigateToPage(new vPatientInfo(new Patient { PAT_Idx = vm.SelectedPatient.PAT_Idx }), FromViewType.POPUP,isPopup: true);
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

    private void OnPreviewMouseLeftButtonDown_CheckEdit(object sender, MouseButtonEventArgs e)
    {
        var element = sender as Xpf.CheckEdit;
        if (element is null) return;

        e.Handled = true;

        bool isChecked = element.IsChecked.GetValueOrDefault(false);
        if (!isChecked)
        {
            bool bFlag = vm.ClearData(true, false);
            if (!bFlag)
            {
                element.IsChecked = !isChecked;
                
                SearchPanel.ClearData();
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

    private void OnPreviewKeyDown_SearchPanel(object sender, KeyEventArgs e)
    {
        var element = sender as SearchPanel;
        if (element is null) return;

        bool isFocusInSearchEdit = element.SearchEdit.IsKeyboardFocusWithin;
        if (!isFocusInSearchEdit) return;

        if (e.Key == Key.Escape && IsPopupOpen)
        {
            IsPopupOpen = false;
            _isCloseView = false;
        }

        if (e.Key == Key.Down && PatientListBox.SelectedIndex == -1)
        {
            PatientListBox.Focus();
            PatientListBox.SelectedIndex = 0;
            e.Handled = true;
        }
    }

    private void OnPreviewKeyDown_Popup(object sender, System.Windows.Input.KeyEventArgs e)
    {
        var popup = sender as Popup;
        if (popup is null) return;
        
        if (e.Key == Key.Up && PatientListBox.SelectedIndex == 0)
        {
            SearchPanel.SetFocusToSearchEdit();
            PatientListBox.SelectedIndex = -1;
            e.Handled = true;
        }

        if (e.Key == Key.Escape && IsPopupOpen)
        {
            IsPopupOpen = false;
            _isCloseView = false;
        }
    }

    private void OnClosed_Popup(object sender, EventArgs e)
    {
        var popup = sender as Popup;
        if (popup is null) return;

        SearchPanel.SetFocusToSearchEdit();
    }

    private void OnPreviewKeyDown_ListBoxEdit(object sender, System.Windows.Input.KeyEventArgs e)
    {
        var listBoxEdit = sender as ListBoxEdit;
        if (listBoxEdit is null) return;

        if (e.Key == Key.Enter) 
        {
            var patient = listBoxEdit.SelectedItem as Patient;
            if (patient is null) return;

            SetSelectedPatient(patient);
        }

        if (e.Key == Key.Escape)
        {
            IsPopupOpen = false;
            _isCloseView = false;
        }
    }

    private void OnPreviewMouseLeftButtonUp_ListBoxEdit(object sender, MouseButtonEventArgs e)
    {
        var listBoxEdit = sender as ListBoxEdit;
        if (listBoxEdit is null) return;

        var patient = listBoxEdit.SelectedItem as Patient;
        if (patient is null) return;

        SetSelectedPatient(patient);
    }

    private void SetSelectedPatient(Patient item)
    {
        SearchPanel.SetSelectedPatient(item);

        vm.SetSelectedPatient(item);

        chkIsNewPAT.IsChecked = false;

        IsPopupOpen = false;
    }
}
