namespace SmartEMR.Domain.Entities;

public class Reception : BaseEntity
{
    private int? m_RCP_Idx;
    private int? m_PAT_Idx;
    private int? m_MEM_Idx;
    private int? m_MUR_Idx;
    private int? m_MUR_Idx_DOC;
    private int? m_MUR_Idx_STF;
    private int? m_RES_Idx;

    private string? m_MUR_Name_DOC;
    private string? m_MUR_Name_STF;

    private string? m_PAT_Name;
    private string? m_PAT_ChartNo;
    private string? m_PAT_Sex;
    private int? m_PAT_Age;

    private string? m_RCP_Status;
    private string? m_RCP_Route;
    private string? m_RCP_Subject;
    private string? m_RCP_SubjectName;
    private string? m_RCP_InsuranceType;
    private string? m_RCP_ReceiptDate;
    private string? m_RCP_ReceiptTime;
    private string? m_RCP_StartTreatTime;
    private string? m_RCP_EndTreatTime;
    private string? m_RCP_Memo;
    private bool? m_RCP_IsValid;

    #region "NotifyPropertyChanged"

    public int? RCP_Idx
    {
        get => m_RCP_Idx;
        set => SetProperty(ref m_RCP_Idx, value);
    }

    public int? PAT_Idx
    {
        get => m_PAT_Idx;
        set => SetProperty(ref m_PAT_Idx, value);
    }

    public int? MEM_Idx
    {
        get => m_MEM_Idx;
        set => SetProperty(ref m_MEM_Idx, value);
    }

    public int? MUR_Idx
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

    public int? RES_Idx
    {
        get => m_RES_Idx;
        set => SetProperty(ref m_RES_Idx, value);
    }

    public string? MUR_Name_DOC
    {
        get => m_MUR_Name_DOC;
        set => SetProperty(ref m_MUR_Name_DOC, value);
    }

    public string? MUR_Name_STF
    {
        get => m_MUR_Name_STF;
        set => SetProperty(ref m_MUR_Name_STF, value);
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

    public int? PAT_Age
    {
        get => m_PAT_Age;
        set => SetProperty(ref m_PAT_Age, value);
    }

    public string? RCP_Status
    {
        get => m_RCP_Status;
        set => SetProperty(ref m_RCP_Status, value);
    }

    public string? RCP_Route
    {
        get => m_RCP_Route;
        set => SetProperty(ref m_RCP_Route, value);
    }

    public string? RCP_Subject
    {
        get => m_RCP_Subject;
        set => SetProperty(ref m_RCP_Subject, value);
    }

    public string? RCP_SubjectName
    {
        get => m_RCP_SubjectName;
        set => SetProperty(ref m_RCP_SubjectName, value);
    }

    public string? RCP_InsuranceType
    {
        get => m_RCP_InsuranceType;
        set => SetProperty(ref m_RCP_InsuranceType, value);
    }

    public string? RCP_ReceiptDate
    {
        get => m_RCP_ReceiptDate;
        set => SetProperty(ref m_RCP_ReceiptDate, value);
    }

    public string? RCP_ReceiptTime
    {
        get => m_RCP_ReceiptTime;
        set => SetProperty(ref m_RCP_ReceiptTime, value);
    }

    public string? RCP_StartTreatTime
    {
        get => m_RCP_StartTreatTime;
        set => SetProperty(ref m_RCP_StartTreatTime, value);
    }

    public string? RCP_EndTreatTime
    {
        get => m_RCP_EndTreatTime;
        set => SetProperty(ref m_RCP_EndTreatTime, value);
    }

    public string? RCP_Memo
    {
        get => m_RCP_Memo;
        set => SetProperty(ref m_RCP_Memo, value);
    }

    public bool? RCP_IsValid
    {
        get => m_RCP_IsValid;
        set => SetProperty(ref m_RCP_IsValid, value);
    }

    #endregion
}
