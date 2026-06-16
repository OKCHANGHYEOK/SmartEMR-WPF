namespace SmartEMR.Domain.Entities;

public class Insurance : BaseEntity
{
    private int? m_IRC_Idx;
    private int? m_MEM_Idx;
    private int? m_PAT_Idx;
    private int? m_RCP_Idx;
    private string? m_IRC_Type;
    private string? m_vIRC_Type;
    private string? m_IRC_CertNum;
    private string? m_IRC_ContractorName;
    private string? m_IRC_InsuredName;
    private string? m_IRC_Coperation;
    private string? m_IRC_CoName;
    private string? m_IRC_EffectiveYYMMDD;
    private string? m_IRC_ExpiredYYMMDDD;
    private bool? m_IRC_IsValid;

    #region "NotifyPropertyChanged"

    public int? IRC_Idx
    {
        get => m_IRC_Idx;
        set => SetProperty(ref m_IRC_Idx, value);
    }

    public int? MEM_Idx
    {
        get => m_MEM_Idx;
        set => SetProperty(ref m_MEM_Idx, value);
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

    public string? IRC_Coperation
    {
        get => m_IRC_Coperation;
        set => SetProperty(ref m_IRC_Coperation, value);
    }

    public string? IRC_CoName
    {
        get => m_IRC_CoName;
        set => SetProperty(ref m_IRC_CoName, value);
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

    public bool? IRC_IsValid
    {
        get => m_IRC_IsValid;
        set => SetProperty(ref m_IRC_IsValid, value);
    }

    #endregion
}
