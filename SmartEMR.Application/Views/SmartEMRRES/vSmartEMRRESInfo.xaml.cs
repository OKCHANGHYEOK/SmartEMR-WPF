using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using DevExpress.Xpf.Editors;
using SmartEMR.Application.Common;
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
        if (vm.SelectedPatient.PAT_Idx > 0)
        {
            chkIsNewPAT.IsEnabled = false;
        }
        
        if (vm.Model.RES_Idx > 0)
        {
            SearchPanel.IsEnabled = false;
        }
    }

    protected override void SetBindGrid()
    {
        // 뷰 로드시 Collapsed 인 경우 비주얼트리 탐색에 실패해 수동 등록 처리
        AddBindGrid(RESInfo_PatientInfo.PatientInfoGrid);
        AddBindGrid(RESInfo_PatientView.PatientViewGrid);

        var deRES_ReservationDate = RESInfoGrid.GetBindItem<Xpf.DateEdit>("RES_ReservationDate");
        if (deRES_ReservationDate is not null)
        {
            deRES_ReservationDate.ShowToday = false;
            deRES_ReservationDate.ShowClearButton = false;

            // 신규 예약 등록인 경우 과거날짜의 선택을 막음
            if (vm.Model.RES_Idx.GetValueOrDefault(0) == 0)
            {
                if (deRES_ReservationDate is not null)
                {
                    deRES_ReservationDate.MinValue = DateTime.Today;
                }
            }
            else
            {
                var RES_ReservationDate = vm.Model.RES_ReservationDate;
                if (string.IsNullOrWhiteSpace(RES_ReservationDate)) return;

                if (deRES_ReservationDate is not null)
                {
                    deRES_ReservationDate.MinValue = DateTime.Parse(RES_ReservationDate);
                }
            }
        }
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
        if (sender is BindGrid bg)
        {

        }
    }

    public override async void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
        var bindGrid = sender as BindGrid;
        if (bindGrid is null || e.BindItem is null) return;

        var bindItem = e.BindItem;
        var newValue = e.NewValue?.ToString();

        if (bindGrid == RESInfo_PatientInfo.PatientInfoGrid)
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
        else if (bindGrid == RESInfo_PatientView.PatientViewGrid)
        {

        }
        else if (bindGrid == RESInfoGrid)
        {
            if (RESInfoGrid.IsPreventBindGridEvent) return;

            switch (bindItem.FieldName)
            {
                case "RES_ReservationDate":
                    RESInfoGrid.IsPreventBindGridEvent = true;

                    await vm.UpdateReservations(newValue);

                    RESInfoGrid.IsPreventBindGridEvent = false;

                    break;

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

            case "SetRESListBoxScrollToTop":
                {
                    var paramItem = request.MessageParameter as ReservationSlot;
                    if (paramItem is null) return null;

                    ReservationSlotListBox.ScrollIntoView(paramItem);
                    break;
                }

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

    private void SetSelectedSlot(string? reservationTime = "")
    {
        if (vm.Reservations is null) return;

        if (vm.Model.RES_Idx.GetValueOrDefault(0) > 0)
        {
            ReservationSlotListBox.SelectedItem = vm.Reservations.FirstOrDefault(x => x.RESItem is not null && x.RESItem.RES_Idx == vm.Model.RES_Idx);
        }
        else
        {
            ReservationSlotListBox.SelectedItem = vm.Reservations.FirstOrDefault(x => x.RES_Time == reservationTime);
        }
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

    private void OnEditValueChanged_ListBoxEdit(object sender, EditValueChangedEventArgs e)
    {
        var listBoxEdit = sender as ListBoxEdit;
        if (listBoxEdit is null) return;

        SmartUI.BeginInvoke(() =>
        {
            listBoxEdit.ScrollIntoView(e.NewValue);
        }, DispatcherPriority.Background);
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
