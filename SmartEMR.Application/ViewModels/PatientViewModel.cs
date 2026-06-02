using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class PatientViewModel : BaseViewModel<Patient>
{
    public IQueryable<object>? arrPAT_BirthYear { get; set; }
    public IQueryable<object>? arrPAT_BirthMonth { get; set; }
    public IQueryable<object>? arrPAT_BirthDay { get; set; }

    public override void Initialize()
    {
        arrPAT_BirthYear = SmartMVVM.Common.GetBirth(eBirthType.Year);
        arrPAT_BirthMonth = SmartMVVM.Common.GetBirth(eBirthType.Month);
        arrPAT_BirthDay = SmartMVVM.Common.GetBirth(eBirthType.Day);
    }

    protected override Patient GetModel(Patient item)
    {
        return item;
    }
}
