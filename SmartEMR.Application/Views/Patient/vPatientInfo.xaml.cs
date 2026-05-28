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
    private Patient PATItem => vm.Model;

    protected override void Initialize()
    {
        this.ViewSize = new Size(600,400);
        this.ViewTitle = "환자" + (PATItem.PAT_Idx == 0 ? "등록" : "수정");
    }

    public override async Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        // 클릭 이벤트 구현
    }
}
