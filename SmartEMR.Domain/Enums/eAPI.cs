namespace SmartEMR.Domain.Enums;

public enum eAPI
{
    CommonCode_GetCommonCode = 0,

    Consultation_GetConsultation = 1,
    Consultation_GetConsultationByRCP = 2,
    Consultation_SetConsultation = 3,

    ConsultationOrder_GetConsultationOrder = 4,
    ConsultationOrder_SetConsultationOrder = 5,

    Insurance_GetInsurance = 6,
    Insurance_GetRecentInsurance = 7,
    Insurance_SetInsurance = 8,

    Login_login = 9,

    Member_GetMember = 10,
    Member_SetMember = 11,

    MemberUser_GetMemberUser = 12,
    MemberUser_SetMemberUser = 13,

    Order_GetOrder = 14,

    Patient_GetPatient = 15,
    Patient_SetPatient = 16,

    Pay_GetPay = 17,
    Pay_SetPay = 18,

    Reception_CancelReception = 19,
    Reception_GetReception = 20,
    Reception_GetReceptionBoard = 21,
    Reception_SetReception = 22,
    Reception_SetReceptionByRES = 23,

    Reservation_GetReservation = 24,
    Reservation_MoveReservationDate = 25,
    Reservation_SetReservation = 26,
    Reservation_SetReservationByStatus = 27
}