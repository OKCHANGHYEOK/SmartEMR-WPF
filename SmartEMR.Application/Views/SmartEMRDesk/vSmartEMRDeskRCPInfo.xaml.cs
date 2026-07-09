using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Windows;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskRCVInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskRCPInfo : ModelViewLayout<SmartEMRRCPInfoViewModel>
    {
        public Reception RCPItem
        {
            get
            {
                return vm.Model;
            }
        }

        protected override void Initialize()
        {
            MaskControl.ShowButton = false;
        }

        protected override void SetBindGrid()
        {
            this.BindGrids[0].GetBindItem<CheckEdit>("chkSetNowDT")?.EditValueChanged += OnEditValueChanged_CheckBoxEdit;
            this.BindGrids[0].GetBindItem<CheckEdit>("chkSetNowDT")?.IsChecked = true;

            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.Height = 38;
            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.Margin = new Thickness(1,0,1,0);

            this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_DOC")?.ItemsSource = vm.arrMUR_DOC;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_STF")?.ItemsSource = vm.arrMUR_STF;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_Subject")?.ItemsSource = vm.arrRCP_Subject;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_VisitType")?.ItemsSource = vm.arrRCP_VisitType;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_Route")?.ItemsSource = vm.arrRCP_Route;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_InsuranceType")?.ItemsSource = vm.arrRCP_InsuranceType;

            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_Memo")?.Margin = new Thickness(1);
            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_Memo")?.AcceptsReturn = true;
        }

        public override async Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
        {
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.BindItem;
            if (bindItem == null) return;

            switch (bindItem.FieldName)
            {
            }
        }

        public override async void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.BindItem;
            if (bindItem == null) return;

            var fieldName = bindItem.FieldName;
            var newValue = e.NewValue?.ToString();

            switch (fieldName)
            {
                case "RCP_Subject":
                    if (!string.IsNullOrWhiteSpace(newValue) && newValue == "ETC")
                    {
                        this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.IsEnabled = true;
                    }
                    else
                    {
                        this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.IsEnabled = false;
                    }

                    break;

                case "RCP_InsuranceType":
                    if (!string.IsNullOrWhiteSpace(newValue) && newValue == "NON")
                    {
                        this.BindGrids[0].GetBindItem<Button>("btnSetIRC")?.IsEnabled = false;
                    } 
                    else
                    {
                        this.BindGrids[0].GetBindItem<Button>("btnSetIRC")?.IsEnabled = true;
                    }

                    await SmartUI.SendMessage("SetInsuranceType", newValue, viewType:TargetViewType.PageView);

                    break;
            }
        }

        public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
        {
            var response = new ViewMessageResponse { IsSuccess = false };

            switch (request.MessageAction)
            {
                case "SetReception":
                    var paramItem = request.MessageParameter as Reception;
                    if (paramItem == null) return null;

                    UpdateReceptionData(paramItem);

                    break;
            }

            return response;
        }

        public override async void SetPatientData(Patient item)
        {
            if (item.PAT_Idx.GetValueOrDefault(0) == 0) return;

            // 환자 정보 세팅
            vm.SetPatientData(item);

            UpdateReceptionData();
        }

        public void ClearData()
        {
            vm.ClearData();

            MaskControl.MaskText = "환자선택 후 접수 등록할 수 있습니다.";
            MaskControl.ShowButton = false;

            btnSaveRCP.Content = "접수등록";
        }

        private async void UpdateReceptionData(Reception? item = null)
        {
            vm.SetReceptionData(item);

            Insurance? IRCItem = null;

            if (RCPItem.RCP_Idx.GetValueOrDefault(0) > 0)
            {
                var retIRC = await SmartMVVM.DataStore.GetItem<Insurance>(eAPI.Insurance_GetInsurance, new Insurance { PAT_Idx = RCPItem.PAT_Idx, RCP_Idx = RCPItem.RCP_Idx });
                if (retIRC != null)
                {
                    IRCItem = retIRC;
                }

                btnSaveRCP.Content = "접수수정";
            }
            else
            {
                RCPItem.RCP_Subject = "GNR";
                RCPItem.RCP_VisitType = "FIR";
                RCPItem.RCP_Route = "DSK";
                RCPItem.RCP_InsuranceType = "NON";

                MaskControl.MaskText = "오늘 날짜의 접수내역이 없습니다.";
                MaskControl.ShowButton = true;
            }

            await SmartUI.SendMessage("SetInsurance", IRCItem, viewType: TargetViewType.PageView);
        }

        [RelayCommand]
        private void ShowRCPInfo()
        {
            MaskControl.Visibility = Visibility.Collapsed;
        }

        private void OnEditValueChanged_CheckBoxEdit(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var element = sender as CheckEdit;
            if (element == null) return;

            if (element.IsChecked.GetValueOrDefault(true))
            {
                this.BindGrids[0].GetBindItem<DateEdit>("RCP_ReceiptDate")?.IsEnabled = false;
                this.BindGrids[0].GetBindItem<DateEdit>("RCP_ReceiptTime")?.IsEnabled = false;

                RCPItem.RCP_ReceiptDate = DateTime.Now.ToString("yyyy-MM-dd");
                RCPItem.RCP_ReceiptTime = DateTime.Now.ToString("HH:mm");
            }
            else
            {
                this.BindGrids[0].GetBindItem<DateEdit>("RCP_ReceiptDate")?.IsEnabled = true;
                this.BindGrids[0].GetBindItem<DateEdit>("RCP_ReceiptTime")?.IsEnabled = true;
            }
        }
    }
}
