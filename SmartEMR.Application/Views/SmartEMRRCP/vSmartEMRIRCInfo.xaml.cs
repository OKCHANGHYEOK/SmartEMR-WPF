using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;

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

        if (ViewMode == ViewMode.POPUP)
        {
            this.BindGrids[0].GetBindItem<Label>("vIRC_Type")?.Visibility = Visibility.Collapsed;
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

                    SetViewLayout();
                }

                break;
        }
    }

    protected override void SetViewLayout()
    {
        MaskControl.Visibility = Visibility.Collapsed;

        var isEnabled = IRCItem.IRC_Type != "NON";

        foreach (var fieldName in NonInsuranceDisabledFields)
        {
            var element = this.BindGrids[0].GetBindItem<FrameworkElement>(fieldName);
            if (element is null) continue;

            element.IsEnabled = isEnabled;
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
        vm.ClearData(isClearIRCType);
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
