using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace SmartEMR.Domain.Entities;

public partial class BaseEntity : ObservableObject
{
    [ObservableProperty] private string? m_keyword;
    [ObservableProperty] private int? m_PageIndex;
    [ObservableProperty] private int? m_PageSize;
    [ObservableProperty] private string? m_SortField;
    [ObservableProperty] private string? m_SortDir;
    [ObservableProperty] private string? m_ViewType;
    [ObservableProperty] private bool? m_IsUpdated;
    
}
