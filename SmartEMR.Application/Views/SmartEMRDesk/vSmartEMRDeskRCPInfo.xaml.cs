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

        public Patient PATItem { get; set; } = new();

        protected override void Initialize()
        {
            MaskControl.ShowButton = false;
        }

        protected override void SetBindGrid()
        {
            this.BindGrids[0].GetBindItem<CheckEdit>("chkSetNowDT")?.EditValueChanged += OnEditValueChanged_CheckBoxEdit;
            this.BindGrids[0].GetBindItem<CheckEdit>("chkSetNowDT")?.IsChecked = true;

            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.Height = 40;
            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.Margin = new Thickness(1);

            this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_DOC")?.ItemsSource = vm.arrMUR_DOC;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_STF")?.ItemsSource = vm.arrMUR_STF;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_Subject")?.ItemsSource = vm.arrRCP_Subject;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_VisitType")?.ItemsSource = vm.arrRCP_VisitType;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_Route")?.ItemsSource = vm.arrRCP_Route;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_InsuranceType")?.ItemsSource = vm.arrRCP_InsuranceType;

            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_Memo")?.Margin = new Thickness(1);
        }

        public override async Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
        {
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.bindItem;
            if (bindItem == null) return;

            switch (bindItem.FieldName)
            {
                case "btnSetIRC":
                    break;
            }
        }

        public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.BindItem;
            if (bindItem == null) return;

            var fieldName = bindItem.FieldName;
            switch (fieldName)
            {
                case "RCP_Subject":
                    var newValue = e.NewValue?.ToString();
                    if (!string.IsNullOrWhiteSpace(newValue) && newValue == "ETC")
                    {
                        this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.IsEnabled = true;
                    }
                    else
                    {
                        this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.IsEnabled = false;
                    }

                    break;
            }
        }

        public override async Task SetPatientData(Patient item)
        {
            if (item.PAT_Idx.GetValueOrDefault(0) == 0) return;

            // 환자 정보 세팅
            SmartMVVM.ModelProperty.SetPatientData(PATItem, item);

            // 오늘 날짜의 접수정보 조회
            var getRCP = new Reception
            {
                PAT_Idx = item.PAT_Idx,
                RCP_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd")
            };

            var retRCP = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_GetReception, getRCP);
            if (SmartMVVM.DataStore.retIsSuccess == false)
            {
                SmartUI.SetNofification("접수 정보를 불러오지 못했습니다.", NotificationType.Error);
            }

            SetReceptionData(retRCP);
        }

        public void ClearData()
        {
            RCPItem.RCP_Idx = 0;
            RCPItem.MUR_Idx_DOC = 0;
            RCPItem.MUR_Idx_STF = 0;
            RCPItem.RES_Idx = 0;
            RCPItem.RCP_VisitType = "FIR";
            RCPItem.RCP_Status = "";
            RCPItem.RCP_Route = "DSK";
            RCPItem.RCP_Subject = "GNR";
            RCPItem.RCP_SubjectName = "";
            RCPItem.RCP_InsuranceType = "NOR";
            RCPItem.RCP_ReceiptDate = DateTime.Now.ToString("yyyy-MM-dd");
            RCPItem.RCP_ReceiptTime = DateTime.Now.ToString("HH:mm");
            RCPItem.RCP_StartTreatTime = "";
            RCPItem.RCP_EndTreatTime = "";
            RCPItem.RCP_Memo = "";

            PATItem = new();

            MaskControl.MaskText = "환자선택 후 접수 등록할 수 있습니다.";
            MaskControl.Visibility = Visibility.Visible;
            MaskControl.ShowButton = false;
        }

        [RelayCommand]
        private void ShowRCPInfo()
        {
            MaskControl.Visibility = Visibility.Collapsed;
        }

        private void SetReceptionData(Reception? item)
        {
            if (item == null)
            {
                item = new Reception();
            }

            SmartMVVM.ModelProperty.SetReceptionData(RCPItem, item);

            if (RCPItem.RCP_Idx == 0)
            {
                RCPItem.RCP_Subject = "GNR";
                RCPItem.RCP_VisitType = "FIR";
                RCPItem.RCP_Route = "DSK";
                RCPItem.RCP_InsuranceType = "NOR";

                MaskControl.MaskText = "오늘 날짜의 접수내역이 없습니다.";
                MaskControl.ShowButton = true;
            }
            else
            {
                btnSaveRCP.Content = "접수수정";

                MaskControl.MaskVisibility = Visibility.Collapsed;
            }
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
