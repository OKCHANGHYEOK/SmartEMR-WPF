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
public partial class vSmartEMRRCPInfo : ModelViewLayout<SmartEMRRCPInfoViewModel>
{
    private Reception RCPItem => vm.Model;

    public vSmartEMRRCPInfo() { }
    public vSmartEMRRCPInfo(Reception item) : base(item) { }

    protected override async void Initialize()
    {
    }

    public void SetData(Reception item)
    {
        vm.SetReceptionData(item);
    }

    protected override void SetBindGrid()
    {
        var chkSetNowDT = this.BindGrids[0].GetBindItem<CheckEdit>("chkSetNowDT");
        if (chkSetNowDT != null)
        {
            chkSetNowDT.EditValueChanged += OnEditValueChanged_CheckBoxEdit;
            chkSetNowDT.IsChecked = true;
        }

        var stbRCP_SubjectName = this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName");
        if (stbRCP_SubjectName != null)
        {
            stbRCP_SubjectName.HorizontalAlignment = HorizontalAlignment.Stretch;
            stbRCP_SubjectName.Height = 38;
            stbRCP_SubjectName.Margin = new Thickness(1, 0, 1, 0);
        }

        var cmbMUR_Idx_DOC = this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_DOC");
        if (cmbMUR_Idx_DOC != null)
        {
            cmbMUR_Idx_DOC.ItemsSource = vm.arrMUR_DOC;
        }

        var cmbMUR_Idx_STF = this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_STF");
        if (cmbMUR_Idx_STF != null)
        {
            cmbMUR_Idx_STF.ItemsSource = vm.arrMUR_STF;
        }

        var cmbRCP_Subject = this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_Subject");
        if (cmbRCP_Subject != null)
        {
            cmbRCP_Subject.ItemsSource = vm.arrRCP_Subject;
        }

        var cmbRCP_VisitType = this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_VisitType");
        if (cmbRCP_VisitType != null)
        {
            cmbRCP_VisitType.ItemsSource = vm.arrRCP_VisitType;
        }

        var cmbRCP_Route = this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_Route");
        if (cmbRCP_Route != null)
        {
            cmbRCP_Route.ItemsSource = vm.arrRCP_Route;
        }

        var cmbRCP_InsuranceType = this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_InsuranceType");
        if (cmbRCP_InsuranceType != null)
        {
            cmbRCP_InsuranceType.ItemsSource = vm.arrRCP_InsuranceType;
        }

        var stbRCP_Memo = this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_Memo");
        if (stbRCP_Memo != null)
        {
            stbRCP_Memo.Margin = new Thickness(1);
            stbRCP_Memo.AcceptsReturn = true;
        }
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

                await SmartUI.SendMessage("SetInsuranceType", newValue, viewType: TargetViewType.PageView);

                break;
        }
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse { IsSuccess = false };

        switch (request.MessageAction)
        {
            case "CloseView":
                SmartUI.CloseView();
                break;
        }

        return response;
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