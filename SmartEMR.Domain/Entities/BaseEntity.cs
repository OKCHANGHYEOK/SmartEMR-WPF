using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartEMR.Domain.Entities;

public partial class BaseEntity : ObservableObject
{
    private string? m_keyword;
    private int? m_PageIndex;
    private int? m_PageSize;
    private string? m_SortField;
    private string? m_SortDir;
    private string? m_ViewType;
    private bool? m_IsUpdated;
    private bool? m_IsVisible;

    #region "NotifyPropertyChanged"

    public string? Keyword
    {
        get => m_keyword;
        set => SetProperty(ref m_keyword, value);
    }

    public int? PageIndex
    {
        get => m_PageIndex;
        set => SetProperty(ref m_PageIndex, value);
    }

    public int? PageSize
    {
        get => m_PageSize;
        set => SetProperty(ref m_PageSize, value);
    }

    public string? SortField
    {
        get => m_SortField;
        set => SetProperty(ref m_SortField, value);
    }

    public string? SortDir
    {
        get => m_SortDir;
        set => SetProperty(ref m_SortDir, value);
    }

    public string? ViewType
    {
        get => m_ViewType;
        set => SetProperty(ref m_ViewType, value);
    }

    public bool? IsUpdated
    {
        get => m_IsUpdated;
        set => SetProperty(ref m_IsUpdated, value);
    }

    public bool? IsVisible
    {
        get => m_IsVisible;
        set => SetProperty(ref m_IsVisible, value);
    }

    #endregion
}