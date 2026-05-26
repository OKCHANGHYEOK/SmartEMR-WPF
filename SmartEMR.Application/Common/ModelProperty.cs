using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common;

public class ModelProperty
{
    public void SetPatientData(Patient oldItem, Patient newItem)
    {
        oldItem.PAT_Idx = newItem.PAT_Idx;
        oldItem.MUR_Idx_DOC = newItem.MUR_Idx_DOC;
        oldItem.MUR_Idx_STF = newItem.MUR_Idx_STF;
        oldItem.PAT_ChartNo = newItem.PAT_ChartNo;
        oldItem.PAT_Name = newItem.PAT_Name;
        oldItem.PAT_BloodType = newItem.PAT_BloodType;
        oldItem.PAT_SourceType = newItem.PAT_SourceType;
        oldItem.PAT_Sex = newItem.PAT_Sex;
        oldItem.PAT_Age = newItem.PAT_Age;
        oldItem.vPAT_Info = (newItem.PAT_Sex == "M" ? "남" : "여") + "/" + $"{newItem.PAT_Age}세";
        oldItem.PAT_BirthYear = newItem.PAT_BirthYear;
        oldItem.PAT_BirthMonth = newItem.PAT_BirthMonth;
        oldItem.PAT_BirthDay = newItem.PAT_BirthDay;
        oldItem.PAT_BirthDate = $"{newItem.PAT_BirthYear}-{newItem.PAT_BirthMonth}-{newItem.PAT_BirthDay}";
        oldItem.PAT_RegisterNum1 = newItem.PAT_RegisterNum1;
        oldItem.PAT_RegisterNum2 = newItem.PAT_RegisterNum2;
        oldItem.PAT_Hpp1 = newItem.PAT_Hpp1;
        oldItem.PAT_Hpp2 = newItem.PAT_Hpp2;
        oldItem.PAT_Hpp3 = newItem.PAT_Hpp3;
        oldItem.PAT_PhoneNum = newItem.PAT_PhoneNum;
        oldItem.PAT_Email = newItem.PAT_Email;
        oldItem.PAT_FirstVisitDate = newItem.PAT_FirstVisitDate;
        oldItem.PAT_LastVisitDate = newItem.PAT_LastVisitDate;
        oldItem.PAT_IsSolar = newItem.PAT_IsSolar;
        oldItem.PAT_IsSMS = newItem.PAT_IsSMS;
        oldItem.PAT_ImageSource = newItem.PAT_ImageSource;
        oldItem.PAT_Bigo = newItem.PAT_Bigo;
        oldItem.PAT_Date = newItem.PAT_Date;
        oldItem.PAT_YYMMDD = newItem.PAT_YYMMDD;
        oldItem.NOW_CHT_Idx_RCV = newItem.NOW_CHT_Idx_RCV;
        oldItem.NOW_CHT_Idx_RES = newItem.NOW_CHT_Idx_RES;
        oldItem.NEXT_CHT_Idx_RES = newItem.NEXT_CHT_Idx_RES;
        oldItem.NEXT_CHT_DATE_RES = newItem.NEXT_CHT_DATE_RES;
    }
}
