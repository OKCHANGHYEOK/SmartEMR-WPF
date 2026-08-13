namespace SmartEMR.Domain.Enums;

public enum  eAPI
{
    Login_login = 0,

    Member_GetMember = 1,
    Member_SetMember = 2,
    MemberUser_GetMemberUser = 3,
    MemberUser_SetMemberUser = 4,
    Patient_GetPatient = 5,
    Patient_SetPatient = 6,
    Chart_GetChart = 7,
    Chart_SetChart = 8,
    CommonCode_GetCommonCode = 9,
    Reservation_GetReservation = 10,
    Reservation_SetReservation = 11,
    Reservation_SetReservationByStatus = 12,
    Reservation_MoveReservationDate = 13,
    Reception_GetReception = 14,
    Reception_GetReceptionBoard = 15,
    Reception_SetReception = 16,
    Reception_SetReceptionByRES = 17,
    Reception_CancelReception = 18,
    Insurance_GetInsurance = 19,
    Insurance_SetInsurance = 20,
    Consultation_GetConsultation = 21,
    Consultation_GetConsultationByRCP = 22,
    Consultation_SetConsultation = 23,
    ConsultationOrder_GetConsultationOrder = 24,
    ConsultationOrder_SetConsultationOrder = 25,
    Pay_GetPay = 26,
    Pay_SetPay = 27
}
