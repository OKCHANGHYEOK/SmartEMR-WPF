using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common;

public record ValidateResult(bool IsSuccess, string? Message);

public interface IBaseValidator<T> where T : BaseEntity
{
   abstract static ValidateResult Validate(T item);
}

public class MemberRequiredFieldValidator : IBaseValidator<Member>
{
    private static readonly Dictionary<string, string> requiredFields = new()
    {
        [nameof(Member.MEM_Name)] = "의료기관명",
        [nameof(Member.MEM_BizNum)] = "요양기관번호",
        [nameof(Member.MEM_BizType)] = "기관종구분"
    };

    public static ValidateResult Validate(Member item)
    {
        foreach (var (fieldName, displayName) in requiredFields)
        {
            var property = typeof(Member).GetProperty(fieldName);
            var value = property?.GetValue(item);

            if (value is null || value is string str && string.IsNullOrWhiteSpace(str))
            {
                return new ValidateResult(false, $"{displayName}을 입력해주세요.");
            }
        }

        return new ValidateResult(true, "");
    }
}

public class MemberUserRequiredFieldValidator : IBaseValidator<MemberUser>
{
    private static readonly Dictionary<string, string> requiredFields = new()
    {
        [nameof(MemberUser.MUR_Name)] = "이름",
        [nameof(MemberUser.MUR_Id)] = "아이디",
        [nameof(MemberUser.MUR_PassWord)] = "비밀번호",
        [nameof(MemberUser.MUR_Department)] = "부서",
        [nameof(MemberUser.MUR_JobCode)] = "직책",
        [nameof(MemberUser.MUR_LicenseNo)] = "의료면허번호"
    };

    public static ValidateResult Validate(MemberUser item)
    {
        foreach (var (fieldName, displayName) in requiredFields)
        {
            var property = typeof(MemberUser).GetProperty(fieldName);
            var value = property?.GetValue(item);

            if (value is null || value is string str && string.IsNullOrWhiteSpace(str))
            {
                return new ValidateResult(false, $"{displayName}을 입력해주세요.");
            }
        }

        return new ValidateResult(true, "");
    }
}