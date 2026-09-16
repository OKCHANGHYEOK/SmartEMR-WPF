using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SmartEMR.Application.Views.SmartEMRRCP;

/// <summary>
/// vSmartEMRDeskRCVInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRIRCInfo : ModelViewLayout<InsuranceInfoViewModel>
{
    public Insurance IRCItem
    {
        get
        {
            return vm.Model;
        }
    }

    public Patient PATItem { get; set; } = new();

    private string[] NonInsuranceDisabledFields =
    {
        "IRC_CertNum", "IRC_ContractorName", "IRC_InsuredName",
        "chkIsSameAsContractor", "IRC_CoName", "vIRC_CoName",
        "IRC_EffectiveYYMMDD", "IRC_ExpiredYYMMDD", "IRC_Specific"
    };

    private CheckEdit? chkIsSameWithIRCByRCP
    {
        get
        {
            return this.BindGrids[0].GetBindItem<CheckEdit>("chkIsSameWithIRCByRCP");
        }
    }

    public vSmartEMRIRCInfo() { }

    public vSmartEMRIRCInfo(Insurance item) : base(item) { }

    protected override void Initialize()
    {
    }

    public void SetData(Insurance item)
    {
        vm.SetData(item);
    }

    protected override void SetBindGrid()
    {
        this.BindGrids[0].GetBindItem<Label>("vIRC_Type")?.HorizontalContentAlignment = HorizontalAlignment.Left;

        this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_CoName")?.HorizontalAlignment = HorizontalAlignment.Stretch;
        this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_CoName")?.Height = 38;
        this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_CoName")?.Margin = new Thickness(1, 0, 1, 0);

        this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_CertNum")?.Margin = new Thickness(1);
        this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_ContractorName")?.Margin = new Thickness(1);
        this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_InsuredName")?.Margin = new Thickness(1);
        this.BindGrids[0].GetBindItem<StyleTextBox>("IRC_Specific")?.Margin = new Thickness(1);

        if (this.ViewMode == ViewMode.POPUP)
        {
            bool isEnabled = true;

            if (vm.IsIRCFromRCP())
            {
                this.BindGrids[0].IsPreventBindGridEvent = true;
                chkIsSameWithIRCByRCP?.IsChecked = true;
                this.BindGrids[0].IsPreventBindGridEvent = false;

                isEnabled = false;
            }
             
            if (IRCItem.IRC_Type == "NON")
            {
                isEnabled = false;
            }

            UpdateEnabledState(isEnabled, ["IRC_Type"], ["IRC_Type"]);
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
                {
                    if (e.NewValue is bool isChecked && isChecked)
                    {
                        IRCItem.IRC_InsuredName = IRCItem.IRC_ContractorName;
                    }

                    break;
                }

            case "chkIsSameWithIRCByRCP":
                {
                    if (e.NewValue is not bool isChecked) return;

                    if (isChecked)
                    {
                        var arrRun = new List<Inline>();
                        arrRun.Add(new Run("진료 보험을 접수 보험과 동일하게\n설정하시겠습니까?"));
                        arrRun.Add(new LineBreak());
                        arrRun.Add(new Run("(수정중인 보험 정보는 저장되지 않습니다.)") { Foreground = Brushes.DimGray, FontSize = 12 });

                        if (SmartUI.MsgYesNo(arrRun) is MessageBoxResult.Yes)
                        {
                            vm.SetDataByRCP();
                        }
                        else
                        {
                            IRCItem.IRC_Idx_From = 0;

                            e.Cancel = true;
                            return;
                        }
                    }

                    if (isChecked)
                    {
                        UpdateEnabledState(false, additionalDisabledFields:["IRC_Type"]);
                    }
                    else
                    {
                        UpdateEnabledState(true, additionalEnabledFields:["IRC_Type"]);
                    }

                    break;
                }
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
                if (ViewMode == ViewMode.POPUP)
                {
                    if (IRCItem.IRC_Type == "NON")
                    {
                        if (SmartUI.MsgYesNo("비보험으로 변경시 입력중인 보험정보가 초기화됩니다.\n 변경하시겠습니까?") is MessageBoxResult.Yes)
                        {
                            ClearData(false);
                        }
                        else
                        {
                            e.IsCancel = true;
                            return;
                        }
                    }

                    UpdateEnabledState(IRCItem.IRC_Type != "NON");
                }

                break;
        }
    }


    public override void SetViewData(object? parameter = null)
    {
        if (parameter is ViewMode viewMode)
        {
            this.ViewMode = viewMode;
        }
    }

    public void SetInsurance(Insurance item)
    {
        SmartMVVM.ModelProperty.SetInsuranceData(IRCItem, item);
    }

    public void SetInsuranceType(string IRC_Type)
    {
        IRCItem.IRC_Type = IRC_Type;
        IRCItem.vIRC_Type = SmartMVVM.Common.GetCommonCode("RCP", "InsuranceType")?.FirstOrDefault(x => x.CCI_Cd == IRC_Type)?.CCI_Name;
    }

    public void ClearData(bool isClearIRCType = true)
    {
        vm.ClearIRCData(isClearIRCType);
    
        if (chkIsSameWithIRCByRCP is not null && chkIsSameWithIRCByRCP.IsChecked.GetValueOrDefault(false))
        {
            chkIsSameWithIRCByRCP.IsChecked = false;
        }

        UpdateEnabledState(false, ["IRC_Type"]);
    }

    private void UpdateEnabledState(bool enableInsuranceFields, string[]? additionalEnabledFields = null, string[]? additionalDisabledFields = null)
    {
        IEnumerable<string>? enabledFields;
        IEnumerable<string>? disabledFields;

        if (enableInsuranceFields)
        {
            enabledFields = NonInsuranceDisabledFields.Concat(additionalEnabledFields ?? []);
            disabledFields = additionalDisabledFields;
        }
        else
        {
            enabledFields = additionalEnabledFields;
            disabledFields = NonInsuranceDisabledFields.Concat(additionalDisabledFields ?? []);
        }

        if (enabledFields != null)
        {
            foreach (var fieldName in enabledFields)
            {
                var element = this.BindGrids[0].GetBindItem<FrameworkElement>(fieldName);
                if (element is null) continue;

                element.IsEnabled = true;
            }
        }

        if (disabledFields != null)
        {
            foreach (var fieldName in disabledFields)
            {
                var element = this.BindGrids[0].GetBindItem<FrameworkElement>(fieldName);
                if (element is null) continue;

                element.IsEnabled = false;
            }
        }
    }

    private void OnClick_SimpleButton(object sender, RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        switch (element.Tag)
        {
            case "btnClear":
                if (SmartUI.MsgYesNo("보험정보를 초기화하시겠습니까?") != MessageBoxResult.Yes) return;

                this.BindGrids[0].IsPreventBindGridEvent = true;

                ClearData();

                this.BindGrids[0].IsPreventBindGridEvent = false;

                break;
        }
    }
}
