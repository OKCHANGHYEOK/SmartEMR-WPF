namespace SmartEMR.Domain.Enums;

public enum eAPI
{
    CommonCode_GetCommonCode = 0,

    Consultation_GetConsultation = 1,
    Consultation_GetConsultationByRCP = 2,
    Consultation_CancelConsultation = 3,
    Consultation_SetConsultation = 4,
    Consultation_SetConsultationByCST = 5,

    ConsultationOrder_GetConsultationOrder = 6,
    ConsultationOrder_SetConsultationOrder = 7,

    Insurance_GetInsurance = 8,
    Insurance_GetRecentInsurance = 9,
    Insurance_SetInsurance = 10,

    Login_login = 11,

    Member_GetMember = 12,
    Member_GetMemberByCheckDuplicateMediNo = 13,
    Member_SetMember = 14,
    Member_SignUp = 15,

    MemberUser_GetMemberUserByCheckDuplicateId = 16,
    MemberUser_GetMemberUser = 17,
    MemberUser_SetMemberUser = 18,

    Order_GetOrder = 19,

    Patient_GetPatient = 20,
    Patient_SetPatient = 21,

    Pay_GetPay = 22,
    Pay_SetPay = 23,
    Pay_CancelPay = 24,

    PayItem_GetPayItem = 25,
    PayItem_SetPayItem = 26,

    Reception_CancelReception = 27,
    Reception_GetReception = 28,
    Reception_GetReceptionBoard = 29,
    Reception_SetReception = 30,
    Reception_SetReceptionByRES = 31,

    Reservation_GetReservation = 32,
    Reservation_MoveReservationDate = 33,
    Reservation_SetReservation = 34,
    Reservation_SetReservationByStatus = 35,

    NaverPay_ApplyPayment = 36
}