using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;

namespace SmartEMR.Application.Views;

/// <summary>
/// vSearchView.xaml에 대한 상호 작용 논리
/// </summary>
[ObservableObject]
public partial class vSearchView : UserControl
{
    [ObservableProperty]
    private string m_SearchText = string.Empty;

    public vSearchView()
    {
        InitializeComponent();
    }
}
