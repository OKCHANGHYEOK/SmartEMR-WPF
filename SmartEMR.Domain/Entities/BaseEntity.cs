using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartEMR.Domain.Entities;

public partial class BaseEntity : ObservableObject
{
    private string? m_keyword;
    public string? Keyword
    {
        get => m_keyword;
        set => SetProperty(ref m_keyword, value);
    }

    private int? m_PageIndex;
    public int? PageIndex
    {
        get => m_PageIndex;
        set => SetProperty(ref m_PageIndex, value);
    }

    private int? m_PageSize;
    public int? PageSize
    {
        get => m_PageSize;
        set => SetProperty(ref m_PageSize, value);
    }

    private string? m_SortField;
    public string? SortField
    {
        get => m_SortField;
        set => SetProperty(ref m_SortField, value);
    }

    private string? m_SortDir;
    public string? SortDir
    {
        get => m_SortDir;
        set => SetProperty(ref m_SortDir, value);
    }

    private string? m_ViewType;
    public string? ViewType
    {
        get => m_ViewType;
        set => SetProperty(ref m_ViewType, value);
    }

    private bool? m_IsUpdated;
    public bool? IsUpdated
    {
        get => m_IsUpdated;
        set => SetProperty(ref m_IsUpdated, value);
    }
}