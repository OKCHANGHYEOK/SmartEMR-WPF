using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRIRCInfoViewModel : BaseViewModel<Insurance>
{
    public IQueryable<Insurance> arrIRC_Coperation { get; private set; } = default!;

    public override void Initialize()
    {
        arrIRC_Coperation = SmartMVVM.Master.Query<Insurance>("IRC_Coperation");   
    }

    protected override Insurance GetModel(Insurance item)
    {
        item.IRC_Coperation = "삼성화재";
        item.IRC_EffectiveYYMMDD = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
        item.IRC_ExpiredYYMMDDD = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");

        return item;
    }
}
