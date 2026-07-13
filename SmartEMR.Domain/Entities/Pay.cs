namespace SmartEMR.Domain.Entities;

public class Pay : BaseEntity
{
    private int? m_PAY_Idx;
    private int? m_MEM_Idx;
    private int? m_MUR_Idx;
    private int? m_PAT_Idx;
    private int? m_CST_Idx;
    private string? m_PAY_Status;
    private Decimal? m_PAY_AMOUNT_TOT;
    private Decimal? m_PAY_AMOUNT_INSURED;
    private Decimal? m_PAY_AMOUNT_NONINSURED;
    private Decimal? m_PAY_AMOUNT_PATIENT;
    private Decimal? m_PAY_AMOUNT_PAID;
    private Decimal? m_PAY_AMOUNT_REMAIN;
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

    public Decimal? PAY_AMOUNT_TOT
    {
        get => m_PAY_AMOUNT_TOT;
        set => SetProperty(ref m_PAY_AMOUNT_TOT, value);
    }

    public Decimal? PAY_AMOUNT_INSURED
    {
        get => m_PAY_AMOUNT_INSURED;
        set => SetProperty(ref m_PAY_AMOUNT_INSURED, value);
    }

    public Decimal? PAY_AMOUNT_NONINSURED
    {
        get => m_PAY_AMOUNT_NONINSURED;
        set => SetProperty(ref m_PAY_AMOUNT_NONINSURED, value);
    }

    public Decimal? PAY_AMOUNT_PATIENT
    {
        get => m_PAY_AMOUNT_PATIENT;
        set => SetProperty(ref m_PAY_AMOUNT_PATIENT, value);
    }

    public Decimal? PAY_AMOUNT_PAID
    {
        get => m_PAY_AMOUNT_PAID;
        set => SetProperty(ref m_PAY_AMOUNT_PAID, value);
    }

    public Decimal? PAY_AMOUNT_REMAIN
    {
        get => m_PAY_AMOUNT_REMAIN;
        set => SetProperty(ref m_PAY_AMOUNT_REMAIN, value);
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
