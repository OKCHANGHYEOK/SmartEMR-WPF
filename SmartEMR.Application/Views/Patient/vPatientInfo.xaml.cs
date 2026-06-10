using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Data;

namespace SmartEMR.Application.Views;

/// <summary>
/// vPatientInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vPatientInfo : ModelViewLayout<PatientInfoViewModel>
{
    public Patient PATItem
    {
        get => vm.Model;
        set => vm.Model = value;
    }

    private bool _isUpdatedRegNo1 = false;

    public vPatientInfo() 
    { 
    }

    public vPatientInfo(Patient item) : base(item) 
    {
    }

    protected override void Initialize()
    {
        this.ViewTitle = "환자" + (PATItem.PAT_Idx.GetValueOrDefault(0) == 0 ? "등록" : "수정");

        btnSave.Content = "환자" + (PATItem.PAT_Idx.GetValueOrDefault(0) == 0 ? "등록" : "수정");

        this.BindGrids[0].GetBindItem<StyleTextBox>("PAT_ChartNo")?.Focusable = false;
        this.BindGrids[0].GetBindItem<StyleTextBox>("PAT_ChartNo")?.IsReadOnly = true;

        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_Sex")?.Margin = new Thickness(2);
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_SourceType")?.Margin = new Thickness(2);
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_IsForegin")?.Margin = new Thickness(2);
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_IsAgreePersonalInfo")?.Margin = new Thickness(2);

        this.BindGrids[0].GetBindItem<StyleTextBox>("PAT_Bigo")?.AcceptsReturn = true;
    }

    protected override void SetBindGrid()
    {
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_Sex")?.ItemsSource = vm.arrPAT_Sex;
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_IsSolar")?.ItemsSource = vm.arrPAT_IsSolar;
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_SourceType")?.ItemsSource = vm.arrPAT_SourceType;
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_IsForegin")?.ItemsSource = vm.arrPAT_IsForegin;
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_IsAgreePersonalInfo")?.ItemsSource = vm.arrPAT_IsAgreePersonalInfo;
    }

    public override async Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        var bindItem = sender as BindItem;
        if (bindItem == null) return;

        switch (bindItem.FieldName)
        {
            case "btnSMS":
                SmartUI.SetNofification("현재 지원하지 않는 기능입니다.", NotificationType.Warning);
                break;
        }
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
        var element = sender as BindGrid;
        if (element == null) return;

        var bindItem = e.BindItem;
        var newValue = e.NewValue?.ToString();

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
                    
                    PATItem.PAT_BirthDate = "";
                }

                break;

            case "PAT_RegisterNum2":
                if (!_isUpdatedRegNo1) return;

                if (newValue != null && newValue.Length > 0)
                {
                    var firstChar = newValue[0];
                    var century = firstChar switch
                    {
                        '1' or '2' => "19",
                        '3' or '4' => "20",
                        _ => ""
                    };

                    var gender = firstChar switch
                    {
                        '1' or '3' => "M",
                        '2' or '4' => "F",
                        _ => ""
                    };

                    if (!string.IsNullOrWhiteSpace(century))
                    {
                        PATItem.PAT_BirthDate = century + PATItem.PAT_RegisterNum1;
                    }

                    if (!string.IsNullOrWhiteSpace(gender))
                    {
                        PATItem.PAT_Sex = gender;
                    }
                }
                else
                {
                    PATItem.PAT_BirthDate = "";
                    PATItem.PAT_Sex = "";
                }

                break;
        }
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse { IsSuccess = false };

        switch (request.MessageAction)
        {
            case "CloseView":
                SmartUI.CloseView(TargetViewType.CurrentView);
                break;
        }

        return response;
    }

    private void OnClick_Button(object sender, RoutedEventArgs e)
    {
        var element = sender as Button;
        if (element == null) return;

        switch (element.Name)
        {
            case "btnSelectImage":
                var fileResult = SelectImage();
                if (fileResult == null) return;

                PATItem.PAT_ImageSource = fileResult;

                break;

            case "btnClearImage":
                if (PATItem.PAT_ImageSource != null && PATItem.PAT_ImageSource.Length > 0)
                {
                    PATItem.PAT_ImageSource = null;
                }

                break;

            case "btnFindAddress":
                SmartUI.SetNofification("현재 지원하지 않는 기능입니다.", NotificationType.Warning);
                break;
        }
    }
}
