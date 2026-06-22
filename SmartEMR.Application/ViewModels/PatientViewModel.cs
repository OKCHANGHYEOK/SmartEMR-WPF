using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using System.ComponentModel;

namespace SmartEMR.Application.ViewModels;

public abstract class PatientViewModel : BaseViewModel<Patient>
{
    public PatientViewModel() : base() { }
    public PatientViewModel(Patient item) : base(item) { }

    public IQueryable<object>? arrPAT_BirthYear { get; set; }
    public IQueryable<object>? arrPAT_BirthMonth { get; set; }
    public IQueryable<object>? arrPAT_BirthDay { get; set; }
    public IQueryable<object>? arrPAT_IsSolar { get; set; }

    public IQueryable<Patient>? arrPAT_Sex { get; set; }
    public IQueryable<object>? arrPAT_IsForegin { get; set; }
    public IQueryable<object>? arrPAT_IsAgreePersonalInfo { get; set; }

    private List<ChartCommonCode>? _arrPAT_SourceType;
    public List<ChartCommonCode>? arrPAT_SourceType
    {
        get => _arrPAT_SourceType;
        set
        {
            _arrPAT_SourceType = value;
            OnPropertyChanged(nameof(arrPAT_SourceType));
        }
    }

    public override async Task InitializeAsync()
    {
        arrPAT_BirthYear = SmartMVVM.Common.GetBirth(eBirthType.Year);
        arrPAT_BirthMonth = SmartMVVM.Common.GetBirth(eBirthType.Month);
        arrPAT_BirthDay = SmartMVVM.Common.GetBirth(eBirthType.Day);
        arrPAT_IsSolar = SmartMVVM.Master.Query("PAT_IsSolar");

        arrPAT_Sex = SmartMVVM.Master.Query<Patient>("PAT_Sex");
        arrPAT_IsForegin = SmartMVVM.Master.Query("PAT_IsForegin");
        arrPAT_IsAgreePersonalInfo = SmartMVVM.Master.Query("PAT_IsAgreePersonalInfo");

        arrPAT_SourceType = SmartMVVM.Common.GetChartCommonCode("PAT", "SourceType");
    }

    protected override abstract Patient GetModel(Patient item);
}
