namespace SmartEMR.Domain.Entities;

public class CommonCode : BaseEntity
{
    private int m_CCCM_Idx;
    private int m_CCCG_Idx;
    private int m_CCI_Idx;

    private string? m_CCC_Cd;
    private string? m_CCC_Name;
    private string? m_CCG_Cd;
    private string? m_CCG_Name;
    private string? m_CCI_Cd;
    private string? m_CCI_Name;

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

    public int CCI_Idx
    {
        get => m_CCI_Idx;
        set => SetProperty(ref m_CCI_Idx, value);
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

    public string? CCG_Cd
    {
        get => m_CCG_Cd;
        set => SetProperty(ref m_CCG_Cd, value);
    }

    public string? CCG_Name
    {
        get => m_CCG_Name;
        set => SetProperty(ref m_CCG_Name, value);
    }

    public string? CCI_Cd
    {
        get => m_CCI_Cd;
        set => SetProperty(ref m_CCI_Cd, value);
    }

    public string? CCI_Name
    {
        get => m_CCI_Name;
        set => SetProperty(ref m_CCI_Name, value);
    }

    #endregion
}
