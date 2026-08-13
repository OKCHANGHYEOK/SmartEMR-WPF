using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRCST;

/// <summary>
/// vSmartEMRConsultationTabCST.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRConsultationTabCST : ModelViewLayout<ConsultationViewModel>
{
    private Consultation Consultation => vm.Model;

    public vSmartEMRConsultationTabCST() { }

    protected override async void Initialize()
    {
        if (Consultation.CST_Idx.GetValueOrDefault(0) == 0)
        {
            Consultation.CST_Status = "";
            Consultation.CST_Subject = "";
            Consultation.CST_InsuranceType = "";
        }

        await vm.UpdateConsultationsByRCP();
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override async void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
        var bindGrid = sender as BindGrid;
        if (bindGrid is null) return;

        await vm.UpdateConsultationsByRCP();
    }
}