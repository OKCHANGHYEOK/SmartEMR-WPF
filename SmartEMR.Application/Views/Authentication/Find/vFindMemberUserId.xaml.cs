using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.Authentication.Find;

/// <summary>
/// vFindMemberUserId.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vFindMemberUserId : ModelViewLayout<FindMemberUserViewModel>
{
    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    private async void OnClick_Button(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not Button element) return;

        switch (element.Name)
        {
            case nameof(btnBackToSelectFindType):
                await SmartUI.SendMessage("ShowSelectFindType", viewType:TargetViewType.ParentView);
                break;
        }
    }

    private void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {

    }
}
