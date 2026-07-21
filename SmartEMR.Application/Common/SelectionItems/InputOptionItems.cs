using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.SelectionItems;

public class InputOptionItems
{
    public static IEnumerable<Patient> Sexes => SmartMVVM.Master.Query<Patient>("PAT_Sex");
    public static IEnumerable<object> CalendarTypes => SmartMVVM.Master.Query("PAT_IsSolar");
    public static IEnumerable<object> NationalityTypes => SmartMVVM.Master.Query("PAT_IsForegin");
    public static IEnumerable<CommonCode> SourceTypes => SmartMVVM.Common.GetCommonCode("PAT", "SourceType");

    public static IEnumerable<MemberUser> Docters => SmartMVVM.Master.GetMemberUsers("DOC", true, "의사선택");
    public static IEnumerable<MemberUser> Staffs => SmartMVVM.Master.GetMemberUsers("STF", true, "직원선택");
    public static IEnumerable<CommonCode> Subjects => SmartMVVM.Common.GetCommonCode("RCP", "Subject");
    public static IEnumerable<CommonCode> VisitTypes => SmartMVVM.Common.GetCommonCode("RCP", "VisitType");
    public static IEnumerable<CommonCode> RouteTypes => SmartMVVM.Common.GetCommonCode("RCP", "Route");
    public static IEnumerable<CommonCode> InsuranceTypes => SmartMVVM.Common.GetCommonCode("RCP", "InsuranceType");

    public static IEnumerable<Insurance> InsuranceCoperations => SmartMVVM.Master.Query<Insurance>("IRC_CoName");
}
