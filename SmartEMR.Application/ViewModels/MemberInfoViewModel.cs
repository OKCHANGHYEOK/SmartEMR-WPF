using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class MemberInfoViewModel : MemberViewModel
{
    public MemberInfoViewModel(IMemberService memberService) : base(memberService) { }

    public MemberInfoViewModel(IMemberService memberService, Member item) : base(memberService, item) { }
}
