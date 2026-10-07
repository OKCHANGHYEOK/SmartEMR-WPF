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
    MemberUser_SignUp = 19,

    Order_GetOrder = 20,

    Patient_GetPatient = 21,
    Patient_SetPatient = 22,

    Pay_GetPay = 23,
    Pay_SetPay = 24,
    Pay_CancelPay = 25,

    PayItem_GetPayItem = 26,
    PayItem_SetPayItem = 27,

    Reception_CancelReception = 28,
    Reception_GetReception = 29,
    Reception_GetReceptionBoard = 30,
    Reception_SetReception = 31,
    Reception_SetReceptionByRES = 32,

    Reservation_GetReservation = 33,
    Reservation_MoveReservationDate = 34,
    Reservation_SetReservation = 35,
    Reservation_SetReservationByStatus = 36,

    NaverPay_ApplyPayment = 37
}