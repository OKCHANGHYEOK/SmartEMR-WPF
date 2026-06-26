namespace SmartEMR.Domain.Entities;

public class ReceptionBoard : BaseEntity
{
    private int? m_RCB_Idx;
    private int? m_MEM_Idx;
    private int? m_MUR_Idx_DOC;
    private int? m_MUR_Idx_STF;
    private int? m_PAT_Idx;

    private string? m_RCP_Status;
    private string? m_vRCP_Status;
    private string? m_RCP_InsuranceType;
    private string? m_vRCP_InsuranceType;

    private string? m_RES_Status;
    private string? m_vRES_Status;

    private string? m_RCB_Type;
    private string? m_vRCB_Type;
    private string? m_vRCB_Status;
    private string? m_RCB_VisitType;
    private string? m_vRCB_VisitType;
    private string? m_RCB_Route;
    private string? m_vRCB_Route;
    private string? m_RCB_Subject;
    private string? m_vRCB_Subject;
    private string? m_RCB_SubjectName;
    private string? m_RCB_YYMMDD;
    private string? m_RCB_Date;
    private string? m_RCB_Time;
    private string? m_RCB_Memo;
    
    private string? m_MUR_Name_DOC;
    
    private string? m_PAT_ChartNo;
    private string? m_PAT_Name;
    private string? m_PAT_Sex;
    private string? m_vPAT_Sex;
    private int? m_PAT_Age;
    private string? m_vPAT_Info;

    #region "NotifyPropertChanged"

    public int? RCB_Idx
    {
        get => m_RCB_Idx;
        set => SetProperty(ref m_RCB_Idx, value);
    }

    public int? MEM_Idx
    {
        get => m_MEM_Idx;
        set => SetProperty(ref m_MEM_Idx, value);
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

    public int? PAT_Idx
    {
        get => m_PAT_Idx;
        set => SetProperty(ref m_PAT_Idx, value);
    }

    public string? RCP_Status
    {
        get => m_RCP_Status;
        set => SetProperty(ref m_RCP_Status, value);
    }

    public string? vRCP_Status
    {
        get => m_vRCP_Status;
        set => SetProperty(ref m_vRCP_Status, value);
    }

    public string? RCP_InsuranceType
    {
        get => m_RCP_InsuranceType;
        set => SetProperty(ref m_RCP_InsuranceType, value);
    }

    public string? vRCP_InsuranceType
    {
        get => m_vRCP_InsuranceType;
        set => SetProperty(ref m_vRCP_InsuranceType, value);
    }

    public string? RES_Status
    {
        get => m_RES_Status;
        set => SetProperty(ref m_RES_Status, value);
    }

    public string? vRES_Status
    {
        get => m_vRES_Status;
        set => SetProperty(ref m_vRES_Status, value);
    }

    public string? RCB_Type
    {
        get => m_RCB_Type;
        set => SetProperty(ref m_RCB_Type, value);
    }

    public string? vRCB_Type
    {
        get => m_vRCB_Type;
        set => SetProperty(ref m_vRCB_Type, value);
    }

    public string? vRCB_Status
    {
        get => m_vRCB_Status;
        set => SetProperty(ref m_vRCB_Status, value);
    }

    public string? RCB_VisitType
    {
        get => m_RCB_VisitType;
        set => SetProperty(ref m_RCB_VisitType, value);
    }

    public string? vRCB_VisitType
    {
        get => m_vRCB_VisitType;
        set => SetProperty(ref m_vRCB_VisitType, value);
    }

    public string? RCB_Route
    {
        get => m_RCB_Route;
        set => SetProperty(ref m_RCB_Route, value);
    }

    public string? vRCB_Route
    {
        get => m_vRCB_Route;
        set => SetProperty(ref m_vRCB_Route, value);
    }

    public string? RCB_Subject
    {
        get => m_RCB_Subject;
        set => SetProperty(ref m_RCB_Subject, value);
    }

    public string? vRCB_Subject
    {
        get => m_vRCB_Subject;
        set => SetProperty(ref m_vRCB_Subject, value);
    }

    public string? RCB_SubjectName
    {
        get => m_RCB_SubjectName;
        set => SetProperty(ref m_RCB_SubjectName, value);
    }

    public string? RCB_YYMMDD
    {
        get => m_RCB_YYMMDD;
        set => SetProperty(ref m_RCB_YYMMDD, value);
    }

    public string? RCB_Date
    {
        get => m_RCB_Date;
        set => SetProperty(ref m_RCB_Date, value);
    }

    public string? RCB_Time
    {
        get => m_RCB_Time;
        set => SetProperty(ref m_RCB_Time, value);
    }

    public string? RCB_Memo
    {
        get => m_RCB_Memo;
        set => SetProperty(ref m_RCB_Memo, value);
    }

    public string? MUR_Name_DOC
    {
        get => m_MUR_Name_DOC;
        set => SetProperty(ref m_MUR_Name_DOC, value);
    }

    public string? PAT_ChartNo
    {
        get => m_PAT_ChartNo;
        set => SetProperty(ref m_PAT_ChartNo, value);
    }

    public string? PAT_Name
    {
        get => m_PAT_Name;
        set => SetProperty(ref m_PAT_Name, value);
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

    #endregion
}
