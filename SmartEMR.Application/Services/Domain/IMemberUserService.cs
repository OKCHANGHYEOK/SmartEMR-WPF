using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public interface IMemberUserService
{
    Task<ServiceResult<MemberUser>> GetMemberUserByCheckDuplicateId(string MUR_Id);
    Task<ServiceResult<MemberUser>> GetMemberUser(MemberUser item);
    Task<ServiceResult<MemberUser>> GetMemberUsers(MemberUser item);
    Task<ServiceResult<MemberUser>> SetMemberUser(MemberUser item);
    Task<ServiceResult<MemberUser>> SignUp(MemberUser memberUser);
}
