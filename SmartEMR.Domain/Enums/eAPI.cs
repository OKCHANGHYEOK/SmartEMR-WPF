namespace SmartEMR.Domain.Enums;

public enum eAPI
{
    CommonCode_GetCommonCode = 0,

    Consultation_GetConsultation = 1,
    Consultation_GetConsultationByRCP = 2,
    Consultation_SetConsultation = 3,
    Consultation_SetConsultationByCST = 4,

    ConsultationOrder_GetConsultationOrder = 5,
    ConsultationOrder_SetConsultationOrder = 6,

    Insurance_GetInsurance = 7,
    Insurance_GetRecentInsurance = 8,
    Insurance_SetInsurance = 9,

    Login_login = 10,

    Member_GetMember = 11,
    Member_SetMember = 12,

    MemberUser_GetMemberUser = 13,
    MemberUser_SetMemberUser = 14,

    Order_GetOrder = 15,

    Patient_GetPatient = 16,
    Patient_SetPatient = 17,

    Pay_GetPay = 18,
    Pay_SetPay = 19,

    Reception_CancelReception = 20,
    Reception_GetReception = 21,
    Reception_GetReceptionBoard = 22,
    Reception_SetReception = 23,
    Reception_SetReceptionByRES = 24,

    Reservation_GetReservation = 25,
    Reservation_MoveReservationDate = 26,
    Reservation_SetReservation = 27,
    Reservation_SetReservationByStatus = 28
}