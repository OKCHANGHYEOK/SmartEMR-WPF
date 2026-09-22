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

    MemberUser_GetMemberUser = 14,
    MemberUser_SetMemberUser = 15,

    Order_GetOrder = 16,

    Patient_GetPatient = 17,
    Patient_SetPatient = 18,

    Pay_GetPay = 19,
    Pay_SetPay = 20,

    PayItem_GetPayItem = 21,
    PayItem_SetPayItem = 22,

    Reception_CancelReception = 23,
    Reception_GetReception = 24,
    Reception_GetReceptionBoard = 25,
    Reception_SetReception = 26,
    Reception_SetReceptionByRES = 27,

    Reservation_GetReservation = 28,
    Reservation_MoveReservationDate = 29,
    Reservation_SetReservation = 30,
    Reservation_SetReservationByStatus = 31
}