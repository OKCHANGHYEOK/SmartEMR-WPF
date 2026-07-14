namespace SmartEMR.Domain.Entities;

public class ConsultationOrder : BaseEntity
{
    private int? m_CSTO_Idx;
    private int? m_MEM_Idx;
    private int? m_MUR_Idx;
    private int? m_PAT_Idx;
    private int? m_CST_Idx;
    private string? m_ORDC_Cd;
    private string? m_ORDC_Name;
    private string? m_ORDG_Cd;
    private string? m_ORDG_Name;
    private string? m_ORDI_Cd;
    private string? m_CSTO_InsuredType;
    private string? m_CSTO_Status;
    private string? m_CSTO_Name;
    private int? m_CSTO_Amount;
    private int? m_CSTO_Count;
    private Decimal? m_CSTO_UnitPrice;
    private Decimal? m_CSTO_TotalPrice;
    private string? m_CSTO_Memo;
    private string? m_CSTO_Date;
    private string? m_CSTO_YYMMDD;
    private bool? m_CSTO_IsValid;

    #region "NotifyPropertyChanged"

    public int? CSTO_Idx
    {
        get => m_CSTO_Idx;
        set => SetProperty(ref m_CSTO_Idx, value);
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

    public int? CST_Idx
    {
        get => m_CST_Idx;
        set => SetProperty(ref m_CST_Idx, value);
    }

    public string? ORDC_Cd
    {
        get => m_ORDC_Cd;
        set => SetProperty(ref m_ORDC_Cd, value);
    }

    public string? ORDC_Name
    {
        get => m_ORDC_Name;
        set => SetProperty(ref m_ORDC_Name, value);
    }

    public string? ORDG_Cd
    {
        get => m_ORDG_Cd;
        set => SetProperty(ref m_ORDG_Cd, value);
    }

    public string? ORDG_Name
    {
        get => m_ORDG_Name;
        set => SetProperty(ref m_ORDG_Name, value);
    }

    public string? ORDI_Cd
    {
        get => m_ORDI_Cd;
        set => SetProperty(ref m_ORDI_Cd, value);
    }

    public string? CSTO_InsuredType
    {
        get => m_CSTO_InsuredType;
        set => SetProperty(ref m_CSTO_InsuredType, value);
    }

    public string? CSTO_Status
    {
        get => m_CSTO_Status;
        set => SetProperty(ref m_CSTO_Status, value);
    }

    public string? CSTO_Name
    {
        get => m_CSTO_Name;
        set => SetProperty(ref m_CSTO_Name, value);
    }

    public int? CSTO_Amount
    {
        get => m_CSTO_Amount;
        set => SetProperty(ref m_CSTO_Amount, value);
    }

    public int? CSTO_Count
    {
        get => m_CSTO_Count;
        set => SetProperty(ref m_CSTO_Count, value);
    }

    public Decimal? CSTO_UnitPrice
    {
        get => m_CSTO_UnitPrice;
        set => SetProperty(ref m_CSTO_UnitPrice, value);
    }

    public Decimal? CSTO_TotalPrice
    {
        get => m_CSTO_TotalPrice;
        set => SetProperty(ref m_CSTO_TotalPrice, value);
    }

    public string? CSTO_Memo
    {
        get => m_CSTO_Memo;
        set => SetProperty(ref m_CSTO_Memo, value);
    }

    public string? CSTO_Date
    {
        get => m_CSTO_Date;
        set => SetProperty(ref m_CSTO_Date, value);
    }

    public string? CSTO_YYMMDD
    {
        get => m_CSTO_YYMMDD;
        set => SetProperty(ref m_CSTO_YYMMDD, value);
    }

    public bool? CSTO_IsValid
    {
        get => m_CSTO_IsValid;
        set => SetProperty(ref m_CSTO_IsValid, value);
    }

    #endregion
}
