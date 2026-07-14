namespace SmartEMR.Domain.Entities;

public class Pay : BaseEntity
{
    private int? m_PAY_Idx;
    private int? m_MEM_Idx;
    private int? m_MUR_Idx;
    private int? m_PAT_Idx;
    private int? m_CST_Idx;
    private string? m_PAY_Status;
    private Decimal? m_PAY_TotalPrice;
    private Decimal? m_PAY_InsuredPrice;
    private Decimal? m_PAY_NonInsurecPrice;
    private Decimal? m_PAY_OwnPatientPrice;
    private Decimal? m_PAY_PaidPrice;
    private Decimal? m_PAY_RemainPrice;
    private string? m_PAY_Memo;
    private string? m_PAY_Date;
    private string? m_PAY_YYMMDD;
    private bool? m_PAY_IsValid;

    #region "NotifyPropertyChanged"

    public int? PAY_Idx
    {
        get => m_PAY_Idx;
        set => SetProperty(ref m_PAY_Idx, value);
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

    public string? PAY_Status
    {
        get => m_PAY_Status;
        set => SetProperty(ref m_PAY_Status, value);
    }

    public Decimal? PAY_TotalPrice
    {
        get => m_PAY_TotalPrice;
        set => SetProperty(ref m_PAY_TotalPrice, value);
    }

    public Decimal? PAY_InsuredPrice
    {
        get => m_PAY_InsuredPrice;
        set => SetProperty(ref m_PAY_InsuredPrice, value);
    }

    public Decimal? PAY_NonInsurecPrice
    {
        get => m_PAY_NonInsurecPrice;
        set => SetProperty(ref m_PAY_NonInsurecPrice, value);
    }

    public Decimal? PAY_OwnPatientPrice
    {
        get => m_PAY_OwnPatientPrice;
        set => SetProperty(ref m_PAY_OwnPatientPrice, value);
    }

    public Decimal? PAY_PaidPrice
    {
        get => m_PAY_PaidPrice;
        set => SetProperty(ref m_PAY_PaidPrice, value);
    }

    public Decimal? PAY_RemainPrice
    {
        get => m_PAY_RemainPrice;
        set => SetProperty(ref m_PAY_RemainPrice, value);
    }

    public string? PAY_Memo
    {
        get => m_PAY_Memo;
        set => SetProperty(ref m_PAY_Memo, value);
    }

    public string? PAY_Date
    {
        get => m_PAY_Date;
        set => SetProperty(ref m_PAY_Date, value);
    }

    public string? PAY_YYMMDD
    {
        get => m_PAY_YYMMDD;
        set => SetProperty(ref m_PAY_YYMMDD, value);
    }

    public bool? PAY_IsValid
    {
        get => m_PAY_IsValid;
        set => SetProperty(ref m_PAY_IsValid, value);
    }

    #endregion
}
