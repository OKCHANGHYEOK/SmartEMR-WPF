namespace SmartEMR.Domain.Entities;

public class Order : BaseEntity
{
    private int? m_ORD_Idx;
    private int? m_SUGA_Idx;
    private string? m_ORDC_Cd;
    private string? m_ORDG_Cd;
    private string? m_ORDI_Cd;
    private string? m_ORD_SugaCode;
    private string? m_ORD_ClassCode;
    private string? m_ORD_Name;
    private string? m_ORD_InsuranceType;
    private string? m_vORD_InsuranceType;
    private string? m_ORD_SurgeryType;
    private int? m_ORD_Price;
    private string? m_ORD_Source;
    private DateTime? m_ORD_EffectiveFromDay;
    private DateTime? m_ORD_EffectiveToDay;
    private bool? m_ORD_IsUse;
    private bool? m_ORD_IsQuickOrder;
    private bool? m_ORD_IsView;

    #region "NotityPropertyChanged":

    public int? ORD_Idx
    {
        get => m_ORD_Idx;
        set => SetProperty(ref m_ORD_Idx, value);
    }

    public int? SUGA_Idx
    {
        get => m_SUGA_Idx;
        set => SetProperty(ref m_SUGA_Idx, value);
    }

    public string? ORDC_Cd
    {
        get => m_ORDC_Cd;
        set => SetProperty(ref m_ORDC_Cd, value);
    }

    public string? ORDG_Cd
    {
        get => m_ORDG_Cd;
        set => SetProperty(ref m_ORDG_Cd, value);
    }

    public string? ORDI_Cd
    {
        get => m_ORDI_Cd;
        set => SetProperty(ref m_ORDI_Cd, value);
    }

    public string? ORD_SugaCode
    {
        get => m_ORD_SugaCode;
        set => SetProperty(ref m_ORD_SugaCode, value);
    }

    public string? ORD_ClassCode
    {
        get => m_ORD_ClassCode;
        set => SetProperty(ref m_ORD_ClassCode, value);
    }

    public string? ORD_Name
    {
        get => m_ORD_Name;
        set => SetProperty(ref m_ORD_Name, value);
    }

    public string? ORD_InsuranceType
    {
        get => m_ORD_InsuranceType;
        set => SetProperty(ref m_ORD_InsuranceType, value);
    }

    public string? vORD_InsuranceType
    {
        get => m_vORD_InsuranceType;
        set => SetProperty(ref m_vORD_InsuranceType, value);
    }

    public string? ORD_SurgeryType
    {
        get => m_ORD_SurgeryType;
        set => SetProperty(ref m_ORD_SurgeryType, value);
    }

    public int? ORD_Price
    {
        get => m_ORD_Price;
        set => SetProperty(ref m_ORD_Price, value);
    }

    public string? ORD_Source
    {
        get => m_ORD_Source;
        set => SetProperty(ref m_ORD_Source, value);
    }

    public DateTime? ORD_EffectiveFromDay
    {
        get => m_ORD_EffectiveFromDay;
        set => SetProperty(ref m_ORD_EffectiveFromDay, value);
    }

    public DateTime? ORD_EffectiveToDay
    {
        get => m_ORD_EffectiveToDay;
        set => SetProperty(ref m_ORD_EffectiveToDay, value);
    }

    public bool? ORD_IsUse
    {
        get => m_ORD_IsUse;
        set => SetProperty(ref m_ORD_IsUse, value);
    }

    public bool? ORD_IsQuickOrder
    {
        get => m_ORD_IsQuickOrder;
        set => SetProperty(ref m_ORD_IsQuickOrder, value);
    }

    public bool? ORD_IsView
    {
        get => m_ORD_IsView;
        set => SetProperty(ref m_ORD_IsView, value);
    }

    #endregion
}
