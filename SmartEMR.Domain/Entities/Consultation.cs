namespace SmartEMR.Domain.Entities;

public class Consultation : BaseEntity
{
    private int? m_CST_Idx;
    private int? m_MEM_Idx;
    private int? m_MUR_Idx;
    private int? m_MUR_Idx_DOC;
    private int? m_PAT_Idx;
    private int? m_RCP_Idx;
    private int? m_IRC_Idx;
    private string? m_CST_VisitType;
    private string? m_CST_InsuranceType;
    private string? m_vCST_InsuranceType;
    private string? m_CST_Status;
    private string? m_vCST_Status;
    private string? m_CST_PayStatus;
    private string? m_vCST_PayStatus;
    private string? m_CST_TreatResult;
    private string? m_CST_Subject;
    private string? m_CST_SubjectName;
    private string? m_vCST_SubjectName;
    private string? m_CST_StartTime;
    private string? m_CST_EndTime;
    private Decimal? m_CST_TotalPrice;
    private Decimal? m_CST_InsuredPrice;
    private Decimal? m_CST_NonInsurecPrice;
    private Decimal? m_CST_OwnPatientPrice;
    private Decimal? m_CST_PaidPrice;
    private Decimal? m_CST_RemainPrice;
    private string? m_CST_Opinion;
    private string? m_CST_Memo;
    private string? m_CST_Date;
    private string? m_CST_YYMMDD;
    private bool? m_CST_IsValid;
    private string? m_CSTO_Property;

    private string? m_MUR_Name_DOC;

    private string? m_PAT_Name;
    private string? m_PAT_ChartNo;
    private string? m_PAT_Sex;
    private string? m_vPAT_Sex;
    private int? m_PAT_Age;
    private string? m_vPAT_Info;

    private string? m_IRC_Type;
    private string? m_vIRC_Type;

    #region "NotifyPropertyChanged"

    public int? CST_Idx
    {
        get => m_CST_Idx;
        set => SetProperty(ref m_CST_Idx, value);
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

    public int? PAT_Idx
    {
        get => m_PAT_Idx;
        set => SetProperty(ref m_PAT_Idx, value);
    }

    public int? RCP_Idx
    {
        get => m_RCP_Idx;
        set => SetProperty(ref m_RCP_Idx, value);
    }

    public int? IRC_Idx
    {
        get => m_IRC_Idx;
        set => SetProperty(ref m_IRC_Idx, value);
    }

    public string? CST_VisitType
    {
        get => m_CST_VisitType;
        set => SetProperty(ref m_CST_VisitType, value);
    }

    public string? CST_InsuranceType
    {
        get => m_CST_InsuranceType;
        set => SetProperty(ref m_CST_InsuranceType, value);
    }

    public string? vCST_InsuranceType
    {
        get => m_vCST_InsuranceType;
        set => SetProperty(ref m_vCST_InsuranceType, value);
    }

    public string? CST_Status
    {
        get => m_CST_Status;
        set => SetProperty(ref m_CST_Status, value);
    }

    public string? vCST_Status
    {
        get => m_vCST_Status;
        set => SetProperty(ref m_vCST_Status, value);
    }

    public string? CST_PayStatus
    {
        get => m_CST_PayStatus;
        set => SetProperty(ref m_CST_PayStatus, value);
    }

    public string? vCST_PayStatus
    {
        get => m_vCST_PayStatus;
        set => SetProperty(ref m_vCST_PayStatus, value);
    }

    public string? CST_TreatResult
    {
        get => m_CST_TreatResult;
        set => SetProperty(ref m_CST_TreatResult, value);
    }

    public string? CST_Subject
    {
        get => m_CST_Subject;
        set => SetProperty(ref m_CST_Subject, value);
    }

    public string? CST_SubjectName
    {
        get => m_CST_SubjectName;
        set => SetProperty(ref m_CST_SubjectName, value);
    }

    public string? vCST_SubjectName
    {
        get => m_vCST_SubjectName;
        set => SetProperty(ref m_vCST_SubjectName, value);
    }

    public string? CST_StartTime
    {
        get => m_CST_StartTime;
        set => SetProperty(ref m_CST_StartTime, value);
    }

    public string? CST_EndTime
    {
        get => m_CST_EndTime;
        set => SetProperty(ref m_CST_EndTime, value);
    }

    public Decimal? CST_TotalPrice
    {
        get => m_CST_TotalPrice;
        set => SetProperty(ref m_CST_TotalPrice, value);
    }

    public Decimal? CST_InsuredPrice
    {
        get => m_CST_InsuredPrice;
        set => SetProperty(ref m_CST_InsuredPrice, value);
    }

    public Decimal? CST_NonInsurecPrice
    {
        get => m_CST_NonInsurecPrice;
        set => SetProperty(ref m_CST_NonInsurecPrice, value);
    }

    public Decimal? CST_OwnPatientPrice
    {
        get => m_CST_OwnPatientPrice;
        set => SetProperty(ref m_CST_OwnPatientPrice, value);
    }

    public Decimal? CST_PaidPrice
    {
        get => m_CST_PaidPrice;
        set => SetProperty(ref m_CST_PaidPrice, value);
    }

    public Decimal? CST_RemainPrice
    {
        get => m_CST_RemainPrice;
        set => SetProperty(ref m_CST_RemainPrice, value);
    }

    public string? CST_Opinion
    {
        get => m_CST_Opinion;
        set => SetProperty(ref m_CST_Opinion, value);
    }

    public string? CST_Memo
    {
        get => m_CST_Memo;
        set => SetProperty(ref m_CST_Memo, value);
    }

    public string? CST_Date
    {
        get => m_CST_Date;
        set => SetProperty(ref m_CST_Date, value);
    }

    public string? CST_YYMMDD
    {
        get => m_CST_YYMMDD;
        set => SetProperty(ref m_CST_YYMMDD, value);
    }

    public bool? CST_IsValid
    {
        get => m_CST_IsValid;
        set => SetProperty(ref m_CST_IsValid, value);
    }

    public string? CSTO_Property
    {
        get => m_CSTO_Property;
        set => SetProperty(ref m_CSTO_Property, value);
    }

    public string? MUR_Name_DOC
    {
        get => m_MUR_Name_DOC;
        set => SetProperty(ref m_MUR_Name_DOC, value);
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

    public string? IRC_Type
    {
        get => m_IRC_Type;
        set => SetProperty(ref m_IRC_Type, value);
    }

    public string? vIRC_Type
    {
        get => m_vIRC_Type;
        set => SetProperty(ref m_vIRC_Type, value);
    }

    #endregion
}
