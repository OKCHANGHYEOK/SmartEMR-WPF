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

    protected override void Initialize()
    {
    }

    public async void SetData(Reception item)
    {
       await vm.SetReceptionData(item);
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

        var stbRCP_Memo = this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_Memo");
        if (stbRCP_Memo != null)
        {
            stbRCP_Memo.Margin = new Thickness(1);
            stbRCP_Memo.AcceptsReturn = true;
        }
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
        var bindGrid = sender as BindGrid;
        if (bindGrid == null) return;

        var bindItem = e.BindItem;
        if (bindItem == null) return;

        var newValue = e.NewValue?.ToString();

        switch (bindItem.FieldName)
        {
            case "RCP_ReceiptDate":
                if (string.IsNullOrWhiteSpace(newValue)) return;

                if (SmartMVVM.Common.IsHoliday(newValue) && SmartUI.MsgYesNo("휴일을 선택하셨습니다. 계속 하시겠습니까?") is MessageBoxResult.No)
                {
                    e.Cancel = true;
                }

                break;

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