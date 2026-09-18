namespace SmartEMR.Domain.Entities;

public class PayItem : BaseEntity
{
    private int? m_PAYI_Idx;
    private int? m_MEM_Idx;
    private int? m_MUR_Idx;
    private int? m_PAT_Idx;
    private int? m_PAY_Idx;
    private string? m_PAYI_Type;
    private string? m_vPAYI_Type;
    private string? m_PAYI_Method;
    private string? m_vPAYI_Method;
    private decimal? m_PAYI_Price;
    private string? m_PAYI_Time;
    private string? m_PAYI_YYMMDD;
    private bool? m_PAYI_IsValid;

    private string? m_MUR_Name;


    #region "NotifyPropertyChanged"

    public int? PAYI_Idx
    {
        get => m_PAYI_Idx;
        set => SetProperty(ref m_PAYI_Idx, value);
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

    public int? PAT_Idx
    {
        get => m_PAT_Idx;
        set => SetProperty(ref m_PAT_Idx, value);
    }

    public int? PAY_Idx
    {
        get => m_PAY_Idx;
        set => SetProperty(ref m_PAY_Idx, value);
    }

    public string? PAYI_Type
    {
        get => m_PAYI_Type;
        set => SetProperty(ref m_PAYI_Type, value);
    }

    public string? vPAYI_Type
    {
        get => m_vPAYI_Type;
        set => SetProperty(ref m_vPAYI_Type, value);
    }

    public string? PAYI_Method
    {
        get => m_PAYI_Method;
        set => SetProperty(ref m_PAYI_Method, value);
    }

    public string? vPAYI_Method
    {
        get => m_vPAYI_Method;
        set => SetProperty(ref m_vPAYI_Method, value);
    }

    public decimal? PAYI_Price
    {
        get => m_PAYI_Price;
        set => SetProperty(ref m_PAYI_Price, value);
    }

    public string? PAYI_Time
    {
        get => m_PAYI_Time;
        set => SetProperty(ref m_PAYI_Time, value);
    }

    public string? PAYI_YYMMDD
    {
        get => m_PAYI_YYMMDD;
        set => SetProperty(ref m_PAYI_YYMMDD, value);
    }

    public bool? PAYI_IsValid
    {
        get => m_PAYI_IsValid;
        set => SetProperty(ref m_PAYI_IsValid, value);
    }

    public string? MUR_Name
    {
        get => m_MUR_Name;
        set => SetProperty(ref m_MUR_Name, value);
    }

    #endregion
}
