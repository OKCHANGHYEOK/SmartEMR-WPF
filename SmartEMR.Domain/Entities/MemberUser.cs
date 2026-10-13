namespace SmartEMR.Domain.Entities;

public partial class MemberUser : BaseEntity
{
    #region "Private Fields"
    private int? m_MUR_Idx;
    private int? m_MEM_Idx;

    private string? m_MUR_Department;
    private string? m_vMUR_Department;
    private string? m_MUR_Role;
    private string? m_MUR_JobCode;
    private string? m_vMUR_JobCode;
    private string? m_MUR_CalendarType;
    private string? m_vMUR_CalendarType;

    private string? m_MUR_Id;
    private string? m_MUR_PassWord;
    private string? m_MUR_PassWordCheck;
    private string? m_MUR_PasswordCurrent;
    private string? m_MUR_LicenseNo;
    private string? m_MUR_Name;
    private string? m_MUR_Gender;
    private string? m_MUR_Address1;
    private string? m_MUR_Address2;
    private string? m_MUR_Address3;
    private int? m_MUR_Age;
    private string? m_MUR_BirthDate;
    private string? m_MUR_BirthYear;
    private string? m_MUR_BirthMonth;
    private string? m_MUR_BirthDay;
    private string? m_MUR_PhoneNum;
    private string? m_MUR_PhoneNum1;
    private string? m_MUR_PhoneNum2;
    private string? m_MUR_PhoneNum3;
    private string? m_MUR_Email;
    private string? m_MUR_EmailDomain;
    private string? m_MUR_Date;
    private string? m_MUR_YYMMDD;
    private bool? m_MUR_IsValid;

    private int? m_EV_Idx_Verified;

    #endregion

    #region "NotifyPropertyChanged"

    public int? MUR_Idx
    {
        get => m_MUR_Idx;
        set => SetProperty(ref m_MUR_Idx, value);
    }

    public int? MEM_Idx
    {
        get => m_MEM_Idx;
        set => SetProperty(ref m_MEM_Idx, value);
    }

    public string? MUR_Department
    {
        get => m_MUR_Department;
        set => SetProperty(ref m_MUR_Department, value);
    }

    public string? vMUR_Department
    {
        get => m_vMUR_Department;
        set => SetProperty(ref m_vMUR_Department, value);
    }

    public string? MUR_Role
    {
        get => m_MUR_Role;
        set => SetProperty(ref m_MUR_Role, value);
    }

    public string? MUR_JobCode
    {
        get => m_MUR_JobCode;
        set => SetProperty(ref m_MUR_JobCode, value);
    }

    public string? vMUR_JobCode
    {
        get => m_vMUR_JobCode;
        set => SetProperty(ref m_vMUR_JobCode, value);
    }

    public string? MUR_CalendarType
    {
        get => m_MUR_CalendarType;
        set => SetProperty(ref m_MUR_CalendarType, value);
    }

    public string? vMUR_CalendarType
    {
        get => m_vMUR_CalendarType;
        set => SetProperty(ref m_vMUR_CalendarType, value);
    }

    public string? MUR_Id
    {
        get => m_MUR_Id;
        set => SetProperty(ref m_MUR_Id, value);
    }

    public string? MUR_PassWord
    {
        get => m_MUR_PassWord;
        set => SetProperty(ref m_MUR_PassWord, value);
    }

    public string? MUR_PassWordCheck
    {
        get => m_MUR_PassWordCheck;
        set => SetProperty(ref m_MUR_PassWordCheck, value);
    }

    public string? MUR_PasswordCurrent
    {
        get => m_MUR_PasswordCurrent;
        set => SetProperty(ref m_MUR_PasswordCurrent, value);
    }

    public string? MUR_LicenseNo
    {
        get => m_MUR_LicenseNo;
        set => SetProperty(ref m_MUR_LicenseNo, value);
    }

    public string? MUR_Name
    {
        get => m_MUR_Name;
        set => SetProperty(ref m_MUR_Name, value);
    }

    public string? MUR_Gender
    {
        get => m_MUR_Gender;
        set => SetProperty(ref m_MUR_Gender, value);
    }

    public string? MUR_Address1
    {
        get => m_MUR_Address1;
        set => SetProperty(ref m_MUR_Address1, value);
    }

    public string? MUR_Address2
    {
        get => m_MUR_Address2;
        set => SetProperty(ref m_MUR_Address2, value);
    }

    public string? MUR_Address3
    {
        get => m_MUR_Address3;
        set => SetProperty(ref m_MUR_Address3, value);
    }

    public int? MUR_Age
    {
        get => m_MUR_Age;
        set => SetProperty(ref m_MUR_Age, value);
    }

    public string? MUR_BirthDate
    {
        get => m_MUR_BirthDate;
        set => SetProperty(ref m_MUR_BirthDate, value);
    }

    public string? MUR_BirthYear
    {
        get => m_MUR_BirthYear;
        set => SetProperty(ref m_MUR_BirthYear, value);
    }

    public string? MUR_BirthMonth
    {
        get => m_MUR_BirthMonth;
        set => SetProperty(ref m_MUR_BirthMonth, value);
    }

    public string? MUR_BirthDay
    {
        get => m_MUR_BirthDay;
        set => SetProperty(ref m_MUR_BirthDay, value);
    }

    public string? MUR_PhoneNum
    {
        get => m_MUR_PhoneNum;
        set => SetProperty(ref m_MUR_PhoneNum, value);
    }

    public string? MUR_PhoneNum1
    {
        get => m_MUR_PhoneNum1;
        set => SetProperty(ref m_MUR_PhoneNum1, value);
    }

    public string? MUR_PhoneNum2
    {
        get => m_MUR_PhoneNum2;
        set => SetProperty(ref m_MUR_PhoneNum2, value);
    }

    public string? MUR_PhoneNum3
    {
        get => m_MUR_PhoneNum3;
        set => SetProperty(ref m_MUR_PhoneNum3, value);
    }

    public string? MUR_Email
    {
        get => m_MUR_Email;
        set => SetProperty(ref m_MUR_Email, value);
    }

    public string? MUR_EmailDomain
    {
        get => m_MUR_EmailDomain;
        set => SetProperty(ref m_MUR_EmailDomain, value);
    }

    public string? MUR_Date
    {
        get => m_MUR_Date;
        set => SetProperty(ref m_MUR_Date, value);
    }

    public string? MUR_YYMMDD
    {
        get => m_MUR_YYMMDD;
        set => SetProperty(ref m_MUR_YYMMDD, value);
    }

    public bool? MUR_IsValid
    {
        get => m_MUR_IsValid;
        set => SetProperty(ref m_MUR_IsValid, value);
    }

    public int? EV_Idx_Verified
    {
        get => m_EV_Idx_Verified;
        set => SetProperty(ref m_EV_Idx_Verified, value);
    }

    #endregion
}