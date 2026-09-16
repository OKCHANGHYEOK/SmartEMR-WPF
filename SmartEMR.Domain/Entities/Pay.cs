namespace SmartEMR.Domain.Entities;

public class Pay : BaseEntity
{
    private int? m_PAY_Idx;
    private int? m_MEM_Idx;
    private int? m_MUR_Idx;
    private int? m_PAT_Idx;
    private int? m_CST_Idx;
    private string? m_CST_Status;
    private string? m_vCST_Status;
    private string? m_PAY_Status;
    private string? m_vPAY_Status;
    private Decimal? m_PAY_TotalPrice;
    private Decimal? m_PAY_InsuredPrice;
    private Decimal? m_PAY_NonInsuredPrice;
    private Decimal? m_PAY_OwnPatientPrice;
    private Decimal? m_PAY_PaidPrice;
    private Decimal? m_PAY_RemainPrice;
    private string? m_PAY_Memo;
    private string? m_PAY_Date;
    private string? m_PAY_YYMMDD;
    private bool? m_PAY_IsValid;

    private string? m_PAT_Name;
    private string? m_PAT_ChartNo;
    private string? m_PAT_Sex;
    private string? m_vPAT_Sex;
    private int? m_PAT_Age;
    private string? m_vPAT_Info;

    private string? m_sDay;
    private string? m_eDay;

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

    public string? PAY_Status
    {
        get => m_PAY_Status;
        set => SetProperty(ref m_PAY_Status, value);
    }

    public string? vPAY_Status
    {
        get => m_vPAY_Status;
        set => SetProperty(ref m_vPAY_Status, value);
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

    public Decimal? PAY_NonInsuredPrice
    {
        get => m_PAY_NonInsuredPrice;
        set => SetProperty(ref m_PAY_NonInsuredPrice, value);
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

    public string? sDay
    {
        get => m_sDay;
        set => SetProperty(ref m_sDay, value);
    }

    public string? eDay
    {
        get => m_eDay;
        set => SetProperty(ref m_eDay, value);
    }

    #endregion
}
