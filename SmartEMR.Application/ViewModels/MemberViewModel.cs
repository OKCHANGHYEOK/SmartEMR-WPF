using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class MemberViewModel : BaseViewModel<Member>
{
    protected readonly IMemberService _memberService;

    public MemberViewModel(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public MemberViewModel(IMemberService memberService, Member item) : base(item)
    {
        _memberService = memberService;
    }

    public override void Initialize()
    {
    }

    protected override Member GetModel(Member item)
    {
        return item;
    }
}
