namespace SmartEMR.Domain.Entities;

public partial class MemberUser : BaseEntity
{
    #region "Private Fields"
    private int? m_MUR_Idx;
    private int? m_MEM_Idx;

    private string? m_MUR_Role;
    private string? m_MUR_JobCode;

    private string? m_MUR_Id;
    private string? m_MUR_PassWord;
    private string? m_MUR_Name;
    private string? m_MUR_Gender;
    private string? m_MUR_Address1;
    private string? m_MUR_Address2;
    private string? m_MUR_Address3;
    private int? m_MUR_Age;
    private string? m_MUR_BirthYear;
    private string? m_MUR_BirthMonth;
    private string? m_MUR_BirthDay;
    private string? m_MUR_PhoneNum1;
    private string? m_MUR_PhoneNum2;
    private string? m_MUR_PhoneNum3;
    private string? m_MUR_Email;
    private string? m_MUR_Date;
    private string? m_MUR_YYMMDD;
    private bool? m_MUR_IsValid;
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
    #endregion
}