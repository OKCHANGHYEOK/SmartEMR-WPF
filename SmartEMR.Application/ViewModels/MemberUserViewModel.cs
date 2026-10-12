using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class MemberUserViewModel : BaseViewModel<MemberUser>
{
    public override void Initialize()
    {
    }

    protected override MemberUser GetModel(MemberUser item)
    {
        return item;
    }
}
