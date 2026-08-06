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
    Reception_GetReception = 13,
    Reception_GetReceptionBoard = 14,
    Reception_SetReception = 15,
    Reception_SetReceptionByRES = 16,
    Reception_CancelReception = 17,
    Insurance_GetInsurance = 18,
    Insurance_SetInsurance = 19,
    Consultation_GetConsultation = 20,
    Consultation_SetConsultation = 21,
    ConsultationOrder_GetConsultationOrder = 22,
    ConsultationOrder_SetConsultationOrder = 23,
    Pay_GetPay = 24,
    Pay_SetPay = 25
}
