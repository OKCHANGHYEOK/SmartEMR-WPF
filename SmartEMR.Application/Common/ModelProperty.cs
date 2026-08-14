using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common;

public class ModelProperty
{
    #region "Patient"

    public void SetDefaultPatientData(Patient item)
    {
        item.PAT_Sex = "N";
        item.PAT_SourceType = "WRK";
        item.PAT_IsSolar = "y";
        item.PAT_IsForegin = "n";
        item.PAT_IsAgreePersonalInfo = "n";
        item.PAT_IsSMS = "n";
    }

    public Patient GetPatientDataForSave(Patient paramItem)
    {
        var item = new Patient();
        item.MUR_Idx_DOC = paramItem.MUR_Idx_DOC;
        item.MUR_Idx_STF = paramItem.MUR_Idx_STF;
        item.PAT_Idx = paramItem.PAT_Idx;
        item.PAT_ChartNo = paramItem.PAT_ChartNo;
        item.PAT_Name = paramItem.PAT_Name;
        item.PAT_BloodType = paramItem.PAT_BloodType;
        item.PAT_SourceType = paramItem.PAT_SourceType;
        item.PAT_Sex = paramItem.PAT_Sex;

        if (!string.IsNullOrWhiteSpace(paramItem.PAT_BirthDate))
        {
            var PAT_BirthDate = paramItem.PAT_BirthDate.Replace("-", "");

            if (PAT_BirthDate.Length == 8)
            {
                item.PAT_BirthYear = PAT_BirthDate.Substring(0, 4);
                item.PAT_BirthMonth = PAT_BirthDate.Substring(4, 2);
                item.PAT_BirthDay = PAT_BirthDate.Substring(6, 2);
            } 
        }

        item.PAT_Age = DateTime.Now.Year - Convert.ToInt32(item.PAT_BirthYear);
        item.PAT_RegisterNum1 = paramItem.PAT_RegisterNum1;
        item.PAT_RegisterNum2 = paramItem.PAT_RegisterNum2;
        item.PAT_Hpp1 = paramItem.PAT_PhoneNum?.Substring(0,3);
        item.PAT_Hpp2 = paramItem.PAT_PhoneNum?.Substring(3,4);
        item.PAT_Hpp3 = paramItem.PAT_PhoneNum?.Substring(7,4);
        item.PAT_Email = paramItem.PAT_Email;
        item.PAT_IsSolar = paramItem.PAT_IsSolar;
        item.PAT_IsSMS = paramItem.PAT_IsSMS;
        item.PAT_IsAgreePersonalInfo = paramItem.PAT_IsAgreePersonalInfo;
        item.PAT_ImageSource = paramItem.PAT_ImageSource;
        item.PAT_Bigo = paramItem.PAT_Bigo;
        item.PAT_IsValid = true;

        return item;
    }

    public void SetPatientData(Patient oldItem, Patient newItem)
    {
        oldItem.PAT_Idx = newItem.PAT_Idx;
        oldItem.MUR_Idx_DOC = newItem.MUR_Idx_DOC;
        oldItem.MUR_Idx_STF = newItem.MUR_Idx_STF;
        oldItem.PAT_ChartNo = newItem.PAT_ChartNo;
        oldItem.PAT_Name = newItem.PAT_Name;
        oldItem.PAT_BloodType = newItem.PAT_BloodType;
        oldItem.PAT_SourceType = newItem.PAT_SourceType;
        oldItem.vPAT_SourceType = SmartMVVM.Common.GetCommonCodeName("PAT", "SourceType", newItem.PAT_SourceType ?? "");
        oldItem.PAT_Sex = newItem.PAT_Sex;
        oldItem.vPAT_Sex = newItem.PAT_Sex == "M" ? "남" : "여";
        oldItem.PAT_Age = newItem.PAT_Age;
        oldItem.vPAT_Info = oldItem.vPAT_Sex + "/" + $"{newItem.PAT_Age}세";
        oldItem.PAT_BirthYear = newItem.PAT_BirthYear;
        oldItem.PAT_BirthMonth = newItem.PAT_BirthMonth;
        oldItem.PAT_BirthDay = newItem.PAT_BirthDay;
        oldItem.PAT_BirthDate = $"{newItem.PAT_BirthYear}-{newItem.PAT_BirthMonth}-{newItem.PAT_BirthDay}";
        oldItem.PAT_RegisterNum = newItem.PAT_RegisterNum1 + "-" + new string('*', 7);
        oldItem.PAT_RegisterNum1 = newItem.PAT_RegisterNum1;
        oldItem.PAT_RegisterNum2 = newItem.PAT_RegisterNum2;
        oldItem.PAT_Address1 = newItem.PAT_Address1;
        oldItem.PAT_Address2 = newItem.PAT_Address2;
        oldItem.PAT_Address3 = newItem.PAT_Address3;

        if (!string.IsNullOrWhiteSpace(newItem.PAT_Address1))
        {
            oldItem.vPAT_Address += newItem.PAT_Address1;
        }

        if (!string.IsNullOrWhiteSpace(newItem.PAT_Address2))
        {
            oldItem.vPAT_Address += "  " + newItem.PAT_Address2;
        }

        if (!string.IsNullOrWhiteSpace(newItem.PAT_Address3))
        {
            oldItem.vPAT_Address += "  " + newItem.PAT_Address3;
        }

        if (string.IsNullOrWhiteSpace(oldItem.vPAT_Address))
        {
            oldItem.vPAT_Address = "입력된 주소가 없습니다.";
        }

        oldItem.PAT_Hpp1 = newItem.PAT_Hpp1;
        oldItem.PAT_Hpp2 = newItem.PAT_Hpp2;
        oldItem.PAT_Hpp3 = newItem.PAT_Hpp3;
        oldItem.PAT_PhoneNum = newItem.PAT_Hpp1 + "-" + newItem.PAT_Hpp2 + "-" + newItem.PAT_Hpp3;
        oldItem.PAT_Email = newItem.PAT_Email;
        oldItem.PAT_FirstVisitDate = newItem.PAT_FirstVisitDate;
        oldItem.PAT_LastVisitDate = newItem.PAT_LastVisitDate;
        oldItem.PAT_SourceType = newItem.PAT_SourceType;
        oldItem.PAT_ImageSource = newItem.PAT_ImageSource;
        oldItem.PAT_Bigo = newItem.PAT_Bigo;
        oldItem.PAT_IsAgreePersonalInfo = newItem.PAT_IsAgreePersonalInfo;
        oldItem.vPAT_IsAgreePersonalInfo = newItem.PAT_IsAgreePersonalInfo == "y" ? "개인정보제공동의" : "개인정보제공미동의";
        oldItem.PAT_IsSolar = newItem.PAT_IsSolar;
        oldItem.PAT_IsSMS = newItem.PAT_IsSMS;
        oldItem.NOW_CHT_Idx_RCV = newItem.NOW_CHT_Idx_RCV;
        oldItem.NOW_CHT_Idx_RES = newItem.NOW_CHT_Idx_RES;
        oldItem.NEXT_CHT_Idx_RES = newItem.NEXT_CHT_Idx_RES;
        oldItem.NEXT_CHT_DATE_RES = newItem.NEXT_CHT_DATE_RES;
    }

    public void ClearPATData(Patient item)
    {
        item.PAT_Idx = 0;
        item.MUR_Idx_DOC = 0;
        item.MUR_Idx_STF = 0;
        item.PAT_ChartNo = "";
        item.PAT_Name = "";
        item.PAT_BloodType = "";
        item.PAT_SourceType = "";
        item.vPAT_SourceType = "";
        item.PAT_Sex = "";
        item.vPAT_Sex = "";
        item.PAT_Age = 0;
        item.vPAT_Info = "";
        item.PAT_BirthYear = "";
        item.PAT_BirthMonth = "";
        item.PAT_BirthDay = "";
        item.PAT_BirthDate = "";
        item.PAT_RegisterNum = "";
        item.PAT_RegisterNum1 = "";
        item.PAT_RegisterNum2 = "";
        item.PAT_Address1 = "";
        item.PAT_Address2 = "";
        item.PAT_Address3 = "";
        item.vPAT_Address = "";
        item.PAT_Hpp1 = "";
        item.PAT_Hpp2 = "";
        item.PAT_Hpp3 = "";
        item.PAT_PhoneNum = "";
        item.PAT_Email = "";
        item.PAT_FirstVisitDate = "";
        item.PAT_LastVisitDate = "";
        item.PAT_ImageSource = null;
        item.PAT_IsSMS = "";
        item.PAT_IsSolar = "";
        item.PAT_Bigo = "";
        item.NOW_CHT_Idx_RCV = 0;
        item.NOW_CHT_Idx_RES = 0;
        item.NEXT_CHT_Idx_RES = 0;
        item.NEXT_CHT_DATE_RES = "";
    }

    #endregion

    #region "Reservation"

    public Reservation GetReservationDataForSave(Reservation paramItem, Patient PATItem)
    {
        Reservation item = new Reservation
        {
            RES_Idx = paramItem.RES_Idx,
            MUR_Idx_DOC = paramItem.MUR_Idx_DOC,
            MUR_Idx_STF = paramItem.MUR_Idx_STF,
            RES_Status = string.IsNullOrWhiteSpace(paramItem.RES_Status) ? "CNF" : paramItem.RES_Status,
            RES_Route = paramItem.RES_Route,
            RES_Subject = paramItem.RES_Subject,
            RES_SubjectName = paramItem.RES_SubjectName,
            RES_ReservationDate = SmartMVVM.Common.GetYYMMDDByDateString(paramItem.RES_ReservationDate),
            RES_ReservationTime = paramItem.RES_ReservationTime,
            RES_Memo = paramItem.RES_Memo,
            PATItem = GetPatientDataForSave(PATItem),
            RES_IsValid = true
        };

        return item;
    }

    public Reservation GetReservationDataForSaveByRCB(ReceptionBoard paramItem)
    {
        Reservation item = new Reservation
        {
            RES_Idx = paramItem.RES_Idx,
            PAT_Idx = paramItem.PAT_Idx,
            PAT_Name = paramItem.PAT_Name,
            PAT_Sex = paramItem.PAT_Sex,
            PAT_Age = paramItem.PAT_Age,
            MUR_Idx_DOC = paramItem.MUR_Idx_DOC,
            MUR_Idx_STF = paramItem.MUR_Idx_STF,
            RES_Route = paramItem.RCB_Route,
            RES_Subject = paramItem.RCB_Subject,
            RES_SubjectName = paramItem.RCB_SubjectName,
            RES_ReservationDate = paramItem.RCB_Date,
            RES_ReservationTime = paramItem.RCB_Time,
            RES_Memo = paramItem.RCB_Memo,
            RES_IsValid = true
        };

        return item;
    }

    public void SetReservationData(Reservation oldItem, Reservation newItem)
    {
        oldItem.RES_Idx = newItem.RES_Idx;
        oldItem.PAT_Idx = newItem.PAT_Idx;
        oldItem.MUR_Idx_DOC = newItem.MUR_Idx_DOC;
        oldItem.MUR_Idx_STF = newItem.MUR_Idx_STF;
        oldItem.RES_Status = newItem.RES_Status;
        oldItem.RES_Route = newItem.RES_Route;
        oldItem.RES_Subject = newItem.RES_Subject;
        oldItem.RES_SubjectName = newItem.RES_SubjectName;
        oldItem.vRES_SubjectName = newItem.RES_Subject == "ETC" ? newItem.RES_SubjectName : SmartMVVM.Common.GetCommonCodeName("RES", "Subject", newItem.RES_Subject ?? "");
        oldItem.RES_ReservationDate = newItem.RES_ReservationDate;
        oldItem.RES_ReservationTime = newItem.RES_ReservationTime;
        oldItem.RES_YYMMDD = newItem.RES_YYMMDD;
        oldItem.RES_Memo = newItem.RES_Memo;

        oldItem.PAT_ChartNo = newItem.PAT_ChartNo;
        oldItem.PAT_Name = newItem.PAT_Name;
        oldItem.PAT_Sex = newItem.PAT_Sex;
        oldItem.PAT_Age = newItem.PAT_Age;
        oldItem.vPAT_Info = (newItem.PAT_Sex == "M" ? "남" : "여") + "/" + newItem.PAT_Age + "세";

        oldItem.MUR_Name_DOC = string.IsNullOrWhiteSpace(newItem.MUR_Name_DOC) ? "담당의미정" : newItem.MUR_Name_DOC;
    }

    public void ClearRESData(Reservation item, bool isNewRES = false)
    {
        item.RES_Idx = isNewRES ? 0 : item.RES_Idx;
        item.PAT_Idx = 0;
        item.MUR_Idx_DOC = 0;
        item.MUR_Idx_STF = 0;
        item.RES_Status = "RDY";
        item.RES_Route = "DSK";
        item.RES_Subject = "GNR";
        item.RES_SubjectName = "";
        item.RES_ReservationDate = DateTime.Now.ToString("yyyy-MM-dd");
        item.RES_ReservationTime = SmartMVVM.Common.GetRoundUpTimeByInterval(DateTime.Now, SmartMVVM.AppSession.ReservationTimeInterval);
        item.RES_Memo = "";

        item.PAT_ChartNo = "";
        item.PAT_Name = "";
        item.PAT_Sex = "";
        item.PAT_Age = 0;
        item.vPAT_Info = "";

        item.MUR_Name_DOC = "";
    }

    #endregion

    #region "Reception"

    public Reception GetReceptionDataForSave(Reception RCPItem, Insurance IRCItem)
    {
        Reception item = new Reception
        {
            RCP_Idx = RCPItem.RCP_Idx,
            PAT_Idx = RCPItem.PAT_Idx,
            MUR_Idx_DOC = RCPItem.MUR_Idx_DOC,
            MUR_Idx_STF = RCPItem.MUR_Idx_STF,
            RES_Idx = RCPItem.RES_Idx,
            RCP_VisitType = RCPItem.RCP_VisitType,
            RCP_InsuranceType = RCPItem.RCP_InsuranceType,
            RCP_Status = string.IsNullOrWhiteSpace(RCPItem.RCP_Status) ? "RDY" : RCPItem.RCP_Status,
            RCP_Route = RCPItem.RCP_Route,
            RCP_Subject = RCPItem.RCP_Subject,
            RCP_SubjectName = RCPItem.RCP_SubjectName,
            RCP_ReceiptDate = SmartMVVM.Common.GetYYMMDDByDateString(RCPItem.RCP_ReceiptDate),
            RCP_ReceiptTime = RCPItem.RCP_ReceiptTime,
            RCP_StartTreatTime = RCPItem.RCP_StartTreatTime,
            RCP_EndTreatTime = RCPItem.RCP_EndTreatTime,
            RCP_Memo = RCPItem.RCP_Memo,
            IRCItem = IRCItem,
            RCP_IsValid = true
        };

        return item;
    }

    public Reception GetReceptionDataFromRCB(ReceptionBoard paramItem)
    {
        Reception item = new Reception
        {
            RCP_Idx = paramItem.RCP_Idx,
            PAT_Idx = paramItem.PAT_Idx,
            MUR_Idx_DOC = paramItem.MUR_Idx_DOC,
            MUR_Idx_STF = paramItem.MUR_Idx_STF,
            RES_Idx = paramItem.RES_Idx,
            RCP_VisitType = paramItem.RCB_VisitType,
            RCP_InsuranceType = paramItem.RCP_InsuranceType,
            RCP_Status = string.IsNullOrWhiteSpace(paramItem.RCP_Status) ? "RDY" : paramItem.RCP_Status,
            RCP_Route = paramItem.RCB_Route,
            RCP_Subject = paramItem.RCB_Subject,
            RCP_SubjectName = paramItem.RCB_SubjectName,
            RCP_ReceiptDate = paramItem.RCB_Date,
            RCP_ReceiptTime = paramItem.RCB_Time,
            RCP_Memo = paramItem.RCB_Memo,
        };

        return item;
    }

    public void SetReceptionData(Reception oldItem, Reception newItem)
    {
        oldItem.RCP_Idx = newItem.RCP_Idx.GetValueOrDefault(0);
        oldItem.MUR_Idx_DOC = newItem.MUR_Idx_DOC.GetValueOrDefault(0);
        oldItem.MUR_Idx_STF = newItem.MUR_Idx_STF.GetValueOrDefault(0);
        oldItem.PAT_Idx = newItem.PAT_Idx;
        oldItem.RES_Idx = newItem.RES_Idx;
        oldItem.IRC_Idx = newItem.IRC_Idx;
        oldItem.PAT_Name = newItem.PAT_Name;
        oldItem.RCP_VisitType = newItem.RCP_VisitType;
        oldItem.RCP_Status = newItem.RCP_Status;
        oldItem.RCP_Route = newItem.RCP_Route;
        oldItem.RCP_Subject = newItem.RCP_Subject;
        oldItem.RCP_SubjectName = newItem.RCP_SubjectName;
        oldItem.RCP_InsuranceType = newItem.RCP_InsuranceType;
        oldItem.RCP_ReceiptDate = string.IsNullOrWhiteSpace(newItem.RCP_ReceiptDate) ? DateTime.Now.ToString("yyyy-MM-dd") : newItem.RCP_ReceiptDate;
        oldItem.RCP_ReceiptTime = string.IsNullOrWhiteSpace(newItem.RCP_ReceiptTime) ? DateTime.Now.ToString("HH:mm") : newItem.RCP_ReceiptTime;
        oldItem.RCP_StartTreatTime = newItem.RCP_StartTreatTime;
        oldItem.RCP_EndTreatTime = newItem.RCP_EndTreatTime;
        oldItem.RCP_Memo = newItem.RCP_Memo;
    }

    public void ClearRCPData(Reception item, bool isNewRCP = true)
    {
        item.RCP_Idx = isNewRCP ? 0 : item.RCP_Idx;
        item.MUR_Idx_DOC = 0;
        item.MUR_Idx_STF = 0;
        item.RCP_VisitType = "FIR";
        item.RCP_Status = "";
        item.RCP_Route = "DSK";
        item.RCP_Subject = "GNR";
        item.RCP_SubjectName = "";
        item.RCP_InsuranceType = "NON";
        item.RCP_ReceiptDate = DateTime.Now.ToString("yyyy-MM-dd");
        item.RCP_ReceiptTime = DateTime.Now.ToString("HH:mm");
        item.RCP_StartTreatTime = "";
        item.RCP_EndTreatTime = "";
        item.RCP_Memo = "";
    }

    #endregion

    #region "Insurance"

    public Insurance GetInsuranceDataFromRCP(Reception item)
    {
        return new Insurance
        {
            IRC_Idx = item.IRC_Idx,
            IRC_Type = item.IRC_Idx.GetValueOrDefault(0) == 0 ? "NON" : item.IRC_Type,
            IRC_CertNum = item.IRC_CertNum,
            IRC_InsuredName = item.IRC_InsuredName,
            IRC_CoName = item.IRC_CoName,
            IRC_Specific = item.IRC_Specific,
            IRC_EffectiveYYMMDD = item.IRC_EffectiveYYMMDD,
            IRC_ExpiredYYMMDDD = item.IRC_ExpiredYYMMDDD
        };
    }

    public void SetInsuranceData(Insurance oldItem, Insurance newItem)
    {
        oldItem.IRC_Idx = newItem.IRC_Idx.GetValueOrDefault(0);
        oldItem.IRC_Type = newItem.IRC_Type;
        oldItem.vIRC_Type = SmartMVVM.Common.GetCommonCodeName("RCP", "InsuranceType", newItem.IRC_Type ?? "");
        oldItem.IRC_CertNum = newItem.IRC_CertNum;
        oldItem.IRC_ContractorName = newItem.IRC_ContractorName;
        oldItem.IRC_InsuredName = newItem.IRC_InsuredName;
        oldItem.IRC_CoName = newItem.IRC_CoName;
        oldItem.IRC_Specific = newItem.IRC_Specific;
        oldItem.IRC_EffectiveYYMMDD = newItem.IRC_EffectiveYYMMDD;
        oldItem.IRC_ExpiredYYMMDDD = newItem.IRC_ExpiredYYMMDDD;
    }

    public void ClearIRCData(Insurance item, bool isClearIRCType = false)
    {
        item.IRC_CertNum = "";
        item.IRC_ContractorName = "";
        item.IRC_InsuredName = "";
        item.IRC_CoName = "";
        item.IRC_EffectiveYYMMDD = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
        item.IRC_ExpiredYYMMDDD = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
        item.IRC_Specific = "";
        
        if(isClearIRCType)
        {
            item.IRC_Type = "NON";
            item.vIRC_Type = "비보험";
        }
    }

    #endregion

    #region "Consultation"

    public void SetDefaultConsultationData(Consultation item)
    {
        item.MUR_Idx_DOC = 0;
        item.CST_Status = "RDY";
        item.CST_InsuranceType = "NON";
        item.CST_PayStatus = "RDY";
        item.CST_VisitType = "FIR";
        item.CST_Subject = "GNR";
        item.CST_TreatResult = "CON";
        item.CST_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd");
        item.CST_StartTime = "00:00";
        item.CST_EndTime = "23:59";
    }

    public void ClearCSTData(Consultation item, bool isClearFilter)
    {
        if (isClearFilter)
        {
            item.MUR_Idx_DOC = 0;
            item.CST_Status = "";
            item.CST_Subject = "";
            item.CST_InsuranceType = "";
        }
        else
        {
            item.CST_Idx = 0;
            item.RCP_Idx = 0;
            item.PAT_Idx = 0;
            item.IRC_Idx = 0;
            item.CST_InsuranceType = "";
            item.IRC_Type = "";
            item.vIRC_Type = "";
            item.CST_Status = "";
            item.CST_Subject = "";
            item.CST_SubjectName = "";
            item.vCST_SubjectName = "";
            item.CST_StartTime = "00:00";
            item.CST_EndTime = "23:59";
            item.CST_Opinion = "";
            item.CST_Memo = "";
        }
    }

    #endregion
}
