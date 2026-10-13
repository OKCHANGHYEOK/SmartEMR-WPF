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
    MemberUser_GetMemberUser = 19,
    MemberUser_SetMemberUser = 20,
    MemberUser_SignUp = 21,
    MemberUser_ChangePassword = 22,

    Order_GetOrder = 23,

    Patient_GetPatient = 24,
    Patient_SetPatient = 25,

    Pay_GetPay = 26,
    Pay_SetPay = 27,
    Pay_CancelPay = 28,

    PayItem_GetPayItem = 29,
    PayItem_SetPayItem = 30,

    Reception_CancelReception = 31,
    Reception_GetReception = 32,
    Reception_GetReceptionBoard = 33,
    Reception_SetReception = 34,
    Reception_SetReceptionByRES = 35,

    Reservation_GetReservation = 36,
    Reservation_MoveReservationDate = 37,
    Reservation_SetReservation = 38,
    Reservation_SetReservationByStatus = 39,

    NaverPay_ApplyPayment = 40,
}