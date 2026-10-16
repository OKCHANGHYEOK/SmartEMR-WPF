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
    MemberUser_FindId = 24,

    Order_GetOrder = 25,

    Patient_GetPatient = 26,
    Patient_SetPatient = 27,

    Pay_GetPay = 28,
    Pay_SetPay = 29,
    Pay_CancelPay = 30,

    PayItem_GetPayItem = 31,
    PayItem_SetPayItem = 32,

    Reception_CancelReception = 33,
    Reception_GetReception = 34,
    Reception_GetReceptionBoard = 35,
    Reception_SetReception = 36,
    Reception_SetReceptionByRES = 37,

    Reservation_GetReservation = 38,
    Reservation_MoveReservationDate = 39,
    Reservation_SetReservation = 40,
    Reservation_SetReservationByStatus = 41,

    NaverPay_ApplyPayment = 42,
}