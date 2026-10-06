using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Services.Domain;

public class MemberService : BaseService, IMemberService
{
    public MemberService(IDataStore dataStore) : base(dataStore)
    {
    }

    public async Task<ServiceResult<Member>> GetMember(Member item)
    {
        var result = new ServiceResult<Member>();
        var ret = await _dataStore.GetItem<Member>(eAPI.Member_GetMember, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "기관정보 조회에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Member>> GetMemberByDuplicateMediNo(string mediNo)
    {
        var result = new ServiceResult<Member>();
        var ret = await _dataStore.GetItem<Member>(eAPI.Member_GetMemberByCheckDuplicateMediNo, new Member { MEM_MediNo = mediNo });

        if (!_dataStore.retIsSuccess)
        {
            result.Message = "요양기관번호 중복체크중 오류가 발생했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Member>> GetMembers(Member item)
    {
        var result = new ServiceResult<Member>();
        var ret = await _dataStore.GetItems<Member>(eAPI.Member_GetMember, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "기관목록 조회에 실패했습니다.";
            return result;
        }

        result.Items = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Member>> SignUp(Member member, MemberUser memberUser)
    {
        var result = new ServiceResult<Member>();
        
        var item = SmartMVVM.ModelProperty.GetSignUpData(member, memberUser);
        var ret = await _dataStore.GetItem<Member>(eAPI.Member_SignUp, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "회원가입에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }

    public async Task<ServiceResult<Member>> SetMember(Member item)
    {
        var result = new ServiceResult<Member>();
        var ret = await _dataStore.GetItem<Member>(eAPI.Member_SetMember, item);

        if (ret is null || !_dataStore.retIsSuccess)
        {
            result.Message = "기관정보 저장에 실패했습니다.";
            return result;
        }

        result.Item = ret;
        result.IsSuccess = true;

        return result;
    }
}
