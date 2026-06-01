namespace SmartEMR.Domain.Entities;

public class ChartCommonCode : BaseEntity
{
    private int m_CCCM_Idx;
    private int m_CCCG_Idx;
    private int m_CCC_Idx;

    private string? m_CCCM_Cd;
    private string? m_CCCM_Name;
    private string? m_CCCG_Cd;
    private string? m_CCCG_Name;
    private string? m_CCC_Cd;
    private string? m_CCC_Name;

    #region "NotifyPropertyChanged"

    public int CCCM_Idx
    {
        get => m_CCCM_Idx;
        set => SetProperty(ref m_CCCM_Idx, value);
    }

    public int CCCG_Idx
    {
        get => m_CCCG_Idx;
        set => SetProperty(ref m_CCCG_Idx, value);
    }

    public int CCC_Idx
    {
        get => m_CCC_Idx;
        set => SetProperty(ref m_CCC_Idx, value);
    }

    public string? CCCM_Cd
    {
        get => m_CCCM_Cd;
        set => SetProperty(ref m_CCCM_Cd, value);
    }

    public string? CCCM_Name
    {
        get => m_CCCM_Name;
        set => SetProperty(ref m_CCCM_Name, value);
    }

    public string? CCCG_Cd
    {
        get => m_CCCG_Cd;
        set => SetProperty(ref m_CCCG_Cd, value);
    }

    public string? CCCG_Name
    {
        get => m_CCCG_Name;
        set => SetProperty(ref m_CCCG_Name, value);
    }

    public string? CCC_Cd
    {
        get => m_CCC_Cd;
        set => SetProperty(ref m_CCC_Cd, value);
    }

    public string? CCC_Name
    {
        get => m_CCC_Name;
        set => SetProperty(ref m_CCC_Name, value);
    }

    #endregion
}
