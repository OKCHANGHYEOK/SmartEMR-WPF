using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.Validator;

public static class RequiredFieldMaster
{
    public static readonly Dictionary<string, string> MemberRequiredFields = new()
    {
        [nameof(Member.MEM_BizType)] = "기관종구분",
        [nameof(Member.MEM_Name)] = "의료기관명",
        [nameof(Member.MEM_MediNo)] = "요양기관번호",
        [nameof(Member.MEM_BizNum)] = "사업자등록번호"
    };

    public static readonly Dictionary<string, string> MemberUserRequiredFields = new()
    {
        [nameof(MemberUser.MUR_Name)] = "이름",
        [nameof(MemberUser.MUR_Id)] = "아이디",
        [nameof(MemberUser.MUR_PassWord)] = "비밀번호",
        [nameof(MemberUser.MUR_Department)] = "부서",
        [nameof(MemberUser.MUR_JobCode)] = "직책",
        [nameof(MemberUser.MUR_LicenseNo)] = "의료면허번호"
    };
}

public class ValidateResult
{
    public string? MissingField { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
}

public interface IBaseValidator<T> where T : BaseEntity
{
   abstract static ValidateResult Validate(T item);
}

public class MemberRequiredFieldValidator : IBaseValidator<Member>
{
    public static ValidateResult Validate(Member item)
    {
        foreach (var (fieldName, displayName) in RequiredFieldMaster.MemberRequiredFields)
        {
            var property = typeof(Member).GetProperty(fieldName);
            var value = property?.GetValue(item);

            if (value is null || value is string str && string.IsNullOrWhiteSpace(str))
            {
                return new ValidateResult { MissingField = fieldName, Message = $"{displayName}을 입력해주세요.", IsSuccess = false };
            }
        }

        return new ValidateResult { IsSuccess = true };
    }
}

public class MemberUserRequiredFieldValidator : IBaseValidator<MemberUser>
{
    public static ValidateResult Validate(MemberUser item)
    {
        foreach (var (fieldName, displayName) in RequiredFieldMaster.MemberUserRequiredFields)
        {
            var property = typeof(MemberUser).GetProperty(fieldName);
            var value = property?.GetValue(item);

            if (value is null || value is string str && string.IsNullOrWhiteSpace(str))
            {
                return new ValidateResult { MissingField = fieldName, Message = $"{displayName}을 입력해주세요.", IsSuccess = false };
            }
        }

        return new ValidateResult { IsSuccess = true };
    }
}

public enum SignUpField
{
    None,

    UserName,
    Id,
    Password,

    MemberName,
    MediNo,
    BizNum,
    BizType,

    Department,
    JobCode,
    LicenseNo
}

public class SignUpValidateResult : ValidateResult
{
    public SignUpField SignUpMissingField { get; set; }
}

public class SignUpRequiredFieldValidator
{
    public static SignUpValidateResult ValidateSignUp(Member member, MemberUser memberUser)
    {
        var result = new SignUpValidateResult { };

        SignUpField missingField;

        // 기본입력 항목 검증
        var retMURByDefault = MemberUserRequiredFieldValidator.Validate(memberUser);
        if (!retMURByDefault.IsSuccess)
        {
            missingField = retMURByDefault.MissingField switch
            {
                nameof(MemberUser.MUR_Name) => SignUpField.UserName,
                nameof(MemberUser.MUR_Id) => SignUpField.Id,
                nameof(MemberUser.MUR_PassWord) => SignUpField.Password,
                _ => SignUpField.None
            };

            if (missingField != SignUpField.None)
            {
                result.SignUpMissingField = missingField;
                return result;
            }
        }

        // 기관정보 검증
        var retMEM = MemberRequiredFieldValidator.Validate(member);
        if (!retMEM.IsSuccess)
        {
            missingField = retMEM.MissingField switch
            {
                nameof(Member.MEM_BizType) => SignUpField.BizType,
                nameof(Member.MEM_Name) => SignUpField.MemberName,
                nameof(Member.MEM_MediNo) => SignUpField.MediNo,
                nameof(Member.MEM_BizNum) => SignUpField.BizNum,
                _ => SignUpField.None
            };

            if (missingField != SignUpField.None)
            {
                result.SignUpMissingField = missingField;
                return result;
            }

            return result;
        }

        // 근무 정보검증
        var retMURByWork = MemberUserRequiredFieldValidator.Validate(memberUser);
        if (!retMURByWork.IsSuccess)
        {

            missingField = retMURByWork.MissingField switch
            {
                nameof(MemberUser.MUR_Department) => SignUpField.Department,
                nameof(MemberUser.MUR_JobCode) => SignUpField.JobCode,
                nameof(MemberUser.MUR_LicenseNo) => SignUpField.LicenseNo,
                _ => SignUpField.None
            };
            
            if (missingField != SignUpField.None)
            {
                result.SignUpMissingField = missingField;
                return result;
            }
        }

        result.IsSuccess = true;
        return result;
    }
}