using SmartEMR.Application.Common.SelectionItems;
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
    public partial class vSmartEMRDeskIRCInfo : ModelViewLayout<SmartEMRIRCInfoViewModel>
    {
        public Insurance IRCItem
        {
            get
            {
                return vm.Model;
            }
        }

        public Patient PATItem { get; set; } = new();

        public static DependencyProperty MaskControlVisiblityProperty =
            DependencyProperty.Register(nameof(MaskControlVisiblity), typeof(Visibility), typeof(vSmartEMRDeskIRCInfo), new PropertyMetadata(Visibility.Visible));


        public Visibility MaskControlVisiblity
        {
            get => (Visibility)GetValue(MaskControlVisiblityProperty);
            set => SetValue(MaskControlVisiblityProperty, value);
        }

        private string[] _InputItems = { "IRC_CertNum", "IRC_ContractorName", "IRC_InsuredName", "chkIsSameAsContractor", "IRC_CoName", "vIRC_CoName", "IRC_EffectiveYYMMDD", "IRC_ExpiredYYMMDD", "IRC_Specific" };

        protected override void Initialize()
        {
        }

        protected override void SetBindGrid()
        {
            this.BindGrids[0].GetBindItem<Label>("vIRC_Type")?.HorizontalContentAlignment = HorizontalAlignment.Left;

            var cmbIRC_CoName = this.BindGrids[0].GetBindItem<ComboBoxEdit>("IRC_CoName");
            if (cmbIRC_CoName is not null)
            {
                cmbIRC_CoName.EditValueChanged += (s, e) =>
                {
                    SmartUI.BeginInvoke(() =>
                    {
                        if (string.IsNullOrWhiteSpace(e.NewValue?.ToString()))
                        {
                            cmbIRC_CoName.SelectedIndex = 0;
                        }

                    }, System.Windows.Threading.DispatcherPriority.Background);
                };
            }
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
                case "IRC_Type":
                    var newValue = e.NewValue?.ToString();
                    if (!string.IsNullOrWhiteSpace(newValue))
                    {
                        SetLayoutByInsuranceType(newValue);
                    }

                    break;

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
                cmbIRC_CoName.ItemsSource = InputOptionItems.InsuranceCoperations.Where(x => x.IRC_Type == IRCItem.IRC_Type);
                cmbIRC_CoName.SelectedIndex = 0;
            }
        }

        private void SetLayoutByInsuranceType(string IRC_Type)
        {
            bool isEnabled = IRC_Type != "NON";

            foreach (string fieldName in _InputItems)
            {
                var element = this.BindGrids[0].GetBindItem<FrameworkElement>(fieldName);
                if (element is not null ) 
                {
                    element.IsEnabled = isEnabled;
                }
            }
        }
    }
}
