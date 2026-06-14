using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views;

/// <summary>
/// vSmartEMRDeskTab.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRDeskTab : ModelViewLayout<DeskViewModel>
{

    public vSmartEMRDeskTab() { }

    protected override void Initialize()
    {
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse() { IsSuccess = false};

        switch (request.MessageAction)
        {
            case "SetSelectedPatient":
                var paramItem = request.MessageParameter as Patient;
                if (paramItem == null) return null;

                SmartEMRDeskPATView.UpdatePatient(paramItem);

                break;

            case "ClearPatient":
                await ClearData();
                break;
        }

        response.IsSuccess = true;

        return response;
    }

    private async Task ClearData()
    {
        await SmartUI.SendMessageToSearchView("ClearPatient");

        SmartEMRDeskPATView.ClearData();
    }

    public override async Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }
}
