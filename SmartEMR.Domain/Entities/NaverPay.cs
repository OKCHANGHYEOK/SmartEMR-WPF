namespace SmartEMR.Domain.Entities;

public partial class NaverPay : BaseEntity
{
    private int? m_NPY_Idx;
    private int? m_PAYI_Idx;
    private string? m_paymentId;
    private string? m_payHistId;
    private string? m_merchantName;
    private string? m_merchantPayKey;
    private string? m_merchantUserKey;
    private string? m_admissionTypeCode;
    private string? m_admissionYmdt;
    private string? m_tradeConfirmYmdt;
    private string? m_admissionState;
    private int? m_totalPayAmount;
    private int? m_applyPayAmount;
    private int? m_primaryPayAmount;
    private int? m_npointPayAmount;
    private int? m_giftCardAmount;
    private int? m_discountPayAmount;
    private int? m_taxScopeAmount;
    private int? m_taxExScopeAmount;
    private int? m_environmentDepositAmount;
    private string? m_primaryPayMeans;
    private string? m_cardCorpCode;
    private int? m_cardInstCount;
    private bool? m_usedCardPoint;
    private string? m_bankCorpCode;
    private string? m_productName;

    #region "NotifyPropertyChanged"

    public int? NPY_Idx
    {
        get => m_NPY_Idx;
        set => SetProperty(ref m_NPY_Idx, value);
    }

    public int? PAYI_Idx
    {
        get => m_PAYI_Idx;
        set => SetProperty(ref m_PAYI_Idx, value);
    }

    public string? paymentId
    {
        get => m_paymentId;
        set => SetProperty(ref m_paymentId, value);
    }

    public string? payHistId
    {
        get => m_payHistId;
        set => SetProperty(ref m_payHistId, value);
    }

    public string? merchantName
    {
        get => m_merchantName;
        set => SetProperty(ref m_merchantName, value);
    }

    public string? merchantPayKey
    {
        get => m_merchantPayKey;
        set => SetProperty(ref m_merchantPayKey, value);
    }

    public string? merchantUserKey
    {
        get => m_merchantUserKey;
        set => SetProperty(ref m_merchantUserKey, value);
    }

    public string? admissionTypeCode
    {
        get => m_admissionTypeCode;
        set => SetProperty(ref m_admissionTypeCode, value);
    }

    public string? admissionYmdt
    {
        get => m_admissionYmdt;
        set => SetProperty(ref m_admissionYmdt, value);
    }

    public string? tradeConfirmYmdt
    {
        get => m_tradeConfirmYmdt;
        set => SetProperty(ref m_tradeConfirmYmdt, value);
    }

    public string? admissionState
    {
        get => m_admissionState;
        set => SetProperty(ref m_admissionState, value);
    }

    public int? totalPayAmount
    {
        get => m_totalPayAmount;
        set => SetProperty(ref m_totalPayAmount, value);
    }

    public int? applyPayAmount
    {
        get => m_applyPayAmount;
        set => SetProperty(ref m_applyPayAmount, value);
    }

    public int? primaryPayAmount
    {
        get => m_primaryPayAmount;
        set => SetProperty(ref m_primaryPayAmount, value);
    }

    public int? npointPayAmount
    {
        get => m_npointPayAmount;
        set => SetProperty(ref m_npointPayAmount, value);
    }

    public int? giftCardAmount
    {
        get => m_giftCardAmount;
        set => SetProperty(ref m_giftCardAmount, value);
    }

    public int? discountPayAmount
    {
        get => m_discountPayAmount;
        set => SetProperty(ref m_discountPayAmount, value);
    }

    public int? taxScopeAmount
    {
        get => m_taxScopeAmount;
        set => SetProperty(ref m_taxScopeAmount, value);
    }

    public int? taxExScopeAmount
    {
        get => m_taxExScopeAmount;
        set => SetProperty(ref m_taxExScopeAmount, value);
    }

    public int? environmentDepositAmount
    {
        get => m_environmentDepositAmount;
        set => SetProperty(ref m_environmentDepositAmount, value);
    }

    public string? primaryPayMeans
    {
        get => m_primaryPayMeans;
        set => SetProperty(ref m_primaryPayMeans, value);
    }

    public string? cardCorpCode
    {
        get => m_cardCorpCode;
        set => SetProperty(ref m_cardCorpCode, value);
    }

    public int? cardInstCount
    {
        get => m_cardInstCount;
        set => SetProperty(ref m_cardInstCount, value);
    }

    public bool? usedCardPoint
    {
        get => m_usedCardPoint;
        set => SetProperty(ref m_usedCardPoint, value);
    }

    public string? bankCorpCode
    {
        get => m_bankCorpCode;
        set => SetProperty(ref m_bankCorpCode, value);
    }

    public string? productName
    {
        get => m_productName;
        set => SetProperty(ref m_productName, value);
    }

    #endregion
}