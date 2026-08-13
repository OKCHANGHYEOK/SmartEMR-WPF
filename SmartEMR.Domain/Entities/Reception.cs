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
    private string? m_vPAT_Sex;
    private int? m_PAT_Age;
    private string? m_vPAT_Info;

    private string? m_RCP_VisitType;
    private string? m_RCP_Status;
    private string? m_vRCP_Status;
    private string? m_RCP_Route;
    private string? m_vRCP_Route;
    private string? m_RCP_Subject;
    private string? m_RCP_SubjectName;
    private string? m_vRCP_SubjectName;
    private string? m_RCP_InsuranceType;
    private string? m_vRCP_InsuranceType;
    private string? m_RCP_ReceiptDate;
    private string? m_RCP_ReceiptTime;
    private string? m_RCP_StartTreatTime;
    private string? m_RCP_EndTreatTime;
    private string? m_RCP_Memo;
    private string? m_RCP_Date;
    private string? m_RCP_YYMMDD;
    private bool? m_RCP_IsValid;

    private int? m_IRC_Idx;
    private string? m_IRC_Type;
    private string? m_vIRC_Type;
    private string? m_IRC_CertNum;
    private string? m_IRC_ContractorName;
    private string? m_IRC_InsuredName;
    private string? m_IRC_CoName;
    private string? m_IRC_Specific;
    private string? m_IRC_EffectiveYYMMDD;
    private string? m_IRC_ExpiredYYMMDDD;

    private Insurance? m_IRCItem;

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

    public string? RCP_VisitType
    {
        get => m_RCP_VisitType;
        set => SetProperty(ref m_RCP_VisitType, value);
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

    public string? RCP_Route
    {
        get => m_RCP_Route;
        set => SetProperty(ref m_RCP_Route, value);
    }

    public string? vRCP_Route
    {
        get => m_vRCP_Route;
        set => SetProperty(ref m_vRCP_Route, value);
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

    public string? vRCP_SubjectName
    {
        get => m_vRCP_SubjectName;
        set => SetProperty(ref m_vRCP_SubjectName, value);
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

    public string? RCP_Date
    {
        get => m_RCP_Date;
        set => SetProperty(ref m_RCP_Date, value);
    }

    public string? RCP_YYMMDD
    {
        get => m_RCP_YYMMDD;
        set => SetProperty(ref m_RCP_YYMMDD, value);
    }

    public bool? RCP_IsValid
    {
        get => m_RCP_IsValid;
        set => SetProperty(ref m_RCP_IsValid, value);
    }

    public int? IRC_Idx
    {
        get => m_IRC_Idx;
        set => SetProperty(ref m_IRC_Idx, value);
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

    public string? IRC_CertNum
    {
        get => m_IRC_CertNum;
        set => SetProperty(ref m_IRC_CertNum, value);
    }

    public string? IRC_ContractorName
    {
        get => m_IRC_ContractorName;
        set => SetProperty(ref m_IRC_ContractorName, value);
    }

    public string? IRC_InsuredName
    {
        get => m_IRC_InsuredName;
        set => SetProperty(ref m_IRC_InsuredName, value);
    }

    public string? IRC_CoName
    {
        get => m_IRC_CoName;
        set => SetProperty(ref m_IRC_CoName, value);
    }

    public string? IRC_Specific
    {
        get => m_IRC_Specific;
        set => SetProperty(ref m_IRC_Specific, value);
    }

    public string? IRC_EffectiveYYMMDD
    {
        get => m_IRC_EffectiveYYMMDD;
        set => SetProperty(ref m_IRC_EffectiveYYMMDD, value);
    }

    public string? IRC_ExpiredYYMMDDD
    {
        get => m_IRC_ExpiredYYMMDDD;
        set => SetProperty(ref m_IRC_ExpiredYYMMDDD, value);
    }

    public Insurance? IRCItem
    {
        get => m_IRCItem;
        set => SetProperty(ref m_IRCItem, value);
    }

    #endregion
}
