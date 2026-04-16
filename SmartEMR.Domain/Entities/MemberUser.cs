using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace SmartEMR.Domain.Entities;

// partial 키워드가 있어야 소스 생성기가 코드를 추가할 수 있습니다.
public partial class MemberUser : BaseEntity
{
    [ObservableProperty] private int? m_MUR_Idx;
    [ObservableProperty] private int? m_MEM_Idx;

    [ObservableProperty] private string? m_MUR_Role;
    [ObservableProperty] private string? m_MUR_JobCode;

    [ObservableProperty] private string? m_MUR_Id;
    [ObservableProperty] private string? m_MUR_PassWord;
    [ObservableProperty] private string? m_MUR_Name;
    [ObservableProperty] private string? m_MUR_Gender;
    [ObservableProperty] private string? m_MUR_Address1;
    [ObservableProperty] private string? m_MUR_Address2;
    [ObservableProperty] private string? m_MUR_Address3;
    [ObservableProperty] private int? m_MUR_Age;
    [ObservableProperty] private string? m_MUR_BirthYear;
    [ObservableProperty] private string? m_MUR_BirthMonth;
    [ObservableProperty] private string? m_MUR_BirthDay;
    [ObservableProperty] private string? m_MUR_PhoneNum1;
    [ObservableProperty] private string? m_MUR_PhoneNum2;
    [ObservableProperty] private string? m_MUR_PhoneNum3;
    [ObservableProperty] private string? m_MUR_Email;
    [ObservableProperty] private string? m_MUR_Date;
    [ObservableProperty] private string? m_MUR_YYMMDD;
    [ObservableProperty] private bool? m_MUR_IsValid;
}