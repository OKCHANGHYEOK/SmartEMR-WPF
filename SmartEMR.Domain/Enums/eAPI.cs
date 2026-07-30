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
    Insurance_GetInsurance = 16,
    Insurance_SetInsurance = 17,
    Consultation_GetConsultation = 18,
    Consultation_SetConsultation = 19,
    ConsultationOrder_GetConsultationOrder = 20,
    ConsultationOrder_SetConsultationOrder = 21,
    Pay_GetPay = 22,
    Pay_SetPay = 23
}
