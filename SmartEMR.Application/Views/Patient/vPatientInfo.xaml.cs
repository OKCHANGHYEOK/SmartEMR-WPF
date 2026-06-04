using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;

namespace SmartEMR.Application.Views;

/// <summary>
/// vPatientInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vPatientInfo : ModelViewLayout<PatientInfoViewModel>
{
    public Patient PATItem => vm.Model;

    protected override void Initialize()
    {
        this.ViewTitle = "환자" + (PATItem.PAT_Idx == 0 ? "등록" : "수정");

        btnSave.Content = "환자" + (PATItem.PAT_Idx == 0 ? "등록" : "수정");

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
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_Sex")?.SelectedIndex = 0;

        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_IsSolar")?.ItemsSource = vm.arrPAT_IsSolar;
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_IsSolar")?.SelectedIndex = 0;

        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_SourceType")?.ItemsSource = vm.arrPAT_SourceType;
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_SourceType")?.SelectedIndex = 0;

        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_IsForegin")?.ItemsSource = vm.arrPAT_IsForegin;
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_IsForegin")?.SelectedIndex = 0;

        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_IsAgreePersonalInfo")?.ItemsSource = vm.arrPAT_IsAgreePersonalInfo;
        this.BindGrids[0].GetBindItem<ComboBoxEdit>("PAT_IsAgreePersonalInfo")?.SelectedIndex = 0;
    }

    public override async Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        // 클릭 이벤트 구현
    }
}
