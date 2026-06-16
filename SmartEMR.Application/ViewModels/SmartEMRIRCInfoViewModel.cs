using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRIRCInfoViewModel : BaseViewModel<Insurance>
{
    public IQueryable<Insurance> arrIRC_Coperation { get; private set; } = default!;

    public override void Initialize()
    {
        arrIRC_Coperation = SmartMVVM.Master.Query<Insurance>("IRC");   
    }

    protected override Insurance GetModel(Insurance item)
    {
        return item;
    }
}
