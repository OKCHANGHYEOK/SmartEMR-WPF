using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Common;

public class ModelProperty
{
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
        oldItem.vPAT_SourceType = SmartMVVM.Common.GetChartCommonCode("PAT", "SourceType")?.FirstOrDefault(x => x.CCC_Cd == newItem.PAT_SourceType)?.CCC_Name;
        oldItem.PAT_Sex = newItem.PAT_Sex;
        oldItem.PAT_Age = newItem.PAT_Age;
        oldItem.vPAT_Info = (newItem.PAT_Sex == "M" ? "남" : "여") + "/" + $"{newItem.PAT_Age}세";
        oldItem.PAT_BirthYear = newItem.PAT_BirthYear;
        oldItem.PAT_BirthMonth = newItem.PAT_BirthMonth;
        oldItem.PAT_BirthDay = newItem.PAT_BirthDay;
        oldItem.PAT_BirthDate = $"{newItem.PAT_BirthYear}-{newItem.PAT_BirthMonth}-{newItem.PAT_BirthDay}";
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
