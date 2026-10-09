namespace SmartEMR.Domain.Enums;

public enum eAPI
{
    Auth_RequestVerifyCode = 0,
    Auth_SetIdentityVerification = 1,

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

    Order_GetOrder = 22,

    Patient_GetPatient = 23,
    Patient_SetPatient = 24,

    Pay_GetPay = 25,
    Pay_SetPay = 26,
    Pay_CancelPay = 27,

    PayItem_GetPayItem = 28,
    PayItem_SetPayItem = 29,

    Reception_CancelReception = 30,
    Reception_GetReception = 31,
    Reception_GetReceptionBoard = 32,
    Reception_SetReception = 33,
    Reception_SetReceptionByRES = 34,

    Reservation_GetReservation = 35,
    Reservation_MoveReservationDate = 36,
    Reservation_SetReservation = 37,
    Reservation_SetReservationByStatus = 38,

    NaverPay_ApplyPayment = 39
}