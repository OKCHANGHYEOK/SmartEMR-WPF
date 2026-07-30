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
    Reception_GetReception = 12,
    Reception_GetReceptionBoard = 13,
    Reception_SetReception = 14,
    Reception_SetReceptionByRES = 15,
    Reception_CancelReception = 16,
    Insurance_GetInsurance = 17,
    Insurance_SetInsurance = 18,
    Consultation_GetConsultation = 19,
    Consultation_SetConsultation = 20,
    ConsultationOrder_GetConsultationOrder = 21,
    ConsultationOrder_SetConsultationOrder = 22,
    Pay_GetPay = 23,
    Pay_SetPay = 24
}
