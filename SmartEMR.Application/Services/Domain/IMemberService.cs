using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public interface IMemberService
{
    Task<ServiceResult<Member>> GetMember(Member item);
    Task<ServiceResult<Member>> GetMemberByDuplicateMediNo(string MEM_MediNo);
    Task<ServiceResult<Member>> GetMemberByDuplicateBizNum(string MEM_BizNum);
    Task<ServiceResult<Member>> GetMembers(Member item);
    Task<ServiceResult<Member>> SignUp(Member member, MemberUser memberUser);
    Task<ServiceResult<Member>> SetMember(Member item);
}
