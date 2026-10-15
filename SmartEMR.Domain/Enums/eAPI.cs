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

    Order_GetOrder = 24,

    Patient_GetPatient = 25,
    Patient_SetPatient = 26,

    Pay_GetPay = 27,
    Pay_SetPay = 28,
    Pay_CancelPay = 29,

    PayItem_GetPayItem = 30,
    PayItem_SetPayItem = 31,

    Reception_CancelReception = 32,
    Reception_GetReception = 33,
    Reception_GetReceptionBoard = 34,
    Reception_SetReception = 35,
    Reception_SetReceptionByRES = 36,

    Reservation_GetReservation = 37,
    Reservation_MoveReservationDate = 38,
    Reservation_SetReservation = 39,
    Reservation_SetReservationByStatus = 40,

    NaverPay_ApplyPayment = 41,
}