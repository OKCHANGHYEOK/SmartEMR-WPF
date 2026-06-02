namespace SmartEMR.Domain.Entities;

public partial class Patient : BaseEntity
{
    #region "Private Fields"
    private int m_PAT_Idx;
    private int m_MEM_Idx;
    private int m_MUR_Idx;
    private int? m_MUR_Idx_DOC;
    private int? m_MUR_Idx_STF;

    private string? m_PAT_BloodType;
    private string? m_PAT_SourceType;
    private string? m_PAT_Name;
    private string? m_PAT_ChartNo;
    private string? m_PAT_Sex;
    private string? m_vPAT_Sex;
    private int? m_PAT_Age;
    private string? m_vPAT_Info;
    private string? m_PAT_BirthYear;
    private string? m_PAT_BirthMonth;
    private string? m_PAT_BirthDay;
    private string? m_PAT_BirthDate;
    private string? m_PAT_RegisterNum1;
    private string? m_PAT_RegisterNum2;
    private string? m_PAT_Hpp1;
    private string? m_PAT_Hpp2;
    private string? m_PAT_Hpp3;
    private string? m_PAT_PhoneNum;
    private string? m_PAT_Address1;
    private string? m_PAT_Address2;
    private string? m_PAT_Address3;
    private string? m_PAT_Email;
    private string? m_PAT_FirstVisitDate;
    private string? m_PAT_LastVisitDate;
    private string? m_PAT_IsSolar;
    private string? m_PAT_IsAgreePersonalInfo;
    private string? m_vPAT_IsAgreePersonalInfo;
    private string? m_PAT_IsForeign;
    private string? m_PAT_IsSMS;
    private string? m_PAT_IsEmail;
    private byte[]? m_PAT_ImageSource;
    private string? m_PAT_Bigo;
    private string? m_PAT_Status;
    private bool? m_PAT_IsValid;

    // ModelProperty 연동 추가 필드
    private string? m_PAT_Date;
    private string? m_PAT_YYMMDD;
    private int? m_NOW_CHT_Idx_RCV;
    private int? m_NOW_CHT_Idx_RES;
    private int? m_NEXT_CHT_Idx_RES;
    private string? m_NEXT_CHT_DATE_RES;
    #endregion

    #region "NotifyPropertyChanged"
    public int PAT_Idx
    {
        get => m_PAT_Idx;
        set => SetProperty(ref m_PAT_Idx, value);
    }

    public int MEM_Idx
    {
        get => m_MEM_Idx;
        set => SetProperty(ref m_MEM_Idx, value);
    }

    public int MUR_Idx
    {
        get => m_MUR_Idx;
        set => SetProperty(ref m_MUR_Idx, value);
    }

    public int? MUR_Idx_DOC
    {
        get => m_MUR_Idx_DOC;
        set => SetProperty(ref m_MUR_Idx_DOC, value);
    }

    public int? MUR_Idx_STF
    {
        get => m_MUR_Idx_STF;
        set => SetProperty(ref m_MUR_Idx_STF, value);
    }

    public string? PAT_BloodType
    {
        get => m_PAT_BloodType;
        set => SetProperty(ref m_PAT_BloodType, value);
    }

    public string? PAT_SourceType
    {
        get => m_PAT_SourceType;
        set => SetProperty(ref m_PAT_SourceType, value);
    }

    public string? PAT_Name
    {
        get => m_PAT_Name;
        set => SetProperty(ref m_PAT_Name, value);
    }

    public string? PAT_ChartNo
    {
        get => m_PAT_ChartNo;
        set => SetProperty(ref m_PAT_ChartNo, value);
    }

    public string? PAT_Sex
    {
        get => m_PAT_Sex;
        set => SetProperty(ref m_PAT_Sex, value);
    }

    public string? vPAT_Sex
    {
        get => m_vPAT_Sex;
        set => SetProperty(ref m_vPAT_Sex, value);
    }

    public int? PAT_Age
    {
        get => m_PAT_Age;
        set => SetProperty(ref m_PAT_Age, value);
    }

    public string? vPAT_Info
    {
        get => m_vPAT_Info;
        set => SetProperty(ref m_vPAT_Info, value);
    }

    public string? PAT_BirthYear
    {
        get => m_PAT_BirthYear;
        set => SetProperty(ref m_PAT_BirthYear, value);
    }

    public string? PAT_BirthMonth
    {
        get => m_PAT_BirthMonth;
        set => SetProperty(ref m_PAT_BirthMonth, value);
    }

    public string? PAT_BirthDay
    {
        get => m_PAT_BirthDay;
        set => SetProperty(ref m_PAT_BirthDay, value);
    }

    public string? PAT_BirthDate
    {
        get => m_PAT_BirthDate;
        set => SetProperty(ref m_PAT_BirthDate, value);
    }

    public string? PAT_RegisterNum1
    {
        get => m_PAT_RegisterNum1;
        set => SetProperty(ref m_PAT_RegisterNum1, value);
    }

    public string? PAT_RegisterNum2
    {
        get => m_PAT_RegisterNum2;
        set => SetProperty(ref m_PAT_RegisterNum2, value);
    }

    public string? PAT_Hpp1
    {
        get => m_PAT_Hpp1;
        set => SetProperty(ref m_PAT_Hpp1, value);
    }

    public string? PAT_Hpp2
    {
        get => m_PAT_Hpp2;
        set => SetProperty(ref m_PAT_Hpp2, value);
    }

    public string? PAT_Hpp3
    {
        get => m_PAT_Hpp3;
        set => SetProperty(ref m_PAT_Hpp3, value);
    }

    public string? PAT_PhoneNum
    {
        get => m_PAT_PhoneNum;
        set => SetProperty(ref m_PAT_PhoneNum, value);
    }

    public string? PAT_Address1
    {
        get => m_PAT_Address1;
        set => SetProperty(ref m_PAT_Address1, value);
    }

    public string? PAT_Address2
    {
        get => m_PAT_Address2;
        set => SetProperty(ref m_PAT_Address2, value);
    }

    public string? PAT_Address3
    {
        get => m_PAT_Address3;
        set => SetProperty(ref m_PAT_Address3, value);
    }

    public string? PAT_Email
    {
        get => m_PAT_Email;
        set => SetProperty(ref m_PAT_Email, value);
    }

    public string? PAT_FirstVisitDate
    {
        get => m_PAT_FirstVisitDate;
        set => SetProperty(ref m_PAT_FirstVisitDate, value);
    }

    public string? PAT_LastVisitDate
    {
        get => m_PAT_LastVisitDate;
        set => SetProperty(ref m_PAT_LastVisitDate, value);
    }

    public string? PAT_IsSolar
    {
        get => m_PAT_IsSolar;
        set => SetProperty(ref m_PAT_IsSolar, value);
    }

    public string? PAT_IsAgreePersonalInfo
    {
        get => m_PAT_IsAgreePersonalInfo;
        set => SetProperty(ref m_PAT_IsAgreePersonalInfo, value);
    }

    public string? vPAT_IsAgreePersonalInfo
    {
        get => m_vPAT_IsAgreePersonalInfo;
        set => SetProperty(ref m_vPAT_IsAgreePersonalInfo, value);
    }

    public string? PAT_IsForeign
    {
        get => m_PAT_IsForeign;
        set => SetProperty(ref m_PAT_IsForeign, value);
    }

    public string? PAT_IsSMS
    {
        get => m_PAT_IsSMS;
        set => SetProperty(ref m_PAT_IsSMS, value);
    }

    public string? PAT_IsEmail
    {
        get => m_PAT_IsEmail;
        set => SetProperty(ref m_PAT_IsEmail, value);
    }

    public byte[]? PAT_ImageSource
    {
        get => m_PAT_ImageSource;
        set => SetProperty(ref m_PAT_ImageSource, value);
    }

    public string? PAT_Bigo
    {
        get => m_PAT_Bigo;
        set => SetProperty(ref m_PAT_Bigo, value);
    }

    public string? PAT_Status
    {
        get => m_PAT_Status;
        set => SetProperty(ref m_PAT_Status, value);
    }

    public bool? PAT_IsValid
    {
        get => m_PAT_IsValid;
        set => SetProperty(ref m_PAT_IsValid, value);
    }

    public string? PAT_Date
    {
        get => m_PAT_Date;
        set => SetProperty(ref m_PAT_Date, value);
    }

    public string? PAT_YYMMDD
    {
        get => m_PAT_YYMMDD;
        set => SetProperty(ref m_PAT_YYMMDD, value);
    }

    public int? NOW_CHT_Idx_RCV
    {
        get => m_NOW_CHT_Idx_RCV;
        set => SetProperty(ref m_NOW_CHT_Idx_RCV, value);
    }

    public int? NOW_CHT_Idx_RES
    {
        get => m_NOW_CHT_Idx_RES;
        set => SetProperty(ref m_NOW_CHT_Idx_RES, value);
    }

    public int? NEXT_CHT_Idx_RES
    {
        get => m_NEXT_CHT_Idx_RES;
        set => SetProperty(ref m_NEXT_CHT_Idx_RES, value);
    }

    public string? NEXT_CHT_DATE_RES
    {
        get => m_NEXT_CHT_DATE_RES;
        set => SetProperty(ref m_NEXT_CHT_DATE_RES, value);
    }
    #endregion
}