namespace SmartEMR.Domain.Entities;

public class Member : BaseEntity
{
    private int? m_MEM_Idx;
    private int? m_MEM_AdminUser;
    private string? m_MEM_Name;
    private string? m_MEM_MediNo;
    private string? m_MEM_BizNum;
    private string? m_MEM_BizType;
    private string? m_vMEM_BizType;
    private string? m_MEM_Address1;
    private string? m_MEM_Address2;
    private string? m_MEM_Address3;
    private string? m_MEM_Tel1; 
    private string? m_MEM_Tel2;
    private string? m_MEM_Tel3;
    private string? m_MEM_StartDate;
    private string? m_MEM_EndDate;
    private int? m_MEM_OperationStatus;
    private string? m_MEM_Date;
    private string? m_MEM_YYMMDD;
    private bool? m_MEM_IsValid;

    #region "NotifyPropertyChanged"

    public int? MEM_Idx
    {
        get => m_MEM_Idx;
        set => SetProperty(ref m_MEM_Idx, value);
    }

    public int? MEM_AdminUser
    {
        get => m_MEM_AdminUser;
        set => SetProperty(ref m_MEM_AdminUser, value);
    }

    public string? MEM_Name
    {
        get => m_MEM_Name;
        set => SetProperty(ref m_MEM_Name, value);
    }

    public string? MEM_MediNo
    {
        get => m_MEM_MediNo;
        set => SetProperty(ref m_MEM_MediNo, value);
    }

    public string? MEM_BizNum
    {
        get => m_MEM_BizNum;
        set => SetProperty(ref m_MEM_BizNum, value);
    }

    public string? MEM_BizType
    {
        get => m_MEM_BizType;
        set => SetProperty(ref m_MEM_BizType, value);
    }

    public string? vMEM_BizType
    {
        get => m_vMEM_BizType;
        set => SetProperty(ref m_vMEM_BizType, value);
    }

    public string? MEM_Address1
    {
        get => m_MEM_Address1;
        set => SetProperty(ref m_MEM_Address1, value);
    }

    public string? MEM_Address2
    {
        get => m_MEM_Address2;
        set => SetProperty(ref m_MEM_Address2, value);
    }

    public string? MEM_Address3
    {
        get => m_MEM_Address3;
        set => SetProperty(ref m_MEM_Address3, value);
    }

    public string? MEM_Tel1
    {
        get => m_MEM_Tel1;
        set => SetProperty(ref m_MEM_Tel1, value);
    }

    public string? MEM_Tel2
    {
        get => m_MEM_Tel2;
        set => SetProperty(ref m_MEM_Tel2, value);
    }

    public string? MEM_Tel3
    {
        get => m_MEM_Tel3;
        set => SetProperty(ref m_MEM_Tel3, value);
    }

    public string? MEM_StartDate
    {
        get => m_MEM_StartDate;
        set => SetProperty(ref m_MEM_StartDate, value);
    }

    public string? MEM_EndDate
    {
        get => m_MEM_EndDate;
        set => SetProperty(ref m_MEM_EndDate, value);
    }

    public int? MEM_OperationStatus
    {
        get => m_MEM_OperationStatus;
        set => SetProperty(ref m_MEM_OperationStatus, value);
    }

    public string? MEM_Date
    {
        get => m_MEM_Date;
        set => SetProperty(ref m_MEM_Date, value);
    }

    public string? MEM_YYMMDD
    {
        get => m_MEM_YYMMDD;
        set => SetProperty(ref m_MEM_YYMMDD, value);
    }

    public bool? MEM_IsValid
    {
        get => m_MEM_IsValid;
        set => SetProperty(ref m_MEM_IsValid, value);
    }

    #endregion
}
