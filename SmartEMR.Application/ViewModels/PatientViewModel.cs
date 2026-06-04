using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public abstract class PatientViewModel : BaseViewModel<Patient>
{
    public IQueryable<object>? arrPAT_BirthYear { get; set; }
    public IQueryable<object>? arrPAT_BirthMonth { get; set; }
    public IQueryable<object>? arrPAT_BirthDay { get; set; }
    public IQueryable<object>? arrPAT_IsSolar { get; set; }

    public IQueryable<Patient>? arrPAT_Sex { get; set; }
    public IQueryable<ChartCommonCode>? arrPAT_SourceType { get; set; }
    public IQueryable<object>? arrPAT_IsForegin { get; set; }
    public IQueryable<object>? arrPAT_IsAgreePersonalInfo { get; set; }

    public override async void Initialize()
    {
        arrPAT_BirthYear = SmartMVVM.Common.GetBirth(eBirthType.Year);
        arrPAT_BirthMonth = SmartMVVM.Common.GetBirth(eBirthType.Month);
        arrPAT_BirthDay = SmartMVVM.Common.GetBirth(eBirthType.Day);
        arrPAT_IsSolar = SmartMVVM.Master.Query("PAT_IsSolar");

        arrPAT_Sex = SmartMVVM.Master.Query<Patient>("PAT_Sex");
        arrPAT_IsForegin = SmartMVVM.Master.Query("PAT_IsForegin");
        arrPAT_IsAgreePersonalInfo = SmartMVVM.Master.Query("PAT_IsAgreePersonalInfo");

        arrPAT_SourceType = await SmartMVVM.Common.GetChartCommonCode("PAT", "SourceType");
    }

    protected override abstract Patient GetModel(Patient item);
}
