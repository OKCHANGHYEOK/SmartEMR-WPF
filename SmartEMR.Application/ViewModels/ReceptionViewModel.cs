using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class ReceptionViewModel : BaseViewModel<Reception>
{
    public List<MemberUser> arrMUR_DOC { get; set; } = default!;
    public List<MemberUser> arrMUR_STF { get; set; } = default!;

    public List<ChartCommonCode> arrRCP_Status { get; set; } = default!;
    public List<ChartCommonCode> arrRCP_Subject { get; set; } = default!;
    public List<ChartCommonCode> arrRCP_VisitType { get; set; } = default!;
    public List<ChartCommonCode> arrRCP_Route { get; set; } = default!;
    public List<ChartCommonCode> arrRCP_InsuranceType { get; set; } = default!;

    public override void Initialize()
    {
    }

    protected override Reception GetModel(Reception item)
    {
        return item;
    }

}
