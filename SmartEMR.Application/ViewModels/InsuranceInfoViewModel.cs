using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class InsuranceInfoViewModel : BaseViewModel<Insurance>
{
    public IQueryable<Insurance> arrIRC_CoName { get; private set; } = default!;

    public override void Initialize()
    {
        arrIRC_CoName = SmartMVVM.Master.Query<Insurance>("IRC_CoName");
    }

    protected override Insurance GetModel(Insurance item)
    {
        if (item.IRC_Idx.GetValueOrDefault(0) == 0)
        {
            item.IRC_CoName = "삼성화재";
            item.IRC_EffectiveYYMMDD = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            item.IRC_ExpiredYYMMDDD = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
        }

        return item;
    }

    public void SetData(Insurance item)
    {
        SmartMVVM.ModelProperty.SetInsuranceData(Model, item);
    }
}
