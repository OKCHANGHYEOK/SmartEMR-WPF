using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class MemberUserViewModel : BaseViewModel<MemberUser>
{
    protected readonly IMemberUserService _memberUserService;

    public MemberUserViewModel(IMemberUserService memberUserService)
    {
        _memberUserService = memberUserService;
    }

    public MemberUserViewModel(IMemberUserService memberUserService, MemberUser item) : base(item)
    {
        _memberUserService = memberUserService;
    }

    public override void Initialize()
    {
    }

    protected override MemberUser GetModel(MemberUser item)
    {
        return item;
    }
}
