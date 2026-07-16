using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskRCVInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskPAYInfo : ModelViewLayout<SmartEMRIRCInfoViewModel>
    {
        public Insurance IRCItem
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
            this.BindGrids[0].GetBindItem<Label>("vIRC_Type")?.HorizontalContentAlignment = HorizontalAlignment.Left;

            this.BindGrids[0].GetBindItem<ComboBoxEdit>("IRC_CoName")?.ItemsSource = vm.arrIRC_CoName;

            this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_CoName")?.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_CoName")?.Height = 38;
            this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_CoName")?.Margin = new Thickness(1, 0, 1, 0);

            this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_CertNum")?.Margin = new Thickness(1);
            this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_ContractorName")?.Margin = new Thickness(1);
            this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_InsuredName")?.Margin = new Thickness(1);
            this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_Specific")?.Margin = new Thickness(1);
        }

        public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
        {
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.BindItem;
            if (bindGrid == null) return;

            switch (bindItem.FieldName)
            {
                case "chkIsSameAsContractor":
                    if (e.NewValue == null) return;

                    var isChecked = (bool)e.NewValue;

                    if (isChecked)
                    {
                        IRCItem.IRC_InsuredName = IRCItem.IRC_ContractorName;
                    }

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
                case "IRC_CoName":
                    var selectedItem = this.BindGrids[0].GetBindItem<ComboBoxEdit>("IRC_CoName")?.SelectedItem as Insurance;
                    if (selectedItem != null && selectedItem.IRC_CoCd == "ETC")
                    {
                        this.BindGrids[0].GetBindItem<StyleTextBox>("vIRC_CoName")?.IsEnabled = true;
                    }
                    else
                    {
                        this.BindGrids[0].GetBindItem<StyleTextBox>("vIRC_CoName")?.IsEnabled = false;
                    }

                    break;
            }
        }

        public void SetInsurance(Insurance item)
        {
            SmartMVVM.ModelProperty.SetInsuranceData(IRCItem, item);
        }

        public void SetInsuranceType(string IRC_Type)
        {
            bool isNON = IRC_Type == "NON";

            if (IRCItem.IRC_Type != IRC_Type)
            {
                ClearData(false, isNON);
            }

            IRCItem.IRC_Type = IRC_Type;
            IRCItem.vIRC_Type = SmartMVVM.Common.GetCommonCode("RCP", "InsuranceType")?.FirstOrDefault(x => x.CCI_Cd == IRC_Type)?.CCI_Name;

            if (!isNON)
            {
                SetCoNameComboBoxItemsSource();
            }
        }

        public void ClearData(bool isNewRCP = true, bool isNON = true)
        {
            vm.ClearData();

            if (isNewRCP)
            {
                IRCItem.IRC_Idx = 0;
                IRCItem.PAT_Idx = 0;
                IRCItem.RCP_Idx = 0;
            }

            if (isNON)
            {
                IRCItem.IRC_Type = "NON";
                IRCItem.vIRC_Type = "비보험";
            }
        }

        private void SetCoNameComboBoxItemsSource()
        {
            var cmbIRC_CoName = this.BindGrids[0].GetBindItem<ComboBoxEdit>("IRC_CoName");
            if (cmbIRC_CoName != null)
            {
                cmbIRC_CoName.ItemsSource = vm.arrIRC_CoName.Where(x => x.IRC_Type == IRCItem.IRC_Type);
                cmbIRC_CoName.SelectedIndex = 0;
            }
        }

        private void OnClick_Button(object sender, RoutedEventArgs e)
        {
            var element = sender as Button;
            if (element == null) return;

            switch (element.Name)
            {
                case "btnClear":
                    if (SmartUI.MsgYesNo("보험구분을 제외한 정보가 초기화됩니다. 초기화하시겠습니까?") != MessageBoxResult.Yes) return;

                    ClearData(false);

                    break;
            }
        }
    }
}
