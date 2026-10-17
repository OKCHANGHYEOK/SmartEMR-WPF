using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.MemberUsers;

/// <summary>
/// vMemberUserInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vMemberUserInfo : ModelViewLayout<MemberUserInfoViewModel>
{
    private MemberUser MemberUser => vm.Model;

    protected override void Initialize()
    {
    }

    protected override void SetBindGrid()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
        if (sender is not BindGrid) return;


    }

    private void OnLoaded_StyleGrid(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not StyleGrid element) return;

        element.Opacity = 1;
    }

    private async void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        switch (element.Name)
        {
            case nameof(btnChangedPassword):
                await SmartUI.NavigateToPage(new vMemberUserInfo_ChangePassword(new MemberUser { MUR_Idx = MemberUser.MUR_Idx }), isPopup:true);
                break;
        }
    }
}
