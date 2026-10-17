using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.Members;

/// <summary>
/// vSmartEMRMemberInfoTab.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRMemberInfoTab : ModelViewLayout<MemberInfoViewModel>
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
        var response = new ViewMessageResponse<MemberUser>();

        switch (request.MessageAction)
        {
            case "CloseView":
                SmartUI.CloseView();
                break;
        }

        return response;
    }
}
