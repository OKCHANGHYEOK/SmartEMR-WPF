using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Services.Domain;

public class MemberUserService : BaseService, IMemberUserService
{
    public MemberUserService(IDataStore dataStore) : base(dataStore)
    {
    }

    public async Task<ServiceResult<MemberUser>> GetMemberUserByCheckDuplicateId(string MUR_Id)
    {
        var result = new ServiceResult<MemberUser>();
        var ret = await _dataStore.GetItem<MemberUser>(eAPI.MemberUser_GetMemberUserByCheckDuplicateId, new MemberUser { MUR_Id = MUR_Id });

        if (!_dataStore.retIsSuccess)
        {
            result.Message = "아이디 중복체크에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<MemberUser>> GetMemberUser(MemberUser item)
    {
        var result = new ServiceResult<MemberUser>();
        var ret = await _dataStore.GetItem<MemberUser>(eAPI.MemberUser_GetMemberUser, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "회원정보 조회에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<MemberUser>> GetMemberUsers(MemberUser item)
    {
        var result = new ServiceResult<MemberUser>();
        var ret = await _dataStore.GetItems<MemberUser>(eAPI.MemberUser_GetMemberUser, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "회원목록 조회에 실패했습니다.";
            return result;
        }

        result.Items = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<MemberUser>> SetMemberUser(MemberUser item)
    {
        var result = new ServiceResult<MemberUser>();
        var ret = await _dataStore.GetItem<MemberUser>(eAPI.MemberUser_SetMemberUser, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "회원정보 저장에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<MemberUser>> SignUp(MemberUser item)
    {
        var result = new ServiceResult<MemberUser>();
        var ret = await _dataStore.GetItem<MemberUser>(eAPI.MemberUser_SignUp, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "회원가입에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }
}
