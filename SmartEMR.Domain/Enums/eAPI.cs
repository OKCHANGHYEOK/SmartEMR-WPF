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

    Reception_CancelReception = 21,
    Reception_GetReception = 22,
    Reception_GetReceptionBoard = 23,
    Reception_SetReception = 24,
    Reception_SetReceptionByRES = 25,

    Reservation_GetReservation = 26,
    Reservation_MoveReservationDate = 27,
    Reservation_SetReservation = 28,
    Reservation_SetReservationByStatus = 29
}