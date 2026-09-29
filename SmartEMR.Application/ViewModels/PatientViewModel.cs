using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class PatientViewModel : BaseViewModel<Patient>
{
    protected readonly IPatientService _patientService;

    public PatientViewModel(IPatientService patientService) : base() 
    {
        _patientService = patientService;
    }

    public PatientViewModel(IPatientService patientService, Patient item) : base(item) 
    {
        _patientService = patientService;
    }

    public override void Initialize() { }

    protected override Patient GetModel(Patient item)
    {
        if (item.PAT_Idx.GetValueOrDefault(0) == 0)
        {
            item.PAT_IsAgreePersonalInfo = "y";
            item.vPAT_IsAgreePersonalInfo = item.PAT_IsAgreePersonalInfo == "y" ? "개인정보제공 동의" : "개인정보제공 미동의";
        }

        return item;
    }

    public void SetPatientData(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(Model, item);
    }

    public virtual void ClearData()
    {
        SmartMVVM.ModelProperty.ClearPATData(Model);
    }
}
