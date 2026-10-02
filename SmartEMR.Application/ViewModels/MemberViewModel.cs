using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class MemberViewModel : BaseViewModel<Member>
{
    public override void Initialize()
    {
    }

    protected override Member GetModel(Member item)
    {
        return item;
    }
}
