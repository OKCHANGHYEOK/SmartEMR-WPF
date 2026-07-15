using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class PatientViewModel : BaseViewModel<Patient>
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

    private List<CommonCode>? _arrPAT_SourceType;
    public List<CommonCode>? arrPAT_SourceType
    {
        get => _arrPAT_SourceType;
        set
        {
            _arrPAT_SourceType = value;
            OnPropertyChanged(nameof(arrPAT_SourceType));
        }
    }

    public override void Initialize() { }

    public override async Task InitializeAsync()
    {
        arrPAT_BirthYear = SmartMVVM.Common.GetBirth(eBirthType.Year);
        arrPAT_BirthMonth = SmartMVVM.Common.GetBirth(eBirthType.Month);
        arrPAT_BirthDay = SmartMVVM.Common.GetBirth(eBirthType.Day);
        arrPAT_IsSolar = SmartMVVM.Master.Query("PAT_IsSolar");

        arrPAT_Sex = SmartMVVM.Master.Query<Patient>("PAT_Sex");
        arrPAT_IsForegin = SmartMVVM.Master.Query("PAT_IsForegin");
        arrPAT_IsAgreePersonalInfo = SmartMVVM.Master.Query("PAT_IsAgreePersonalInfo");

        arrPAT_SourceType = SmartMVVM.Common.GetCommonCode("PAT", "SourceType");
    }

    protected override Patient GetModel(Patient item)
    {
        if (item.PAT_Idx.GetValueOrDefault(0) == 0)
        {
            item.PAT_IsAgreePersonalInfo = "y";
            item.vPAT_IsAgreePersonalInfo = item.PAT_IsAgreePersonalInfo == "y" ? "개인정보제공 동의" : "개인정보제공 미동의";
        }

        return item;
    }

    public void ClearData()
    {
        SmartMVVM.ModelProperty.ClearPATData(Model);
    }
}
