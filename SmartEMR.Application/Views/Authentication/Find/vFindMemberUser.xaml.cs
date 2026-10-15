using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.Authentication.Find;

/// <summary>
/// vFindMemberUser.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vFindMemberUser : ModelViewLayout<FindMemberUserViewModel>
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

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse();

        switch (request.MessageAction) 
        {
            case "ShowSelectFindType":
                vm.SetFindType(FindType.None);
                break;
        }

        response.IsSuccess = true;

        return response;
    }

    private async void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;
        
        switch (element.Name)
        {
            case nameof(btnBackToLogin):
                await SmartUI.SendMessage("ShowLogin", viewType:TargetViewType.RootView);
                break;
        }
    }
}
