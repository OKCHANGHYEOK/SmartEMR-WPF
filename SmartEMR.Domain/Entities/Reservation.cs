namespace SmartEMR.Domain.Entities;

public class Reservation : BaseEntity
{
    private int? m_RES_Idx;
    private int? m_MEM_Idx;
    private int? m_MUR_Idx;
    private int? m_MUR_Idx_DOC;
    private int? m_MUR_Idx_STF;
    private int? m_PAT_Idx;
    private string? m_PAT_ChartNo;
    private string? m_PAT_Name;
    private string? m_PAT_Sex;
    private int? m_PAT_Age;
    private string? m_vPAT_Info;
    private string? m_RES_Status;
    private string? m_vRES_Status;
    private string? m_RES_Route;
    private string? m_vRES_Route;
    private string? m_RES_Subject;
    private string? m_RES_SubjectName;
    private string? m_vRES_SubjectName;
    private string? m_RES_ReservationDate;
    private string? m_RES_ReservationTime;
    private string? m_RES_Memo;
    private string? m_RES_Date;
    private string? m_RES_YYMMDD;
    private bool? m_RES_IsValid;

    private string? m_MUR_Name_DOC;

    private string? m_sDay;
    private string? m_eDay;

    private Patient? m_PATItem;

    #region "NotifyPropertyChanged"

    public int? RES_Idx
    {
        get => m_RES_Idx;
        set => SetProperty(ref m_RES_Idx, value);
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

    public int? MUR_Idx_DOC
    {
        get => m_MUR_Idx_DOC;
        set => SetProperty(ref m_MUR_Idx_DOC, value);
    }

    public int? MUR_Idx_STF
    {
        get => m_MUR_Idx_STF;
        set => SetProperty(ref m_MUR_Idx_STF, value);
    }

    public int? PAT_Idx
    {
        get => m_PAT_Idx;
        set => SetProperty(ref m_PAT_Idx, value);
    }

    public string? PAT_ChartNo
    {
        get => m_PAT_ChartNo;
        set => SetProperty(ref m_PAT_ChartNo, value);
    }

    public string? PAT_Name
    {
        get => m_PAT_Name;
        set => SetProperty(ref m_PAT_Name, value);
    }

    public string? PAT_Sex
    {
        get => m_PAT_Sex;
        set => SetProperty(ref m_PAT_Sex, value);
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

    public string? RES_Status
    {
        get => m_RES_Status;
        set => SetProperty(ref m_RES_Status, value);
    }

    public string? vRES_Status
    {
        get => m_vRES_Status;
        set => SetProperty(ref m_vRES_Status, value);
    }


    public string? RES_Route
    {
        get => m_RES_Route;
        set => SetProperty(ref m_RES_Route, value);
    }


    public string? vRES_Route
    {
        get => m_vRES_Route;
        set => SetProperty(ref m_vRES_Route, value);
    }

    public string? RES_Subject
    {
        get => m_RES_Subject;
        set => SetProperty(ref m_RES_Subject, value);
    }

    public string? RES_SubjectName
    {
        get => m_RES_SubjectName;
        set => SetProperty(ref m_RES_SubjectName, value);
    }

    public string? vRES_SubjectName
    {
        get => m_vRES_SubjectName;
        set => SetProperty(ref m_vRES_SubjectName, value);
    }

    public string? RES_ReservationDate
    {
        get => m_RES_ReservationDate;
        set => SetProperty(ref m_RES_ReservationDate, value);
    }

    public string? RES_ReservationTime
    {
        get => m_RES_ReservationTime;
        set => SetProperty(ref m_RES_ReservationTime, value);
    }

    public string? RES_Memo
    {
        get => m_RES_Memo;
        set => SetProperty(ref m_RES_Memo, value);
    }

    public string? RES_Date
    {
        get => m_RES_Date;
        set => SetProperty(ref m_RES_Date, value);
    }

    public string? RES_YYMMDD
    {
        get => m_RES_YYMMDD;
        set => SetProperty(ref m_RES_YYMMDD, value);
    }

    public bool? RES_IsValid
    {
        get => m_RES_IsValid;
        set => SetProperty(ref m_RES_IsValid, value);
    }

    public string? MUR_Name_DOC
    {
        get => m_MUR_Name_DOC;
        set => SetProperty(ref m_MUR_Name_DOC, value);
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


    public Patient? PATItem
    {
        get => m_PATItem;
        set => SetProperty(ref m_PATItem, value);
    }

    #endregion
}
