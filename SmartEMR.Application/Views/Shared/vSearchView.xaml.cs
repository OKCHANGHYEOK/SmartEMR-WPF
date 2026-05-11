using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.Shared;

/// <summary>
/// vSearchView.xaml에 대한 상호 작용 논리
/// </summary>
[ObservableObject]
public partial class vSearchView : CustomControl
{
    [ObservableProperty]
    private string m_SearchText = string.Empty;

    public vSearchView()
    {
    }
}
