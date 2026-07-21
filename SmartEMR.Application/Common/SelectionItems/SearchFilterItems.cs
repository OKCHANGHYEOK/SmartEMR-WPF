using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common.SelectionItems;

public class SearchFilterItems
{

    public static IEnumerable<MemberUser> Docters => SmartMVVM.Master.GetMemberUsers("DOC", true, "담당의구분");
    public static IEnumerable<CommonCode> ReceptionStatuses => SmartMVVM.Common.GetCommonCode("RCP", "Status", "", true, "접수상태");
    public static IEnumerable<CommonCode> InsuranceTypes => SmartMVVM.Common.GetCommonCode("RCP", "InsuranceType", "", true, "보험구분");
    public static IEnumerable<CommonCode> ReservationStatuses => SmartMVVM.Common.GetCommonCode("RES", "Status", "", true, "예약상태");
    public static IEnumerable<CommonCode> RouteTypes => SmartMVVM.Common.GetCommonCode("RCB", "Route", "", true, "방문구분");
    public static IEnumerable<CommonCode> VisitTypes => SmartMVVM.Common.GetCommonCode("RCB", "VisitType", "", true, "초재진구분");
    public static IEnumerable<CommonCode> Subjects => SmartMVVM.Common.GetCommonCode("RCB", "Subject", "", true, "과목구분");

    public static IEnumerable<CommonCode> PayStatuses => SmartMVVM.Common.GetCommonCode("PAY", "Status", "", true, "수납상태");
}
