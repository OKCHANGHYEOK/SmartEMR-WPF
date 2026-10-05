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
    Member_SetMember = 13,

    MemberUser_GetMemberUserByCheckDuplicateId = 14,
    MemberUser_GetMemberUser = 15,
    MemberUser_SetMemberUser = 16,

    Order_GetOrder = 17,

    Patient_GetPatient = 18,
    Patient_SetPatient = 19,

    Pay_GetPay = 20,
    Pay_SetPay = 21,
    Pay_CancelPay = 22,

    PayItem_GetPayItem = 23,
    PayItem_SetPayItem = 24,

    Reception_CancelReception = 25,
    Reception_GetReception = 26,
    Reception_GetReceptionBoard = 27,
    Reception_SetReception = 28,
    Reception_SetReceptionByRES = 29,

    Reservation_GetReservation = 30,
    Reservation_MoveReservationDate = 31,
    Reservation_SetReservation = 32,
    Reservation_SetReservationByStatus = 33,

    NaverPay_ApplyPayment = 34
}