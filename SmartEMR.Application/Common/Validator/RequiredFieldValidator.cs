using SmartEMR.Application.ViewModels;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.Validator;

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
    private static readonly Dictionary<string, string> _memberRequiredFields = new()
    {
        [nameof(Member.MEM_BizType)] = "기관종구분",
        [nameof(Member.MEM_Name)] = "의료기관명",
        [nameof(Member.MEM_MediNo)] = "요양기관번호",
        [nameof(Member.MEM_BizNum)] = "사업자등록번호"
    };


    public static ValidateResult Validate(Member item)
    {
        foreach (var (fieldName, displayName) in _memberRequiredFields)
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

public enum MemberUserSignUpFieldGroup
{
    Default,
    Work
}

public class MemberUserRequiredFieldValidator : IBaseValidator<MemberUser>
{
    private static readonly Dictionary<string, string> _defaultMemberUserRequiredFields = new()
    {
        [nameof(MemberUser.MUR_Name)] = "이름",
        [nameof(MemberUser.MUR_Id)] = "아이디",
        [nameof(MemberUser.MUR_PassWord)] = "비밀번호"
    };

    private static readonly Dictionary<string, string> _workMemberUserRequiredFields = new()
    {
        [nameof(MemberUser.MUR_Department)] = "부서",
        [nameof(MemberUser.MUR_JobCode)] = "직책",
        [nameof(MemberUser.MUR_LicenseNo)] = "의료면허번호"
    };

    public static ValidateResult Validate(MemberUser item)
    {
        foreach (var (fieldName, displayName) in _defaultMemberUserRequiredFields.Concat(_workMemberUserRequiredFields))
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

    public static ValidateResult Validate(MemberUser item, MemberUserSignUpFieldGroup group)
    {
        var targetFields = group switch
        {
            MemberUserSignUpFieldGroup.Default => _defaultMemberUserRequiredFields,
            MemberUserSignUpFieldGroup.Work => _workMemberUserRequiredFields,
            _ => null
        };

        if (targetFields is null)
            return new ValidateResult { IsSuccess = false };

        foreach (var (fieldName, displayName) in targetFields)
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
    private static readonly Dictionary<string, SignUpField> _newMemberFieldMap = new()
    {
        [nameof(MemberUser.MUR_Name)] = SignUpField.UserName,
        [nameof(MemberUser.MUR_Id)] = SignUpField.Id,
        [nameof(MemberUser.MUR_PassWord)] = SignUpField.Password,

        [nameof(Member.MEM_Name)] = SignUpField.MemberName,

        [nameof(MemberUser.MUR_Department)] = SignUpField.Department,
        [nameof(MemberUser.MUR_JobCode)] = SignUpField.JobCode,
        [nameof(MemberUser.MUR_LicenseNo)] = SignUpField.LicenseNo
    };

    private static readonly Dictionary<string, SignUpField> _existMemberFieldMap = new()
    {
        [nameof(MemberUser.MUR_Name)] = SignUpField.UserName,
        [nameof(MemberUser.MUR_Id)] = SignUpField.Id,
        [nameof(MemberUser.MUR_PassWord)] = SignUpField.Password,

        [nameof(Member.MEM_BizType)] = SignUpField.BizType,
        [nameof(Member.MEM_MediNo)] = SignUpField.MediNo,
        [nameof(Member.MEM_BizNum)] = SignUpField.BizNum,

        [nameof(MemberUser.MUR_Department)] = SignUpField.Department,
        [nameof(MemberUser.MUR_JobCode)] = SignUpField.JobCode,
        [nameof(MemberUser.MUR_LicenseNo)] = SignUpField.LicenseNo
    };

    public static SignUpValidateResult ValidateSignUp(Member member, MemberUser memberUser, SignUpType type)
    {
        var result = new SignUpValidateResult();

        SignUpField missingField;

        // 기본입력 항목 검증
        var retMURByDefault = MemberUserRequiredFieldValidator.Validate(memberUser, MemberUserSignUpFieldGroup.Default);
        if (!retMURByDefault.IsSuccess)
        {
            missingField = GetSignUpField(retMURByDefault.MissingField ?? "", type);
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
            missingField = GetSignUpField(retMEM.MissingField ?? "", type);
            if (missingField != SignUpField.None)
            {
                result.SignUpMissingField = missingField;
                return result;
            }
        }

        // 근무 정보검증
        var retMURByWork = MemberUserRequiredFieldValidator.Validate(memberUser, MemberUserSignUpFieldGroup.Work);
        if (!retMURByWork.IsSuccess)
        {

            missingField = GetSignUpField(retMURByWork.MissingField ?? "", type);
            if (missingField != SignUpField.None)
            {
                result.SignUpMissingField = missingField;
                return result;
            }
        }

        result.IsSuccess = true;
        return result;
    }

    private static SignUpField GetSignUpField(string missingField, SignUpType type)
    {
        var map = type switch
        {
            SignUpType.EXIST => _existMemberFieldMap,
            SignUpType.NEW => _newMemberFieldMap,
            _ => null
        };

        if (map is null) return SignUpField.None;

        return map.GetValueOrDefault(missingField, SignUpField.None);
    }
}