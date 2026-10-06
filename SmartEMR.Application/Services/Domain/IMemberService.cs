using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public interface IMemberService
{
    Task<ServiceResult<Member>> GetMember(Member item);
    Task<ServiceResult<Member>> GetMemberByDuplicateMediNo(string mediNo);
    Task<ServiceResult<Member>> GetMembers(Member item);
    Task<ServiceResult<Member>> SignUp(Member member, MemberUser memberUser);
    Task<ServiceResult<Member>> SetMember(Member item);
}
