namespace SmartEMR.Domain.Enums;

public enum eAPI
{
    Auth_RequestVerifyCode = 0,
    Auth_RequestVerifyCheck = 1,

    CommonCode_GetCommonCode = 2,

    Consultation_GetConsultation = 3,
    Consultation_GetConsultationByRCP = 4,
    Consultation_CancelConsultation = 5,
    Consultation_SetConsultation = 6,
    Consultation_SetConsultationByCST = 7,

    ConsultationOrder_GetConsultationOrder = 8,
    ConsultationOrder_SetConsultationOrder = 9,

    Insurance_GetInsurance = 10,
    Insurance_GetRecentInsurance = 11,
    Insurance_SetInsurance = 12,

    Login_login = 13,

    Member_GetMember = 14,
    Member_GetMemberByCheckDuplicateMediNo = 15,
    Member_SetMember = 16,
    Member_SignUp = 17,

    MemberUser_GetMemberUserByCheckDuplicateId = 18,
    Member_GetMemberByCheckDuplicateBizNum = 19,
    MemberUser_GetMemberUser = 20,
    MemberUser_SetMemberUser = 21,
    MemberUser_SignUp = 22,
    MemberUser_ChangePassword = 23,
    MemberUser_ResetPassword = 24,
    MemberUser_FindId = 25,

    Order_GetOrder = 26,

    Patient_GetPatient = 27,
    Patient_SetPatient = 28,

    Pay_GetPay = 29,
    Pay_SetPay = 30,
    Pay_CancelPay = 31,

    PayItem_GetPayItem = 32,
    PayItem_SetPayItem = 33,

    Reception_CancelReception = 34,
    Reception_GetReception = 35,
    Reception_GetReceptionBoard = 36,
    Reception_SetReception = 37,
    Reception_SetReceptionByRES = 38,

    Reservation_GetReservation = 39,
    Reservation_MoveReservationDate = 40,
    Reservation_SetReservation = 41,
    Reservation_SetReservationByStatus = 42,

    NaverPay_ApplyPayment = 43,
}