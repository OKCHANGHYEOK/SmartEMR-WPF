using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class PatientViewModel : BaseViewModel<Patient>
{
    public PatientViewModel() : base() { }
    public PatientViewModel(Patient item) : base(item) { }

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

    public virtual void ClearData()
    {
        SmartMVVM.ModelProperty.ClearPATData(Model);
    }
}
