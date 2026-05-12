using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.Shared;

/// <summary>
/// vSearchView.xaml에 대한 상호 작용 논리
/// </summary>
[ObservableObject]
public partial class vSearchView : ModelViewLayout<Patient>
{
    [ObservableProperty]
    private string m_SearchText = string.Empty;

    public vSearchView()
    {
    }

    protected override void Initialize()
    {
    }

    public override async Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {

    }

    public void SetFocusToSearch()
    {
        this.txtSearch.Focus();
    }
}
